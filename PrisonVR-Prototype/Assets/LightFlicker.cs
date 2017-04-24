using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    public bool screenOn;

    Light mL;
    float timer;
	// Use this for initialization
	void Start () {
        mL = GetComponent<Light>();
        screenOn = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (screenOn)
        {
            timer += Time.deltaTime;
            
            if (timer >= .1f)
            {
                timer -= .1f;
                mL.intensity = Random.Range(.5f, 0.7f);
            }
        }
        else
        {
            mL.intensity = 0;
        }
	}
}
