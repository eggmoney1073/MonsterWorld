using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class MonsterMushroom : Monster
{
    [Header("Mushroom Setting")]
    [SerializeField] float _checkDistance = 5;
    [SerializeField] float _attackDistance = 3;
    [SerializeField] MonsterType _monsterType = MonsterType.Mushroom;

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


    public override void SetMonsterData(MonsterType type, int level, GameObject target)
    {
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

        base.CommonInitialize();
        base.SetMonsterData(type, level, target);
        base.SetMonsterAI(monsterAI, target);

        InitStateFunction();
    }

    protected override void Update()
    {
        base.Update();
    }
}
