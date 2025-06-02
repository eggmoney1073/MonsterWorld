using UnityEngine;
using DefineEnums;

namespace DefineStructs
{
    public struct PlayerData
    {
        public int Level;
        public float HP;
        public float EXP;
        public Vector3 Position;

        public MonsterType[] Monster_Type;
        public int[] Monster_Level;
    }

    public struct MonsterData
    {
        public float HealthScale;
        public float AttackScale;
        public SkillType Skill1;
        public SkillType Skill2;
    }

    public struct MonsterLevelData
    {
        public float DropEXP;
        public float RequiredCaputrePower;
        public float Health;
        public float Attack;
    }
}
