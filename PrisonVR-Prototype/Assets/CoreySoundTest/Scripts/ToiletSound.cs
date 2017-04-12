using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToiletSound : MonoBehaviour {

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
					StartFlush ();
				}
				else
				{
					StopFlush ();
				}
			}
		}
	}

	AudioSource[] myAudioSources;

	[SerializeField] Advertisement adScript;


	// Use this for initialization
	void Start () {

		adScript = GameObject.Find ("TempAdScreen").GetComponent<Advertisement> ();

		myBPP = GameObject.Find ("Toilet_Sink(ReqCollOnHands)/toilet_button").GetComponent<ButtonPressPercent> ();
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

	void StartFlush() {
		myAudioSources [0].Play ();
		myAudioSources [1].PlayDelayed (0.2f);
		myAudioSources [1].loop = true;
		adScript.PlayAd ();
	}
	void StopFlush() {
		myAudioSources [2].Play ();
		myAudioSources [1].Stop ();
	}
}
