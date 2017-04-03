using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleToFaucet : MonoBehaviour {

    private bool _on;
    private bool on
    {
        get
        {
            return _on;
        }
        set
        {
            if (value != _on)
            {
                _on = value;
                if (_on)
                {
                    myPS.Play();
                }
                else
                {
                    myPS.Stop();
                }
            }
        }
    }

    [SerializeField] private ButtonPressPercent myBPP;

    private ParticleSystem myPS;

	// Use this for initialization
	void Start () {
        myPS = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (myBPP.percentActuated >= .05f)
        {
            on = true;
        }
        else
        {
            on = false;
        }
	}
}
