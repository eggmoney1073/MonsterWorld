using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SArrow : MonoBehaviour
{
    enum ArrowState
    {
        Init,
        MoveToStartPos,
        MoveToTarget
    }

    bool _isAttackFinish = false;
    float _chargeTime;
    float _chargeSpped;
    float _moveMaxTime;
    float _moveSpeed;

    float _checkTime = 0;

    Vector3 _startPos;
    Vector3 _moveDir;
    public Transform _target;

    Collider _collider;
    Skill_Arrow _skillArrow;
    ArrowState _currentState = ArrowState.Init;

    public bool _IsAttackFinish {  get { return _isAttackFinish; } }

    void Update()
    {
        switch (_currentState)
        {
            case ArrowState.Init:
                break;

            case ArrowState.MoveToStartPos:
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _startPos, _chargeSpped * Time.deltaTime);
                _moveDir = _target.position - transform.position;
                break;

            case ArrowState.MoveToTarget:
                transform.position = Vector3.MoveTowards(transform.position, transform.position + _moveDir, _moveSpeed * Time.deltaTime);

                _checkTime += Time.deltaTime;
                if (_checkTime > _moveMaxTime)
                { 
                    FinishSkill();
                }
                break;
        }
    }

    public void InitSetArrow(float chargeTime, float chargeSpeed, float moveMaxTime, float moveSpeed, Vector3 startPos, Skill_Arrow skillArrow)
    {
        _collider = transform.GetChild(0).GetComponent<Collider>();

        Init();

        _chargeTime = chargeTime;
        _chargeSpped = chargeSpeed;
        _moveMaxTime = moveMaxTime;
        _moveSpeed = moveSpeed;

        _startPos = startPos;

        _skillArrow = skillArrow;
    }

    public void MoveToStartPos()
    {
        gameObject.SetActive(true);
        _currentState = ArrowState.MoveToStartPos;
    }

    public void MoveToTarget()
    {
        transform.LookAt(_target);
        
        _currentState = ArrowState.MoveToTarget;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void Init()
    {
        _isAttackFinish = false;
        _collider.enabled = true;
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));        
    }

    public void FinishSkill()
    {
        _isAttackFinish = true;
        _collider.enabled = false;
        _checkTime = 0;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_target.gameObject.tag))
        {
            FinishSkill();
            _skillArrow.TargetHit(other.gameObject);
        }
    }
}
