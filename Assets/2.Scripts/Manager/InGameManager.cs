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
        Pause,
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
    public bool _IsPause { get { return _currentState == InGameState.Pause; } }

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
        _currentState = InGameState.Pause;
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

        GameObject prefab = Resources.Load("Player") as GameObject;

        if (prefab == null)
            Debug.Log("Player prefab is null");

        _playerGO = Instantiate(prefab, _playerStartPoint.position, _playerStartPoint.rotation);
        _player = _playerGO.GetComponent<PlayerController>();
        _player.Init();

        //=============================


        // Managers ===================

        string path = "InGameManagers/";

        prefab = Resources.Load(path + "TableManager") as GameObject;

        if (prefab == null)
            Debug.Log("TableManager prefab is null");

        _tableManager = Instantiate(prefab, transform).GetComponent<GameTableManager>();
        _tableManager.AllLoadTables();



        prefab = Resources.Load(path + "GameUI") as GameObject;

        if (prefab == null)
            Debug.Log("GameUI prefab is null");

        _uiManager = Instantiate(prefab).GetComponent<UIManager>();
        _uiManager.gameObject.SetActive(true);
        _uiManager.Init();

        _playerManager = _playerGO.GetComponent<PlayerManager>();
        _playerManager.Init();



        prefab = Resources.Load(path + "MonsterManager") as GameObject;

        if (prefab == null)
            Debug.Log("MonsterManager prefab is null");

        _monsterManager = Instantiate(prefab, transform).GetComponent<MonsterManager>();
        _monsterManager.InitMonsterManager();



        prefab = Resources.Load(path + "ProjectileManager") as GameObject;

        if (prefab == null)
            Debug.Log("ProjectileManager prefab is null");

        _projectileManager = Instantiate(prefab, transform).GetComponent<ProjectileManager>();
        _projectileManager.Init();



        prefab = Resources.Load(path + "SkillManager") as GameObject;

        if (prefab == null)
            Debug.Log("SkillManager prefab is null");

        _skillManager = Instantiate(prefab, transform).GetComponent<SkillManager>();
        _skillManager.InitSkillManager();



        prefab = Resources.Load(path + "EffectManger") as GameObject;

        if (prefab == null)
            Debug.Log("EffectManger prefab is null");

        _effectManager = Instantiate(prefab).GetComponent<EffectManager>();
        _effectManager.gameObject.SetActive(true);
        _effectManager.Init();

        _uiManager.LateInit();

        _camera.transform.position = _player.transform.position + (Vector3.up * 3);

        path = "SpawnFactory/";
        int monsterTypeCount = (int)MonsterType.Max;
        GameObject map = GameObject.FindGameObjectWithTag("Map");

        _spawnManagers = new MonsterSpawnManager[monsterTypeCount + 1];

        for (int i = 1; i < monsterTypeCount; i++)
        {
            prefab = Resources.Load(path + "SpawnFactory_" + ((MonsterType)i).ToString()) as GameObject;

            if (prefab == null)
                Debug.Log("SpawnFactory_" + ((MonsterType)i).ToString() + "prefab is null");

            Transform point = map.transform.GetChild(2).GetChild(i - 1);

            MonsterSpawnManager monsterSpawnFactory = Instantiate(prefab, point.position, point.rotation).GetComponent<MonsterSpawnManager>();
            monsterSpawnFactory.transform.SetParent(transform);
            monsterSpawnFactory.InitSpawnManager();

            _spawnManagers[i] = monsterSpawnFactory;
        }

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
