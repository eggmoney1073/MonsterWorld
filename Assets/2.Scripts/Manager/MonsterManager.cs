using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;
using DefineStructs;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class MonsterManager : SingletonGameobject<MonsterManager>
{
    GameObject _playerGO;
    PlayerManager _player;
    Monster _playerMonster;

    GameObject[] _monsterPrefabs;
    Dictionary<MonsterType, GameObjectPool<Monster>> _monsterPools;

    TableBase _monsterTalbe;
    TableBase _monsterLevelTalbe;

    bool _isPlayerMonsterSpawned;
    float _playerCapturePower;

    public List<Monster> _monstersOnBattle;

    public GameObject _Player { get { return _playerGO; } }
    public float _PlayerCapturePower { get { return _playerCapturePower; } }

    public MonsterData GetMonsterData(MonsterType type)
    {
        int index = (int)type;

        float healthScale = _monsterTalbe.ToF(index, MonsterTable.Index.Health_Scale.ToString());
        float attackScale = _monsterTalbe.ToF(index, MonsterTable.Index.Attack_Scale.ToString());

        int skill1 = _monsterTalbe.ToI(index, MonsterTable.Index.Skill1.ToString());
        int skill2 = _monsterTalbe.ToI(index, MonsterTable.Index.Skill2.ToString());

        return new MonsterData() { HealthScale = healthScale, AttackScale = attackScale, Skill1 = (SkillType)skill1, Skill2 = (SkillType)skill2 };
    }

    public MonsterLevelData GetMonsterLevelData(int level)
    {
        float dropEXP = _monsterLevelTalbe.ToF(level, MonsterLevelTable.Index.Drop_EXP.ToString());
        float reqCapturePower = _monsterLevelTalbe.ToF(level, MonsterLevelTable.Index.CapturePower.ToString());
        float health = _monsterLevelTalbe.ToF(level, MonsterLevelTable.Index.Health.ToString());
        float attack = _monsterLevelTalbe.ToF(level, MonsterLevelTable.Index.Attack.ToString());

        return new MonsterLevelData() { DropEXP = dropEXP, RequiredCaputrePower = reqCapturePower, Health = health, Attack = attack };
    }

    public Monster GetMonster(MonsterType type)
    {
        return _monsterPools[type].Get();
    }

    public void ReturnMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
        _monsterPools[monster._Type].Set(monster);
    }

    public void SpawnPlayerMonster(MonsterType type, int level)
    {
        // 플레이어 몬스터가 이미 스폰했으면 죽이고 새로 스폰
        if (_isPlayerMonsterSpawned)
        {
            _playerMonster.Dead();
        }

        // 오브젝트 풀에서 몬스터를 받아와서 사용
        Monster monster = _monsterPools[type].Get();
        monster.SetMonsterData(type, level, null);
        _playerMonster = monster;
        _playerMonster.gameObject.tag = "PlayerMonster";

        _playerMonster.SpawnFriendMonster(_playerGO.transform.position + _playerGO.transform.right);

        // 전투중인 몬스터 리스트에 몬스터가 있으면 전투 진입 순서대로 타겟지정
        if (_monstersOnBattle.Count > 0)
        {
            _playerMonster._Target = _monstersOnBattle[0].gameObject;
            _playerMonster.StartBattle();
        }
        _isPlayerMonsterSpawned = true;
    }

    public void SetEnemyOnBattle(Monster monster)
    {
        _monstersOnBattle.Add(monster);
        if (_playerMonster != null)
        {
            _playerMonster._Target = monster.gameObject;
            _playerMonster.StartBattle();
        }
    }

    public void RemoveEnemyOnBattle(Monster monster)
    {
        _monstersOnBattle.Remove(monster);
        if (_playerMonster != null && _monstersOnBattle.Count == 0)
        {
            _playerMonster._Target = null;
            _playerMonster.EndBattle();
        }
    }

    public void InitMonsterManager()
    {
        _playerGO = GameObject.FindGameObjectWithTag("Player");
        _player = _playerGO.GetComponent<PlayerManager>();
        _playerCapturePower = _player._CapturePower;

        _monstersOnBattle = new List<Monster>();
        _monsterPools = new Dictionary<MonsterType, GameObjectPool<Monster>>();

        _monsterTalbe = GameTableManager._Instance.Get(TableName.MonsterTable);
        _monsterLevelTalbe = GameTableManager._Instance.Get(TableName.MonsterLevelTable);

        int monsterCount = (int)MonsterType.Max;
        _monsterPrefabs = new GameObject[monsterCount + 1];

        for (int i = 1; i < monsterCount; i++)
        {
            MonsterType type = (MonsterType)i;

            string path = "Monsters/Monster";
            GameObject prefab = Resources.Load(path + type.ToString()) as GameObject;

            _monsterPrefabs[i] = prefab;

            GameObjectPool<Monster> monsterPool = new GameObjectPool<Monster>(2, () =>
            {
                GameObject monsterGO = Instantiate(prefab, transform);
                Monster monster = monsterGO.GetComponent<Monster>();

                int minLevel = _monsterTalbe.ToI((int)type, MonsterTable.Index.Min_Level.ToString());
                int maxLevel = _monsterTalbe.ToI((int)type, MonsterTable.Index.Max_Level.ToString());

                int level = Random.Range(minLevel, maxLevel + 1);

                monster.SetMonsterData(type, level, _playerGO);
                //monster.SetTarget(_player);
                monsterGO.SetActive(false);
                return monster.GetComponent<Monster>();
            });

            _monsterPools.Add((MonsterType)i, monsterPool);
        }
    }
}
