using UnityEngine;
using System.Collections;
using VadrSdk;

[ExecuteInEditMode]
public class AdEditor : MonoBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        AdFetch ad = transform.GetComponent<AdFetch>();
        if(ad != null)
        {
            ad.AdjustAdType();
            ad.AdjustAdPosition();
        }
    }
#endif
}