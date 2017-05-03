using UnityEngine;
using System.Collections;

public class Advertisement: MonoBehaviour {
	public MovieTexture[] movieTextures;
	public AudioClip[] movieAudio;

	MovieTexture currentMovie;

	AudioSource movieSource;
	float playTime;

	int index;

	void Awake() {
		
		GetComponent<Renderer>().material.SetTexture("_emissiveTexture", movieTextures[0]);
		movieSource = GetComponent<AudioSource>();
		movieSource.clip = movieAudio[0];
		currentMovie = movieTextures [0];
	
		index = 0;

		PlayAd ();
	}

	void Update() {

		if (movieSource.isPlaying) {
			//Debug.Log ("isPLaying");
		}
		
	}

	public void PlayAd () { 
		//Debug.Log ("Play Ad");

		//this code is annoying; movie textures are apparently really finnicky about starting/stopping
		if (movieSource) {
			movieSource.Stop ();
		}
		if (currentMovie) {
			currentMovie.Stop ();
		}
		GetComponent<Renderer>().material.SetTexture("_emissiveTexture", movieTextures[(index + 1) % movieTextures.Length]);
		currentMovie = movieTextures [(index + 1) % movieTextures.Length];
		movieSource.clip = movieAudio[(index+1) % movieAudio.Length];
		movieSource.Play();
		movieTextures[(index + 1) % movieTextures.Length].Play();

		index++;
	}
}