using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Skill_Arrow : Skill
{
    [Header("Skill Parameter")]
    [SerializeField] float _chargeTime = 1f;
    [SerializeField] float _chargeSpped = 1f;
    [SerializeField] float _moveTime = 2f;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _shootDiffTime = 0.2f;

    [Header("Arrow Parameter")]
    [SerializeField] Vector3[] _arrowStartPos;

    int _arrowIndex = 0;

    SArrow[] _arrows = new SArrow[3];

    public void TargetHit(GameObject targetGameObject)
    {
        if (_targetTag == "Player")
        {
            // Player Damaged
        }
        else if (_targetTag == "Monster" || _targetTag == "EnemyMonster")
        {
            targetGameObject.GetComponent<Monster>().Damaged(1);
        }
    }

    public override void InitSkill()
    {
        _skillType = SkillType.Skill_Arrow;


        for (int i = 0; i < 3; i++)
        {
            _arrows[i] = transform.GetChild(i).GetComponent<SArrow>();
            _arrows[i].InitSetArrow(_chargeTime, _chargeSpped, _moveTime, _moveSpeed, _arrowStartPos[i], this);
        }

        base.InitSkill();

        SetStateUpadte(SkillState.Charge, delegate ()
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _chargeTime)
            {
                _checkTime = 0;
                ChangeState(SkillState.Attack);
            }
        });

        SetStateUpadte(SkillState.Attack, delegate ()
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _shootDiffTime)
            {
                _arrows[_arrowIndex++].MoveToTarget();
                _checkTime = 0;

                if (_arrowIndex >= 3)
                {
                    ChangeState(SkillState.Finish);
                }
            }
        });

        SetStateUpadte(SkillState.Finish, delegate ()
        {
            bool isFinish = true;

            for (int i = 0; i < 3; i++)
            {
                if (!_arrows[i]._IsAttackFinish)
                    isFinish = false;
            }

            if (isFinish)
                FinishSkill();
        });


        SetStateEnter(SkillState.Charge, delegate ()
        {
            for (int i = 0; i < 3; i++)
            {
                _arrows[i].MoveToStartPos();
                _arrows[i].SetTarget(_target);
            }
        });
    }

    protected override void Initialize()
    {
        base.Initialize();

        _arrowIndex = 0;

        for (int i = 0; i < 3; i++)
            _arrows[i].Init();
    }
}
