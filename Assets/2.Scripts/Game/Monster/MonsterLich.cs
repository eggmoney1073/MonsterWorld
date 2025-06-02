using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class MonsterLich : Monster
{
    [Header("Lich Setting")]
    [SerializeField] float _checkDistance = 5;
    [SerializeField] float _attackDistance = 3;
    [SerializeField] float _maxHealthPoint = 3;
    [SerializeField] MonsterType _monsterType = MonsterType.Lich;

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


    public override void InitMonster(MonsterType type, int level, GameObject target)
    {
        base.InitMonster(type,level, target);

        // юс╫ц
        MonsterAI monsterAI = new MonsterAI()
        {
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

        Init(monsterInfo, monsterAI, target);
    }

    public override void Init(MonsterInfo info, MonsterAI aiInfo, GameObject target)
    {
        base.Init(info, aiInfo, target);

        SetStateUpadte(MonsterState.Idle, delegate ()
        {
            UpdateBaseIdle(_stopMoveTime);
        });

        SetStateUpadte(MonsterState.Patrol, delegate ()
        {
            UpdateBasePatrol();
        });

        SetStateUpadte(MonsterState.Chase, delegate ()
        {
            UpdateBaseChase();
        });

        SetStateUpadte(MonsterState.Attack, delegate ()
        {
            UpdateBaseAttack();
        });

        SetStateUpadte(MonsterState.Return, delegate ()
        {
            UpdateBaseReturn();
        });




        SetStateEnter(MonsterState.Idle, delegate ()
        {
            EnterBaseIdle();
        });

        SetStateEnter(MonsterState.Patrol, delegate ()
        {
            EnterBasePatrol();
        });

        SetStateEnter(MonsterState.Chase, delegate ()
        {
            EnterBaseChase();
        });

        SetStateEnter(MonsterState.Attack, delegate ()
        {
            EnterBaseAttack();

            if (_skillIndex == 0)
                ActiveSkill(_skill2Type);
            else
                ActiveSkill(_skill1Type);
        });

        SetStateEnter(MonsterState.Return, delegate ()
        {
            EnterBaseReturn(_returnSpeed);
        });
    }

    protected override void Update()
    {
        base.Update();
    }
}
