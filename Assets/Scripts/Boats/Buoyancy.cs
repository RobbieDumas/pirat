using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(WaterCheck))]
public class Buoyancy : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float depthBefSub;
    [SerializeField] private float displacementAmt;
    [SerializeField] private int floaters;
    [SerializeField] private float waterDrag;
    [SerializeField] private float waterAngularDrag;

    private WaterCheck _waterCheck;

    private void Start()
    {
        _waterCheck = GetComponent<WaterCheck>();
    }

    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);

        if (_waterCheck.UnderWater())
        {
            float displacementMulti = Mathf.Clamp01((_waterCheck.WaterHeight().y - transform.position.y) / depthBefSub) * displacementAmt;

            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMulti * -rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMulti * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
