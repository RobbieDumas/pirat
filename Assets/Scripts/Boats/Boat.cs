using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boat : MonoBehaviour
{
    private Vector3 _sailSpeed;
    private Vector3 _turnSpeed;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    [Obsolete("Obsolete")]
    private void FixedUpdate()
    {
        _rb.AddRelativeForce(_sailSpeed, ForceMode.Acceleration);
        _rb.AddRelativeTorque(_turnSpeed, ForceMode.Acceleration);
    }

    public void Steer(float speed)
    {
        _turnSpeed.y = speed;
    }
    
    public void Sail(float speed)
    {
        _sailSpeed.z = speed;
    }
    
    public float GetTurnSpeed() => _turnSpeed.y;
    
    public float GetSailSpeed() => _sailSpeed.z;
}
