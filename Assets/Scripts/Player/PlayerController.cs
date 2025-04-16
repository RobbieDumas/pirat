using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(WaterCheck))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float swimSpeed = 6f;
    [SerializeField] private float climbSpeed = 6f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 10f;

    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 60f;

    private CharacterController _characterController;
    private WaterCheck _waterCheck;

    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;

    private GameObject _boat;

    private float _turnAmount;
    private float _sailSpeed;
    
    private bool _climbing;
    private bool _steering;
    private bool _rigging;

    private enum WalkState
    {
        Walking,
        Swimming,
        Climbing,
        Steering,
        Rigging
    }


    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _waterCheck = GetComponent<WaterCheck>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        var walkState = WalkState.Walking;

        if (_climbing)
            walkState = WalkState.Climbing;
        else if (_steering)
            walkState = WalkState.Steering;
        else if (_rigging)
            walkState = WalkState.Rigging;
        else
            walkState = (_waterCheck.UnderWater() ? WalkState.Swimming : WalkState.Walking);

        switch (walkState)
        {
            case (WalkState.Walking):
                bool isRunning = Input.GetKey(KeyCode.LeftShift);
                float curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
                float curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");
                float movementDirectionY = _moveDirection.y;
                _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                if (Input.GetButton("Jump") && _characterController.isGrounded)
                    _moveDirection.y = jumpPower;
                else
                    _moveDirection.y = movementDirectionY;

                if (!_characterController.isGrounded)
                    _moveDirection.y -= gravity * Time.deltaTime;
                break;
            case (WalkState.Swimming):
                var axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * swimSpeed;
                _moveDirection = playerCamera.transform.right * axis.x + playerCamera.transform.forward * axis.y;
                break;
            case (WalkState.Climbing):
                _moveDirection = new Vector3(0, Input.GetAxis("Vertical"), 0) * climbSpeed;
                break;
            case (WalkState.Steering):
                _turnAmount += Input.GetAxis("Horizontal");
                _boat.GetComponent<Boat>().Steer(_turnAmount);
                print(_turnAmount);
                break;
            case (WalkState.Rigging):
                _sailSpeed -= Input.GetAxis("Vertical");
                var speed = Mathf.Clamp(_sailSpeed, 0f, 200f);
                _boat.GetComponent<Boat>().Sail(speed);
                print(speed);
                break;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);

        _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ladder") && !_climbing && Input.GetKeyDown(KeyCode.F))
        {
            _climbing = true;
        }
        else if (other.CompareTag("Ladder") && _climbing && Input.GetKeyDown(KeyCode.F))
        {
            _climbing = false;
        }
        
        if (other.CompareTag("Wheel") && !_steering && Input.GetKeyDown(KeyCode.F))
        {
            _boat = other.GetComponent<FollowOrigin>().GetOrigin().transform.parent.gameObject;
            _turnAmount = _boat.GetComponent<Boat>().GetTurnSpeed();
            _steering = true;
        }
        else if (other.CompareTag("Wheel") && _steering && Input.GetKeyDown(KeyCode.F))
        {
            _boat = null;
            _steering = false;
        }
        
        if (other.CompareTag("Rigging") && !_rigging && Input.GetKeyDown(KeyCode.F))
        {
            _boat = other.GetComponent<FollowOrigin>().GetOrigin().transform.parent.gameObject;
            _sailSpeed = _boat.GetComponent<Boat>().GetSailSpeed();
            _rigging = true;
        }
        else if (other.CompareTag("Rigging") && _rigging && Input.GetKeyDown(KeyCode.F))
        {
            _boat = null;
            _rigging = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            _climbing = false;
        }
        
        if (other.CompareTag("Wheel"))
        {
            _steering = false;
        }
        
        if (other.CompareTag("Rigging"))
        {
            _rigging = false;
        }
    }
}