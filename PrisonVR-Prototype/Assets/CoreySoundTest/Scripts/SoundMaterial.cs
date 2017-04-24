using System.Collections;
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

			colliderPrevPos = other.transform.position;
			other.GetComponent<SoundCollider> ().ResetVolume ();

			switch (thisMaterial) {
			case soundMaterial.Steel:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.steelImpact;
				nextClip = PrisonAudioDirector.instance.steelImpact;
				other.GetComponent<AudioSource> ().PlayOneShot (nextClip);
				//PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.steelImpact, other.gameObject.transform.position, 0.2f);
				break;
			case soundMaterial.SteelDoor:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.steelDoorImpact;
				nextClip = PrisonAudioDirector.instance.steelDoorImpact;
				other.GetComponent<AudioSource> ().PlayOneShot  (nextClip);
				break;
			case soundMaterial.Water:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.waterImpact;
				nextClip = PrisonAudioDirector.instance.waterImpact;
				other.GetComponent<AudioSource> ().PlayOneShot  (nextClip);
				break;
			case soundMaterial.Mattress:
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.mattressImpact;
				nextClip = PrisonAudioDirector.instance.steelDoorImpact;
				other.GetComponent<AudioSource> ().PlayOneShot  (nextClip);
				break;
			case soundMaterial.Plexi:
				int index = Random.Range (0, PrisonAudioDirector.instance.plexiImpact.Length);
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.plexiImpact[index];
				nextClip = PrisonAudioDirector.instance.plexiImpact[index];
				other.GetComponent<AudioSource> ().PlayOneShot  (nextClip);
				break;
			default: 
				index = Random.Range (0, PrisonAudioDirector.instance.concreteImpact.Length);
				//other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.concreteImpact[index];
				nextClip = PrisonAudioDirector.instance.concreteImpact [index];
				other.GetComponent<AudioSource> ().PlayOneShot (nextClip);
				other.GetComponent<SoundCollider> ().PassNewVolume (0f, 0.5f);
				break;

			}

			//soundCooldown = 0f;

		}
	}

	void OnTriggerStay ( Collider other) {
		if (other.gameObject.name == "SoundCollider") {

			colliderPos = other.transform.position;

			float colSpeed = (colliderPos - colliderPrevPos).sqrMagnitude;
			//Debug.Log (colSpeed);

			colliderPrevPos = colliderPos;

			AudioSource otherSource = other.gameObject.GetComponent<AudioSource> ();
			other.GetComponent<SoundCollider> ().PassNewVolume (Mathf.Clamp(colSpeed * 100f, 0f, 1f), 0.4f);

			otherSource.loop = true;

			scrapingTime += Time.fixedDeltaTime;

			if (scrapingTime >= scrapingStartTime && !otherSource.isPlaying) {

				switch (thisMaterial) {
				case soundMaterial.Steel:

					nextClip = PrisonAudioDirector.instance.steelImpact;
					otherSource.clip = nextClip;
					//otherSource.Play ();
			
					break;
				case soundMaterial.SteelDoor:
					nextClip = PrisonAudioDirector.instance.steelImpact;
					otherSource.clip = nextClip;
					//otherSource.Play ();
					break;
				case soundMaterial.Water:

					nextClip = PrisonAudioDirector.instance.waterImpact;
					otherSource.clip = nextClip;
					//otherSource.Play ();
					break;
				case soundMaterial.Mattress:
			
					nextClip = PrisonAudioDirector.instance.steelDoorImpact;
					otherSource.clip = nextClip;
					//otherSource.Play ();
					break;
				case soundMaterial.Plexi:
					int index = Random.Range (0, PrisonAudioDirector.instance.plexiImpact.Length);
			
					nextClip = PrisonAudioDirector.instance.plexiImpact [index];
					otherSource.clip = nextClip;	
					//otherSource.Play ();
					break;
				default: 

					nextClip = PrisonAudioDirector.instance.concreteScrape;
					otherSource.clip = nextClip;
					otherSource.Play ();
					break;

				}



			}
		}


	}

	void OnTriggerExit ( Collider other) {

		if (other.gameObject.name == "SoundCollider") {

			AudioSource otherSource = other.gameObject.GetComponent<AudioSource> ();

			otherSource.loop = false;
			otherSource.Stop ();
			//other.GetComponent<SoundCollider>().PassNewVolume(0f,0.6f);
		}

		scrapingTime = 0;


	}



}
