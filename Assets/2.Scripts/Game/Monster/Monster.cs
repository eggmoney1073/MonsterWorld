using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineEnums;
using DefineStructs;

public class Monster : StateMachineBase<MonsterState>
{
    public struct MonsterAI
    {
        public float StopMoveTime;

        public float PatrolSpeed;
        public float ChaseSpeed;
        public float ReturnSpeed;

        public float NormalCheckDistance;
        public float BattleCheckDistance;
        public float AttackDistance;

        public float ChaseDistance;
    }

    #region [ Fields ]

    const float _refreshDestinationOnChase = 0.5f;
    const float _checkTerm = 0.2f;

    protected bool _isAlive;
    protected bool _isOnBattle;

    protected bool _isFriend = false;

    protected int _patrolIndex = 0;
    protected int _skillIndex = 0;
    protected int _level;
    protected int _spawnID;

    protected float _currentHp;
    protected float _maxHp;
    protected float _attack;

    protected float _capturePercentage;
    protected float _checkTime;
    protected float _stateTime;

    protected Vector3 _startPos;

    protected MonsterType _type;

    protected GameObject _target;
    protected GameObject _monsterModel;
    protected GameObject _player;
    protected Transform[] _patrolsTFs;
    protected Animator _animator;
    protected NavMeshAgent _agent;

    protected UIMonsterState _uiMonsterState;
    protected MonsterSpawnManager _monsterSpawnFactory;

    protected MonsterAI _monsterAI;
    protected MonsterData _monsterData;
    protected MonsterLevelData _monsterLevelData;

    protected Dictionary<MonsterState, StateFunction> _friendStateEnter;
    protected Dictionary<MonsterState, StateFunction> _friendStateUpdate;
    protected Dictionary<MonsterState, StateFunction> _friendStateExit;

    #endregion

    #region [ Properties ]
    public bool _IsAlive { get { return _isAlive; } }
    public bool _IsFriend { get { return _isFriend; } }
    public float _Attack { get { return _attack; } }
    public float _CurrentHp { get { return _currentHp; } }
    public float _CapturePercentage { get { return _capturePercentage; } }
    public MonsterType _Type { get { return _type; } }
    public GameObject _Target { get { return _target; } set { _target = value; } }
    public int _SpawnID { get { return _spawnID; } }

    #endregion

    public virtual void CommonInitialize()
    {
        InitStateMachine();

        _friendStateEnter = new Dictionary<MonsterState, StateFunction>();
        _friendStateUpdate = new Dictionary<MonsterState, StateFunction>();
        _friendStateExit = new Dictionary<MonsterState, StateFunction>();

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _monsterModel = transform.GetChild(0).gameObject;

        _uiMonsterState = transform.GetChild(2).GetComponent<UIMonsterState>();        
    }

    public virtual void SetMonsterData(MonsterType type, int level, GameObject target)
    {
        _type = type;
        _level = level;

        _monsterData = MonsterManager._Instance.GetMonsterData(_type);
        _monsterLevelData = MonsterManager._Instance.GetMonsterLevelData(_level);

        _maxHp = _monsterLevelData.Health * _monsterData.HealthScale;
        _attack = _monsterLevelData.Attack * _monsterData.AttackScale;
        _currentHp = _maxHp;
    }

    public virtual void SetMonsterAI(MonsterAI aiInfo, GameObject target)
    {
        _monsterAI = aiInfo;
        _target = target;       

        _player = InGameManager._Instance._Player;

        ResetMonster();
    }

    public void StartBattle()
    {
        _isOnBattle = true;
        if (!_isFriend)
        {
            MonsterManager._Instance.SetEnemyOnBattle(this);
            gameObject.tag = "EnemyMonster";
        }
        else
            ChangeState(MonsterState.Chase);
    }

    public void EndBattle()
    {
        _isOnBattle = false;
        if (!_isFriend)
        {
            MonsterManager._Instance.RemoveEnemyOnBattle(this);
            gameObject.tag = "Monster";
        }
        else
            ChangeState(MonsterState.Idle);
    }

    public void SpawnEnemyMonster(int spawndID, int startIndex, Transform[] patrols, MonsterSpawnManager spawnFactory)
    {
        _spawnID = spawndID;
        _monsterSpawnFactory = spawnFactory;
        _isAlive = true;
        _isFriend = false;
        gameObject.SetActive(true);
        _monsterModel.SetActive(true);
        _patrolsTFs = patrols;
        _patrolIndex = startIndex;

        ChangeState(MonsterState.Patrol);
    }

    public void SpawnFriendMonster(Vector3 position)
    {
        _isAlive = true;
        _isFriend = true;
        _agent.enabled = false;
        transform.position = position;
        _agent.enabled = true;
        gameObject.SetActive(true);
    }

    public void Damaged(float damage)
    {
        _currentHp -= damage;
        _uiMonsterState.SetHpPercentage(_currentHp / _maxHp);
        float capturePower = PlayerManager._Instance._CapturePower;
        _capturePercentage = capturePower / (_monsterLevelData.RequiredCaputrePower + (10 * (_currentHp / _maxHp)));
        //Debug.LogFormat("Player : {0}",_playerCapturePower);
        //Debug.LogFormat("_capturePercentage : {0}", _capturePercentage);
        _uiMonsterState.SetCaputrePercentage(_capturePercentage);

        if (_isAlive && !_isOnBattle)
            ChangeState(MonsterState.Chase);

        if (_currentHp <= 0)
        {
            if (!_isFriend)
            {
                Dead();
            }
        }
    }

    public void HitBall()
    {
        _isAlive = false;
        _monsterModel.SetActive(false);
        EffectManager._Instance.StartEffect(EffectManager.OtherEffectName.HitBall, transform.position);
    }

    public void EscapeBall()
    {
        _isAlive = true;
        _monsterModel.SetActive(true);
    }

    public void Captured()
    {
        Dead();
        PlayerManager._Instance.SaveCapturedMonster(_type, _level);
    }

    public void Dead()
    {
        ChangeState(MonsterState.Return);
        ResetMonster();
        MonsterManager._Instance.ReturnMonster(this);
        PlayerManager._Instance.GetEXP(_monsterLevelData.DropEXP);
        _monsterSpawnFactory.MonsterDead(this);
    }

    public void Despawn()
    {
        ChangeState(MonsterState.Return);
        ResetMonster();
        MonsterManager._Instance.ReturnMonster(this);
    }

    protected void ResetMonster()
    {
        _isAlive = false;

        _currentHp = _maxHp;

        _checkTime = 0;
        _stateTime = 0;

        _prevState = MonsterState.Idle;
        _currentState = MonsterState.Idle;

        _capturePercentage = PlayerManager._Instance._CapturePower / _monsterLevelData.RequiredCaputrePower;
        _uiMonsterState.Init(_player.transform, _type.ToString(), _level);
        _uiMonsterState.SetCaputrePercentage(_capturePercentage);
        _uiMonsterState.ResetUIMonsterState(_level);
    }

    protected void InitStateFunction()
    {
        // Enemy
        SetState(_stateEnter, MonsterState.Idle, () =>
        {
            _animator.SetBool("IsMove", false);
            _animator.SetBool("IsBattle", false);

            _agent.ResetPath();
        });
        SetState(_stateEnter, MonsterState.Patrol, () =>
        {
            _animator.SetBool("IsMove", true);

            Vector3 destination;

            destination = _patrolsTFs[_patrolIndex].position;

            _agent.speed = _monsterAI.PatrolSpeed;
            _agent.ResetPath();
            _agent.SetDestination(destination);
        });
        SetState(_stateEnter, MonsterState.Chase, () =>
        {
            _animator.SetBool("IsMove", true);
            _animator.SetBool("IsBattle", true);

            Vector3 destination = _target.transform.position;

            _startPos = transform.position;

            _agent.ResetPath();
            _agent.speed = _monsterAI.ChaseSpeed;
            _agent.SetDestination(destination);

            if (!_isOnBattle)
                StartBattle();
        });
        SetState(_stateEnter, MonsterState.Attack, () =>
        {
            _animator.SetBool("IsMove", false);

            if (_skillIndex == 0)
            {
                _animator.SetTrigger("Skill1");
                _skillIndex++;
            }
            else
            {
                _animator.SetTrigger("Skill2");
                _skillIndex++;
            }

            if (_skillIndex > 1)
                _skillIndex = 0;

            _agent.ResetPath();
            _agent.isStopped = true;


            if (_skillIndex == 0)
                SkillManager._Instance.ActiveSkill(_monsterData.Skill1, gameObject, _target.transform);
            else
                SkillManager._Instance.ActiveSkill(_monsterData.Skill2, gameObject, _target.transform);
        });
        SetState(_stateEnter, MonsterState.Return, () =>
        {
            Vector3 destination;

            destination = _patrolsTFs[_patrolIndex].position;
            EndBattle();

            _agent.speed = _monsterAI.ReturnSpeed;
            _agent.ResetPath();
            _agent.SetDestination(destination);
            transform.LookAt(destination);
        });

        SetState(_stateUpdates, MonsterState.Idle, () =>
        {
            _stateTime += Time.deltaTime;
            _checkTime = _stateTime;

            if (_stateTime > _monsterAI.StopMoveTime)
            {
                _stateTime = 0;
                ChangeState(MonsterState.Patrol);
            }

            if (_prevState == MonsterState.Attack)
                CheckTarget(_monsterAI.BattleCheckDistance);
            else
                CheckTarget(_monsterAI.NormalCheckDistance);
        });
        SetState(_stateUpdates, MonsterState.Patrol, () =>
        {
            Vector3 destination = _patrolsTFs[_patrolIndex].position;

            transform.LookAt(destination);

            if (_agent.desiredVelocity == Vector3.zero)
            {
                _patrolIndex++;
                if (_patrolIndex >= _patrolsTFs.Length)
                    _patrolIndex = 0;

                ChangeState(MonsterState.Idle);
            }

            CheckTarget(_monsterAI.NormalCheckDistance);
        });
        SetState(_stateUpdates, MonsterState.Chase, () =>
        {
            _checkTime += Time.deltaTime;

            if (_checkTime > _refreshDestinationOnChase)
            {
                Vector3 destination = _target.transform.position;

                _checkTime = 0;

                _agent.ResetPath();
                _agent.SetDestination(destination);

                if (Vector3.Distance(transform.position, _target.transform.position) < _monsterAI.AttackDistance)
                    ChangeState(MonsterState.Attack);

                transform.LookAt(destination);
            }

            if (Vector3.Distance(transform.position, _startPos) > _monsterAI.ChaseDistance)
                ChangeState(MonsterState.Return);
        });
        SetState(_stateUpdates, MonsterState.Attack, () =>
        {
            _checkTime += Time.deltaTime;

            if (_checkTime > 3)
            {
                _checkTime = 0;

                if (Vector3.Distance(transform.position, _target.transform.position) < _monsterAI.AttackDistance)
                    _stateEnter[MonsterState.Attack]();
                else
                    ChangeState(MonsterState.Chase);

                if (Vector3.Distance(transform.position, _startPos) > _monsterAI.ChaseDistance)
                    ChangeState(MonsterState.Return);
            }
        });
        SetState(_stateUpdates, MonsterState.Return, () =>
        {
            if (_agent.desiredVelocity == Vector3.zero)
            {
                _patrolIndex++;
                if (_patrolIndex >= _patrolsTFs.Length)
                    _patrolIndex = 0;

                ChangeState(MonsterState.Idle);
            }
        });

        // Friend
        SetState(_friendStateEnter, MonsterState.Idle, () =>
        {
            _animator.SetBool("IsMove", false);
            _animator.SetBool("IsBattle", false);

            _agent.ResetPath();
        });
        SetState(_friendStateEnter, MonsterState.Patrol, () =>
        {
            _animator.SetBool("IsMove", true);

            Vector3 destination;
            destination = _player.transform.position;

            _agent.speed = _monsterAI.PatrolSpeed;
            _agent.ResetPath();
            _agent.SetDestination(destination);
        });
        SetState(_friendStateEnter, MonsterState.Chase, () =>
        {
            _animator.SetBool("IsMove", true);
            _animator.SetBool("IsBattle", true);

            Vector3 destination = _target.transform.position;

            _startPos = transform.position;

            _agent.ResetPath();
            _agent.speed = _monsterAI.ChaseSpeed;
            _agent.SetDestination(destination);
        });
        SetState(_friendStateEnter, MonsterState.Attack, () =>
        {
            _animator.SetBool("IsMove", false);

            if (_skillIndex == 0)
            {
                _animator.SetTrigger("Skill1");
                _skillIndex++;
            }
            else
            {
                _animator.SetTrigger("Skill2");
                _skillIndex++;
            }

            if (_skillIndex > 1)
                _skillIndex = 0;

            _agent.ResetPath();
            _agent.isStopped = true;


            if (_skillIndex == 0)
                SkillManager._Instance.ActiveSkill(_monsterData.Skill1, gameObject, _target.transform);
            else
                SkillManager._Instance.ActiveSkill(_monsterData.Skill2, gameObject, _target.transform);
        });
        SetState(_friendStateEnter, MonsterState.Return, () =>
        {
            Vector3 destination;

            destination = _player.transform.position;

            _agent.speed = _monsterAI.ReturnSpeed;
            _agent.ResetPath();
            _agent.SetDestination(destination);
            transform.LookAt(destination);
        });

        SetState(_friendStateUpdate, MonsterState.Idle, () =>
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > 3)
                ChangeState(MonsterState.Patrol);
        });
        SetState(_friendStateUpdate, MonsterState.Patrol, () =>
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < 2)
                ChangeState(MonsterState.Idle);


            _checkTime += Time.deltaTime;
            if (_checkTime > _checkTerm)
            {
                if (Vector3.Distance(_player.transform.position, transform.position) > 3)
                {
                    _checkTime = 0;
                    _agent.SetDestination(_player.transform.position);
                }
            }
        });
        SetState(_friendStateUpdate, MonsterState.Chase, () =>
        {
            _checkTime += Time.deltaTime;

            if (_checkTime > _refreshDestinationOnChase)
            {
                Vector3 destination = _target.transform.position;

                _checkTime = 0;

                _agent.ResetPath();
                _agent.SetDestination(destination);

                if (Vector3.Distance(transform.position, _target.transform.position) < _monsterAI.AttackDistance)
                    ChangeState(MonsterState.Attack);

                transform.LookAt(destination);
            }
        });
        SetState(_friendStateUpdate, MonsterState.Attack, () =>
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > 3)
            {
                _checkTime = 0;
                if (Vector3.Distance(transform.position, _target.transform.position) < _monsterAI.AttackDistance)
                    _friendStateEnter[MonsterState.Attack]();
                else
                    ChangeState(MonsterState.Chase);
            }
        });
        SetState(_friendStateUpdate, MonsterState.Return, () =>
        {
            if (_agent.desiredVelocity == Vector3.zero)
            {
                ChangeState(MonsterState.Idle);
            }
        });
    }

    protected override void ChangeState(MonsterState state)
    {
        _checkTime = 0;
        _stateTime = 0;

        if (_isFriend)
        {
            _prevState = _currentState;
            _currentState = state;

            if (_friendStateExit.ContainsKey(_prevState))
                _friendStateExit[_prevState]();

            if (_friendStateEnter.ContainsKey(_currentState))
                _friendStateEnter[_currentState]();

            return;
        }
        base.ChangeState(state);
    }

    protected void CheckTarget(float distance, MonsterState nextState = MonsterState.Chase)
    {
        _checkTime += Time.deltaTime;

        if (_checkTime > _checkTerm)
        {
            _checkTime = 0;

            if (_target != null && Vector3.Distance(transform.position, _target.transform.position) < distance)
                ChangeState(nextState);
        }
    }

    protected override void Update()
    {
        if (_isFriend)
        {
            if (_friendStateUpdate.ContainsKey(_currentState))
                _friendStateUpdate[_currentState]();

            return;
        }

        if (_isAlive && _stateUpdates.ContainsKey(_currentState))
            _stateUpdates[_currentState]();
    }

    private void OnDrawGizmos()
    {
        if (!_isOnBattle)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _monsterAI.NormalCheckDistance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _monsterAI.BattleCheckDistance);
        }
    }
}
