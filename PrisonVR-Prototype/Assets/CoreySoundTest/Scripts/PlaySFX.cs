using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour {

	public AudioClip[] sfxClips;
	AudioSource thisSource;

	public float timeInterval;
	public float timeJitter;

	float coolDown;

	public float pitchJitter;


	// Use this for initialization
	void Start () {

		coolDown = 0;
		thisSource = gameObject.GetComponent<AudioSource> ();
		coolDown += Random.value * timeJitter - (0.5f * timeJitter);

	}
	
	// Update is called once per frame
	void Update () {
		coolDown += Time.deltaTime;

		if (coolDown >= timeInterval) {
			thisSource.clip = sfxClips [Random.Range (0, sfxClips.Length)];
			thisSource.pitch = 1.0f + (Random.value * pitchJitter);
			thisSource.Play ();
			coolDown = Random.value * timeJitter - (0.5f * timeJitter);
		}
			
	}
}
