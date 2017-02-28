using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour {

    private float adjuster;

    private float _bookCloseOpen;
    [SerializeField] public float bookCloseOpen
    {
        get
        {
            return _bookCloseOpen;
        }
        set
        {
            if (value != _bookCloseOpen)
            {
                _bookCloseOpen = value;

                leftCover.localEulerAngles = Vector3.Lerp(new Vector3 (90, 0, 0), new Vector3(180, 0, 0), _bookCloseOpen);
                rightCover.localEulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), new Vector3(0, 0, 0), _bookCloseOpen);

                leftPage0.localEulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), new Vector3(62.9f, 0, 0), _bookCloseOpen);
                leftPage1.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(42.2f, 0, 0), _bookCloseOpen);
                leftPage2.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(36.1f, 0, 0), _bookCloseOpen);
                leftPage3.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(33.1f, 0, 0), _bookCloseOpen);
                leftPage4.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(17.8f, 0, 0), _bookCloseOpen);
                leftPage5.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-1.7f, 0, 0), _bookCloseOpen);
                leftPage6.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-4.1f, 0, 0), _bookCloseOpen);
                leftPage7.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-3.67f, 0, 0), _bookCloseOpen);
                leftPage8.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-14.23f, 0, 0), _bookCloseOpen);
                leftPage9.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(12.95f, 0, 0), _bookCloseOpen);
                leftPage10.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-25.62f, 0, 0), _bookCloseOpen);

                rightPage0.localEulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), new Vector3(62.9f, 0, 0), _bookCloseOpen);
                rightPage1.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(42.2f, 0, 0), _bookCloseOpen);
                rightPage2.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(36.1f, 0, 0), _bookCloseOpen);
                rightPage3.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(33.1f, 0, 0), _bookCloseOpen);
                rightPage4.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(17.8f, 0, 0), _bookCloseOpen);
                rightPage5.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-1.7f, 0, 0), _bookCloseOpen);
                rightPage6.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-4.1f, 0, 0), _bookCloseOpen);
                rightPage7.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-3.67f, 0, 0), _bookCloseOpen);
                rightPage8.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-14.23f, 0, 0), _bookCloseOpen);
                rightPage9.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(12.95f, 0, 0), _bookCloseOpen);
                rightPage10.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-25.62f, 0, 0), _bookCloseOpen);

            }
        }
    }


    [SerializeField] private Transform leftCover;
    [SerializeField] private Transform rightCover;

    [SerializeField] private Transform leftPage0;
    [SerializeField] private Transform leftPage1;
    [SerializeField] private Transform leftPage2;
    [SerializeField] private Transform leftPage3;
    [SerializeField] private Transform leftPage4;
    [SerializeField] private Transform leftPage5;
    [SerializeField] private Transform leftPage6;
    [SerializeField] private Transform leftPage7;
    [SerializeField] private Transform leftPage8;
    [SerializeField] private Transform leftPage9;
    [SerializeField] private Transform leftPage10;

    [SerializeField] private Transform rightPage0;
    [SerializeField] private Transform rightPage1;
    [SerializeField] private Transform rightPage2;
    [SerializeField] private Transform rightPage3;
    [SerializeField] private Transform rightPage4;
    [SerializeField] private Transform rightPage5;
    [SerializeField] private Transform rightPage6;
    [SerializeField] private Transform rightPage7;
    [SerializeField] private Transform rightPage8;
    [SerializeField] private Transform rightPage9;
    [SerializeField] private Transform rightPage10;


    void Start()
    {
        bookCloseOpen = 10;
        bookCloseOpen = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            adjuster += Time.deltaTime / 2;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            adjuster -= Time.deltaTime / 2;
        }

        adjuster = Mathf.Clamp01(adjuster);
        bookCloseOpen = adjuster;
    }


}
