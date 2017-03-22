using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressPercent : MonoBehaviour {

	public Vector3 actuationDirection;
	public Vector3 fullyEngagedButtonPosition;
	Vector3 startPosition;
	float maxDistance;

	public float percentActuated = 0;
	
	void Start () {
		startPosition = transform.localPosition;
		maxDistance = Vector3.Distance(MultiplyVectorAxisAbs(startPosition, actuationDirection), MultiplyVectorAxisAbs(fullyEngagedButtonPosition, actuationDirection));
		maxDistance -= GetSingleAxisPosition(transform.localScale, actuationDirection) * 0.5f;
	}

	void Update () {
		percentActuated = GetPercentActuated();
	}

	float GetPercentActuated () {
		float currentDistance = Vector3.Distance(MultiplyVectorAxisAbs(transform.localPosition, actuationDirection), MultiplyVectorAxisAbs(fullyEngagedButtonPosition, actuationDirection));
		return Mathf.Clamp01(currentDistance / maxDistance);
	}

	Vector3 MultiplyVectorAxisAbs (Vector3 a, Vector3 b) {
		return new Vector3 (a.x * Mathf.Abs(b.x), a.y * Mathf.Abs(b.y), a.z * Mathf.Abs(b.z));
	}

	float GetSingleAxisPosition (Vector3 position, Vector3 axisToGet) {
	Vector3 result = MultiplyVectorAxisAbs(position, axisToGet.normalized);
	if (result.x != 0) {
		return result.x;
	} else if (result.y != 0) {
		return result.y;
	} else if (result.z != 0) {
		return result.z;
	} else {
		return 0;
	}
	}
}
