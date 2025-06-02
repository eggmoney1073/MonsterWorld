using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerWindow : SingletonGameobject<UIPlayerWindow>
{
    [SerializeField] Vector3 _wholeCamPosition;
    [SerializeField] Vector3 _faceCamePosition;

    GameObject _uiPlayer;
    Animator _uiPlayerAnimator;
    Camera _uiPlayerCamera;

    protected override void Awake()
    {
        base.Awake();

        _uiPlayer = transform.GetChild(0).gameObject;
        _uiPlayerAnimator = _uiPlayer.GetComponent<Animator>();
        _uiPlayerCamera = transform.GetChild(1).GetComponent<Camera>();

        SetFaceCame();
    }

    public void UIPlayerGetHit()
    {
        _uiPlayerAnimator.SetTrigger("Damage");
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
}
