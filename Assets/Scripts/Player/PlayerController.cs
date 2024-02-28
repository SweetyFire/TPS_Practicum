using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private Animator _animator;

    private float _fallVelocity;
    private Vector3 _moveVector;
    private CharacterController _controller;

    private Vector2 _speedVector;
    private Vector2 _currentSpeedVector;

    private void Awake()
    {
        InitComponents();
    }

    private void Update()
    {
        MoveInputUpdate();
    }

    private void FixedUpdate()
    {
        MoveFixedUpdate();
        GravityFixedUpdate();
    } 

    private void InitComponents()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void GravityFixedUpdate()
    {
        _controller.Move(_fallVelocity * Time.fixedDeltaTime * Vector3.down);

        if (_controller.isGrounded)
            _fallVelocity = 1f;
        else
            _fallVelocity -= Physics.gravity.y * Time.fixedDeltaTime;
    }

    private void MoveFixedUpdate()
    {
        _controller.Move(_speed * Time.fixedDeltaTime * _moveVector);
    }

    private void MoveInputUpdate()
    {
        _moveVector = Vector3.zero;
        _speedVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _moveVector += transform.forward;
            _speedVector.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _moveVector -= transform.forward;
            _speedVector.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _moveVector += transform.right;
            _speedVector.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _moveVector -= transform.right;
            _speedVector.x = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            _fallVelocity = -_jumpHeight;
            _animator.SetTrigger("IsJumping");
        }

        _currentSpeedVector = Vector2.Lerp(_currentSpeedVector, _speedVector, 8f * Time.deltaTime);
        _animator.SetFloat("SpeedX", _currentSpeedVector.x);
        _animator.SetFloat("SpeedZ", _currentSpeedVector.y);

        if (_moveVector == Vector3.zero)
        {
            _animator.SetBool("IsRunning", false);
        }
        else
        {
            _animator.SetBool("IsRunning", true);
        }
    }
}
