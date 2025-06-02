using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    [Header("Cross Hair Setting")]
    [SerializeField] float _length;
    [SerializeField] float _thickness;
    [SerializeField] Color _color;

    Image[] _crossHairs;

    public void Init()
    {
        _crossHairs = new Image[4];

        for (int i = 0; i < 4; i++)
            _crossHairs[i] = transform.GetChild(i).GetComponent<Image>();

        SetCrossHair();
    }

    public void SetCrossHair()
    {
        for (int i = 0; i < 4; i++)
        {
            _crossHairs[i].color = _color;
            if (i < 2)
                _crossHairs[i].rectTransform.sizeDelta= new Vector2(_length, _thickness);
            else
                _crossHairs[i].rectTransform.sizeDelta = new Vector2(_thickness, _length);
        }
    }
}
