using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    RaycastHit hit;
    Ray ray;
    float turnLerpDuration = 0.1f;

    //constants
    [SerializeField] float _runMultiplier = 5f;
    [SerializeField] float _rotationFactorPerFrame = 15.0f;
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
    bool _isRotating;
    bool _isReloading;

    //Abilities Variables
    [SerializeField] Abilities[] _abilities = new Abilities[] { };
    Abilities _abilityTrigerred;
    bool _isAbilityTrigerred;
    Vent _currentVent;
   
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
    public bool IsReloading { get { return _isReloading; } set { _isReloading = value; } }
    public bool IsRotating { get { return _isRotating; } set { _isRotating = value; } }
    public Abilities AbilityTrigerred { get { return _abilityTrigerred; } }
    public bool IsAbilityTrigerred {  get { return _isAbilityTrigerred; } set {  _isAbilityTrigerred = value; } }
    public Vent CurrentVent { get { return _currentVent; } }
    
    public Bullet BulletPrefab { get { return _bulletPrefab; } }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

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
        _playerInput.CharacterControls.Reload.performed += OnReload;
    }

    #region Input Action Methods

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        Debug.Log("Current Movement: " + _currentMovementInput);
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

    async void OnShoot(InputAction.CallbackContext context)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _isRotating = true;
        await RotateToShootDirection();
        _isShooting = context.ReadValueAsButton();
        ReduceAmmo(_activeGun.AmmoReduceAmount);
    }

    void OnReload(InputAction.CallbackContext context)
    {
        _isReloading = true;
        int _bulletsFired = (_activeGun.MagSize - _activeGun.CurrentMagSize);
        if (_activeGun.AmmoAmount > 0 && _activeGun.CurrentMagSize < _activeGun.MagSize)
        {
            if (_bulletsFired > _activeGun.AmmoAmount)
            {
                _activeGun.CurrentMagSize += _activeGun.AmmoAmount;
                _activeGun.AmmoAmount = 0;               
            }
            else
            {
                _activeGun.AmmoAmount -= _bulletsFired;
                _activeGun.CurrentMagSize += _bulletsFired;
            }                     
        }
        _isReloading = false;
    }

    void ReduceAmmo(int ammoToReduce)
    {
        if(_activeGun.CurrentMagSize > 0)
        {
            _activeGun.CurrentMagSize -= ammoToReduce;
            Debug.Log("ammo reduced");                      
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
        
    }

    private async void GameManager_OnGameStateChanged(GameState state)
    {
        if(state == GameState.Deliver)
        {
            _playerInput?.CharacterControls.Disable();
        }
        if (state == GameState.StartGame)
        {
            await Task.Delay(2000);
            _playerInput?.CharacterControls.Enable();
        }
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
        if (!_isRotating)
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
    }

    async Task RotateToShootDirection()
    {
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePos = new Vector3(hit.point.x, 0, hit.point.z);
            Vector3 lookDirection = mousePos - transform.position;
            float angle = Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg - 90f;
            //Debug.Log("Angle:" + angle);
            float timeElapsed = 0;
            while (timeElapsed < turnLerpDuration)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -angle, 0), timeElapsed/ turnLerpDuration);
                timeElapsed += Time.deltaTime;
                await Task.Yield();
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Delivery Point"))
        {
            if (GameManager.instance)
            {
                GameManager.instance.UpdateGameState(GameState.Deliver);
            }
        }

        if (other.CompareTag("NoReload") && _activeGun != null)
        {
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[0];
            Debug.Log("Ability Trigerred");
            Destroy(other.gameObject);
        }

        /*if (other.gameObject.TryGetComponent(out Vent vent) && other.gameObject.CompareTag("Location1"))
        {
            vent.SetPlayer(transform);
            _currentVent = vent;
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[1];
        }

        else if (other.gameObject.TryGetComponent(out Vent ventTwo) && other.gameObject.CompareTag("Location2"))
        {
            ventTwo.SetPlayer(transform);
            _currentVent = ventTwo;
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[2];
        }*/
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.gameObject.TryGetComponent(out Vent vent) && hit.gameObject.CompareTag("Location1") && !_isMovementPressed)
        {
            vent.SetPlayer(transform);
            _currentVent = vent;
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[1];
        }

        else if (hit.gameObject.TryGetComponent(out Vent ventTwo) && hit.gameObject.CompareTag("Location2") && !_isMovementPressed)
        {
            ventTwo.SetPlayer(transform);
            _currentVent = ventTwo;
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[2];
        }
    }
    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        _playerInput?.CharacterControls.Disable();
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

}

public enum GunType
{
    Pistol,
    Shotgun,
    Rifle
}

public enum AbilityType
{
    NoReload,
    Vent
}

