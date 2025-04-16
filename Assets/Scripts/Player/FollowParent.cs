using System;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    [SerializeField] private GameObject _parent = null;
    private Vector3 _lastPosition;
    
    private bool _climbing;
    private bool _steering;
    private bool _rigging;

    private void Start()
    {
        if (_parent)
            _lastPosition = _parent.transform.position;
    }

    private void FixedUpdate()
    {
        if (_parent)
        {
            Vector3 newPosition = _parent.transform.position - _lastPosition;
            transform.position += newPosition;
            _lastPosition = _parent.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Boat") && !_parent)
        {
            _parent = other.gameObject;
            _lastPosition = _parent.transform.position;
        }
        
        if (other.CompareTag("Ladder") && !_parent && Input.GetKeyDown(KeyCode.F) && !_climbing)
        {
            _climbing = true;
            _parent = other.gameObject;
            _lastPosition = _parent.transform.position;
        }
        else if (other.CompareTag("Ladder") && _parent && Input.GetKeyDown(KeyCode.F) && _climbing)
        {
            _climbing = false;
            _parent = null;
        }
        
        if (other.CompareTag("Wheel") && Input.GetKeyDown(KeyCode.F) && !_steering)
        {
            _steering = true;
            _parent = other.gameObject;
            _lastPosition = _parent.transform.position;
        }
        else if (other.CompareTag("Wheel") && Input.GetKeyDown(KeyCode.F) && _steering)
        {
            _steering = false;
            _parent = null;
        }
        
        if (other.CompareTag("Rigging") && Input.GetKeyDown(KeyCode.F) && !_rigging)
        {
            _rigging = true;
            _parent = other.gameObject;
            _lastPosition = _parent.transform.position;
        }
        else if (other.CompareTag("Rigging") && Input.GetKeyDown(KeyCode.F) && _rigging)
        {
            _rigging = false;
            _parent = null;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            _parent = null;
        }
        
        if (other.CompareTag("Ladder"))
        {
            _climbing = false;
            _parent = null;
        }
        
        if (other.CompareTag("Wheel"))
        {
            _steering = false;
            _parent = null;
        }
        
        if (other.CompareTag("Rigging"))
        {
            _rigging = false;
            _parent = null;
        }
    }
}