using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;
    MenuSelector _menuSelector;

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
    //int _zero = 0;

    //State variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //Gun Variables
    [SerializeField] private Gun[] _guns = new Gun[] { };
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
    NoReload _noReload;
    GunSwitch _gunSwitch;
    [SerializeField] private MineTrap mineTrapPrefab;
    [SerializeField] private StunGrenade stunGrenadePrefab;
    [SerializeField] private Transform spawnLocation;

    private AbilityType _currentAbility;
    [SerializeField] VoidEventSO abilityUseEvent;
    public AbilityType CurrentAbility { get { return _currentAbility; } }
   
    //Prefabs
    [SerializeField] Bullet _bulletPrefab;

    //Animation Lerp
    float lerpDuration = 0.3f;
    float valueToLerp;

    //Events
    [SerializeField] private IntEventSO bulletCountEvent = default;
    [SerializeField] private PlayerLoadoutSO playerLoadout;

    //getters and setters
    public PlayerInput PlayerInput { get { return _playerInput; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public MenuSelector MenuSelector { get { return _menuSelector; } }
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
    public NoReload NoReload {  get { return _noReload; } }
    public GunSwitch GunSwitch { get { return _gunSwitch; } }
    public Gun[] Guns { get { return _guns; } }
    
    
    public Bullet BulletPrefab { get { return _bulletPrefab; } }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _menuSelector = GetComponent<MenuSelector>();
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
        _playerInput.CharacterControls.Shoot.performed += OnShoot;
        _playerInput.CharacterControls.Reload.performed += OnReload;
        _playerInput.CharacterControls.UseAbility.performed += OnUseAbility; // TODO: GO TO THIS METHOD TO IMPLEMENT USE ABILITY
    }

    

    #region Input Action Methods

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        //Debug.Log("Current Movement: " + _currentMovementInput);
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (ActiveGun.CurrentMagSize > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _isRotating = true;
            //await RotateToShootDirection();
            _isShooting = true;
            ReduceAmmo(_activeGun.AmmoReduceAmount);
            bulletCountEvent.RaiseEvent(ActiveGun.CurrentMagSize, ActiveGun.MagSize, ActiveGun.AmmoAmount);

        }
        else
        {
            // TODO : PLAY SOUND TO INDICATE - NO BULLET
        }
    }

    public void OnReload(InputAction.CallbackContext context)
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
        bulletCountEvent.RaiseEvent(ActiveGun.CurrentMagSize, ActiveGun.MagSize, ActiveGun.AmmoAmount);
        _isReloading = false;
    }

    private void OnUseAbility(InputAction.CallbackContext obj)
    {
        // This executes when ability button is pressed
        UseAbility();
    }

    public void ReduceAmmo(int ammoToReduce)
    {
        if(_activeGun.CurrentMagSize > 0)
        {
            _activeGun.CurrentMagSize -= ammoToReduce;
            //Debug.Log("ammo reduced");                      
        }
    }

    #endregion

    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
        for (int i = 0; i < _guns.Length; i++)
        {
            if (_guns[i].GunType == playerLoadout.GunType)
            {
                _activeGun = _guns[i];
            }
        }
        _activeGun.gameObject.SetActive(true);
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

    public float gravity = 9.8f; // Adjust the gravity force as needed
    public float fallSpeed = 0f; // Initial falling speed
    public float maxFallSpeed = 10f;

    void Update()
    {
        HandleRotation();
        if(_characterController.enabled)
        {
            _characterController.Move(_appliedMovement * Time.deltaTime);

            // Check if the character controller is grounded
            if (_characterController.isGrounded)
            {
                // Reset the falling speed when grounded
                fallSpeed = -0.5f;
            }
            else
            {
                // Apply gravity-like effect when not grounded
                fallSpeed -= gravity * Time.deltaTime;
                fallSpeed = Mathf.Max(fallSpeed, -maxFallSpeed);

                // Move the character controller down
                _characterController.Move(Vector3.up * fallSpeed * Time.deltaTime);
            }
        }
        
        _currentState.UpdateStates();

        DummyControl();
    }

    private void DummyControl()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseAbility();
        }
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
        if (!_isRotating)
        {
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
        bulletCountEvent.RaiseEvent(ActiveGun.CurrentMagSize, ActiveGun.MagSize, ActiveGun.AmmoAmount);
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

        if (other.TryGetComponent(out NoReload noReload) && _activeGun != null)
        {
            _noReload = noReload;
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[0];
            Destroy(other.gameObject);
        }
        if (other.TryGetComponent(out GunSwitch gunSwitch))
        {
            _gunSwitch = gunSwitch;
            _isAbilityTrigerred = true;
            _abilityTrigerred = _abilities[3];
            Destroy(other.gameObject);
        }
        if (other.TryGetComponent(out VentTriggers vent))
        {
            SetVent(vent.Vent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out VentTriggers vent))
        {
            SetVent(null);
        }
    }

    public bool SetAbility(AbilityType type)
    {
        if (CurrentAbility == AbilityType.None)
        {
            _currentAbility = type;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        //if (hit.gameObject.TryGetComponent(out Vent vent) && hit.gameObject.CompareTag("Location1") && !_isMovementPressed)
        //{
        //    vent.SetPlayer(transform);
        //    _currentVent = vent;
        //    _isAbilityTrigerred = true;
        //    _abilityTrigerred = _abilities[1];
        //}

        //else if (hit.gameObject.TryGetComponent(out Vent ventTwo) && hit.gameObject.CompareTag("Location2") && !_isMovementPressed)
        //{
        //    ventTwo.SetPlayer(transform);
        //    _currentVent = ventTwo;
        //    _isAbilityTrigerred = true;
        //    _abilityTrigerred = _abilities[2];
        //}
    }
    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        _playerInput?.CharacterControls.Disable();
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    #region Ability

    public void EquipAbility(AbilityType ability)
    {
        if (CurrentAbility == AbilityType.None)
        {
            _currentAbility = ability;

            // UPDATE UI
        }
    }

    public void UseAbility()
    {
        int num = UnityEngine.Random.Range(0, _guns.Length);
        switch (CurrentAbility)
        {
            case AbilityType.None:
                //Debug.LogWarning("No Ability Available");
                break;
            case AbilityType.NoReload:
                if (ActiveGun != null)
                {
                    ActiveGun.InitiateNoReload(10);
                }
                break;
            case AbilityType.Vent:
                if (CurrentVent != null)
                {
                    CurrentVent.UseAbility(this);
                }
                _currentAbility = AbilityType.None;
                break;
            case AbilityType.GunSwitch:
                //SwitchGun();
                //ToggleGun(num);
                break;
            case AbilityType.MineTrap:
                if (mineTrapPrefab != null)
                {
                    MineTrap mineTrap = Instantiate(mineTrapPrefab, new Vector3(transform.position.x, transform.position.y , transform.position.z), Quaternion.identity);
                }
                break;
            case AbilityType.StunGrenade:
                if (stunGrenadePrefab != null)
                {
                    StunGrenade stunGrenade = Instantiate(stunGrenadePrefab, spawnLocation.position, Quaternion.identity);
                    stunGrenade.Initialize(transform.forward);
                }

                break;
            default:
                break;
        }
    }

    private void SwitchGun()
    {
        var activeGun = ActiveGun;
        int randomIndex = UnityEngine.Random.Range(0, _guns.Length);

        // Get the random GameObject from the list
        Gun randomGun = _guns[randomIndex];
        _activeGun = randomGun;
        _activeGun.gameObject.SetActive(true);
        //_activeGun.ResetGun();
    }

    private void ToggleGun(int num)
    {
        Debug.Log("Changing Gun");
        if (ActiveGun == null || ActiveGun != Guns[num])
        {
            if (ActiveGun != null)
            {
                ActiveGun.gameObject.SetActive(false);
            }
            ActiveGun = Guns[num];
            ActiveGun.gameObject.SetActive(true);
        }
        else if (ActiveGun != null)
        {
            ActiveGun.gameObject.SetActive(false);
            ActiveGun = null;
        }
    }
    #endregion

    public void SetVent(Vent vent)
    {
        if (CurrentVent == null)
        {
            _currentVent = vent;
        }
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
    None,
    NoReload,
    Vent,
    GunSwitch,
    MineTrap,
    StunGrenade
}

