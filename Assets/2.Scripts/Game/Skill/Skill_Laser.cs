using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class Skill_Laser : Skill
{
    [Header("Laser Parameter")]
    [SerializeField] float _chargeTime = 1f;
    [SerializeField] float _chargeLength = 0.5f;
    [SerializeField] float _laserTime = 3f;
    [SerializeField] float _laserLength = 3f;
    [SerializeField] float _hitTerm = 0.6f;
    [SerializeField] float _turnSpeed = 3;

    float _checkTerm;

    SLaser _laser;
    Transform _player;
    GameObject _laserGO;
    Transform _lookTarget;

    public override void InitSkill()
    {
        _laserGO = transform.GetChild(0).gameObject;
        _lookTarget = transform.GetChild(1);

        _laser = _laserGO.GetComponent<SLaser>();
        _laser.Init();

        _player = MonsterManager._Instance._Player.transform;

        _skillType = SkillType.Skill_Laser;

        base.InitSkill();

        SetStateUpadte(SkillState.Charge, delegate ()
        {
            FollowTarget();

            _checkTime += Time.deltaTime;
            if (_checkTime > _chargeTime)
            {
                _checkTime = 0;
                _laserGO.transform.localPosition = new Vector3(0, 0, _laserLength);
                _laserGO.transform.localScale = new Vector3(1, _laserLength, 1);
                ChangeState(SkillState.Attack);
            }
        });

        SetStateUpadte(SkillState.Attack, delegate ()
        {
            FollowTarget();

            _checkTime += Time.deltaTime;
            _checkTerm += Time.deltaTime;

            if (_checkTerm > _hitTerm)
            {
                _checkTerm = 0;
                List<GameObject> targetList = _laser._Target;
                int targetCount = targetList.Count;

                for (int i = 0; i < targetCount; i ++)
                {
                    if (_targetTag == "Player")
                    {
                        PlayerManager._Instance.Damaged(_caster.GetComponent<Monster>()._Attack);
                    }
                    else
                    {
                        Monster monster = targetList[i].GetComponent<Monster>();
                        if (monster != null)
                            monster.Damaged(1);
                    }
                }
            }

            if (_checkTime > _laserTime)
            {
                _checkTime = 0;
                ChangeState(SkillState.Finish);
            }
        });

        SetStateEnter(SkillState.Charge, delegate ()
        {
            _laser.SetLaserTarget(_targetTag);
        });

        SetStateEnter(SkillState.Finish, delegate ()
        {
            FinishSkill();
        });
    }

    void FollowTarget()
    {
        _lookTarget.LookAt(_player);

        if (_caster.CompareTag("EnemyMonster") || _caster.CompareTag("PlayerMonster"))
            _caster.transform.rotation = Quaternion.Lerp(_caster.transform.rotation, _lookTarget.transform.rotation, _turnSpeed * Time.deltaTime);

        transform.position = _caster.transform.position;
        transform.rotation = _caster.transform.rotation;
    }

    protected override void Initialize()
    {
        base.Initialize();
        _checkTerm = 0;
        _laserGO.transform.localPosition = new Vector3(0, 0, _chargeLength);
        _laserGO.transform.localScale = new Vector3(1, _chargeLength, 1);
    }
}
