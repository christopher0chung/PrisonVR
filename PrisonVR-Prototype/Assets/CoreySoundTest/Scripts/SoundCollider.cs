using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour {

	AudioSource mySource;

	float vol;

	float smoothing = 0.1f;

	// Use this for initialization
	void Start () {

		mySource = GetComponent<AudioSource> ();
		vol = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {

		mySource.volume = Mathf.Lerp(mySource.volume, vol, smoothing);

	}

	public void PassNewVolume (float v, float t) {

		vol = Mathf.Lerp(vol, v, t);
		smoothing = t;

	}

	public void ResetVolume() {
		
		vol = 1.0f;
		mySource.volume = 1.0f;

	}




}
