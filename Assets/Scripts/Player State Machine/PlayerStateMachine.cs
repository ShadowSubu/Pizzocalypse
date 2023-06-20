using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;

    // variables to store optimized setter/getter IDs
    int _isRunningHash;

    //values to store player input values
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _appliedMovement;
    bool _isMovementPressed;

    //constants
    float _runMultiplier = 5f;
    float _rotationFactorPerFrame = 15.0f;
    int _zero = 0;

    //State variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //Gun Variables
    [SerializeField] Gun[] _guns = new Gun[] { };
    Gun _activeGun;

    //getters and setters
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public int IsRunningHash { get { return _isRunningHash; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float RunMultiplier { get { return _runMultiplier; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public Gun ActiveGun { get { return _activeGun; } set { _activeGun = value; } }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // setup States
        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();

        _isRunningHash = Animator.StringToHash("IsRunning");

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.EquipPistol.performed += OnEquipPistol;
        _playerInput.CharacterControls.EquipShotgun.performed += OnEquipShotgun;
        _playerInput.CharacterControls.EquipRifle.performed += OnEquipRifle;
    }

    #region Input Action Methods

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnEquipPistol(InputAction.CallbackContext context)
    {
        ToggleGun(0);
    }

    void OnEquipShotgun(InputAction.CallbackContext context)
    {
        ToggleGun(1);
    }

    void OnEquipRifle(InputAction.CallbackContext context)
    {
        ToggleGun(2);
    }

    private void ToggleGun(int num)
    {
        if (ActiveGun == null || ActiveGun != _guns[num])
        {
            if (ActiveGun != null)
            {
                ActiveGun.gameObject.SetActive(false);
            }
            ActiveGun = _guns[num];
            ActiveGun.gameObject.SetActive(true);
        }
        else if (ActiveGun != null)
        {
            ActiveGun.gameObject.SetActive(false);
            ActiveGun = null;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        _characterController.Move(_appliedMovement * Time.deltaTime);
        _currentState.UpdateStates();
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    #region Gun



    #endregion

    private void OnEnable()
    {
        _playerInput?.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput?.CharacterControls.Disable();
    }
}

public enum GunType
{
    Pistol,
    Shotgun,
    Rifle
}
