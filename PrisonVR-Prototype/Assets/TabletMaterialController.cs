using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class TabletMaterialController : MonoBehaviour {

    [Header("MeshRenderers")]
    [SerializeField] private MeshRenderer glassMR;
    [SerializeField] private MeshRenderer displayMR;

    [Header ("For glass")]
    [SerializeField] private Material newGlass;
    [SerializeField] private Material brokenGlass;

    [Header("For display")]
    [SerializeField] private Material workingDisplay;
    [SerializeField] private Material brokenDisplay;

    [Header("For tuning")]
    [SerializeField] private float breakThreshold;

    private float _velocityMagnitude;
    private bool _wasWetted;

    private float _timer;
    private float _flickerTime;

    void Start()
    {
        glassMR.material = newGlass;
        displayMR.material = workingDisplay;
    }

    void Update()
    {
        _velocityMagnitude = Vector3.Magnitude(GetComponent<Rigidbody>().velocity);

        if (_wasWetted)
        {
            _timer += Time.deltaTime;
            if (_timer >= _flickerTime)
            {
                _timer -= _flickerTime;
                _flickerTime = Random.Range(.1f, .4f);
                if (displayMR.material == workingDisplay)
                    displayMR.material = brokenDisplay;
                else displayMR.material = workingDisplay;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ObjectsTypeTag>() != null)
        {
            if (other.gameObject.GetComponent<ObjectsTypeTag>().thisCollidersTag == TypeTag.Hard)
            {
                if (_velocityMagnitude >= breakThreshold)
                {
                    glassMR.material = brokenGlass;
                }
            }
            if (other.gameObject.GetComponent<ObjectsTypeTag>().thisCollidersTag == TypeTag.Wet)
                _wasWetted = true;
        }
    }
}
