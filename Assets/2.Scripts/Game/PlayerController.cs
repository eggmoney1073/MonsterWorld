using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class PlayerController : MonoBehaviour
{
    #region Contants and Fields

    [Header("Player Control Setting")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _rotateSpeed = 3f;
    [SerializeField] Vector2 _zoomRotTest;

    [Header("Data")]
    [SerializeField] Transform _weaponPos;
    [SerializeField] Transform _spine;
    [SerializeField] Material[] _materials;

    bool _outLineOn;

    bool _isMove;
    bool _isZoom;

    float _checkTime;
    float _moveX;
    Vector3 _moveDir;

    PlayerWeapon _currentWeapon = PlayerWeapon.None;

    GunController _gun;
    BowController _bow;
    BallController _ball;

    Animator _animator;
    Transform _playerModel;
    GameObject[] _weapons;
    CameraController _cameraController;
    Camera _camera;
    CharacterController _characterController;

    #endregion

    #region Public Properties

    #endregion

    #region Public Methods and Operators

    public void PlayerDeadAnimation()
    {
        _animator.SetTrigger("Dead");
    }

    public void PlayerRespawnAnimation()
    {
        _animator.SetBool("Respawn",true);
    }

    public void Init()
    {
        _isMove = false;
        _isZoom = false;
        _outLineOn = false;

        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
        _playerModel = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        // Instance Weapon
        int weaponCount = (int)PlayerWeapon.Max;
        _weapons = new GameObject[weaponCount];
        string path = "Weapon/";
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject prefab = Resources.Load(path + ((PlayerWeapon)i).ToString()) as GameObject;
            _weapons[i] = Instantiate(prefab, _weaponPos);
            _weapons[i].SetActive(false);
        }
        _gun = _weapons[(int)PlayerWeapon.Gun].GetComponent<GunController>();
        _bow = _weapons[(int)PlayerWeapon.Bow].GetComponent<BowController>();
        _ball = _weapons[(int)PlayerWeapon.Ball].GetComponent<BallController>();
    }

    #endregion

    #region Event Handler Methods

    void Ani_RespawnOver()
    {
        _animator.SetBool("Respawn", false);
    }

    void Ani_ThrowBall()
    {
        Projectile ball = ProjectileManager._Instance.GetProjectile(ProjectileType.Ball);
        ball.Shoot(_weaponPos.position, (_cameraController._Aim.position - transform.position).normalized, 10);
    }

    void Ani_WeaponHolster()
    {
        _animator.SetLayerWeight(2, 0);
        EquipWeapon(_currentWeapon);
    }

    void Ani_DrawCurrentWaepon()
    {
        EquipWeapon(_currentWeapon);
    }

    #endregion

    #region Methods

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        _moveX = moveX;
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(moveX, 0, moveZ);

        dir = _camera.transform.rotation * dir;
        dir.y = 0;
        dir = dir.magnitude > 1 ? dir.normalized : dir;
        _moveDir = dir;

        _characterController.Move(dir * _moveSpeed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, _moveSpeed * Time.deltaTime);

        if (dir != Vector3.zero)
            _isMove = true;
        else
            _isMove = false;

        if (!_isZoom)
        {
            if (dir != Vector3.zero)
                _playerModel.forward = Vector3.Lerp(_playerModel.forward, dir, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 offset = new Vector3(_zoomRotTest.x, 0f, _zoomRotTest.y);
            transform.forward = _cameraController._CameraY0Dir + _camera.transform.rotation * offset;
        }
    }

    void WalkAnimationChange()
    {
        if (_isMove)
            _animator.SetFloat("Speed", Vector3.Dot(_moveDir, _playerModel.forward));
        else
            _animator.SetFloat("Speed", 0f);
    }

    void ZoomAnimation()
    {
        if (Input.GetMouseButton(1))
        {
            _isZoom = true;
            _animator.SetLayerWeight(1, 1);
            _animator.SetFloat("LeftRight", _moveX); 
        }
        else
        {
            _isZoom = false;
            _animator.SetLayerWeight(1, 0);
        }

        if (_isZoom)
        {
            float downUp = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.x = transform.forward.x;
            cameraForward.z = transform.forward.z;
            Vector3 cross = Vector3.Cross(transform.forward, cameraForward);
            _animator.SetFloat("DownUp", -Vector3.Dot(cross,transform.right));
        }
    }

    void ShootWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch(_currentWeapon)
            {
                case PlayerWeapon.Gun:
                    _gun.ShootGun();
                    break;

                case PlayerWeapon.Bow:
                    _bow.ShootBow();
                    break;

            }
        }
    }

    void WeaponAnimation()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(PlayerWeapon.None);
            _animator.SetBool("IsGunDraw", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_currentWeapon != PlayerWeapon.Bow)
            {
                ChangeWeapon(PlayerWeapon.Bow);
                _animator.SetTrigger("DrawWeapon");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            if (_currentWeapon != PlayerWeapon.Gun)
            {
                ChangeWeapon(PlayerWeapon.Gun);
                _animator.SetTrigger("DrawWeapon");
            }
        }

        if (_currentWeapon != PlayerWeapon.None && _currentWeapon != PlayerWeapon.Ball)
        {
            _animator.SetLayerWeight(2, 1);
            _animator.SetBool("IsGunDraw", true);
        }
        _animator.SetBool("IsZoom", _isZoom);
    }




    void ChangeWeapon(PlayerWeapon weapon)
    {
        _currentWeapon = weapon;
        UIManager._Instance.ChangeWeapon(weapon);
    }

    void EquipWeapon(PlayerWeapon weapon)
    {
        for (int i = 0; i < (int)PlayerWeapon.Max; i++)
        {
            _weapons[i].SetActive(false);
        }
        _weapons[(int)weapon].SetActive(true);
    }

    void ThrowBallFuction()
    {
        // Test

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentWeapon = PlayerWeapon.None;
            _animator.SetBool("IsGunDraw", false);
            _animator.SetLayerWeight(2, 0);
            EquipWeapon(PlayerWeapon.Ball);
            _animator.SetTrigger("ThrowBall");
        }
    }

    void SpawnMonster()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Spawn Monster
            // type, level 
            PlayerManager._Instance.SpawnPlayerMonster();
        }
    }

    void InvincibleState()
    {
        if(PlayerManager._Instance._IsInvincible)
        {
            _checkTime += Time.deltaTime;

            if (_checkTime > 0.5f)
            {
                if(_outLineOn)
                {
                    OutLine(false);
                    _outLineOn = false;
                }
                else
                {
                    OutLine(true);
                    _outLineOn = true;
                }

                _checkTime = 0;
            }
        }
    }

    void OutLine(bool isOn)
    {
        float onoffValue = 0;

        if (isOn)
            onoffValue = 1;

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_On", onoffValue);
        }
    }

    #endregion

    #region Call by Unity

    void Update()
    {
        if (InGameManager._Instance._IsPause || InGameManager._Instance._IsGameOver)
            return;

        Move();
        WalkAnimationChange();
        ZoomAnimation();
        WeaponAnimation();
        ThrowBallFuction();
        SpawnMonster();
        ShootWeapon();
        InvincibleState();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (_isZoom)
        {
            _animator.SetLookAtWeight(1);
            _animator.SetLookAtPosition(_camera.transform.GetChild(0).position);
        }
        else
        {
            _animator.SetLookAtWeight(0);
        }

    }

    #endregion
}
