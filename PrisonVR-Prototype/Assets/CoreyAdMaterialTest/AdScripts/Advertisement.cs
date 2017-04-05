using UnityEngine;
using System.Collections;

public class Advertisement: MonoBehaviour {
	public MovieTexture[] movieTextures;
	public AudioClip[] movieAudio;

	MovieTexture currentMovie;

	AudioSource movieSource;

	float playTime;

	int index;

	void Start() {
		
		GetComponent<Renderer>().material.mainTexture = movieTextures[0];
		movieSource = GetComponent<AudioSource> ();
		movieSource.clip = movieAudio[0];
		currentMovie = movieTextures [0];
	
		index = 0;
	}

	void Update() {

		if (movieSource.isPlaying) {
			//Debug.Log ("isPLaying");
		}
		
	}

	public void PlayAd () { 
		Debug.Log ("Play Ad");

		//this code is annoying; movie textures are apparently really finnicky about starting/stopping

		movieSource.Stop();
		currentMovie.Stop();
		GetComponent<Renderer>().material.mainTexture = movieTextures[(index + 1) % movieTextures.Length];
		currentMovie = movieTextures [(index + 1) % movieTextures.Length];
		movieSource.clip = movieAudio[(index+1) % movieAudio.Length];
		movieSource.Play();
		movieTextures[(index + 1) % movieTextures.Length].Play();

		index++;
	}
}