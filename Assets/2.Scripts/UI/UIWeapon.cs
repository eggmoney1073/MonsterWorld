using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineEnums;
using System.Linq;

public class UIWeapon : MonoBehaviour
{
    [SerializeField] Image _weaponImageComponent;
    [SerializeField] Sprite[] _weaponSprites;
    PlayerWeapon _currentWeapon;

    Dictionary<PlayerWeapon, Sprite> _weaponImages;

    public void SetWeapon(PlayerWeapon weapon)
    {
        _currentWeapon = weapon;
        _weaponImageComponent.sprite = _weaponImages[weapon];
    }

    public void InitUIWeapon()
    {
        _weaponImages = new Dictionary<PlayerWeapon, Sprite>();

        int weaponCount = _weaponSprites.Length;
        for (int i = 0; i < weaponCount; i++)
            _weaponImages.Add((PlayerWeapon)i, _weaponSprites[i]);
    }
}
