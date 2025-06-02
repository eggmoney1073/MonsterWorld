using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class ProjectileManager : SingletonGameobject<ProjectileManager>
{
    int _projectileTypeCount;

    [SerializeField]GameObject[] _projectilePrefabs;
    Dictionary<ProjectileType, GameObjectPool<Projectile>> _projectilePools;

    public void Init()
    {
        string path = "Weapon/";
        _projectileTypeCount = (int)ProjectileType.Max;

        _projectilePrefabs = new GameObject[_projectileTypeCount];

        for (int i = 0; i < _projectileTypeCount; i++)
        {
            ProjectileType type = (ProjectileType)i;
            _projectilePrefabs[i] = Resources.Load(path + type.ToString()) as GameObject;
        }

        _projectilePools = new Dictionary<ProjectileType, GameObjectPool<Projectile>>();

        for (int i = 0; i < _projectileTypeCount; i++)
        {
            GameObject prefab = _projectilePrefabs[i];
            ProjectileType type = (ProjectileType)i;

            _projectilePools.Add((ProjectileType)i, new GameObjectPool<Projectile>(12, () =>
            {
                GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
                go.GetComponent<Projectile>().Init(type);
                //go.SetActive(false);
                return go.GetComponent<Projectile>();
            }));
        }
    }

    public Projectile GetProjectile(ProjectileType type)
    {
        Projectile projectile = _projectilePools[type].Get();
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void SetProjectilePool(ProjectileType type, Projectile projectile)
    {
        _projectilePools[type].Set(projectile);
    }
}
