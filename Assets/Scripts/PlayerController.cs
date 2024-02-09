using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpHeight = 10f;

    private float _fallVelocity;
    private Vector3 _moveVector;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
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
        }
    }

    private void FixedUpdate()
    {
        _controller.Move(_speed * Time.fixedDeltaTime * _moveVector);

        if (_controller.isGrounded)
            _fallVelocity = 0f;
        else
            _fallVelocity += -Physics.gravity.y * Time.fixedDeltaTime;

        _controller.Move(_fallVelocity * Time.fixedDeltaTime * Vector3.down);
    }
}
