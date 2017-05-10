using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public delegate void Handler(GameEvent e);
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
