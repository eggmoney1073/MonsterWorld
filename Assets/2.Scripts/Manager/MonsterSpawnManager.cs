using DefineEnums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    #region [ Fields ]

    [Header("Spawn Rocation Setting")]
    [SerializeField] MonsterType _monsterType;
    [SerializeField] int _monsterCount;
    [SerializeField] float _spawnTerm;

    [Header("Gizmo Setting")]
    [SerializeField] Color _gizomShpereColor = Color.green;
    [SerializeField] float _gizmoSphereRad = 1f;
    [SerializeField] Color _gizmoLineColor = Color.red;

    bool _isFactoryOn;

    int _monsterIndex;
    int _childCount;

    float _checkTime = 0;

    Transform _playerTransform;
    Transform[] _patrolPoses;
    Dictionary<int, Monster> _aliveMonsterDictionary;

    #endregion

    #region [ Unity ]

    void Update()
    {
        if(_isFactoryOn) // Factory가 켜져있을 때
        {
            // 플레이어가 범위 내에 있는지 확인하고, 범위 밖에 있으면 몬스터를 디스폰합니다.
            if (!IsPlayerInRange(50f))
            {
                _isFactoryOn = false;
                DespawnAllMonsters();
                return;
            }

            // 살아있는 몬스터의 수를 확인하여 정해진 몬스터 수보다 적으면 몬스터를 소환합니다.
            int needSpawnCount = _monsterCount - _aliveMonsterDictionary.Count;
            if (needSpawnCount > 0)
            {
                SpawnMonsters(needSpawnCount);
            }
        }
        else // Factory가 꺼져있을 때
        {
            // 플레이어가 범위 내에 있는지 확인하고, 범위 내에 있으면 몬스터를 소환합니다.
            if (IsPlayerInRange(50f))
            {
                _isFactoryOn = true;
                SpawnMonsters(_monsterCount);
            }
        }
    }
    void OnDrawGizmos()
    {
        _childCount = transform.childCount;

        _patrolPoses = new Transform[_childCount];

        for (int i = 0; i < _childCount; i++)
        {
            _patrolPoses[i] = transform.GetChild(i);
            Gizmos.color = Color.red;
            if (_isFactoryOn)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawWireSphere(_patrolPoses[i].position, _gizmoSphereRad);
            Gizmos.color = Color.red;
            if (i > 0)
                Gizmos.DrawLine(_patrolPoses[i - 1].position, _patrolPoses[i].position);
        }
    }

    #endregion

    #region [ Public Methods ]

    public void InitSpawnManager()
    {
        _isFactoryOn = false;
        _monsterIndex = 0;

        _aliveMonsterDictionary = new Dictionary<int, Monster>();
        _childCount = transform.childCount;

        _patrolPoses = new Transform[_childCount];
        for (int i = 0; i < _childCount; i++)
            _patrolPoses[i] = transform.GetChild(i);

        _playerTransform = PlayerManager._Instance.transform;
    }

    public void SpawnMonsters(int monsterCount)
    {
        for (int i = 1; i < monsterCount; i++)
        {
            Monster monster = MonsterManager._Instance.GetMonster(_monsterType);
            SpawnMonster(monster);
        }
    }

    public void DespawnAllMonsters()
    {
        foreach(KeyValuePair<int,Monster> keyValuePair in _aliveMonsterDictionary)
        {
            keyValuePair.Value.Despawn();
        }
        _aliveMonsterDictionary.Clear();
    }

    /// <summary>
    /// 몬스터가 죽으면 Dictionary에서 해당 몬스터를 제거합니다.
    /// </summary>
    public void MonsterDead(Monster monster)
    {
        if(_aliveMonsterDictionary.ContainsKey(monster._SpawnID))
        {
            _aliveMonsterDictionary.Remove(monster._SpawnID);
        }
    }

    #endregion

    #region [ Private Methods ]

    void SpawnMonster(Monster monster)
    {
        int index = Random.Range(0, _childCount);

        monster.transform.position = _patrolPoses[index].position;
        monster.SpawnEnemyMonster(_monsterIndex, index, _patrolPoses, this);
        _aliveMonsterDictionary.Add(_monsterIndex++, monster);
    }

    /// <summary>
    /// 플레이어가 지정된 범위 내에 있는지 확인합니다.
    /// 플레이어가 범위 내에 있으면 true를 반환하고, 그렇지 않으면 false를 반환합니다.
    /// </summary>
    bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(_playerTransform.position, transform.position) < range;
    }

    #endregion
}
