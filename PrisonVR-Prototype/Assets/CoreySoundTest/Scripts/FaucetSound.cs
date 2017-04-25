using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FaucetSound : MonoBehaviour {

	[SerializeField] ButtonPressPercent myBPP; 
	[SerializeField] AudioClip onSound, offSound, onLoop;

	double attackDSPTime;
	double prevSustainSamples;

	int loopCount;

	private bool _on;
	private bool on
	{
		get
		{
			return _on;
		}
		set
		{
			if (value != _on)
			{
				_on = value;
				if (_on)
				{
					StartSink ();
				}
				else
				{
					StopSink ();
				}
			}
		}
	}
	AudioSource[] myAudioSources;

	[SerializeField] Advertisement adScript;


	// Use this for initialization
	void Start () {

		//startDSPTime = AudioSettings.dspTime;

		adScript = GameObject.Find ("TempAdScreen").GetComponent<Advertisement> ();

		myBPP = GameObject.FindGameObjectWithTag("FaucetButton").GetComponent<ButtonPressPercent> ();
		myAudioSources = GetComponents<AudioSource> ();
		myAudioSources [0].clip = onSound;
		myAudioSources [1].clip = onLoop;
		myAudioSources [2].clip = offSound;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (myBPP.percentActuated >= .05f)
		{
			on = true;
		}
		else
		{
			on = false;
		}

		if (myAudioSources [1].isPlaying) {
			CountSustainLoop ();
		}
		
	}

	void StartSink() {

		attackDSPTime = AudioSettings.dspTime;

		myAudioSources [0].Play ();
		myAudioSources [1].PlayScheduled (onSound.length + attackDSPTime);
		myAudioSources [1].loop = true;
		adScript.PlayAd ();
	}
	void StopSink() {
		myAudioSources [2].PlayScheduled ();
		myAudioSources [1].SetScheduledEndTime ();
	}

	void CountSustainLoop() {

		myAudioSources[1].loop = true;
		if (prevSustainSamples > myAudioSources[1].timeSamples) {
			loopCount++;
		}
		prevSustainSamples = myAudioSources[1].timeSamples;

	}
}
