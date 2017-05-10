using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardText : MonoBehaviour {

    [SerializeField] private Text cardText;

    private bool _startTimer;
    private float _timer;
    private bool _startWatchTimer;
    private float _watchTimer;

    private int watched;

    void Start()
    {
        EventManager.instance.Register<Commercial_GE>(LocalHandler);
    }

    void Update()
    {
        if(_startTimer)
        {
            _timer += Time.deltaTime;
            if (_timer >= 10)
            {
                if (_watchTimer >= 5)
                {
                    watched++;
                }
                _startTimer = false;
            }
        }
        if(_startWatchTimer)
        {
            _watchTimer += Time.deltaTime;
        }

        cardText.text = watched + " / 8";
    }

    void LocalHandler(GameEvent e)
    {
        if (e.GetType() == typeof(Commercial_GE))
        {
            Commercial_GE g = (Commercial_GE)e;
            _startTimer = g.playTrueEndFalse;
            if (g.playTrueEndFalse)
            {
                Debug.Log("Timers initialized and started");
                _timer = 0;
                _watchTimer = 0;
            }
        }
        if (e.GetType() == typeof(Look_GE))
        {
            Look_GE l = (Look_GE)e;
            _startWatchTimer = l.lookingTrueNotFalse;
            if(_startWatchTimer)
            {
                Debug.Log("watching");
            }
            else
            {
                Debug.Log("not watching");
            }
        }
    }
}
