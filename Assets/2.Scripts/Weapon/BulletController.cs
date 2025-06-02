using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : Projectile
{
    [Header("Bullet Setting")]
    [SerializeField] Color _hitColor = Color.blue;
    [SerializeField] Color _unHitColor= Color.red;

    bool _isHit;

    GameObject _bullet;
    MeshRenderer _bulletModel;
    TrailRenderer _bulletTail;

    public bool _IsHit { get { return _isHit; } set { _isHit = value; } }

    void Awake()
    {
        _bullet = transform.GetChild(0).gameObject;
        _bulletModel = _bullet.GetComponent<MeshRenderer>();
        _bulletTail = _bullet.GetComponent<TrailRenderer>();
    }

    public override void Shoot(Vector3 startPosition, Vector3 direction, float speed)
    {
        if(_isHit)
            SetBulletColor(_hitColor);
        else
            SetBulletColor(_unHitColor);



        base.Shoot(startPosition, direction, speed);
    }

    void SetBulletColor(Color color)
    {
        _bulletModel.material.color = color;
        _bulletTail.material.color = color;
    }
}