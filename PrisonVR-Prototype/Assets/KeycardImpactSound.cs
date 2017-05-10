using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardImpactSound : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name != "Mattress") {
			PrisonAudioDirector.instance.Play3DSFX(
				PrisonAudioDirector.instance.plexiImpact[Random.Range(0, PrisonAudioDirector.instance.plexiImpact.Length)],
				transform.position,
				0f
				);
		}
	}
}
