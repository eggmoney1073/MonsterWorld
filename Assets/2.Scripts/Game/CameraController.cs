using DefineEnums;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Setting")]
    [SerializeField] float _maxY = 80f;
    [SerializeField] float _minY = -45f;
    [SerializeField] float _mouseSensitivity = 400f;
    [SerializeField] float _cameraSpeed = 3f;
    [SerializeField] Vector3 _normalCameraOffset;
    [SerializeField] Vector3 _zoomCameraOffset;

    bool _isShake;
    bool _isInit;
    bool _isGameOver;


    float _mouseY;
    float _mouseX;
    float _checkTime;
    float _shakeTime;
    Vector3 _Y0dir;

    Transform _aim;
    Transform _player;
    Transform _cameraPos;
    Transform _deadCameraPos;


    public Transform _Aim { get { return _aim; } }
    public Vector3 _CameraY0Dir {  get { return _Y0dir; } }

    public void InitCamera(GameObject player)
    {
        _player = player.transform;
        _cameraPos = _player.transform.GetChild(1).transform;
        _deadCameraPos = _player.transform.GetChild(2).transform;
        transform.position = _player.position;
        _checkTime = 0;
        _isInit = true;
        _isGameOver = false;
    }

    void Awake()
    {
        _aim = transform.GetChild(0);
        _isInit = false;
    }

    void Update()
    {
        if (_isInit)
        {
            _isGameOver = UIManager._Instance._GameOver;

            if (_isGameOver)
            {
                GameOverCameraMove();  
            }
            else
            {
                Rotate();
                Move();

                if (_isShake)
                {
                    _checkTime += Time.deltaTime;

                    Vector3 shakePos = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, Random.insideUnitCircle.x);
                    transform.position += shakePos * 0.1f;

                    if (_checkTime > _shakeTime)
                    {
                        _isShake = false;
                        _checkTime = 0;
                    }
                }
            }
        }
    }

    void Rotate()
    {
        _mouseX += Input.GetAxisRaw("Mouse X") * _mouseSensitivity * Time.deltaTime;
        _mouseY -= Input.GetAxisRaw("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        _mouseY = Mathf.Clamp(_mouseY, _minY, _maxY); 

        transform.localRotation = Quaternion.Euler(_mouseY, _mouseX, 0f);

        Vector3 dir = transform.forward;
        dir.y = 0;
        dir = dir.magnitude > 1 ? dir.normalized : dir;

        _Y0dir = dir;
    }

    void Move()
    {
        Vector3 destPos = _player.transform.position - transform.forward;        

        if (Input.GetMouseButton(1))
            destPos += transform.rotation * _zoomCameraOffset;
        else
            destPos += transform.rotation * _normalCameraOffset;

        transform.position = Vector3.MoveTowards(transform.position, destPos, _cameraSpeed * Time.deltaTime);
    }

    public void ShakeCamera(float time)
    {
        _shakeTime = time;
        _isShake = true;
    }

    void GameOverCameraMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, _deadCameraPos.position, _cameraSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _deadCameraPos.rotation, 40 * Time.deltaTime);

        
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
