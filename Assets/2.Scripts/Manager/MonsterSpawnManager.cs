using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class MonsterSpawnManager : MonoBehaviour
{
    [Header("Spawn Rocation Setting")]
    [SerializeField] MonsterType _monsterType;
    [SerializeField] int _monsterCount;
    [SerializeField] float _spawnTerm;

    [Header("Gizmo Setting")]
    [SerializeField] Color _gizomShpereColor = Color.green;
    [SerializeField] float _gizmoSphereRad = 1f;
    [SerializeField] Color _gizmoLineColor = Color.red;

    bool _isSpawn = false;
    bool _isDespawn;

    int _childCount;

    float _checkTime = 0;

    Transform[] _patrolPoses;
    List<Monster> _aliveMonsters;
    Dictionary<int, Monster> _aliveMonsterDictionary;

    void Start()
    {
        InitSpawnManager();
    }

    public void InitSpawnManager()
    {
        _isDespawn = false;

        _aliveMonsters = new List<Monster>();
        _aliveMonsterDictionary = new Dictionary<int, Monster>();
        _childCount = transform.childCount;

        _patrolPoses = new Transform[_childCount];
        for (int i = 0; i < _childCount; i++)
            _patrolPoses[i] = transform.GetChild(i);
    }



    void Update()
    {
        if (_isDespawn)
        {
            return;
        }

        if (!_isSpawn)
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _spawnTerm)
            {
                _checkTime = 0;
                SpawnMonsters();
            }
        }
        else
        {
            int monsterCount = _aliveMonsters.Count;

            for (int i = 0; i < monsterCount; i++)
            {
                if (_aliveMonsters[i]._IsAlive)
                    return;
            }

            _isSpawn = false;
        }
    }

    void OnDrawGizmos()
    {
        _childCount = transform.childCount;

        _patrolPoses = new Transform[_childCount];

        for (int i = 0; i < _childCount; i++)
        {
            _patrolPoses[i] = transform.GetChild(i);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_patrolPoses[i].position, _gizmoSphereRad);
            Gizmos.color = Color.red;
            if (i > 0)
                Gizmos.DrawLine(_patrolPoses[i - 1].position, _patrolPoses[i].position);
        }
    }

    public void SpawnMonsters()
    {
        _isSpawn = true;

        for (int i = 1; i < _monsterCount; i++)
        {
            Monster monster = MonsterManager._Instance.GetMonster(_monsterType);

            int index = Random.Range(0, _childCount);

            monster.transform.position = _patrolPoses[index].position;
            monster.SpawnEnemyMonster(index, _patrolPoses, this);
            _aliveMonsters.Add(monster);
        }
    }

    public void DespawnAllMonsters()
    {
        int monsterCount = _aliveMonsters.Count;

        for (int i = 0; i < monsterCount; i++)
        {

        }
    }

    public void MonsterDead(Monster monster)
    {
        if (_aliveMonsters.Count == 0)
            return;

        if (_aliveMonsters.Contains(monster))
        {
            _aliveMonsters.Remove(monster);
        }
    }
}
