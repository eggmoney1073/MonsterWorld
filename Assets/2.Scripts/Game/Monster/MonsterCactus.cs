using DefineEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class MonsterCactus : Monster
{
    [Header("Cactus Setting")]
    [SerializeField] float _checkDistance = 5;
    [SerializeField] float _attackDistance = 3;
    [SerializeField] float _maxHealthPoint = 3;
    [SerializeField] MonsterType _monsterType = MonsterType.Cactus;

    [Header("Patrol Setting")]
    [SerializeField] float _stopMoveTime = 1f;
    [SerializeField] float _checkTargetTerm = 0.5f;
    [SerializeField] float _moveSpeed = 3f;

    [Header("Chase Setting")]
    [SerializeField] float _chaseSpeed = 5f;
    [SerializeField] float _chaseDistance = 5f;

    [Header("Attack Setting")]
    [SerializeField] float _checkDistanceOnBattle = 10;

    [Header("Return Setting")]
    [SerializeField] float _returnSpeed = 15f;

    public override void SetInitialMonster(MonsterType type, int level, GameObject target)
    {
        base.SetInitialMonster(type, level, target);

        MonsterManager._Instance.GetMonsterData(type);

        // юс╫ц
        MonsterAI monsterAI = new MonsterAI()
        {
            StopMoveTime = _stopMoveTime,

            PatrolSpeed = _moveSpeed,
            ChaseSpeed = _chaseSpeed,
            ReturnSpeed = _returnSpeed,

            NormalCheckDistance = _checkDistance,
            BattleCheckDistance = _checkDistanceOnBattle,
            AttackDistance = _attackDistance,
            ChaseDistance = _chaseDistance
        };

        MonsterInfo monsterInfo = new MonsterInfo()
        {
            Type = _monsterType,
            MaxHp = _maxHealthPoint
        };

        base.Init(monsterInfo, monsterAI, target);

        InitStateFunction();
    }

    protected override void Update()
    {
        base.Update();
    }
}
