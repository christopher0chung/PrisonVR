using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commercial_GE : GameEvent
{
    public bool playTrueEndFalse;
    public Commercial_GE (bool b)
    {
        playTrueEndFalse = b;
    }
}
public class Look_GE : GameEvent
{
    public bool lookingTrueNotFalse;
    public Look_GE(bool b)
    {
        lookingTrueNotFalse = b;
    }
}

public class TurnLightRed : MonoBehaviour {

    [SerializeField] private Light myLight;
    [SerializeField] LayerMask myLM;
    private RaycastHit[] _myHits;
    private Color startColor;
    private bool _commercialIsPlaying;

    void Awake()
    {
        EventManager.instance.Register<Commercial_GE>(EventHandler);
    }

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

        if (_commercialIsPlaying)
        {
            if (_myHits.Length > 0)
            {
                myLight.color = startColor;
                EventManager.instance.Fire(new Look_GE(true));
            }
            else
            {
                myLight.color = Color.red;
                EventManager.instance.Fire(new Look_GE(false));
            }
        }
        else myLight.color = startColor;
	}

    void EventHandler(GameEvent e)
    {
        if (e.GetType() == typeof(Commercial_GE))
        {
            Commercial_GE g = (Commercial_GE)e;
            if (g.playTrueEndFalse)
            {
                _commercialIsPlaying = true;
            }
            else
            {
                _commercialIsPlaying = false;
            }
        }
    }
}
