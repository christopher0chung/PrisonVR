﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundMaterial {Concrete = 1, Steel = 2, SteelDoor = 3, Water = 4, Mattress = 5, Plexi = 6};

public class SoundMaterial : MonoBehaviour {

	public soundMaterial thisMaterial;

	AudioClip nextClip;

	Vector3 colliderPos, colliderPrevPos;

	float soundCooldown = 0.0f;
	float scrapingStartTime = 0.1f;
	float scrapingTime = 0.0f;
	//float cooldownTime = 0.25f;

	void Start () {

		
	}

	void Update () {
		soundCooldown += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "SoundCollider") {

			SoundCollider s_coll = other.GetComponent<SoundCollider> ();


			colliderPrevPos = other.transform.position;
			s_coll.ResetVolume ();

			switch (thisMaterial) {
			case soundMaterial.Steel:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.steelImpact;
				nextClip = PrisonAudioDirector.instance.steelImpact;
				s_coll.attackSource.PlayOneShot (nextClip);
				//PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.steelImpact, other.gameObject.transform.position, 0.2f);
				break;
			case soundMaterial.SteelDoor:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.steelDoorImpact;
				nextClip = PrisonAudioDirector.instance.steelDoorImpact;
				s_coll.attackSource.PlayOneShot(nextClip);
				break;
			case soundMaterial.Water:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.waterImpact;
				nextClip = PrisonAudioDirector.instance.waterImpact;
				s_coll.attackSource.PlayOneShot (nextClip);
				break;
			case soundMaterial.Mattress:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.mattressImpact;
				nextClip = PrisonAudioDirector.instance.steelDoorImpact;
				s_coll.attackSource.PlayOneShot  (nextClip);
				break;
			case soundMaterial.Plexi:
				int index = Random.Range (0, PrisonAudioDirector.instance.plexiImpact.Length);
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.plexiImpact[index];
				nextClip = PrisonAudioDirector.instance.plexiImpact[index];
				s_coll.attackSource.PlayOneShot  (nextClip);
				break;
			default: 
				index = Random.Range (0, PrisonAudioDirector.instance.concreteImpact.Length);
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.concreteImpact[index];
				nextClip = PrisonAudioDirector.instance.concreteImpact [index];
				s_coll.attackSource.PlayOneShot (nextClip);
				other.GetComponent<SoundCollider> ().PassNewVolume (0f, 0.5f);
				break;

			}

			//soundCooldown = 0f;

		}
	}

	void OnTriggerStay ( Collider other) {
		if (other.gameObject.name == "SoundCollider") {

			SoundCollider s_coll = other.GetComponent<SoundCollider> ();

			colliderPos = other.transform.position;

			float colSpeed = (colliderPos - colliderPrevPos).sqrMagnitude;
			//Debug.Log (colSpeed);

			colliderPrevPos = colliderPos;


			s_coll.PassNewVolume (Mathf.Clamp(colSpeed * 100f, 0f, 1f), 0.4f);

			s_coll.sustainSource.loop = true;

			scrapingTime += Time.fixedDeltaTime;

			if (scrapingTime >= scrapingStartTime && !s_coll.sustainSource.isPlaying) {

				switch (thisMaterial) {
				case soundMaterial.Steel:

					nextClip = PrisonAudioDirector.instance.steelImpact;
					s_coll.sustainSource.clip = nextClip;
					//otherSource.Play ();
			
					break;
				case soundMaterial.SteelDoor:
					nextClip = PrisonAudioDirector.instance.steelImpact;
					s_coll.sustainSource.clip = nextClip;
					//otherSource.Play ();
					break;
				case soundMaterial.Water:

					nextClip = PrisonAudioDirector.instance.waterImpact;
					s_coll.sustainSource.clip = nextClip;
					//otherSource.Play ();
					break;
				case soundMaterial.Mattress:
			
					nextClip = PrisonAudioDirector.instance.steelDoorImpact;
					s_coll.sustainSource.clip = nextClip;
					//otherSource.Play ();
					break;
				case soundMaterial.Plexi:
					int index = Random.Range (0, PrisonAudioDirector.instance.plexiImpact.Length);
			
					nextClip = PrisonAudioDirector.instance.plexiImpact [index];
					s_coll.sustainSource.clip = nextClip;
					//otherSource.Play ();
					break;
				default: 

					nextClip = PrisonAudioDirector.instance.concreteScrape;
					s_coll.sustainSource.clip = nextClip;
					s_coll.sustainSource.Play ();
					break;

				}



			}
		}


	}

	void OnTriggerExit ( Collider other) {

		if (other.gameObject.name == "SoundCollider") {

			SoundCollider s_coll = other.GetComponent<SoundCollider> ();


			s_coll.sustainSource.loop = false;
			s_coll.sustainSource.Stop ();
			//other.GetComponent<SoundCollider>().PassNewVolume(0f,0.6f);
		}

		scrapingTime = 0;


	}

	void StartSound() {

	}

	void SustainSound() {

	}

	void StopSound() {

	}



}
