using UnityEngine;
using System.Collections;
using VadrSdk;

[ExecuteInEditMode]
public class NativePrefabManager : MonoBehaviour {

    // Update is called once per frame
#if UNITY_EDITOR
    void Update () {
        FetchNative native = transform.GetComponent<VadrSdk.FetchNative>();
        if(native != null)
            native.UpdateNativePrefab();
	}
#endif
}

