using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform _shootPos;

    ProjectileType _projectileType;

    Transform _aim;

    Vector3 _dir;

    public void SetGunAim(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        transform.forward = new Vector3(dir.z, dir.y, dir.x);
    }

    public void ShootGun()
    {
        _dir = _aim.transform.position - _shootPos.position;
        Ray bullet = new Ray(transform.position, _dir);
        RaycastHit hit;

        if (Physics.Raycast(bullet, out hit, 100f))
        {
            Collider hitCollider = hit.collider;

            if (hitCollider.CompareTag("EnemyMonster") || hitCollider.CompareTag("Monster"))
            {
                StartCoroutine(Co_ShootBullet(hit.point, true));
                Monster monster = hitCollider.GetComponent<Monster>();
                EffectManager._Instance.StartEffect(EffectManager.HitEffectName.Basic_Hit4, hit.point);
                monster.Damaged(PlayerManager._Instance.WeaponDamage(PlayerWeapon.Gun));
            }
            else
                StartCoroutine(Co_ShootBullet(transform.position + _dir * 10f, false));
        }
        else
            StartCoroutine(Co_ShootBullet(transform.position + _dir * 10f, false));
    }

    void Start()
    {
        _aim = Camera.main.GetComponent<CameraController>()._Aim;
        _projectileType = ProjectileType.Bullet;
    }

    IEnumerator Co_ShootBullet(Vector3 hitPosition, bool isHit)
    {
        Projectile projectile = ProjectileManager._Instance.GetProjectile(_projectileType);
        BulletController bullet = projectile.GetComponent<BulletController>();

        bullet._IsHit = isHit;

        bullet.Shoot(_shootPos.position, _dir, 3);

        yield return null;

        bullet.transform.position = hitPosition;

        for (int i = 0; i < 60; i++)
            yield return null;

        projectile.EndShooting();
    }
}