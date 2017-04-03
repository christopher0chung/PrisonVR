using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressPercent : MonoBehaviour {

    [SerializeField]
    float speed;

	//public Vector3 actuationDirection;
	//public Vector3 fullyEngagedButtonPosition;
	Vector3 startPosition;
	[SerializeField] float maxDistance;

    // if inContact is being turned true, it locks down free to move.
    // inContact can be switched back to false independently of free to move based on timer.
    private bool _inContact;
    private bool inContact
    {
        get
        {
            return _inContact;
        }
        set
        {
            _inContact = value;
            contactTimer = 0;
        }
    }

    private float contactTimer;

    public float percentActuated = 0;
	
	void Start () {
		startPosition = transform.localPosition;
		//maxDistance = Vector3.Distance(MultiplyVectorAxisAbs(startPosition, actuationDirection), MultiplyVectorAxisAbs(fullyEngagedButtonPosition, actuationDirection));

        //Why?
		//maxDistance -= GetSingleAxisPosition(transform.localScale, actuationDirection) * 0.5f;
	}

	void Update () {
		//percentActuated = GetPercentActuated();
        if (!inContact)
            MoveTowardsMax();
	}

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Hand")
            inContact = true;

        ContactTimer();
        ThrowPercent();
    }

    void MoveTowardsMax()
    {
        GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, transform.root.position + Vector3.up * maxDistance, speed * Time.deltaTime));
    }

    void ContactTimer()
    {
        contactTimer += Time.deltaTime;
        if (contactTimer >= .25f)
        {
            inContact = false;
        }
    }

    void ThrowPercent()
    {
        percentActuated = Mathf.Clamp01(1 - Vector3.Distance(transform.position, transform.root.position) / maxDistance);
    }


	//float GetPercentActuated () {
	//	float currentDistance = Vector3.Distance(MultiplyVectorAxisAbs(transform.localPosition, actuationDirection), MultiplyVectorAxisAbs(fullyEngagedButtonPosition, actuationDirection));
	//	return Mathf.Clamp01(currentDistance / maxDistance);
	//}

	//Vector3 MultiplyVectorAxisAbs (Vector3 a, Vector3 b) {
	//	return new Vector3 (a.x * Mathf.Abs(b.x), a.y * Mathf.Abs(b.y), a.z * Mathf.Abs(b.z));
	//}

	//float GetSingleAxisPosition (Vector3 position, Vector3 axisToGet) {
	//Vector3 result = MultiplyVectorAxisAbs(position, axisToGet.normalized);
	//if (result.x != 0) {
	//	return result.x;
	//} else if (result.y != 0) {
	//	return result.y;
	//} else if (result.z != 0) {
	//	return result.z;
	//} else {
	//	return 0;
	//}
	//}
}
