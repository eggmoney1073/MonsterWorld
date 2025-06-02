using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Skill_Dash : Skill
{
    [Header("Dash Parameter")]
    [SerializeField] float _dashDistance = 3f;
    [SerializeField] float _chargeTime = 1f;
    [SerializeField] float _tailRemainTime = 1f;

    Vector3 _startPos;
    Vector3 _endPos;

    GameObject _dashTailGO;
    TrailRenderer _trailRenderer;


    public override void InitSkill()
    {
        _skillType = SkillType.Skill_Dash;

        _dashTailGO = transform.GetChild(0).gameObject;
        _trailRenderer = _dashTailGO.GetComponent<TrailRenderer>();

        _trailRenderer.time = _tailRemainTime;

        base.InitSkill();

        SetStateUpadte(SkillState.Charge, delegate ()
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _chargeTime)
            {
                _checkTime = 0;
                _caster.transform.position = _endPos;
                _dashTailGO.transform.position = _endPos;
                ChangeState(SkillState.Finish);
            }
        });

        SetStateUpadte(SkillState.Finish, delegate ()
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > _tailRemainTime)
            {
                _checkTime = 0;
                FinishSkill();
            }
        });
    }

    public override void StartSkill(GameObject caster)
    {
        base.StartSkill(caster);
        _startPos = _caster.transform.position;
        _endPos = _startPos + (_caster.transform.forward * _dashDistance);
        _dashTailGO.transform.localPosition = Vector3.zero;
    }

    protected override void Initialize()
    {
        base.Initialize();

    }
}
