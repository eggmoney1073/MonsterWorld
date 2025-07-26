using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class ArrowController : Projectile
{
    [SerializeField] Transform _arrowModel;
    [SerializeField] float _arrowMoveSpeed = 2f;
    [SerializeField] float _arrowRotSpeed = 2f;
    [SerializeField] float _moveTime = 1f;

    bool _isStop = true;
    float _checkTime = 0f;
    float _gravity = 0.4f;
    Vector3 _direction;
    float _nowAngle;

    public override void Shoot(Vector3 startPosition, Vector3 direction, float speed)
    {
        transform.right = direction;
        _direction = direction.normalized;
        _isStop = false;
        transform.forward = direction;
        _arrowModel.transform.localRotation = Quaternion.Euler(0, -90, 0);
        base.Shoot(startPosition, direction, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("EnemyMonster"))
            other.GetComponent<Monster>().Damaged(PlayerManager._Instance.WeaponDamage(PlayerWeapon.Bow));

        EndShooting();
    }

    protected override void ShootingFunction()
    {
        if (_isStop)
            return;

        _checkTime += Time.deltaTime;
        if (_checkTime > _moveTime)
            EndShooting();

        _direction = _direction + Vector3.down * _gravity * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, _arrowMoveSpeed * Time.deltaTime);
        transform.forward = _direction;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _checkTime = 0;
        _isStop = true;
    }
}
