﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOpenCloseController : MonoBehaviour {

    private Transform myCam;
    private bool _open;
    private bool open
    {
        get
        {
            return _open;
        }
        set
        {
            if (value != _open)
            {
                _open = value;
                if (_open)
                {
                    runningOCA = Opening;
                }
                else
                {
                    runningOCA = Closing;
                }
            }
        }
    }

    private delegate void openCloseAnim ();
    private openCloseAnim runningOCA;

    public AnimationCurve myAC;

    private float openCloseFloat;

	// Use this for initialization
	void Start () {
        myCam = Camera.main.transform;
        runningOCA = Closing;
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Angle(transform.right, -myCam.forward) < 30f)
        {
            open = true;
        }
        else
        {
            open = false;
        }

        runningOCA();
		
	}

    private void Opening()
    {
        openCloseFloat += Time.deltaTime;
        openCloseFloat = Mathf.Clamp01(openCloseFloat);
    }
    private void Closing()
    {
        openCloseFloat -= Time.deltaTime;
        openCloseFloat = Mathf.Clamp01(openCloseFloat);
    }
}