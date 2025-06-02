using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EffectManager : SingletonGameobject<EffectManager>
{
    public enum HitEffectName
    {
        Basic_Hit1,
        Basic_Hit2,
        Basic_Hit3,
        Basic_Hit4,
        Fire_Hit,
        Ice_Hit,
        Lightning_Hit,
        Love_Hit,
        Magic_Hit,


        Max
    }

    public enum OtherEffectName
    {
        HitBall,
        LevelUp,

        Max,
    }


    Dictionary<HitEffectName, GameObject> _effectPrefabs;
    Dictionary<HitEffectName, GameObjectPool<Transform>> _effectPools;
    Dictionary<HitEffectName, List<GameObject>> _activeEffects;

    Dictionary<OtherEffectName, GameObject> _otherEffectPrefabs;
    Dictionary<OtherEffectName, GameObjectPool<Transform>> _otherEffectPools;
    Dictionary<OtherEffectName, List<GameObject>> _activeOtherEffects;

    public void Init()
    {
        _effectPrefabs = new Dictionary<HitEffectName, GameObject>();
        _effectPools = new Dictionary<HitEffectName, GameObjectPool<Transform>>();
        _activeEffects = new Dictionary<HitEffectName, List<GameObject>>();


        _otherEffectPrefabs = new Dictionary<OtherEffectName, GameObject>();
        _otherEffectPools = new Dictionary<OtherEffectName, GameObjectPool<Transform>>();
        _activeOtherEffects = new Dictionary<OtherEffectName, List<GameObject>>();

        GetPrefabs();

        PoolingEffects();
    }

    public void StartEffect(OtherEffectName name, Vector3 position)
    {
        GameObject effectGO = _otherEffectPools[name].Get().gameObject;

        effectGO.transform.position = position;
        effectGO.SetActive(true);

        if (_activeOtherEffects.ContainsKey(name))
        {
            _activeOtherEffects[name].Add(effectGO);
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(effectGO);
            _activeOtherEffects.Add(name, list);
        }
    }

    public void StartEffect(HitEffectName name, Vector3 position)
    {
        GameObject effectGO = _effectPools[name].Get().gameObject;

        effectGO.transform.position = position;
        effectGO.SetActive(true);

        if(_activeEffects.ContainsKey(name))
        {
            _activeEffects[name].Add(effectGO);
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(effectGO);
            _activeEffects.Add(name, list);
        }
    }

    void GetPrefabs()
    {
        int effectCount = (int)HitEffectName.Max;
        string path = "Effects/HitEffects/";

        for (int i = 0; i < effectCount; i++)
        {
            HitEffectName effectName = (HitEffectName)i;
            GameObject prefab = Resources.Load(path + effectName.ToString()) as GameObject;
            _effectPrefabs.Add(effectName, prefab);
        }

        effectCount = (int)OtherEffectName.Max;
        path = "Effects/";

        for (int i = 0; i < effectCount; i++)
        {
            OtherEffectName effectName = (OtherEffectName)i;
            GameObject prefab = Resources.Load(path + effectName.ToString()) as GameObject;
            _otherEffectPrefabs.Add(effectName, prefab);
        }
    }

    void PoolingEffects()
    {
        int effectCount = (int)HitEffectName.Max;

        for (int i = 0; i < effectCount; i++)
        {
            HitEffectName effectName = (HitEffectName)i;
            GameObject prefab = _effectPrefabs[effectName];

            GameObjectPool<Transform> effectPool = new GameObjectPool<Transform>(3, () => 
            {
                GameObject effect = Instantiate(prefab, transform);
                effect.SetActive(false);
                return effect.GetComponent<Transform>();
            });

            _effectPools.Add(effectName, effectPool);
        }

        int otherEffectCount = (int)OtherEffectName.Max;

        for (int i = 0; i < otherEffectCount; i++)
        {
            OtherEffectName otherEffectName = (OtherEffectName)i;
            GameObject prefab = _otherEffectPrefabs[otherEffectName];

            GameObjectPool<Transform> effectPool = new GameObjectPool<Transform>(3, () =>
            {
                GameObject effect = Instantiate(prefab, transform);
                effect.SetActive(false);
                return effect.GetComponent<Transform>();
            });

            _otherEffectPools.Add(otherEffectName, effectPool);
        }
    }

    void Update()
    {
        if (_activeEffects.Count != 0)
            return;

        int activeEffectCount = _activeEffects.Count;

        for (int i = 0; i < activeEffectCount; i++)
        {
            HitEffectName effectName = (HitEffectName)i;
            int effectListCount = _activeEffects[effectName].Count;

            for (int j = 0; j < effectListCount; j++)
            {
                GameObject effect = _activeEffects[effectName][j];
                if (!effect.activeSelf)
                {
                    _activeEffects[effectName].Remove(effect);
                    if (_activeEffects[effectName].Count == 0)
                        _activeEffects.Remove(effectName);

                    _effectPools[effectName].Set(effect.transform);
                }
            }
        }
    }
}
