using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VadrSdk;

public class CloseVideo : MonoBehaviour
{

    // Use this for initialization
    Camera cameraMain;
    float time;
    VadrVideoCtrl media;
    void Start()
    {
        cameraMain = Camera.main;
        time = Time.realtimeSinceStartup;
        StartCoroutine(Back());
        GameObject videoAd = GameObject.Find("VideoAd");
        media = videoAd.GetComponent<VadrVideoCtrl>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Back()
    {
        while (true)
        {
            close();
            float timeNow = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - timeNow < 0.5f)
            {
                yield return 0;
            }
        }
    }
    void close()
    {
        RaycastHit objectHit;
        Vector3 cameraPosition = cameraMain.transform.position;
        if (Physics.Raycast(cameraPosition, cameraMain.transform.forward, out objectHit) && objectHit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID())
        {
            if ((Time.realtimeSinceStartup - time) > 1)
            {
                media.Skip();
                StartCoroutine(media.LoadNewScene());
            }
        }
        else
        {
            time = Time.realtimeSinceStartup;
        }
    }
}
