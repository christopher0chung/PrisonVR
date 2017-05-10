using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

	public float speed;
	bool startOpen = false;

	void Start () {
		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.O)) {
			startOpen = true;
			PrisonAudioDirector.instance.Play3DSFX(PrisonAudioDirector
		}

		if (startOpen) MoveDoor();
	}

	void MoveDoor () {
		transform.position += Vector3.right * speed * Time.deltaTime;
	}
}
