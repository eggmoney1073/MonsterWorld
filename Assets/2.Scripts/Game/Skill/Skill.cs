using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Skill : StateMachineBase<SkillState>
{
    protected bool _isSkillFinish;

    protected float _checkTime;

    protected string _targetTag;

    protected SkillType _skillType;

    protected Transform _target;
    protected GameObject _caster;

    public bool _IsSkillFinish { get { return _isSkillFinish; } }
    public SkillType _SkillType { get { return _skillType; } }

    public void SetTarget(Transform target)
    {
        _target = target;
        _targetTag = target.tag;
    }

    public virtual void InitSkill()
    {
        InitStateMachine();

        Initialize();
    }

    public virtual void StartSkill(GameObject caster)
    {
        _caster = caster;
        transform.position = _caster.transform.position;
        transform.rotation = _caster.transform.rotation;
        ChangeState(SkillState.Charge);
    }

    protected virtual void FinishSkill()
    {
        SkillManager._Instance.ReturnSkill(this);
        transform.parent = SkillManager._Instance.transform;
        Initialize();
        gameObject.SetActive(false);
        ChangeState(SkillState.Init);
    }

    protected virtual void Initialize()
    {
        _isSkillFinish = false;
        _checkTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            //FinishSkill();
            Debug.Log(other.gameObject.name);
        }
    }
}
