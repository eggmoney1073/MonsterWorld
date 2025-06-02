using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineEnums;

public class UIMonster : MonoBehaviour
{
    [SerializeField] Image _monsterImageComponent;
    [SerializeField] Sprite[] _monsterSprites;

    Dictionary<MonsterType, Sprite> _monsterImages;

    public void SetUIMonster(MonsterType type)
    {
        _monsterImageComponent.sprite = _monsterImages[type];
    }

    public void InitMonsterUI()
    {
        _monsterImages = new Dictionary<MonsterType, Sprite>();

        int weaponCount = _monsterSprites.Length;
        for (int i = 0; i < weaponCount; i++)
        {
            _monsterImages.Add((MonsterType)i, _monsterSprites[i]);
        }
    }
}
