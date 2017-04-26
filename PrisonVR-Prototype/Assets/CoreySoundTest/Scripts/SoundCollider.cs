using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour {

	AudioSource[] mySources;
	public AudioSource attackSource, sustainSource, releaseSource;

	float vol;

	float smoothing = 0.1f;

	// Use this for initialization
	void Start () {

		mySources = GetComponents<AudioSource> ();
		attackSource = mySources [0];
		sustainSource = mySources [1];
		releaseSource = mySources [2];
		vol = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {

		sustainSource.volume = Mathf.Lerp(sustainSource.volume, vol, smoothing);

	}

	public void PassNewVolume (float v, float t) {

		vol = Mathf.Lerp(vol, v, t);
		smoothing = t*2f;

	}

	public void ResetVolume() {
		
		vol = 1.0f;
		sustainSource.volume = 1.0f;

	}




}
