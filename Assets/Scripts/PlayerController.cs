using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private Animator _animator;

    private float _fallVelocity;
    private Vector3 _moveVector;
    private CharacterController _controller;

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
        if (_controller.isGrounded)
            _fallVelocity = 1f;
        else
            _fallVelocity -= Physics.gravity.y * Time.fixedDeltaTime;

        _controller.Move(_fallVelocity * Time.fixedDeltaTime * Vector3.down);
    }

    private void MoveFixedUpdate()
    {
        _controller.Move(_speed * Time.fixedDeltaTime * _moveVector);
    }

    private void MoveInputUpdate()
    {
        _moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            _moveVector += transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _moveVector -= transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _moveVector += transform.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _moveVector -= transform.right;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            _fallVelocity = -_jumpHeight;
            _animator.SetTrigger("IsJumping");
        }

        _animator.SetBool("IsRunning", _moveVector != Vector3.zero);
    }
}
