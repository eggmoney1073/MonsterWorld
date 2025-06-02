using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Skill_Cross : Skill
{
    [Header("Cross Parameter")]
    [SerializeField] int _hitCount;
    [SerializeField] float _hitTerm;
    [SerializeField] float _chargeTime = 1f;

    int _count;

    GameObject _hitGO;
    List<GameObject> _targets;
    Collider _collider;

    public override void InitSkill()
    {
        _skillType = SkillType.Skill_Cross;

        _hitGO = transform.GetChild(0).gameObject;
        _collider = GetComponent<Collider>();

        _targets = new List<GameObject>();

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
            _hitGO.SetActive(false);
            _checkTime += Time.deltaTime;
            if (_checkTime > _hitTerm)
            {
                _hitGO.SetActive(true);
                DamageAll();

                _count++;
                _checkTime = 0;
                if (_count == _hitCount)
                {
                    ChangeState(SkillState.Finish);
                }
            }
        });


        SetStateEnter(SkillState.Charge, delegate ()
        {
            _collider.enabled = false;
        });

        SetStateEnter(SkillState.Attack, delegate ()
        {
            _hitGO.SetActive(true);
            _collider.enabled = true;
        });

        SetStateEnter(SkillState.Finish, delegate ()
        {
            _collider.enabled = false;
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

    protected override void Initialize()
    {
        _count = 0;
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
