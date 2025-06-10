using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMonsterState : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _levelTXT;
    [SerializeField] TextMeshProUGUI _nameTXT;
    [SerializeField] TextMeshProUGUI _percentageTXT;
    [SerializeField] Image _percentageImage;
    [SerializeField] Slider _hpSlider;

    bool _isUIShow;
    float _checkTime;
    Transform _player;
    GameObject _stateWindow;
    Monster _monster;

    public void Init(Transform player, string type, int level)
    {
        _monster = transform.parent.GetComponent<Monster>();
        _stateWindow = transform.GetChild(0).gameObject;
        _player = player;
        _nameTXT.text = type;

        ResetUIMonsterState(level);
    }

    public void SetHpPercentage(float percentage)
    {
        _hpSlider.value = percentage;
        ShowState();
    }

    public void SetCaputrePercentage(float percentage)
    {
        if (percentage < 0)
            percentage = 0;
        else if (percentage > 1)
            percentage = 1;
        
        int percent = (int)(percentage * 100);
        _percentageTXT.text = percent.ToString();
    }

    void Update()
    {
        transform.LookAt(_player);

        if(_isUIShow)
        {
            transform.position = _monster.transform.position + (Vector3.up * 3);

            _checkTime += Time.deltaTime;
            if (_checkTime > 5)
            {
                _checkTime = 0;

                if (Vector3.Distance(_player.transform.position, transform.position) > 20)
                    HideState();
            }
        }
        else
        {
            _checkTime += Time.deltaTime;
            if (_checkTime > 1)
            {
                _checkTime = 0;

                if (Vector3.Distance(_player.transform.position, transform.position) < 20)
                    ShowState();
            }
        }
    }

    void ShowState()
    {
        _stateWindow.SetActive(true);
        _isUIShow = true;
        _checkTime = 0;
    }

    public void HideState()
    {
        _stateWindow.SetActive(false);
        _isUIShow = false;
        _checkTime = 0;
    }

    public void ResetUIMonsterState(int level)
    {
        string levelText = level.ToString();
        _levelTXT.text = levelText;
        if (level < 10)
            _levelTXT.text = "0" + levelText;

        _hpSlider.value = 1;

        HideState();
    }

}
