using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSounds : MonoBehaviour {

	[SerializeField] AudioClip[] PageSounds;
	[SerializeField] AudioClip[] BindingSounds;
	[SerializeField] AudioClip[] ImpactSounds;
	[SerializeField] AudioClip closeBookSound;


	public float pitchJitter = 0.1f;

	//May not need this later
	public float sfxCooldown = 0.5f;


	void Start () {
		

		GameObject bookObject = GameObject.Find ("Book2");

		transform.position = bookObject.transform.position;
		transform.SetParent (bookObject.transform);
	}
	


	public void OpenBook() {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter; 
		PrisonAudioDirector.instance.Play3DSFX(BindingSounds[Random.Range(0,BindingSounds.Length)], transform.position, jitter);
	}

	public void CloseBook() {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter; 
		PrisonAudioDirector.instance.Play3DSFX(closeBookSound, transform.position, jitter);
	}

	public void TurnPage() {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter;
		PrisonAudioDirector.instance.Play3DSFX(PageSounds[Random.Range(0,PageSounds.Length)], transform.position, jitter);
	}

	public void BookDrop () {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter; 

		PrisonAudioDirector.instance.Play3DSFX (ImpactSounds [0], transform.position, jitter);
	}

}
