using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _expSlider;
    [SerializeField] TextMeshProUGUI _levelTXT;

    public void SetLevel(int level)
    {
        _levelTXT.text = level.ToString();
    }

    public void SetHp(float percentage)
    {
        _hpSlider.value = percentage;
    }

    public void SetEXp(float percentage)
    {
        _expSlider.value = percentage;
    }
}
