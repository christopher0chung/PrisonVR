using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRBMove : MonoBehaviour {

	public Rigidbody rb;
	Vector3 forceDirection;
	public float force;

	void Start () {
		
	}
	
	void Update () {
		forceDirection.y = -(BoolToInt(Input.GetKey(KeyCode.K)));
	}

	void FixedUpdate () {
		rb.AddForce(forceDirection * force);
	}

	int BoolToInt (bool b) {
		if (b) return 1;
		else return 0;
	}
}
