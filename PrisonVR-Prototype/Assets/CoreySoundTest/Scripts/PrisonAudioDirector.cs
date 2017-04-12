
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;



public class PrisonAudioDirector : MonoBehaviour {

	public static PrisonAudioDirector instance = null;

	

	//POPULATE THIS
	public List<AudioClip> audioClipPool;
	public AudioClip[] concreteImpact;
	public AudioClip[] plexiImpact;
	public AudioClip steelImpact, steelDoorImpact, mattressImpact, waterImpact;

	[SerializeField] GameObject myPrefabSFX;

	[SerializeField] AudioSource myAudioSource;


	[SerializeField] AudioMixerGroup SFXGroup;



	//========================================================================
	//enforce singleton pattern



	void Awake () {
		
	
		if (instance == null) {

			instance = this;

		} else if (instance != this) {
			
			Destroy (gameObject);    

		}


		DontDestroyOnLoad(gameObject);

	}
	//========================================================================
	//SFX INSTANTIATION
	//========================================================================

	//right now have 2 overloads one for just playing an SFX clip, another for pitch ctrl
	//could make more overloads for PLaySFX in the future

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

	public void Play3DSFX(AudioClip g_SFX, Vector3 g_position, float g_pitchJitter) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.transform.position = g_position;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().pitch = (1f + (Random.value * g_pitchJitter)-(0.5f * g_pitchJitter));
		t_SFX.GetComponent<AudioSource> ().outputAudioMixerGroup = SFXGroup;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}


	void Start() {

	}

	void Update() {

	}
		




		

}
