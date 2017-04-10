using UnityEngine;
using System.Collections;
using Viveport;

public class ViveportDemo : MonoBehaviour {

    private int nInitValue = 0, nResult = 0;
    private int nWidth = 80, nHeight = 40;
    private int nXStart = 10, nYStart = 35;
    private string stringToEdit = "Input Stats name";
    private string StatsCount = "Input max index";
    private string achivToEdit = "Input achieve name";
    private string leaderboardToEdit = "Input leaderboard name";
    private string leaderboardScore = "Input score"; 

    static string APP_ID = "bd67b286-aafc-449d-8896-bb7e9b351876";
    static string APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDFypCg0OHf"
                          + "BC+VZLSWPbNSgDo9qg/yQORDwGy1rKIboMj3IXn4Zy6h6bgn"
                          + "8kiMY7VI0lPwIj9lijT3ZxkzuTsI5GsK//Y1bqeTol4OUFR+"
                          + "47gj+TUuekAS2WMtglKox+/7mO6CA1gV+jZrAKo6YSVmPd+o"
                          + "FsgisRcqEgNh5MIURQIDAQAB";

    // Use this for initialization
    void Start () {
        Api.Init(InitStatusHandler, APP_ID);
        Viveport.Core.Logger.Log("Version: " + Api.Version());
        Viveport.Core.Logger.Log("UserId: " + User.GetUserId());
        Viveport.Core.Logger.Log("userName: " + User.GetUserName());
        Viveport.Core.Logger.Log("userAvatarUrl: " + User.GetUserAvatarUrl());
        Api.GetLicense(new MyLicenseChecker(), APP_ID, APP_KEY);
    }

    // Update is called once per frame
    void Update () {
    }

    void OnGUI()
    {
        // GUI.Label(new Rect(10, 30, Screen.width, Screen.height), "Version: " + Api.Version());
        // GUI.Label(new Rect(10, 50, Screen.width, Screen.height), "UserId: " + User.GetUserId());
        stringToEdit = GUI.TextField(new Rect(10, 10, 120, 20), stringToEdit, 50);
        StatsCount = GUI.TextField(new Rect(130, 10, 220, 20), StatsCount, 50);

        // GetSate function.

        if (GUI.Button(new Rect(nXStart, nYStart, nWidth, nHeight), "GetState"))
        {
            nResult = UserStats.GetStat(stringToEdit, nInitValue);
            Viveport.Core.Logger.Log("Get " + stringToEdit + " stat name as => " + nResult);
        }

        // SetSate function.

        if (GUI.Button(new Rect(nXStart + nWidth + 10, nYStart, nWidth, nHeight), "SetState"))
        {
            Viveport.Core.Logger.Log("MaxStep is => " + int.Parse(StatsCount));
            nResult = int.Parse(StatsCount);
            UserStats.SetStat(stringToEdit, nResult);
            Viveport.Core.Logger.Log("Set" + stringToEdit + " stat name as =>" + nResult);
        }

        // StoreSate function.
        if (GUI.Button(new Rect((nXStart + 2*(nWidth + 10)), nYStart, nWidth, nHeight), "UploadState"))
        {
            UserStats.UploadStats(UploadStatsHandler);
        }

        // Download function.
        if (GUI.Button(new Rect((nXStart + 3 * (nWidth + 10)), nYStart, nWidth, nHeight), "Download"))
        {
            UserStats.DownloadStats(DownloadStatsHandler);
        }

        // Init function.
        if (GUI.Button(new Rect((nXStart + 4 * (nWidth + 10)), nYStart, nWidth, nHeight), "Init"))
        {
            Api.Init(InitStatusHandler, APP_ID);
        }

        // Shutdown function.
        if (GUI.Button(new Rect((nXStart + 5 * (nWidth + 10)), nYStart, nWidth, nHeight), "Shutdown"))
        {
            Api.Shutdown(ShutdownHandler);
        }
        // IsReady function.
        if (GUI.Button(new Rect((nXStart + 6 * (nWidth + 10)), nYStart, nWidth, nHeight), "IsReady"))
        {
            UserStats.IsReady(IsReadyHandler);
        }
       /***************************************************************************/
        /*                          Achievement sample code                        */
        /***************************************************************************/

        achivToEdit = GUI.TextField(new Rect(10, nWidth + 10, 120, 20), achivToEdit, 50);

        if (GUI.Button(new Rect(nXStart, nYStart + nWidth + 10, nWidth, nHeight), "GetAchieve"))
        {
            bool bAchievement = false;
            bAchievement  = UserStats.GetAchievement(achivToEdit);
            Viveport.Core.Logger.Log("Get achievement => " + achivToEdit + " , and value is => " + bAchievement);
        }

        if (GUI.Button(new Rect(nXStart + nWidth + 10 , nYStart + nWidth + 10, nWidth, nHeight), "SetAchieve"))
        {
            UserStats.SetAchievement(achivToEdit);
            Viveport.Core.Logger.Log("Set achievement => " + achivToEdit);
        }

        if (GUI.Button(new Rect(nXStart + 2* (nWidth + 10), nYStart + nWidth + 10, nWidth, nHeight), "ClearAchi"))
        {
            UserStats.ClearAchievement(achivToEdit);
            Viveport.Core.Logger.Log("Clear achievement => " + achivToEdit);
        }

        if (GUI.Button(new Rect(nXStart + 3 * (nWidth + 10), nYStart + nWidth + 10, nWidth, nHeight), "Achi&Time"))
        {
            int nunlockTime = 0;
            nunlockTime = UserStats.GetAchievementUnlockTime(achivToEdit);
            Viveport.Core.Logger.Log("The achievement's unlock time is =>" + nunlockTime);
        }


        /***************************************************************************/
        /*                          Leaderboard sample code                        */
        /***************************************************************************/

        leaderboardToEdit = GUI.TextField(new Rect(10, 2 * nWidth + 20, 160, 20), leaderboardToEdit, 150);

        if (GUI.Button(new Rect(nXStart, nYStart + 2 * nWidth + 20, nWidth, nHeight), "DLBUser"))
        {
            UserStats.DownloadLeaderboardScores(DownloadLeaderboardHandler, leaderboardToEdit, UserStats.LeaderBoardRequestType.GlobalDataAroundUser, UserStats.LeaderBoardTimeRange.AllTime, -5, 5);
            Viveport.Core.Logger.Log("DownloadLeaderboardScores");
        }

        leaderboardScore = GUI.TextField(new Rect(10 + 160, 2 * nWidth + 20, 160, 20), leaderboardScore, 50);

        if (GUI.Button(new Rect(nXStart + nWidth + 10, nYStart + 2 * nWidth + 20, nWidth, nHeight), "UploadLB"))
        {
            UserStats.UploadLeaderboardScore(UploadLeaderboardScoreHandler, leaderboardToEdit, int.Parse(leaderboardScore));
            Viveport.Core.Logger.Log("UploadLeaderboardScore");
        }

        if (GUI.Button(new Rect(nXStart + 2 * (nWidth + 10), nYStart + 2 * nWidth + 20, nWidth, nHeight), "GCount"))
        {
            nResult = UserStats.GetLeaderboardScoreCount();
            Viveport.Core.Logger.Log("GetLeaderboardScoreCount=> " + nResult);
        }

        if (GUI.Button(new Rect(nXStart + 3 * (nWidth + 10), nYStart + 2 * nWidth + 20, nWidth, nHeight), "GSortMeth"))
        {
            int nResult = (int)UserStats.GetLeaderboardSortMethod();
            Viveport.Core.Logger.Log("GetLeaderboardSortMethod=> " + nResult);
        }

        if (GUI.Button(new Rect(nXStart + 4 * (nWidth + 10), nYStart + 2 * nWidth + 20, nWidth, nHeight), "GDispType"))
        {
            int nResult = (int)UserStats.GetLeaderboardDisplayType();
            Viveport.Core.Logger.Log("GetLeaderboardDisplayType=> " + nResult);
        }


        if (GUI.Button(new Rect(nXStart + 5 * (nWidth + 10), nYStart + 2 * nWidth + 20, nWidth, nHeight), "GetDownLB"))
        {
            int nResult = (int)UserStats.GetLeaderboardScoreCount();

            Viveport.Core.Logger.Log("GetLeaderboardScoreCount => " + nResult);

            for (int i = 0; i < nResult; i++)
            {
                Viveport.Leaderboard lbdata;
                lbdata = UserStats.GetLeaderboardScore(i);
                Viveport.Core.Logger.Log("UserName = " + lbdata.UserName + ", Score = " + lbdata.Score + ", Rank = " + lbdata.Rank);
            }

        }

        if (GUI.Button(new Rect(nXStart + 6 * (nWidth + 10), nYStart + 2 * nWidth + 20, nWidth, nHeight), "DLBRan"))
        {
            UserStats.DownloadLeaderboardScores(DownloadLeaderboardHandler, leaderboardToEdit, UserStats.LeaderBoardRequestType.GlobalData, UserStats.LeaderBoardTimeRange.AllTime, 0, 10);
            Viveport.Core.Logger.Log("DownloadLeaderboardScores");
        }
    }

    private static void InitStatusHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("InitStatusHandler is successful");
        else
            Viveport.Core.Logger.Log("IsReadyHandler error : " + nResult);
    }

    private static void IsReadyHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("IsReadyHandler is successful");
        else
            Viveport.Core.Logger.Log("IsReadyHandler error: " + nResult);
    }

    private static void ShutdownHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("ShutdownHandler is successful");
        else
            Viveport.Core.Logger.Log("ShutdownHandler error: " + nResult);
    }
    private static void DownloadStatsHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("DownloadStatsHandler is successful ");
        else
            Viveport.Core.Logger.Log("DownloadStatsHandler error: " + nResult);
    }

    private static void UploadStatsHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("UploadStatsHandler is successful");
        else
            Viveport.Core.Logger.Log("UploadStatsHandler error: " + nResult);
    }

    private static void DownloadLeaderboardHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("DownloadLeaderboardHandler is successful");
        else
            Viveport.Core.Logger.Log("DownloadLeaderboardHandler error: " + nResult);
    }

    private static void UploadLeaderboardScoreHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("UploadLeaderboardScoreHandler is successful.");
        else
            Viveport.Core.Logger.Log("UploadLeaderboardScoreHandler error : " + nResult);
    }

    class MyLicenseChecker : Api.LicenseChecker
    {
        public override void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired)
        {
            Viveport.Core.Logger.Log("[MyLicenseChecker] issueTime: " + issueTime);
            Viveport.Core.Logger.Log("[MyLicenseChecker] expirationTime: " + expirationTime);
            Viveport.Core.Logger.Log("[MyLicenseChecker] latestVersion: " + latestVersion);
            Viveport.Core.Logger.Log("[MyLicenseChecker] updateRequired: " + updateRequired);
        }

        public override void OnFailure(int errorCode, string errorMessage)
        {
            Viveport.Core.Logger.Log("[MyLicenseChecker] errorCode: " + errorCode);
            Viveport.Core.Logger.Log("[MyLicenseChecker] errorMessage: " + errorMessage);
        }
    }
}
