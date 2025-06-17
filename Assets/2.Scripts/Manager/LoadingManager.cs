
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneState
{
    None = -1,
    Title,    
    Game
}

public class LoadingManager : SingletonDontDestroyOnLoad<LoadingManager>
{
    #region Contants and Fields

    [SerializeField] Image _loadingBar;
    [SerializeField] AsyncOperation _loadingState;
    [SerializeField] GameObject _loadingBarGameObject;
    [SerializeField] Image _loadingBG;

    SceneState _currentState = SceneState.Title;
    SceneState _loadState = SceneState.None;

    float _loadingProcess;

    #endregion

    #region Public Methods and Operators

    public void LoadSceneAsync(SceneState state)
    {
        if (_loadingState != null)
            return;
        _loadState = state;
        _loadingState = SceneManager.LoadSceneAsync((int)state);
        ShowUI();
    }

    public void ShowUI()
    {
        _loadingBarGameObject.gameObject.SetActive(true);
        _loadingBG.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        _loadingBarGameObject.gameObject.SetActive(false);
        _loadingBG.gameObject.SetActive(false);
    }
    #endregion

    #region Methods

    void SetLoadingImage()
    {
        if (_loadingState != null)
        {
            if (_loadingState.isDone)
            {
                _loadingState = null;
                _loadingBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1680f);
                _currentState = _loadState;
                _loadState = SceneState.None;
                HideUI();
            }
            else
            {
                _loadingProcess = 1680f * _loadingState.progress;
                _loadingBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _loadingProcess);
            }
        }
    }
    #endregion

    #region Call by Unity

    void Start()
    {
        _loadingBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
        HideUI();
    }

    void Update()
    {
        SetLoadingImage();
    }

    #endregion
}
