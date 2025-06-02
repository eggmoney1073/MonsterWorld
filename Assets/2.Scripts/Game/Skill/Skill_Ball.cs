using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Skill_Ball : Skill
{
    [Header("Ball Parameter")]
    [SerializeField] float _initScale;
    [SerializeField] float _lastScale;
    [SerializeField] float _chargeTime = 3f;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _followwTime;
    [SerializeField] float _lastMoveTime;
    
    float _scaleDiff;

    Vector3 _startScale;
    Vector3 _finalScale;
    Vector3 _lastMoveDir;

    Transform _ball;
    Collider _collider;

    public override void InitSkill()
    {
        _skillType = SkillType.Skill_Ball;

        _ball = transform.GetChild(0).GetComponent<Transform>();
        _collider = _ball.GetComponent<Collider>();

        _ball.gameObject.SetActive(false);
        float diff = (_lastScale - _initScale) / _chargeTime;
        _scaleDiff = diff;
        _startScale = new Vector3(_initScale, _initScale, _initScale);
        _finalScale = new Vector3(_lastScale, _lastScale, _lastScale);

        base.InitSkill();

        SetStateUpadte(SkillState.Charge, delegate ()
        {
            _checkTime += Time.deltaTime;
            _ball.transform.localScale = Vector3.MoveTowards(_ball.transform.localScale, _finalScale, _scaleDiff * Time.deltaTime);
            if (_checkTime > _chargeTime)
            {
                _ball.transform.localScale = new Vector3(_lastScale, _lastScale, _lastScale);
                _checkTime = 0;
                ChangeState(SkillState.Attack);
                _isSkillFinish = true;
            }
        });

        SetStateUpadte(SkillState.Attack, delegate ()
        {
            _checkTime += Time.deltaTime;
            _ball.transform.position = Vector3.MoveTowards(_ball.transform.position, _target.position, _moveSpeed * Time.deltaTime);
            if (_checkTime > _followwTime)
            {
                _checkTime = 0;
                _lastMoveDir = _target.position - _ball.transform.position;
                _lastMoveDir.Normalize();
                ChangeState(SkillState.Finish);
            }
        });

        SetStateUpadte(SkillState.Finish, delegate ()
        {
            _checkTime += Time.deltaTime;
            _ball.transform.position = Vector3.MoveTowards(_ball.transform.position, _ball.transform.position + _lastMoveDir, _moveSpeed * Time.deltaTime);
            if (_checkTime > _lastMoveTime)
            {
                _checkTime = 0;
                FinishSkill();
            }
        });

        SetStateEnter(SkillState.Charge, delegate ()
        {
            _ball.gameObject.SetActive(true);
        });

        SetStateEnter(SkillState.Attack, delegate ()
        {
            _collider.enabled = true;
        });
    }

    protected override void Initialize()
    {
        base.Initialize();
        _collider.enabled = false;
        _ball.localScale = _startScale;
        _ball.transform.localPosition = Vector3.up * 3;
    }
}
