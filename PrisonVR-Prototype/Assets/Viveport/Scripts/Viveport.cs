using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using PublicKeyConvert;
using Viveport.Core;

namespace Viveport
{
    namespace Core
    {
        public class Logger
        {
            private const string LoggerTypeNameUnity = "UnityEngine.Debug";

            private static bool _hasDetected = false;
            private static bool _usingUnityLog = true;
            private static Type _unityLogType = null;

            public static void Log(string message)
            {
                if (!_hasDetected || _usingUnityLog)
                {
                    UnityLog(message);
                }
                else
                {
                    ConsoleLog(message);
                }
            }

            private static void ConsoleLog(string message)
            {
                Console.WriteLine(message);
                _hasDetected = true;
            }

            private static void UnityLog(string message)
            {
                try
                {
                    if (_unityLogType == null)
                    {
                        _unityLogType = GetType(LoggerTypeNameUnity);
                    }
                    MethodInfo methodInfo = _unityLogType.GetMethod("Log", new[] { typeof(string) });
                    methodInfo.Invoke(null, new object[] { message });
                    _usingUnityLog = true;
                }
                catch (Exception)
                {
                    ConsoleLog(message);
                    _usingUnityLog = false;
                }
                _hasDetected = true;
            }

            private static Type GetType(string typeName)
            {
                Type type = Type.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
                return null;
            }
        }
    }

    public delegate void StatusCallback(int nResult);

    class Leaderboard
    {
        private int _ScoreRank;
        private int _Score;
        private string _UserName;

        public int Rank
        {
            get { return _ScoreRank; }
            set { _ScoreRank = value; }
        }

        public int Score
        {
            get { return _Score; }
            set { _Score = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
    public partial class Api
    {
        internal static readonly List<Internal.GetLicenseCallback> InternalGetLicenseCallbacks = new List<Internal.GetLicenseCallback>();
        internal static readonly List<Internal.StatusCallback> InternalStatusCallbacks = new List<Internal.StatusCallback>();
        internal static readonly List<LicenseChecker> InternalLicenseCheckers = new List<LicenseChecker>();

        private static readonly string VERSION = "1.5.0.31";

        private static string _appId = "";
        private static string _appKey = "";

        public static void GetLicense(LicenseChecker checker, string appId, string appKey)
        {
            if (checker == null || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey))
            {
                throw new InvalidOperationException("checker == null || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appKey)");
            }
            _appId = appId;
            _appKey = appKey;

            InternalLicenseCheckers.Add(checker);
            Internal.Api.GetLicense(GetLicenseHandler, _appId, _appKey);
        }

        public static int Init(StatusCallback callback, string appId)
        {
            if (callback == null || string.IsNullOrEmpty(appId))
            {
                throw new InvalidOperationException("callback == null || string.IsNullOrEmpty(appId)");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            InternalStatusCallbacks.Add(internalCallback);
            return Internal.Api.Init(internalCallback, appId);
        }

        public static int Shutdown(StatusCallback callback)
        {
            if (callback == null)
            {
                throw new InvalidOperationException("callback == null");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            InternalStatusCallbacks.Add(internalCallback);
            return Internal.Api.Shutdown(internalCallback);
        }

        public static string Version()
        {
            string nativeVersion = "";
            try
            {
                nativeVersion += Marshal.PtrToStringAnsi(Internal.Api.Version());
            }
            catch (Exception)
            {
                Logger.Log("Can not load version from native library");
            }
            return "C# version: " + VERSION + ", Native version: " + nativeVersion;
        }

        /*
         * Responsed license JSON format:
         * {
         *   "issueTime": 1442301893123, // epoch time in milliseconds, Long
         *   "expirationTime": 1442451893123, // epoch time in milliseconds, Long
         *   "latestVersion": 1001, // versionId, Integer
         *   "updateRequired": true // Boolean
         * }
         */
        private static void GetLicenseHandler([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature)
        {
            // Logger.Log("Raw Message: " + message);
            // Logger.Log("Raw Signature: " + signature);

            bool isVerified = !string.IsNullOrEmpty(message);
            if (!isVerified)
            {
                for (int i = InternalLicenseCheckers.Count - 1; i >= 0; i--)
                {
                    LicenseChecker checker = InternalLicenseCheckers[i];
                    checker.OnFailure(90003, "License message is empty");
                    InternalLicenseCheckers.Remove(checker);
                }
                return;
            }

            isVerified = !string.IsNullOrEmpty(signature);
            if (!isVerified) // signature is empty - error code mode
            {
                JsonData jsonData = JsonMapper.ToObject(message);
                int errorCode = 99999;
                string errorMessage = "";

                try
                {
                    errorCode = int.Parse((string)jsonData["code"]);
                }
                catch
                {
                    // ignored
                }
                try
                {
                    errorMessage = (string)jsonData["message"];
                }
                catch
                {
                    // ignored
                }

                for (int i = InternalLicenseCheckers.Count - 1; i >= 0; i--)
                {
                    LicenseChecker checker = InternalLicenseCheckers[i];
                    checker.OnFailure(errorCode, errorMessage);
                    InternalLicenseCheckers.Remove(checker);
                }
                return;
            }

            isVerified = VerifyMessage(_appId + "\n" + message, signature, _appKey);
            if (!isVerified)
            {
                for (int i = InternalLicenseCheckers.Count - 1; i >= 0; i--)
                {
                    LicenseChecker checker = InternalLicenseCheckers[i];
                    checker.OnFailure(90001, "License verification failed");
                    InternalLicenseCheckers.Remove(checker);
                }
                return;
            }

            string decodedLicense = Encoding.UTF8.GetString(Convert.FromBase64String(message.Substring(message.IndexOf("\n") + 1)));
            JsonData jsonData2 = JsonMapper.ToObject(decodedLicense);
            Logger.Log("License: " + decodedLicense);

            long issueTime = -1;
            long expirationTime = -1;
            int latestVersion = -1;
            bool updateRequired = false;

            try
            {
                issueTime = (long)jsonData2["issueTime"];
            }
            catch
            {
                // ignored
            }
            try
            {
                expirationTime = (long)jsonData2["expirationTime"];
            }
            catch
            {
                // ignored
            }
            try
            {
                latestVersion = ((int)jsonData2["latestVersion"]);
            }
            catch
            {
                // ignored
            }
            try
            {
                updateRequired = (bool)jsonData2["updateRequired"];
            }
            catch
            {
                // ignored
            }

            for (int i = InternalLicenseCheckers.Count - 1; i >= 0; i--)
            {
                LicenseChecker checker = InternalLicenseCheckers[i];
                checker.OnSuccess(issueTime, expirationTime, latestVersion, updateRequired);
                InternalLicenseCheckers.Remove(checker);
            }
        }

        private static bool VerifyMessage(string message, string signature, string publicKey)
        {
            try
            {
                RSACryptoServiceProvider provider = PEMKeyLoader.CryptoServiceProviderFromPublicKeyInfo(publicKey);
                byte[] decodedSignature = Convert.FromBase64String(signature);
                SHA1Managed sha = new SHA1Managed();
                byte[] data = Encoding.UTF8.GetBytes(message);

                return provider.VerifyData(data, sha, decodedSignature);
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
            return false;
        }

        public abstract class LicenseChecker
        {
            public abstract void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired);
            public abstract void OnFailure(int errorCode, string errorMessage);
        }
    }

    public partial class User
    {
        private static readonly int MaxIdLength = 256;
        private static readonly int MaxNameLength = 256;
        private static readonly int MaxUrlLength = 512; 

        public static string GetUserId()
        {
            StringBuilder userId = new StringBuilder(MaxIdLength);
            Internal.User.GetUserID(userId, MaxIdLength);
            return userId.ToString();
        }

        public static string GetUserName()
        {
            StringBuilder userName = new StringBuilder(MaxNameLength);
            Internal.User.GetUserName(userName, MaxNameLength);
            return userName.ToString();
        }

        public static string GetUserAvatarUrl()
        {
            StringBuilder userAvatarUrl = new StringBuilder(MaxUrlLength);
            Internal.User.GetUserAvatarUrl(userAvatarUrl, MaxUrlLength);
            return userAvatarUrl.ToString();
        }
    }

    partial class UserStats
    {
        public enum LeaderBoardRequestType
        {
            GlobalData = Internal.ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal /* 0 */,
            GlobalDataAroundUser = Internal.ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser /* 1 */,
            LocalData = Internal.ELeaderboardDataRequest.k_ELeaderboardDataRequestLocal /* 2 */,
            LocalDataAroundUser = Internal.ELeaderboardDataRequest.k_ELeaderboardDataRequestLocaleAroundUser /* 3 */,
        }

        public enum LeaderBoardTimeRange
        {
            AllTime = Internal.ELeaderboardDataTimeRange.k_ELeaderboardDataScropeAllTime /* 0 */,
            Daily = Internal.ELeaderboardDataTimeRange.k_ELeaderboardDataScropeDaily /* 1 */,
            Weekly = Internal.ELeaderboardDataTimeRange.k_ELeaderboardDataScropeWeekly /* 2 */,
            Monthly = Internal.ELeaderboardDataTimeRange.k_ELeaderboardDataScropeMonthly /* 3 */,
        }

        public enum LeaderBoardSortMethod
        {
            None = Internal.ELeaderboardSortMethod.k_ELeaderboardSortMethodNone /* 0 */,
            Ascending = Internal.ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending /* 1 */,
            Descending = Internal.ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending /* 2 */,
        }

        public enum LeaderBoardDiaplayType
        {
            None = Internal.ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNone /* 0 */,
            Numeric = Internal.ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric /* 1 */,
            TimeSeconds = Internal.ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds /* 2 */,
            TimeMilliSeconds = Internal.ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeMilliSeconds /* 3 */,
        }

        public enum LeaderBoardScoreMethod
        {
            None = Internal.ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodNone /* 0 */,
            KeepBest = Internal.ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest /* 1 */,
            ForceUpdate = Internal.ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate /* 2 */,
        }

        public static int IsReady(StatusCallback callback)
        {
            if (callback == null)
            {
                throw new InvalidOperationException("callback == null");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            Api.InternalStatusCallbacks.Add(internalCallback);

            return Internal.UserStats.IsReady(internalCallback);
        }

        public static int DownloadStats(StatusCallback callback)
        {
            if (callback == null)
            {
                throw new InvalidOperationException("callback == null");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            Api.InternalStatusCallbacks.Add(internalCallback);

            return Internal.UserStats.DownloadStats(internalCallback);
        }

        public static int GetStat(string name, int defaultValue)
        {
            int result = defaultValue;
            Internal.UserStats.GetStat(name, ref result);
            return result;
        }

        public static float GetStat(string name, float defaultValue)
        {
            float result = defaultValue;
            Internal.UserStats.GetStat(name, ref result);
            return result;
        }

        public static void SetStat(string name, int value)
        {
            Internal.UserStats.SetStat(name, value);
        }

        public static void SetStat(string name, float value)
        {
            Internal.UserStats.SetStat(name, value);
        }

        public static int UploadStats(StatusCallback callback)
        {
            if (callback == null)
            {
                throw new InvalidOperationException("callback == null");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            Api.InternalStatusCallbacks.Add(internalCallback);

            return Internal.UserStats.UploadStats(internalCallback);
        }

        // for Achievements
        public static bool GetAchievement(string pchName)
        {
            int nAchieved = 0;
            Internal.UserStats.GetAchievement(pchName, ref nAchieved);
            if (nAchieved == 1)
                return true;
            else
                return false;
        }

        public static int GetAchievementUnlockTime(string pchName)
        {
            int nUnlockTime = 0;
            Internal.UserStats.GetAchievementUnlockTime(pchName, ref nUnlockTime);
            return nUnlockTime;
        }

        public static int SetAchievement(string pchName)
        {
            return Internal.UserStats.SetAchievement(pchName);
        }

        public static int ClearAchievement(string pchName)
        {
            return Internal.UserStats.ClearAchievement(pchName);
        }

        // for Leaderboards
        public static int DownloadLeaderboardScores(StatusCallback callback, string pchLeaderboardName, LeaderBoardRequestType eLeaderboardDataRequest, LeaderBoardTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd)
        {
            if (callback == null)
            {
                throw new InvalidOperationException("callback == null");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            Api.InternalStatusCallbacks.Add(internalCallback);

            return Internal.UserStats.DownloadLeaderboardScores(internalCallback, pchLeaderboardName, (Internal.ELeaderboardDataRequest)eLeaderboardDataRequest, (Internal.ELeaderboardDataTimeRange)eLeaderboardDataTimeRange, nRangeStart, nRangeEnd);
        }

        public static int UploadLeaderboardScore(StatusCallback callback, string pchLeaderboardName, int nScore)
        {
            if (callback == null)
            {
                throw new InvalidOperationException("callback == null");
            }

            Internal.StatusCallback internalCallback = new Internal.StatusCallback(callback);
            Api.InternalStatusCallbacks.Add(internalCallback);

            return Internal.UserStats.UploadLeaderboardScore(internalCallback, pchLeaderboardName, nScore);
        }

        public static Leaderboard GetLeaderboardScore(int index)
        {
            Internal.LeaderboardEntry_t pLeaderboardEntry;
            pLeaderboardEntry.m_nGlobalRank = 0;
            pLeaderboardEntry.m_nScore = 0;
            pLeaderboardEntry.m_pUserName = "";
            Internal.UserStats.GetLeaderboardScore(index, ref pLeaderboardEntry);
            Leaderboard lbData = new Leaderboard();
            lbData.Rank = pLeaderboardEntry.m_nGlobalRank;
            lbData.Score = pLeaderboardEntry.m_nScore;
            lbData.UserName = pLeaderboardEntry.m_pUserName;
            return lbData;
        }

        public static int GetLeaderboardScoreCount()
        {
            return Internal.UserStats.GetLeaderboardScoreCount();
        }

        public static Internal.ELeaderboardSortMethod GetLeaderboardSortMethod()
        {
            return Internal.UserStats.GetLeaderboardSortMethod();
        }

        public static Internal.ELeaderboardDisplayType GetLeaderboardDisplayType()
        {
            return Internal.UserStats.GetLeaderboardDisplayType();
        }
    }
    partial class IAPurchase
    {

        public static void IsReady(IAPurchaseListener listener, string pchAppKey)
        {
            ViveportIAPHandler handler = new ViveportIAPHandler(listener);
            Internal.IAPurchase.IsReady(handler.getIsReadyHandler(), pchAppKey);
        }
        public static void Request(IAPurchaseListener listener, string pchPrice)
        {
            ViveportIAPHandler handler = new ViveportIAPHandler(listener);
            Internal.IAPurchase.Request(handler.getRequestHandler(), pchPrice);
        }
        public static void Purchase(IAPurchaseListener listener, string pchPurchaseId)
        {
            ViveportIAPHandler handler = new ViveportIAPHandler(listener);
            Internal.IAPurchase.Purchase(handler.getPurchaseHandler(), pchPurchaseId);
        }

        public static void Query(IAPurchaseListener listener, string pchPurchaseId)
        {
            ViveportIAPHandler handler = new ViveportIAPHandler(listener);
            Internal.IAPurchase.Query(handler.getQueryHandler(), pchPurchaseId);
        }
        public static void GetBalance(IAPurchaseListener listener)
        {
            ViveportIAPHandler handler = new ViveportIAPHandler(listener);
            Internal.IAPurchase.GetBalance(handler.getBalanceHandler());
        }

        partial class ViveportIAPHandler : BaseHandler
        {
            IAPurchaseListener listener;

            public ViveportIAPHandler(IAPurchaseListener cb)
            {
                listener = cb;
            }

            #region IsReady

            public Internal.IAPurchaseCallback getIsReadyHandler()
            {
                return IsReadyHandler;
            }

            /*
             * TODO
             * 
             * Responsed JSON format:
             * {
             *   "statusCode": 500,     // status code, Integer
             *   "currencyName": "",     // user's setting currencyName
             *   "message": "",         // error message information, String
             * }
             * 
             */
            protected override void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
            {
                Viveport.Core.Logger.Log("[IsReadyHandler] message=" + message);
                JsonData jsonData = JsonMapper.ToObject(message);
                int statusCode = -1;
                string currencyName = "";
                string errMessage = "";
                if (code == 0)
                {
                    try
                    {
                        statusCode = (int)jsonData["statusCode"];
                        errMessage = (string)jsonData["message"];
                    }
                    catch (Exception ex)
                    {
                        Viveport.Core.Logger.Log("[IsReadyHandler] statusCode, message ex=" + ex);
                    }
                    Viveport.Core.Logger.Log("[IsReadyHandler] statusCode =" + statusCode + ",errMessage=" + errMessage);
                    if (statusCode == 0)
                    {
                        try
                        {
                            currencyName = (string)jsonData["currencyName"];
                        }
                        catch (Exception ex)
                        {
                            Viveport.Core.Logger.Log("[IsReadyHandler] currencyName ex=" + ex);
                        }
                        Viveport.Core.Logger.Log("[IsReadyHandler] currencyName=" + currencyName);
                    }
                }

                if (listener != null)
                {
                    if (code == 0)
                    {
                        // TODO The actual success judgement.
                        if (statusCode == 0)
                        {
                            listener.OnSuccess(currencyName);
                        }
                        else
                        {
                            listener.OnFailure(statusCode, errMessage);
                        }
                    }
                    else
                    {
                        listener.OnFailure(code, message);
                    }
                }
            }

            #endregion IsReady
            #region Request

            public Internal.IAPurchaseCallback getRequestHandler()
            {
                return RequestHandler;
            }

            /*
             * TODO
             * 
             * Responsed JSON format:
             * {
             *   "statusCode": 500,     // status code, Integer
             *   "purchase_id": "",     // specific purchase id, String
             *   "message": "",         // error message information, String
             * }
             * 
             */
            protected override void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
            {
                Viveport.Core.Logger.Log("[RequestHandler] message=" + message);

                JsonData jsonData = JsonMapper.ToObject(message);
                int statusCode = -1;
                string purchaseId = "";
                string errMessage = "";

                if (code == 0)
                {
                    try
                    {
                        statusCode = (int)jsonData["statusCode"];
                        errMessage = (string)jsonData["message"];
                    }
                    catch (Exception ex)
                    {
                        Viveport.Core.Logger.Log("[RequestHandler] statusCode, message ex=" + ex);
                    }
                    Viveport.Core.Logger.Log("[RequestHandler] statusCode =" + statusCode + ",errMessage=" + errMessage);
                    if (statusCode == 0)
                    {
                        try
                        {
                            purchaseId = (string)jsonData["purchase_id"];
                        }
                        catch (Exception ex)
                        {
                            Viveport.Core.Logger.Log("[RequestHandler] purchase_id ex=" + ex);
                        }
                        Viveport.Core.Logger.Log("[RequestHandler] purchaseId =" + purchaseId);
                    }
                }
                if (listener != null)
                {
                    if (code == 0)
                    {
                        // TODO The actual success judgement.
                        if (statusCode == 0)
                        {
                            listener.OnRequestSuccess(purchaseId);
                        }
                        else
                        {
                            listener.OnFailure(statusCode, errMessage);
                        }
                    }
                    else
                    {
                        listener.OnFailure(code, message);
                    }
                }
            }

            #endregion Request

            #region Purchase

            public Internal.IAPurchaseCallback getPurchaseHandler()
            {
                return PurchaseHandler;
            }

            /*
             * TODO
             * 
             * Responsed JSON format:
             * {
             *   "statusCode": 500,     // status code, Integer
             *   "purchase_id": "",     // specific purchase id, String
             *   "paid_timestamp": 0,   // paid_timestamp in milli seconds, Long, 
             *   "message": "",         // error message information, String
             * }
             * 
             */
            protected override void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
            {
                Viveport.Core.Logger.Log("[PurchaseHandler] message=" + message);

                JsonData jsonData = JsonMapper.ToObject(message);
                int statusCode = -1;
                string purchaseId = "";
                string errMessage = "";
                long paid_timestamp = 0L;

                if (code == 0)
                {
                    try
                    {
                        statusCode = (int)jsonData["statusCode"];
                        errMessage = (string)jsonData["message"];
                    }
                    catch (Exception ex)
                    {
                        Viveport.Core.Logger.Log("[PurchaseHandler] statusCode, message ex=" + ex);
                    }
                    Viveport.Core.Logger.Log("[PurchaseHandler] statusCode =" + statusCode + ",errMessage=" + errMessage);
                    if (statusCode == 0)
                    {
                        try
                        {
                            purchaseId = (string)jsonData["purchase_id"];
                            paid_timestamp = (long)jsonData["paid_timestamp"];
                        }
                        catch (Exception ex)
                        {
                            Viveport.Core.Logger.Log("[PurchaseHandler] purchase_id,paid_timestamp ex=" + ex);
                        }
                        Viveport.Core.Logger.Log("[PurchaseHandler] purchaseId =" + purchaseId + ",paid_timestamp=" + paid_timestamp);
                    }
                }
                if (listener != null)
                {
                    if (code == 0)
                    {
                        // TODO The actual success judgement.
                        if (statusCode == 0)
                        {
                            listener.OnPurchaseSuccess(purchaseId);
                        }
                        else
                        {
                            listener.OnFailure(statusCode, errMessage);
                        }
                    }
                    else
                    {
                        listener.OnFailure(code, message);
                    }
                }
            }

            #endregion Purchase

            #region Query

            public Internal.IAPurchaseCallback getQueryHandler()
            {
                return QueryHandler;
            }


            /*
             * TODO
             * 
             * Responsed JSON format:
             * 
             * { 
             *   "order_id": "",                // , String
             *   "purchase_id": "string",       // , String
             *   "status": "string",            // , String
             *   "app_id": "string",            // , String
             *   "price": "string",             // , String
             *   "item_list": [
             *     {
             *       "item_id": "string",           // , String
             *       "quantity": 0,                 // , Integer
             *       "subtotal_price": "string",    // , String
             *       "category": "string",          // , String
             *       "description": "string"        // , String
             *     }
             *   ],
             *   "currency": "string",          // , String
             *   "paid_timestamp": 0,           // epoch time in milliseconds, Long
             *   "user_data": "string"          // , String
             * }
             * 
             */
            protected override void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
            {
                Viveport.Core.Logger.Log("[QueryHandler] message=" + message);
                JsonData jsonData = JsonMapper.ToObject(message);
                int statusCode = -1;
                string purchaseId = "";
                string errMessage = "";
                string order_id = "";
                string status = "";
                long paid_timestamp = 0L;

                if (code == 0)
                {
                    try
                    {
                        statusCode = (int)jsonData["statusCode"];
                        errMessage = (string)jsonData["message"];
                    }
                    catch (Exception ex)
                    {
                        Viveport.Core.Logger.Log("[QueryHandler] statusCode, message ex=" + ex);
                    }
                    Viveport.Core.Logger.Log("[QueryHandler] statusCode =" + statusCode + ",errMessage=" + errMessage);
                    if (statusCode == 0)
                    {
                        try
                        {
                            purchaseId = (string)jsonData["purchase_id"];
                            order_id = (string)jsonData["order_id"];
                            status = (string)jsonData["status"];
                            paid_timestamp = (long)jsonData["paid_timestamp"];
                        }
                        catch (Exception ex)
                        {
                            Viveport.Core.Logger.Log("[QueryHandler] purchase_id, order_id ex=" + ex);
                        }
                        Viveport.Core.Logger.Log("[QueryHandler] purchaseId =" + purchaseId + ",order_id=" + order_id + ",paid_timestamp=" + paid_timestamp);
                    }
                }
                if (listener != null)
                {
                    if (code == 0)
                    {
                        // TODO The actual success judgement.
                        if (statusCode == 0)
                        {
                            QueryResponse response = new QueryResponse();
                            response.purchase_id = purchaseId;
                            response.order_id = order_id;
                            response.paid_timestamp = paid_timestamp;
                            response.status = status;
                            listener.OnQuerySuccess(response);
                        }
                        else
                        {
                            listener.OnFailure(statusCode, errMessage);
                        }

                    }
                    else
                    {
                        listener.OnFailure(code, message);
                    }
                }

                /*
                if (listener != null)
                {
                    if (code == 0)
                    {
                        //string sampleText = "{\"order_id\":\"response_order_id_000\",\"purchase_id\":null,\"status\":\"response_status_000\",\"price\":null,\"item_list\":null,\"currency\":null,\"paid_timestamp\":0,\"user_data\":null}";
                        QueryResponse response = JsonMapper.ToObject<QueryResponse>(message);
                        if (response != null && string.IsNullOrEmpty(response.message))
                        {
                            listener.OnQuerySuccess(response);
                        }
                        else
                        {
                            int statusCode = 999;
                            if (response != null && !string.IsNullOrEmpty(response.code))
                            {
                                // TODO code shoud be Integer
                                string[] codes = response.code.Split('.');
                                if (codes != null && codes.Length > 0)
                                    statusCode = Int32.Parse(codes[0]);
                            }

                            string errMessage = (response != null) ? response.message : "";
                            listener.OnFailure(statusCode, errMessage);
                        }
                    }
                    else
                    {
                        listener.OnFailure(code, message);
                    }
                }
                */
            }

            #endregion Query
            #region GetBalance

            public Internal.IAPurchaseCallback getBalanceHandler()
            {
                return BalanceHandler;
            }
            /*
             * TODO
             * 
             * Responsed JSON format:
             * {
             *   "statusCode": 500,     // status code, Integer
             *   "currencyName": "USD",     // currency name, String
             *   "balance": "100",     // balance, String
             *   "message": "",         // error message information, String
             * }
             * 
             */
            protected override void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
            {
                Viveport.Core.Logger.Log("[BalanceHandler] code=" + code + ",message= " + message);
                JsonData jsonData = JsonMapper.ToObject(message);
                int statusCode = -1;
                string currencyName = "";
                string balance = "";
                string errMessage = "";

                if (code == 0)
                {
                    try
                    {
                        statusCode = (int)jsonData["statusCode"];
                        errMessage = (string)jsonData["message"];
                    }
                    catch (Exception ex)
                    {
                        Viveport.Core.Logger.Log("[BalanceHandler] statusCode, message ex=" + ex);
                    }
                    Viveport.Core.Logger.Log("[BalanceHandler] statusCode =" + statusCode + ",errMessage=" + errMessage);
                    if (statusCode == 0)
                    {
                        try
                        {
                            currencyName = (string)jsonData["currencyName"];
                            balance = (string)jsonData["balance"];
                        }
                        catch (Exception ex)
                        {
                            Viveport.Core.Logger.Log("[BalanceHandler] currencyName, balance ex=" + ex);
                        }
                        Viveport.Core.Logger.Log("[BalanceHandler] currencyName=" + currencyName + ",balance=" + balance);
                    }
                }

                if (listener != null)
                {
                    if (code == 0)
                    {
                        // TODO The actual success judgement.
                        if (statusCode == 0)
                        {
                            listener.OnBalanceSuccess(balance);
                        }
                        else
                        {
                            listener.OnFailure(statusCode, errMessage);
                        }
                    }
                    else
                    {
                        listener.OnFailure(code, message);
                    }
                }
            }
            #endregion GetBalance
        }

        abstract partial class BaseHandler
        {
            protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
            protected abstract void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
            protected abstract void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
            protected abstract void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
            protected abstract void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
        }

        public partial class IAPurchaseListener
        {
            public virtual void OnSuccess(string pchCurrencyName) { }
            public virtual void OnRequestSuccess(string pchPurchaseId) { }
            public virtual void OnPurchaseSuccess(string pchPurchaseId) { }
            public virtual void OnQuerySuccess(QueryResponse response) { }
            public virtual void OnBalanceSuccess(string pchBalance) { }
            public virtual void OnFailure(int nCode, string pchMessage) { }
        }


        public class QueryResponse
        {
            public string order_id { get; set; }
            public string purchase_id { get; set; }
            public string status { get; set; }//the value of status is "created" or "processing" or "success" or "failure" or "expired"
            public string price { get; set; }//currently, not assign value
            public string currency { get; set; }//currently, not assign value
            public long paid_timestamp { get; set; }
        }
    }
}
