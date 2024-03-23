using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _footstepsAudioSource;
    [SerializeField] private SoundMaker _soundMaker;
    [SerializeField] private float _pushRigidbodiesPower = 4f;
    [SerializeField] private Collider _cameraCollider;
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PopupUI _popupUI;

    public Vector3 SpeedVector => _speedVector;
    public float CurrentSpeed => _moveVector.magnitude * _speed;
    public float FallVelocity => _fallVelocity;
    public Collider CameraCollider => _cameraCollider;
    public SkinnedMeshRenderer Skin => _skin;
    public CameraController CameraController => _cameraController;
    public PopupUI PopupUI => _popupUI;

    private float _fallVelocity;
    private Vector3 _moveVector;
    private CharacterController _controller;

    private bool _isRunning;
    public float runSpeed = 0f;

    private Vector2 _speedVector;
    private Vector2 _currentSpeedVector;

    private bool _disabledInput;

    private void Awake()
    {
        InitComponents();
    }

    private void Start()
    {
        _popupUI.AddTextToQueue("Двигайся на WASD", 3f);
        _popupUI.AddTextToQueue("Стреляй на ЛКМ", 3f);
        _popupUI.AddTextToQueue("Прыжок на Space", 3f);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic) return;

        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDirection = new(hit.moveDirection.x, 0f, hit.moveDirection.z);
        //body.velocity = pushDirection * _pushRigidbodiesPower;
        Vector3 collisionPoint = hit.point;
        body.AddForceAtPosition(pushDirection * _pushRigidbodiesPower, collisionPoint, ForceMode.Impulse);
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
        if (_disabledInput) return;

        float speed = _isRunning ? runSpeed : _speed;
        _moveVector = _moveVector.normalized;
        _controller.Move(speed * Time.fixedDeltaTime * _moveVector);
    }

    private void MoveInputUpdate()
    {
        _moveVector = Vector3.zero;
        _speedVector = Vector2.zero;

        if (_disabledInput) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (runSpeed > 0f)
                _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

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

        if (_fallVelocity > 3)
        {
            _animator.SetBool("IsFalling", true);
        }
        else
        {
            _animator.SetBool("IsFalling", false);
        }

        if (_controller.isGrounded)
        {
            _animator.SetBool("IsGrounded", true);
        }
        else
        {
            _animator.SetBool("IsGrounded", false);
        }
    }

    public void DisableInput()
    {
        _disabledInput = true;
    }

    public void PlayOneShotSound(CustomizableSound sound)
    {
        _audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        _audioSource.volume = Random.Range(sound.minVolume, sound.maxVolume);
        _audioSource.PlayOneShot(sound.clip);
    }

    public void PlayFootsteps(CustomizableSound sound)
    {
        PlayFootsteps(sound, _footstepsAudioSource.maxDistance);
    }

    public void PlayFootsteps(CustomizableSound sound, float distance)
    {
        _footstepsAudioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        _footstepsAudioSource.volume = Random.Range(sound.minVolume, sound.maxVolume);
        _footstepsAudioSource.PlayOneShot(sound.clip);
        _soundMaker.MakeSound(distance, _footstepsAudioSource.GetClipDuration(sound.clip));
    }
}
