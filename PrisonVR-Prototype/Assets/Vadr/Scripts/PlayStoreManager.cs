using UnityEngine;
using System.Collections;
using System;
using VadrSdk;

public class PlayStoreManager : MonoBehaviour {

    GameObject[] allObjects;
    GameObject[] allObjectsVideo;
    GameObject pentagonStore;
	GameObject iOSWebviewGameObject;
    GameObject vadrManager;
    GameObject interstitialVideo;
    float timeScaleStore = -1.0f;
    float timeScaleInterstitial = -1.0f;
    bool playStore = false;
	bool iOSWebview = false;

    void Start()
    {
        vadrManager = gameObject;
    }

    void OnEnable()
    {
        CentralAdManager.VadrPlayStore += showPlayStore;
        VadrVideoCtrl.VadrAppStore += showPlayStore;
        CentralAdManager.VadrVideo += showInterstitialVideo;
        VadrVideoCtrl.VadrInterstitial += showGameVideo;
        RotateStore.VadrGame += showGame;
		UseVadrWebview.OpenAppStoreDelegate += showPlayStoreFromiOSWebview;
		UseVadrWebview.OpenGamefromWebview += showGameFromiOSWebview;
		CentralAdManager.OpeniOSWebview += openiOSWebview;
		VadrVideoCtrl.VadrVideoiOSWebview += openiOSWebview;
    }


    void OnDisable()
    {
        CentralAdManager.VadrPlayStore -= showPlayStore;
        CentralAdManager.VadrVideo -= showInterstitialVideo;
        VadrVideoCtrl.VadrAppStore -= showPlayStore;
        VadrVideoCtrl.VadrInterstitial -= showGameVideo;
        RotateStore.VadrGame -= showGame;
		UseVadrWebview.OpenGamefromWebview -= showGameFromiOSWebview;
		UseVadrWebview.OpenAppStoreDelegate -= showPlayStoreFromiOSWebview;
		CentralAdManager.OpeniOSWebview -= openiOSWebview;
		VadrVideoCtrl.VadrVideoiOSWebview -= openiOSWebview;
    }

	public void openiOSWebview(string url){
		if (!iOSWebview)
		{
			iOSWebview = true;
			if (allObjects == null)
				allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
			foreach (GameObject go in allObjects)
			{
				GameObject goParent = FindParent(go);
				if (goParent != vadrManager && goParent.name != "GvrViewerMain" && goParent.name != "VadrAdManager")
				{
					goParent.SetActive(false);
				}
			}
			timeScaleStore = Time.timeScale;
			Debug.Log(timeScaleStore);
			Time.timeScale = 0.0f;
			iOSWebviewGameObject = (GameObject)Instantiate(Resources.Load("VadrWebview"));
			//GvrViewer.AddStereoControllerToCameras();
			Debug.Log ("Webview Debug: THe name of webview created is: " + iOSWebviewGameObject.transform.name);
			iOSWebviewGameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			UseVadrWebview store = iOSWebviewGameObject.GetComponent<UseVadrWebview>();
			if (store != null)
			{
				store.Init(url);
			}
		}
	}

    public void showPlayStore(string url)
    {
        if (!playStore)
        {
            playStore = true;
            if (allObjects == null)
                allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                GameObject goParent = FindParent(go);
                if (goParent != vadrManager && goParent.name != "GvrViewerMain" && goParent.name != "VadrAdManager")
                {
                    goParent.SetActive(false);
                }
            }
            timeScaleStore = Time.timeScale;
            Debug.Log(timeScaleStore);
            Time.timeScale = 0.0f;
            pentagonStore = (GameObject)Instantiate(Resources.Load("PentagonStore"));
           // GvrViewer.AddStereoControllerToCameras();
            pentagonStore.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            PlayStore store = pentagonStore.GetComponent<PlayStore>();
            if (store != null)
            {
                store.initPlayStore(url);
            }
        }
    }

	public void showPlayStoreFromiOSWebview(string url)
	{
		Debug.Log ("Webview Debug: Command to playstore with url: " + url);
		if (!playStore)
		{
			Debug.Log ("Webview Debug: Within playstore with url: " + url);
			playStore = true;
			iOSWebview = false;
			UnityEngine.Object.Destroy(iOSWebviewGameObject);
			iOSWebviewGameObject = null;
			pentagonStore = (GameObject)Instantiate(Resources.Load("PentagonStore"));
			//GvrViewer.AddStereoControllerToCameras();
			pentagonStore.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			PlayStore store = pentagonStore.GetComponent<PlayStore>();
			if (store != null)
			{
				Debug.Log ("Webview Debug: Store script is not null");
				store.initPlayStore(url);
			}
		}
	}

    public void showGame()
    {
        if (timeScaleStore >= 0)
            Time.timeScale = timeScaleStore;
        else
            Time.timeScale = 1.0f;
        timeScaleStore = -1.0f;
        playStore = false;
		iOSWebview = false;
        foreach (GameObject go in allObjects)
        {
            GameObject goParent = FindParent(go);
            if (goParent.transform.name == "PentagonStore")
            {
                goParent.SetActive(false);
            }
            else {
                goParent.SetActive(true);
            }
        }
        UnityEngine.Object.Destroy(pentagonStore);
        pentagonStore = null;
        allObjects = null;
    }

	public void showGameFromiOSWebview()
	{
		if (timeScaleStore >= 0)
			Time.timeScale = timeScaleStore;
		else
			Time.timeScale = 1.0f;
		timeScaleStore = -1.0f;
		playStore = false;
		iOSWebview = false;
		foreach (GameObject go in allObjects)
		{
			GameObject goParent = FindParent(go);
			if (goParent.transform.name == "VadrWebview")
			{
				goParent.SetActive(false);
			}
			else {
				goParent.SetActive(true);
			}
		}
		UnityEngine.Object.Destroy(iOSWebviewGameObject);
		iOSWebviewGameObject = null;
		allObjects = null;
	}

	public void showInterstitialVideo()
    {
        if (allObjectsVideo == null)
            allObjectsVideo = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjectsVideo)
        {
            GameObject goParent = FindParent(go);
            if (goParent != vadrManager && goParent.name != "GvrViewerMain" && goParent.name != "VadrAdManager")
            {
                goParent.SetActive(false);
            }
        }
        timeScaleInterstitial = Time.timeScale;
        Time.timeScale = 0.0f;
        interstitialVideo = (GameObject)Instantiate(Resources.Load("InterstitialVideo"));
        //GvrViewer.AddStereoControllerToCameras();
        interstitialVideo.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        //VadrVideoCtrl video = interstitialVideo.transform.Find("VideoAd").GetComponent<VadrVideoCtrl>();
        //video.init();
    }

    public void showGameVideo()
    {
        Debug.Log("Showing Game");
        if (timeScaleInterstitial > 0) {
            Time.timeScale = timeScaleInterstitial;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        timeScaleInterstitial = -1.0f;
        playStore = false;
        foreach (GameObject go in allObjectsVideo)
        {
            GameObject goParent = FindParent(go);
            if (goParent.transform.name == "InterstitialVideo")
            {
                goParent.SetActive(false);
            }
            else {
                goParent.SetActive(true);
            }
        }
        UnityEngine.Object.Destroy(interstitialVideo);
        interstitialVideo = null;
        allObjectsVideo = null;
    }

    GameObject FindParent(GameObject go)
    {
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
        }
        return go;
    }
}
