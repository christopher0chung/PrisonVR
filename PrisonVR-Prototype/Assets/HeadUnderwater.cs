using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUnderwater : MonoBehaviour {

	AudioLowPassFilter lowPass;

	// Use this for initialization
	void Start () {

		lowPass = transform.parent.GetComponent<AudioLowPassFilter> ();
		lowPass.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "WaterTrigger") {
			lowPass.enabled = true;
			lowPass.cutoffFrequency = 500f;
			lowPass.lowpassResonanceQ = 1.2f;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.name == "WaterTrigger") {
			lowPass.enabled = false;
		}
	}
}
