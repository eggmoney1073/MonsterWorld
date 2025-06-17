using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class BowController : MonoBehaviour
{
    [SerializeField] GameObject _arrowPrefab;
    [SerializeField] Transform _originPos;
    [SerializeField] Transform _shootPos;

    Vector3 _dir;
    Transform _aim;

    ProjectileType _type;

    public void ShootBow()
    {
        _dir = _aim.transform.position - _shootPos.position;
        Projectile projectile = ProjectileManager._Instance.GetProjectile(_type);
        ArrowController arrow = projectile.GetComponent<ArrowController>();
        arrow.gameObject.SetActive(true);
        arrow.transform.position = _shootPos.position;
        arrow.Shoot(_shootPos.position, _dir, 0);
    }

    void Start()
    {
        _aim = Camera.main.GetComponent<CameraController>()._Aim;
        _type = ProjectileType.Arrow;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ShootBow();
        }
    }
}
