using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;
using DefineStructs;

public class PlayerManager : SingletonGameobject<PlayerManager>
{
    #region [ Contants and Fields ]
    // ============================================================

    bool _isGameOver;
    bool _isInvincible;

    int _currentMonsterIndex;
    int _level;
    int[] _capturedMonsterLevel;

    float _exp;
    float _requiredEXP;
    float _currentHP;
    float _maxHP;
    float _attack;
    float _capturePower;

    CameraController _cameraController;
    PlayerController _playerController;
    TableBase _playerLevelTable;
    TableBase _playerWeaponTable;

    MonsterType[] _capturedMonsters;

    PlayerData _playerData;

    public bool _IsInvincible { get { return _isInvincible; } }

    public PlayerData _PlayerData
    {
        get 
        {
            _playerData.Level = _level;
            _playerData.EXP = _exp;
            _playerData.Position = transform.position;
            return _playerData; 
        }
    }

    public float _CapturePower { get { return _capturePower; } }

    // ============================================================
    #endregion

    #region [ Public Method ]
    // ============================================================

    public float WeaponDamage(PlayerWeapon weapon)
    {
        if (weapon == PlayerWeapon.Ball)
            return 0;

        float scale = _playerWeaponTable.ToF((int)weapon + 1, PlayerWeaponTable.Index.Damage_Scale.ToString());

        return _attack * scale;
    }


    public void SpawnPlayerMonster()
    {
        if (_capturedMonsters[_currentMonsterIndex] == MonsterType.None)
            return;

        MonsterManager._Instance.SpawnPlayerMonster(_capturedMonsters[_currentMonsterIndex], _capturedMonsterLevel[_currentMonsterIndex]);
    }

    public void SetCapturedMonster(MonsterType type, int level)
    {
        int index = 0;
        MonsterType monster = _capturedMonsters[index];

        while(index < 5 && monster != MonsterType.None)
        {
            monster = _capturedMonsters[index];
            index++;
            Debug.Log(index);
        }

        if(index == 5)
        {
            // Open capture window and Select Discard Monster
        }
        else
        {
            index--;

            _capturedMonsters[index] = type;
            _capturedMonsterLevel[index] = level;

            _playerData.Monster_Type[index] = type;
            if (type != MonsterType.None)
                UIManager._Instance.GetNewMonster(type);
            _playerData.Monster_Level[index] = level;
        }
    }

    public void ChangeMonster(int index)
    {
        _currentMonsterIndex = index;
        UIManager._Instance.ChangeMonster(_capturedMonsters[_currentMonsterIndex]);
    }

    public int GetPlayerMonsterLevel(int index)
    {
        return _capturedMonsterLevel[index];
    }

    public MonsterType GetPlayerMonsterType(int index)
    {
        return _capturedMonsters[index];
    }

    public void GetEXP(float exp)
    {
        _exp += exp;

        if (_exp >= _requiredEXP)
        {
            _exp = _exp - _requiredEXP;
            _level++;
            EffectManager._Instance.StartEffect(EffectManager.OtherEffectName.LevelUp, transform.position);
            SetLevelData(_level);
        }
        SetEXP();
    }

    public PlayerData GetPlayerData()
    {
        PlayerData data = new PlayerData()
        {
            Level = _level,
            EXP = _exp,
            Position = transform.position,

            Monster_Type = new MonsterType[5],
            Monster_Level = new int[5]
        };

        for (int i = 0; i < 5; i++)
        {
            _capturedMonsters[i] = data.Monster_Type[i];
            _capturedMonsterLevel[i] = data.Monster_Level[i];
        }

        return data;
    }

    public void Init()
    {
        _isGameOver = false;

        //test
        //_isInvincible = true;

        _playerData = DataManager._Instance._CurrentData;
        _cameraController = Camera.main.GetComponent<CameraController>();

        _level = _playerData.Level;

        if (_level == 0)
            _level = 1;

        transform.position = _playerData.Position;

        _capturedMonsters = new MonsterType[5];
        _capturedMonsterLevel = new int[5];
        _exp = _playerData.EXP;

        for (int i = 0; i < 5; i++)
        {
            _capturedMonsters[i] = _playerData.Monster_Type[i];
            //Debug.Log(_playerData.Monster_Type[i]);
            _capturedMonsterLevel[i] = _playerData.Monster_Level[i];
            //Debug.Log(_playerData.Monster_Level[i]);

            UIManager._Instance.GetNewMonster(_playerData.Monster_Type[i]);
        }

        _playerController = GetComponent<PlayerController>();
        _playerLevelTable = GameTableManager._Instance.Get(TableName.PlayerLevelTable);
        _playerWeaponTable = GameTableManager._Instance.Get(TableName.PlayerWeaponTable);


        SetLevelData(_level);

        _currentHP = _maxHP;

        InitUI();
    }

    public void Damaged(float damage)
    {
        if (_isGameOver || _isInvincible)
            return;

        _currentHP -= damage;
        SetHP();
        _cameraController.ShakeCamera(0.5f);

        if (_currentHP <= 0)
        {
            // Dead
            Debug.Log("Player Dead");
            PlayerDead();
        }
    }

    public void Respawn()
    {
        _isGameOver = false;
        _isInvincible = true;
        InGameManager._Instance.Respawn();
        _playerController.PlayerRespawnAnimation();
        UIManager._Instance.Respawn();

        _currentHP = _maxHP;
        SetHP();

        StartCoroutine(Co_Invincilbe(3f));
    }

    public void InitUI()
    {
        SetHP();
        SetEXP();
        UIManager._Instance._PlayerUI.SetLevel(_level);
    }

    // ============================================================
    #endregion

    #region [ Method ]
    // ============================================================

    IEnumerator Co_Invincilbe(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            yield return null;
        }
        _isInvincible = false;
    }

    void SetHP()
    {
        float hpPercentage = _currentHP / _maxHP;
        UIManager._Instance._PlayerUI.SetHp(hpPercentage);
    }

    void SetEXP()
    {
        float expPercentage = _exp / _requiredEXP;
        UIManager._Instance._PlayerUI.SetEXp(expPercentage);
    }

    void SetLevelData(int level)
    {
        _requiredEXP = _playerLevelTable.ToF(level, PlayerLevelTable.Index.Required_EXP.ToString());
        _maxHP = _playerLevelTable.ToF(level, PlayerLevelTable.Index.Health.ToString());
        _capturePower = _playerLevelTable.ToF(level, PlayerLevelTable.Index.CapturePower.ToString());
        _attack = _playerLevelTable.ToF(level, PlayerLevelTable.Index.Attack.ToString());

        UIManager._Instance._PlayerUI.SetLevel(level);
    }

    void PlayerDead()
    {
        _isGameOver = true;
        UIManager._Instance.ShowGameOverUI(GameOverUI.GameOver);
        InGameManager._Instance.GameOver();
        _playerController.PlayerDeadAnimation();
    }

    // ============================================================
    #endregion
}
