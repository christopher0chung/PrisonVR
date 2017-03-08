using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class CS_PlaySFX : MonoBehaviour {
	[SerializeField] AudioClip[] mySFX;
	[SerializeField] GameObject myPrefabSFX;
	[SerializeField] AudioMixerGroup SFXGroup;

	[SerializeField] bool playOnStart;
	[SerializeField] bool playRandomly;
	[SerializeField] bool playOnce;

	[SerializeField] float playVolume;
	// Use this for initialization
	void Start () {
		if (playOnStart) {
			if (playRandomly)
				PlaySFX (mySFX[Random.Range (0, mySFX.Length)]);
			else
				PlaySFX (mySFX[0]);
		}
		if (playOnce) {
			Destroy (this);
		}
	}

	public void PlaySFX (AudioClip g_SFX) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().outputAudioMixerGroup = SFXGroup;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlaySFX (AudioClip g_SFX, float g_Pitch) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().pitch = g_Pitch;
		t_SFX.GetComponent<AudioSource> ().outputAudioMixerGroup = SFXGroup;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}
}
