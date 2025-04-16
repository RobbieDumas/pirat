using UnityEngine;

public class FollowOrigin : MonoBehaviour
{
    [SerializeField] private GameObject origin;

    private void FixedUpdate()
    {
        transform.position = origin.transform.position;
        transform.rotation = origin.transform.rotation;
    }
    
    public GameObject GetOrigin() => origin;
}
