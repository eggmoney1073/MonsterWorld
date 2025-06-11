using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerWindow : SingletonGameobject<UIPlayerWindow>
{
    [SerializeField] Vector3 _wholeCamPosition;
    [SerializeField] Vector3 _faceCamePosition;

    bool _isDamaged;
    bool _isDefaltColor;

    float _checkTime;
    float _damagedTime = 0.2f;

    GameObject _uiPlayer;
    Animator _uiPlayerAnimator;
    Camera _uiPlayerCamera;

    Color _defaltColor = new Color(0.2264151f, 0.2264151f, 0.2264151f);
    Color _damagedColor = new Color(1, 0.4858491f, 0.4858491f);

    protected override void Awake()
    {
        base.Awake();

        _isDamaged = false;
        _isDefaltColor = true;

        _uiPlayer = transform.GetChild(0).gameObject;
        _uiPlayerAnimator = _uiPlayer.GetComponent<Animator>();
        _uiPlayerCamera = transform.GetChild(1).GetComponent<Camera>();

        SetFaceCame();
    }

    void Update()
    {
        if (!_isDamaged)
            return;

        _checkTime += Time.deltaTime;
        if(_checkTime > _damagedTime)
        {
            _checkTime = 0f;
            _uiPlayerCamera.backgroundColor = _defaltColor;
            _isDamaged = false;
        }
    }

    public void UIPlayerGetHit()
    {
        _uiPlayerAnimator.SetTrigger("Damage");
        _isDamaged = true;
        _uiPlayerCamera.backgroundColor = _damagedColor;
    }

    public void SetWholeBodyCam()
    {
        _uiPlayerCamera.orthographic = true;
        _uiPlayerCamera.transform.localPosition = _wholeCamPosition;
    }

    public void SetFaceCame()
    {
        _uiPlayerCamera.orthographic = false;
        _uiPlayerCamera.transform.localPosition = _faceCamePosition;
    }

    void ChangeColor()
    {
        if (_isDefaltColor)
            _uiPlayerCamera.backgroundColor = _damagedColor;
        else
            _uiPlayerCamera.backgroundColor = _defaltColor;
    }
}
