using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FaucetSound : MonoBehaviour {

	[SerializeField] ButtonPressPercent myBPP; 
	[SerializeField] AudioClip onSound, offSound, onLoop;

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

	public Advertisement adScript;


	// Use this for initialization
	void Start () {

		myBPP = GameObject.Find ("Toilet_Sink/Faucet/faucet_button").GetComponent<ButtonPressPercent> ();
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
		
	}

	void StartSink() {
		myAudioSources [0].Play ();
		myAudioSources [1].PlayDelayed (0.2f);
		myAudioSources [1].loop = true;
		adScript.PlayAd ();
	}
	void StopSink() {
		myAudioSources [2].Play ();
		myAudioSources [1].Stop ();
	}
}
