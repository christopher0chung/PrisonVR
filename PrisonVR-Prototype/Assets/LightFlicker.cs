using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    public bool screenOn;

    Light mL;
    float timer;
    float targetIntensity = 0;
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

            if (timer >= .15f)
            {
                timer = 0;
                targetIntensity = Random.Range(.5f, 1f);

            }
        }
        else
        {
            targetIntensity = 0;
        }

        mL.intensity += (targetIntensity - mL.intensity) * 0.15f;
	}
}
