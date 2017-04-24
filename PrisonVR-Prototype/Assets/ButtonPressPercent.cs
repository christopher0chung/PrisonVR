using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressPercent : MonoBehaviour
{

    [SerializeField]
    float speed;

    //public Vector3 actuationDirection;
    //public Vector3 fullyEngagedButtonPosition;
    Vector3 startPosition;
    [SerializeField]
    float maxDistance;

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

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!inContact)
            MoveTowardsMax();

        ContactTimer();
        ThrowPercent();
        transform.position = Vector3.ClampMagnitude((transform.position + (startPosition + transform.up * maxDistance)) / 2, maxDistance / 2);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Hand")
            inContact = true;
    }

    void MoveTowardsMax()
    {
        GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, startPosition + transform.up * maxDistance, speed * Time.deltaTime));
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
        percentActuated = Mathf.Clamp01(1 - Vector3.Distance(transform.position, startPosition) / maxDistance);
    }
}
