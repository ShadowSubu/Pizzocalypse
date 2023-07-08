using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;

    // variables to store optimized setter/getter IDs
    int _isRunningHash;
    int _isPistolFireHash;
    int _isShotgunFireHash;
    int _isRifleFireHash;

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
    bool _isGunToggled;
    bool _requireNewGunToggle;
    bool _isShooting;

    //Prefabs
    [SerializeField] Bullet _bulletPrefab;

    //Animation Lerp
    float lerpDuration = 0.3f;
    float valueToLerp;

    //getters and setters
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public int IsRunningHash { get { return _isRunningHash; } }
    public int IsPistolFireHash { get { return _isPistolFireHash; } }
    public int IsShotgunFireHash { get { return _isShotgunFireHash; } }
    public int IsRifleFireHash { get { return _isRifleFireHash; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float RunMultiplier { get { return _runMultiplier; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public Gun ActiveGun { get { return _activeGun; } set { _activeGun = value; } }
    public bool IsGunToggled { get { return _isGunToggled; } set { _isGunToggled = value; } }
    public bool RequireNewGunToggle { get { return _requireNewGunToggle; } set { _requireNewGunToggle = value; } }
    public bool IsShooting { get { return _isShooting; } set { _isShooting = value; } }
    public Bullet BulletPrefab { get { return _bulletPrefab; } }

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
        _isPistolFireHash = Animator.StringToHash("IsPistolFire");
        _isShotgunFireHash = Animator.StringToHash("IsShotgunFire");
        _isRifleFireHash = Animator.StringToHash("IsRifleFire");

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.EquipPistol.started += OnEquipPistol;
        _playerInput.CharacterControls.EquipPistol.canceled += OnEquipPistol;
        _playerInput.CharacterControls.EquipShotgun.started += OnEquipShotgun;
        _playerInput.CharacterControls.EquipShotgun.canceled += OnEquipShotgun;
        _playerInput.CharacterControls.EquipRifle.started += OnEquipRifle;
        _playerInput.CharacterControls.EquipRifle.canceled += OnEquipRifle;
        _playerInput.CharacterControls.Shoot.performed += OnShoot;
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
        _isGunToggled = context.ReadValueAsButton();
        if (IsGunToggled)
        {
            ToggleGun(0);
        }
        else
        {
            RequireNewGunToggle = false;
        }
    }

    void OnEquipShotgun(InputAction.CallbackContext context)
    {
        _isGunToggled = context.ReadValueAsButton();
        if (IsGunToggled)
        {
            ToggleGun(1);
        }
        else
        {
            RequireNewGunToggle = false;
        }
    }

    void OnEquipRifle(InputAction.CallbackContext context)
    {
        _isGunToggled = context.ReadValueAsButton();
        if (IsGunToggled)
        {
            ToggleGun(2);
        }
        else
        {
            RequireNewGunToggle = false;
        }
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        _isShooting = context.ReadValueAsButton();
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

    public async void EquipAnimation(float startValue, float endValue)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            //Debug.Log("Lerp: " + startValue);
            Animator.SetLayerWeight(1, valueToLerp);
            await Task.Yield();
        }
    }

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
