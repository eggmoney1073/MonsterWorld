using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Projectile : MonoBehaviour
{
    protected bool _isShoot;

    protected float _speed;

    protected ProjectileType _type;

    protected Vector3 _shootDirection;

    public void Init(ProjectileType type)
    {
        _type = type;

        Initialize();
    }

    public virtual void Shoot(Vector3 startPosition ,Vector3 direction, float speed)
    {
        transform.position = startPosition;
        _shootDirection = direction;
        _speed = speed;

        gameObject.SetActive(true);
        _isShoot = true;
    }

    public virtual void EndShooting()
    {
        Initialize();
        ProjectileManager._Instance.SetProjectilePool(_type, this);
    }

    protected virtual void Update()
    {
        if (_isShoot)
        {
            ShootingFunction();
        }
    }

    protected virtual void ShootingFunction()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + _shootDirection, _speed * Time.deltaTime);
    }

    protected virtual void Initialize()
    {
        _isShoot = false;

        gameObject.SetActive(false);
    }
}
