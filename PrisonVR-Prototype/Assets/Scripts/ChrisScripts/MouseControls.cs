using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
public class MouseControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (VRDevice.isPresent)
        {
            return;
        }
        else
        {
            transform.position = transform.position + Vector3.up * 2;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(VRDevice.isPresent)
        {
            return;
        }
        else
        {
            Camera.main.transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0);
        }
	}
}
