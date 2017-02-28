using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class TriggerMixerSnapshot : MonoBehaviour {

	public AudioMixerSnapshot targetSnapshot;
	public float fadeTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "HeadCollider") { 
			targetSnapshot.TransitionTo (fadeTime);
		}
	}
}
