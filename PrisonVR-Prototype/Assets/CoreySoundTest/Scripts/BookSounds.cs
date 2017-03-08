using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSounds : MonoBehaviour {

	CS_PlaySFX sfxPlayer;

	[SerializeField] AudioClip[] PageSounds;
	[SerializeField] AudioClip[] BindingSounds;
	[SerializeField] AudioClip[] ImpactSounds;
	[SerializeField] AudioClip closeBookSound;


	public float pitchJitter = 0.1f;

	//May not need this later
	public float sfxCooldown = 0.5f;


	void Start () {
		sfxPlayer = gameObject.GetComponent<CS_PlaySFX> ();

		GameObject bookObject = GameObject.Find ("Book2");

		transform.position = bookObject.transform.position;
		transform.SetParent (bookObject.transform);
	}
	


	public void OpenBook() {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter; 
		sfxPlayer.PlaySFX(BindingSounds[Random.Range(0,BindingSounds.Length)], jitter, transform.position);
	}

	public void CloseBook() {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter; 
		sfxPlayer.PlaySFX (closeBookSound, jitter, transform.position);
	}

	public void TurnPage() {
		float jitter = pitchJitter * Random.value - 0.5f * pitchJitter;
		sfxPlayer.PlaySFX(PageSounds[Random.Range(0,PageSounds.Length)], jitter, transform.position);
	}

}
