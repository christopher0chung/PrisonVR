using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour {

	AudioSource mySource;

	float vol;

	// Use this for initialization
	void Start () {

		mySource = GetComponent<AudioSource> ();
		vol = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {

		mySource.volume = Mathf.Lerp(mySource.volume, vol, .1f);

	}

	public void PassNewVolume (float v) {

		vol = Mathf.Lerp(vol, v, .1f);

	}

	public void ResetVolume() {
		
		vol = 1.0f;
		mySource.volume = 1.0f;

	}

	public void SetZeroVolume() {
		vol = 0f;
		mySource.volume = 0f;
	}


}
