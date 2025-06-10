using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;
using DefineStructs;

public class InGameManager : SingletonGameobject<InGameManager>
{
    enum InGameState
    {
        Init,
        Play,
        Puase,
        GameOver,

        Max
    }

    GameTableManager _tableManager;
    SkillManager _skillManager;
    MonsterManager _monsterManager;
    ProjectileManager _projectileManager;
    UIManager _uiManager;
    EffectManager _effectManager;

    PlayerController _player;
    PlayerManager _playerManager;
    GameObject _playerGO;
    Transform _playerStartPoint;

    //GameObject[] _spawnFactories;
    MonsterSpawnManager[] _spawnManagers;

    int _factoryCount;
    InGameState _currentState;
    Camera _camera;

    public GameObject _Player { get { return _playerGO; } }
    public bool _IsPause { get { return _currentState == InGameState.Puase; } }

    public bool _IsGameOver { get { return _currentState == InGameState.GameOver; } }


    #region [ Call by Unity ]
    // ============================================================

    protected override void Awake()
    {
        base.Awake();
        _playerStartPoint = transform.GetChild(0);
    }

    void Start()
    {
        InitGameScene();
        _currentState = InGameState.Play;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion

    #region [ Public Method ]
    // ============================================================

    public void PauseGame()
    {
        SaveGame();
        _currentState = InGameState.Puase;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _currentState = InGameState.Play;
        Time.timeScale = 1;
    }

    public void CursorVisibleControl(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void GoToTitle()
    {
        LoadingManager._Instance.LoadSceneAsync(SceneState.Title);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        _currentState = InGameState.GameOver;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        _currentState = InGameState.Play;
    }

    #endregion

    void InitGameScene()
    {
        _camera = Camera.main;

        // Player =====================

        string path = "Player";
        GameObject prefab = Resources.Load(path) as GameObject;
        _playerGO = Instantiate(prefab, _playerStartPoint.position, _playerStartPoint.rotation);
        _player = _playerGO.GetComponent<PlayerController>();
        _player.Init();

        //=============================


        // Managers ===================

        path = "InGameManagers/";

        prefab = Resources.Load(path + "TableManager") as GameObject;
        _tableManager = Instantiate(prefab, transform).GetComponent<GameTableManager>();
        _tableManager.AllLoadTables();

        prefab = Resources.Load(path + "GameUI") as GameObject;
        _uiManager = Instantiate(prefab).GetComponent<UIManager>();
        _uiManager.gameObject.SetActive(true);
        _uiManager.Init();

        _playerManager = _playerGO.GetComponent<PlayerManager>();
        _playerManager.Init();

        prefab = Resources.Load(path + "MonsterManager") as GameObject;
        _monsterManager = Instantiate(prefab, transform).GetComponent<MonsterManager>();
        _monsterManager.InitMonsterManager();

        prefab = Resources.Load(path + "ProjectileManager") as GameObject;
        _projectileManager = Instantiate(prefab, transform).GetComponent<ProjectileManager>();
        _projectileManager.Init();

        prefab = Resources.Load(path + "SkillManager") as GameObject;
        _skillManager = Instantiate(prefab, transform).GetComponent<SkillManager>();
        _skillManager.InitSkillManager();

        prefab = Resources.Load(path + "EffectManger") as GameObject;
        _effectManager = Instantiate(prefab).GetComponent<EffectManager>();
        _effectManager.gameObject.SetActive(true);
        _effectManager.Init();

        _uiManager.LateInit();

        _camera.transform.position = _player.transform.position + (Vector3.up * 3);

        path = "UI/";
        prefab = Resources.Load(path + "UIInventoryCanvas") as GameObject;

        //path = "SpawnFactory/";
        //int monsterTypeCount = (int)MonsterType.Max;
        //GameObject map = GameObject.FindGameObjectWithTag("Map");

        //_spawnManagers = new MonsterSpawnManager[monsterTypeCount + 1];

        //for (int i = 1; i < monsterTypeCount; i++)
        //{
        //    prefab = Resources.Load(path + "SpawnFactory_" + ((MonsterType)i).ToString()) as GameObject;
        //    Transform point = map.transform.GetChild(2).GetChild(i - 1);
        //    _spawnManagers[i] = Instantiate(prefab, point.position, point.rotation, transform).GetComponent<MonsterSpawnManager>();
        //    _spawnManagers[i].InitSpawnManager();
        //}

        //============================


        // Camera ======================
        Camera.main.GetComponent<CameraController>().InitCamera(_playerGO);
        //==============================
    }

    public void SaveGame()
    {
        PlayerData data = PlayerManager._Instance._PlayerData;
        DataManager._Instance.SaveData(data);
    }
}
