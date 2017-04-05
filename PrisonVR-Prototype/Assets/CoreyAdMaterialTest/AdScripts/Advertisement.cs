using UnityEngine;
using System.Collections;

public class Advertisement: MonoBehaviour {
	public MovieTexture movTexture;
	public AudioClip movieAudio;

	AudioSource movieSource;

	void Start() {
		
		GetComponent<Renderer>().material.mainTexture = movTexture;
		GetComponent<AudioSource> ();

	}

	public void PlayAd () { 

		movTexture.Play();
		movieSource.Play();

	}
}