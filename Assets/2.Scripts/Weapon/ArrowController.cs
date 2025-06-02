using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class ArrowController : Projectile
{
    [SerializeField] Transform _headPos;
    [SerializeField] float _arrowMoveSpeed = 2f;
    [SerializeField] float _arrowRotSpeed = 2f;
    [SerializeField] float _moveTime = 1f;

    bool _isStop = true;
    float _checkTime = 0f;

    public override void Shoot(Vector3 startPosition, Vector3 direction, float speed)
    {
        transform.right = direction;
        _isStop = false;
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

        transform.position = Vector3.MoveTowards(transform.position, _headPos.position, _arrowMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.back * 90), _arrowRotSpeed * Time.deltaTime);
    }

    protected override void Initialize()
    {
        base.Initialize();

        _checkTime = 0;
        _isStop = true;
    }
}
