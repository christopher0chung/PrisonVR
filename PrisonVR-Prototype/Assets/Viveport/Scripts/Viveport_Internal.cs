using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void GetLicenseCallback([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string signature);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void StatusCallback(int nResult);

    enum ELeaderboardDataRequest
    {
        k_ELeaderboardDataRequestGlobal = 0,
        k_ELeaderboardDataRequestGlobalAroundUser = 1,
        k_ELeaderboardDataRequestLocal = 2,
        k_ELeaderboardDataRequestLocaleAroundUser = 3,
    };

    enum ELeaderboardDataTimeRange
    {
        k_ELeaderboardDataScropeAllTime = 0,
        k_ELeaderboardDataScropeDaily = 1,
        k_ELeaderboardDataScropeWeekly = 2,
        k_ELeaderboardDataScropeMonthly = 3,
    };

    enum ELeaderboardSortMethod
    {
        k_ELeaderboardSortMethodNone,
        k_ELeaderboardSortMethodAscending,
        k_ELeaderboardSortMethodDescending,
    };

    enum ELeaderboardDisplayType
    {
        k_ELeaderboardDisplayTypeNone = 0,
        k_ELeaderboardDisplayTypeNumeric = 1,           // simple numerical score
        k_ELeaderboardDisplayTypeTimeSeconds = 2,       // the score represents a time, in seconds
        k_ELeaderboardDisplayTypeTimeMilliSeconds = 3,  // the score represents a time, in milliseconds
    };

    enum ELeaderboardUploadScoreMethod
    {
        k_ELeaderboardUploadScoreMethodNone = 0,
        k_ELeaderboardUploadScoreMethodKeepBest = 1,    // Leaderboard will keep user's best score
        k_ELeaderboardUploadScoreMethodForceUpdate = 2, // Leaderboard will always replace score with specified
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct LeaderboardEntry_t
    {
        public int m_nGlobalRank;       // [1..N], where N is the number of users with an entry in the leaderboard
        public int m_nScore;            // score as set in the leaderboard
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string m_pUserName;      // the user name showing in the leaderboard
    };

    internal partial class Api
    {
        static Api()
        {
            LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_GetLicense", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetLicense(GetLicenseCallback callback, string appId, string appKey);

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_Init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Init(StatusCallback initCallback, string appId);

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_Shutdown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Shutdown(StatusCallback initCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportAPI_Version", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Version();

        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        internal static void LoadLibraryManually(string dllName)
        {
#if UNITY_5
            return;
#else
            if (string.IsNullOrEmpty(dllName))
            {
                return;
            }

            var is64 = IntPtr.Size == 8;
            if (is64)
            {
                LoadLibrary("x64/" + dllName);
            }
            else
            {
                LoadLibrary("x86/" + dllName);
            }
#endif
        }
    }

    internal partial class User
    {
        static User()
        {
            Api.LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportUser_GetUserID", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetUserID(StringBuilder userId, int size);

        [DllImport("viveport_api", EntryPoint = "IViveportUser_GetUserName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetUserName(StringBuilder userName, int size);

        [DllImport("viveport_api", EntryPoint = "IViveportUser_GetUserAvatarUrl", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetUserAvatarUrl(StringBuilder userAvatarUrl, int size);
    }


    internal partial class UserStats
    {
        static UserStats()
        {
            Api.LoadLibraryManually("viveport_api.dll");
        }

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int IsReady(StatusCallback IsReadyCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_DownloadStats", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DownloadStats(StatusCallback downloadStatsCallback);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetStat0", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetStat(string pchName, ref int pnData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetStat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetStat(string pchName, ref float pfData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_SetStat0", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetStat(string pchName, int nData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_SetStat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetStat(string pchName, float fData);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_UploadStats", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int UploadStats(StatusCallback uploadStatsCallback);

        // for Achievements
        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetAchievement", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetAchievement(string pchName, ref int pbAchieved);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetAchievementUnlockTime", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetAchievementUnlockTime(string pchName, ref int punUnlockTime);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_SetAchievement", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetAchievement(string pchName);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_ClearAchievement", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ClearAchievement(string pchName);

        // for Leaderboards
        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_DownloadLeaderboardScores", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DownloadLeaderboardScores(StatusCallback downloadLeaderboardScoresCB, string pchLeaderboardName, ELeaderboardDataRequest eLeaderboardDataRequest, ELeaderboardDataTimeRange eLeaderboardDataTimeRange, int nRangeStart, int nRangeEnd);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_UploadLeaderboardScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int UploadLeaderboardScore(StatusCallback uploadLeaderboardScoreCB, string pchLeaderboardName, int nScore);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardScore", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardScore(int index, ref LeaderboardEntry_t pLeaderboardEntry);

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardScoreCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLeaderboardScoreCount();

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardSortMethod", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ELeaderboardSortMethod GetLeaderboardSortMethod();

        [DllImport("viveport_api", EntryPoint = "IViveportUserStats_GetLeaderboardDisplayType", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ELeaderboardDisplayType GetLeaderboardDisplayType();
    }
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IAPurchaseCallback(int code, [MarshalAs(UnmanagedType.LPStr)] string message);


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct IAPCurrency_t
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string m_pName;		    // the name of user setting currency 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string m_pSymbol;		// the symbol of user setting currency 
    };

    partial class IAPurchase
    {
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_IsReady", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IsReady(IAPurchaseCallback callback, string pchAppKey);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Request", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Request(IAPurchaseCallback callback, string pchPrice);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Purchase", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Purchase(IAPurchaseCallback callback, string pchPurchaseId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_Query", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Query(IAPurchaseCallback callback, string pchPurchaseId);
        [DllImport("viveport_api", EntryPoint = "IViveportIAPurchase_GetBalance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetBalance(IAPurchaseCallback callback);
    }
}
