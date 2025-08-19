using DefineEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Skill_Turn : Skill
{
    [Header("Turn Parameter")]
    [SerializeField] float _chargeTime = 0.5f;
    [SerializeField] float _hitBoxRadius = 2f;
    [SerializeField] float _hitBoxRemainTime = 0.5f;

    GameObject _turnHitGO;
    List<GameObject> _targets;

    public override void InitSkill()
    {
        _skillType = SkillType.Skill_Turn;
        _turnHitGO = transform.GetChild(0).gameObject;
        _targets = new List<GameObject>();

        base.InitSkill();

        SetState(_stateUpdates, SkillState.Charge, delegate ()
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _chargeTime)
            {
                _checkTime = 0;
                _turnHitGO.SetActive(true);
                ChangeState(SkillState.Attack);
            }
        });
        SetState(_stateUpdates, SkillState.Attack, delegate ()
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _hitBoxRemainTime)
            {
                _checkTime = 0;
                _turnHitGO.SetActive(false);
                ChangeState(SkillState.Finish);
            }
        });
        SetState(_stateEnter, SkillState.Attack, delegate ()
        {
            DamageAll();
        });
        SetState(_stateEnter, SkillState.Finish, delegate ()
        {
            FinishSkill();
        });
    }
    void DamageAll()
    {
        int targetCount = _targets.Count;

        for (int i = 0; i < targetCount; i++)
        {
            if (_targetTag == "Player")
            {
                PlayerManager._Instance.Damaged(_caster.GetComponent<Monster>()._Attack);
            }
            else
            {
                Monster monster = _targets[i].GetComponent<Monster>();
                if (monster != null)
                    monster.Damaged(1);
            }
        }
    }

    protected override void FinishSkill()
    {
        base.FinishSkill();
        _turnHitGO.SetActive(false);
    }

    protected override void Initialize()
    {
        base.Initialize();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
            _targets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_targetTag))
            _targets.Remove(other.gameObject);
    }
}
