using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerID { p1, p2 }
public enum Stick { Left, Right }
public enum Button { Action, Dialogue, Choice1, Choice2, Start }

public abstract class GameEvent
{
    public delegate void Handler(GameEvent e);
}

//---------------------------------------
// Character Events
//---------------------------------------

public enum GroundStates { Grounded, NotGrounded }

public class Character_Grounded_GE: GameEvent
{
    public string Name;
    public GroundStates G;

    public Character_Grounded_GE (string n, GroundStates g)
    {
        Name = n;
        G = g;
    }
}


//---------------------------------------
// Value Storage Test Events
//---------------------------------------

public class Test_GE: GameEvent
{
    public readonly float myA;
    public readonly float myB;
    public readonly string myC;
    public readonly int myD;

    public Test_GE(float a, float b, string c, int d)
    {
        myA = a;
        myB = b;
        myC = c;
        myD = d;
    }
}


//---------------------------------------
// Rumble Events
//---------------------------------------

public class P1_DialogueChoiceRumble_GE : GameEvent { }

public class P2_DialogueChoiceRumble_GE : GameEvent { }


//---------------------------------------
// Control Events
//---------------------------------------

public class Device_GE :GameEvent
{
    public readonly PlayerID thisPID;
    public readonly InControl.InputDevice thisDev;

    public Device_GE (PlayerID pID, InControl.InputDevice dev)
    {
        thisPID = pID;
        thisDev = dev;
    }
}

public class Stick_GE : GameEvent
{
    public readonly PlayerID thisPID;
    public readonly Stick stick;
    public readonly float upDown;
    public readonly float leftRight;

    public Stick_GE(PlayerID pID, Stick s, float uD, float lR)
    {
        thisPID = pID;
        stick = s;
        upDown = uD;
        leftRight = lR;
    }
}

public class Button_GE : GameEvent
{
    public PlayerID thisPID;
    public Button button;
    public bool pressedReleased;

    public Button_GE(PlayerID pID, Button b, bool pR)
    {
        thisPID = pID;
        button = b;
        pressedReleased = pR;
    }
}


//---------------------------------------
// Event Manager
//---------------------------------------

public class EventManager {
    //---------------------
    // Creates singleton for ease of access
    //---------------------

    static private EventManager _instance;
    static public EventManager instance
    {
        get
        {
            if (_instance == null)
                return _instance = new EventManager();
            else
                return _instance;
        }
    }

    //---------------------
    // Storage of Events
    //---------------------

    private Dictionary<Type, GameEvent.Handler> registeredHandlers = new Dictionary<Type, GameEvent.Handler>();


    //---------------------
    // Register and Unregister
    //---------------------

    public void Register<T>(GameEvent.Handler handler) where T : GameEvent
    {
        Type type = typeof(T);
        if(registeredHandlers.ContainsKey(type))
        {
            registeredHandlers[type] += handler;
            //Debug.Log("Added Handler");
        }
        else
        {
            registeredHandlers[type] = handler;

            //Debug.Log("Registered Handler " + handler);
        }
        //if (handler != null)
            //Debug.Log(handler);
    }

    public void Unregister<T>(GameEvent.Handler handler) where T : GameEvent
    {
        Type type = typeof(T);
        GameEvent.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers -= handler;
            if (handlers == null)
            {
                registeredHandlers.Remove(type);
            }
            else
            {
                registeredHandlers[type] = handlers;
            }
        }
    }

    //---------------------
    // Call event
    //---------------------

    public void Fire(GameEvent e)
    {
        Type type = e.GetType();
        //Debug.Log(e.GetType());
        GameEvent.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers))
        {
            handlers(e);
            //Debug.Log("Event Fired");
        }
        //else
        //{
        //    Debug.Log(handlers);
        //}
    }
}
