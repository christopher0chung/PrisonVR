using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightRed : MonoBehaviour {

    [SerializeField] private Light myLight;
    [SerializeField] LayerMask myLM;
    private RaycastHit[] _myHits;
    private Color startColor;

    void Start()
    {
        startColor = myLight.color;
    }

	void Update () {
        Debug.Log("in update");

        Ray theRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(theRay.origin, theRay.direction, Color.red, 100);
        _myHits = Physics.RaycastAll(theRay, 100, myLM, QueryTriggerInteraction.Collide);

        Debug.Log(_myHits.Length);

        if (_myHits.Length > 0)
            myLight.color = startColor;
        else myLight.color = Color.red;
	}
}
