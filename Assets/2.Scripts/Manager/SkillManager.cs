using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnums;

public class SkillManager : SingletonGameobject<SkillManager>
{
    Dictionary<SkillType, GameObject> _skillPrefabs;
    Dictionary<SkillType, ObjectPool<Skill>> _skillPools;

    public Skill ActiveSkill(SkillType type, GameObject caster, Transform target)
    {
        Skill skill = _skillPools[type].Get();
        GameObject skillGO = skill.gameObject;
        skillGO.SetActive(true);
        skill.SetTarget(target);
        skill.StartSkill(caster);

        return skill;
    }

    public void ReturnSkill(Skill skill)
    {
        _skillPools[skill._SkillType].Set(skill);
    }

    public void InitSkillManager()
    {
        _skillPrefabs = new Dictionary<SkillType, GameObject>();
        _skillPools = new Dictionary<SkillType, ObjectPool<Skill>>();

        for (int i = 1; i < (int)SkillType.Max; i++)
        {
            SkillType type = (SkillType)i;
            GameObject skillprefab = GetSkillPrefab(type);
            _skillPrefabs.Add(type, skillprefab);
            _skillPools.Add(type, new ObjectPool<Skill>(2, () =>
            {
                GameObject go = Instantiate(skillprefab, transform);
                Skill skill = go.GetComponent<Skill>();
                skill.InitSkill();
                go.SetActive(false);
                return skill;
            }));
        }
    }

    GameObject GetSkillPrefab(SkillType type)
    {
        string path = "Skills/";
        path += type;
        return Resources.Load(path) as GameObject;
    }


}
