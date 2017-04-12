using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundMaterial {Concrete = 1, Steel = 2, SteelDoor = 3, Water = 4, Mattress = 5, Plexi = 6};

public class SoundMaterial : MonoBehaviour {

	public soundMaterial thisMaterial;

	float soundCooldown = 0.0f;
	float scrapingStartTime = 0.3f;
	float scrapingTime = 0.0f;
	float cooldownTime = 0.25f;

	void Start () {


		
	}

	void Update () {
		soundCooldown += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "SoundCollider" && soundCooldown > cooldownTime) {
			switch (thisMaterial) {
				case soundMaterial.Steel:
					other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.steelImpact;
					other.GetComponent<AudioSource> ().Play ();
					//PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.steelImpact, other.gameObject.transform.position, 0.2f);
					break;
				case soundMaterial.SteelDoor:
					other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.steelDoorImpact;
					other.GetComponent<AudioSource> ().Play ();
					break;
				case soundMaterial.Water:
					other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.waterImpact;
					other.GetComponent<AudioSource> ().Play ();
					break;
				case soundMaterial.Mattress:
					other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.mattressImpact;
					other.GetComponent<AudioSource> ().Play ();
					break;
				case soundMaterial.Plexi:
					int index = Random.Range (0, PrisonAudioDirector.instance.plexiImpact.Length);
					other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.plexiImpact[index];
					other.GetComponent<AudioSource> ().Play ();
					break;
				default: 
					index = Random.Range (0, PrisonAudioDirector.instance.concreteImpact.Length);
					other.GetComponent<AudioSource> ().clip = PrisonAudioDirector.instance.concreteImpact[index];
					other.GetComponent<AudioSource> ().Play ();
					break;

			}

			soundCooldown = 0f;

		}
	}

	void OnTriggerStay ( Collider other) {
		scrapingTime += Time.fixedDeltaTime;
		if (scrapingTime >= scrapingStartTime) {

		}
	}

	void OnTriggerExit ( Collider other) {
		scrapingTime = 0;
	}



}
