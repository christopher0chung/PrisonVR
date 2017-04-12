using System;
using UnityEngine;
using Viveport;


public class ViveportDemo_IAP : MonoBehaviour
{
    private int nWidth = 80, nHeight = 40;
    private int nXStart = 10, nYStart = 35;

    static string IAP_APP_TEST_ID = "app_test_id";//replace it with your app id
    static string IAP_APP_TEST_KEY = "app_test_key";//replace it with your app key
    private Result mListener;

    // Use this for initialization
    void Start()
    {
        mListener = new Result();
        Api.Init(InitStatusHandler, IAP_APP_TEST_ID);
        Viveport.Core.Logger.Log("Version: " + Api.Version());
        Viveport.Core.Logger.Log("UserId: " + User.GetUserId());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //******************************************************
        //*                  IAP sample code
        //*****************************************************

        if (GUI.Button(new Rect(nXStart, nYStart, nWidth, nHeight), "IsReady"))
        {
            Viveport.Core.Logger.Log("IsReady");
            IAPurchase.IsReady(mListener, IAP_APP_TEST_KEY);
        }

        if (GUI.Button(new Rect(nXStart, nYStart + 1 * nWidth + 10, nWidth, nHeight), "Request"))
        {
            Viveport.Core.Logger.Log("Request");
            //add virtual items into cache
            mListener.mItem.items = new string[3];
            mListener.mItem.items[0] = "sword";
            mListener.mItem.items[1] = "knife";
            mListener.mItem.items[2] = "medicine";
            IAPurchase.Request(mListener, "100");
        }

        if (GUI.Button(new Rect(nXStart, nYStart + 2 * nWidth + 20, nWidth, nHeight), "Purchase"))
        {
            Viveport.Core.Logger.Log("Purchase mListener.mItem.ticket=" + mListener.mItem.ticket);
            IAPurchase.Purchase(mListener, mListener.mItem.ticket);
        }

        if (GUI.Button(new Rect(nXStart, nYStart + 3 * nWidth + 30, nWidth, nHeight), "Query"))
        {
            Viveport.Core.Logger.Log("Query");
            IAPurchase.Query(mListener, mListener.mItem.ticket);
        }

        if (GUI.Button(new Rect(nXStart, nYStart + 4 * nWidth + 40, nWidth, nHeight), "GetBalance"))
        {
            Viveport.Core.Logger.Log("GetBalance");
            IAPurchase.GetBalance(mListener);
        }
    }

    private static void InitStatusHandler(int nResult)
    {
        Viveport.Core.Logger.Log("InitStatusHandler: " + nResult);
    }
    //a sample class which store purchase id and puchased items
    public class Item
    {
        public string ticket = "test_id";
        public string[] items;
    }
    //Declare class which extends IAPurchase.IAPurchaseListener and implement callback to get the response of APIs
    //You can make this class for your own customization, for the example here, we store necessary purchase information into cache
    //You can store it in db or use any other cache mechanism
    class Result : IAPurchase.IAPurchaseListener
    {
        public Item mItem = new Item();
        public override void OnSuccess(string pchCurrencyName)
        {
            Viveport.Core.Logger.Log("[OnSuccess] pchCurrencyName=" + pchCurrencyName);
        }

        public override void OnRequestSuccess(string pchPurchaseId)
        {
            mItem.ticket = pchPurchaseId;
            Viveport.Core.Logger.Log("[OnRequestSuccess] pchPurchaseId=" + pchPurchaseId + ",mItem.ticket=" + mItem.ticket);
        }

        public override void OnPurchaseSuccess(string pchPurchaseId)
        {
            Viveport.Core.Logger.Log("[OnPurchaseSuccess] pchPurchaseId=" + pchPurchaseId);
            if (mItem.ticket == pchPurchaseId)//if stored id equals the purchase id which is returned by OnPurchaseSuccess(), give the virtual items to user
            {
                Viveport.Core.Logger.Log("[OnPurchaseSuccess] give items to user");
                //to implement: give virtual items to user
            }
        }

        public override void OnQuerySuccess(IAPurchase.QueryResponse response)
        {
            //when status equals "success", then this purchase is valid, you can deliver virtual items to user
            Viveport.Core.Logger.Log("[OnQuerySuccess] purchaseId=" + response.purchase_id + ",status=" + response.status);
        }
        public override void OnBalanceSuccess(string pchBalance)
        {
            Viveport.Core.Logger.Log("[OnBalanceSuccess] pchBalance=" + pchBalance);
        }

        public override void OnFailure(int nCode, string pchMessage)
        {
            Viveport.Core.Logger.Log("[OnFailed] " + nCode + ", " + pchMessage);
        }
    }

}
