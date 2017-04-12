using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundMaterial {Concrete = 1, Steel = 2, SteelDoor = 3, Water = 4, Mattress = 5, Plexi = 6};

public class SoundMaterial : MonoBehaviour {

	public soundMaterial thisMaterial;

	float soundCooldown = 0.0f;
	float cooldownTime = 0.5f;

	void Start () {


		
	}

	void Update () {
		soundCooldown += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "SoundCollider" && soundCooldown > cooldownTime) {
			switch (thisMaterial) {
				case soundMaterial.Steel:
					PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.steelImpact, other.gameObject.transform.position, 0.2f);
					break;
				case soundMaterial.SteelDoor:
					PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.steelDoorImpact, other.gameObject.transform.position, 0.2f);
					break;
				case soundMaterial.Water:
					PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.waterImpact, other.gameObject.transform.position, 0.2f);
					break;
				case soundMaterial.Mattress:
					PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.mattressImpact, other.gameObject.transform.position, 0.2f);
					break;
				case soundMaterial.Plexi:
					int index = Random.Range (0, PrisonAudioDirector.instance.plexiImpact.Length);
					PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.plexiImpact[index], other.gameObject.transform.position, 0.2f);
					break;
				default: 
					index = Random.Range (0, PrisonAudioDirector.instance.concreteImpact.Length);
					PrisonAudioDirector.instance.Play3DSFX (PrisonAudioDirector.instance.concreteImpact[index], other.gameObject.transform.position, 0.2f);
					break;

			}

			soundCooldown = 0f;

		}
	}



}
