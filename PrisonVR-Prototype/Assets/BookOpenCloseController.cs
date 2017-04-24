using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookOpenCloseController : MonoBehaviour {

    public Transform myCam;
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

    private float _passedValue;
    private float passedValue
    {
        get
        {
            return _passedValue;
        }
        set
        {
            if (value != _passedValue)
            {
                _passedValue = value;
                if (_passedValue == 0)
                    OnClose.Invoke();
                else if (_passedValue == 1)
                    OnOpen.Invoke();
            }
        }
    }


    [SerializeField] private float normalizedTime;

    private BookController myBC;

    public UnityEvent OnOpen;
    public UnityEvent OnClose;

	public UnityEvent OnDrop;

	// Use this for initialization
	void Start () {
		//myCam = GameObject.Find("VRCamera").transform;
        runningOCA = Closing;
        myBC = GetComponentInChildren<BookController>();
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Vector3.Angle(this.transform.right, -myCam.forward));

        if (Vector3.Angle(this.transform.right, -myCam.forward) < 30f)
        {
            open = true;
        }
        else
        {
            open = false;
        }

        if (runningOCA != null && transform.parent != null)
        {
            runningOCA();
        }
        else
        {
            //Debug.Log("No running oca");
        }
		
	}

    private void Opening()
    {
        openCloseFloat += Time.deltaTime/normalizedTime;
        openCloseFloat = Mathf.Clamp01(openCloseFloat);
        passedValue = openCloseFloat;
        myBC.bookCloseOpen = myAC.Evaluate(passedValue);
    }
    private void Closing()
    {
        openCloseFloat -= Time.deltaTime / normalizedTime;
        openCloseFloat = Mathf.Clamp01(openCloseFloat);
        passedValue = openCloseFloat;
        myBC.bookCloseOpen = myAC.Evaluate(passedValue);
    }

	void OnCollisionEnter (Collision collision) {

		OnDrop.Invoke ();

	}

}
