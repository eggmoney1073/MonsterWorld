
namespace  DefineEnums
{
    public enum Slot
    {
        Slot1,
        Slot2,
        Slot3,

        Max
    }

    public enum TableName
    {
        MonsterLevelTable,
        MonsterTable,
        PlayerLevelTable,
        PlayerWeaponTable,
        SkillTable,

        Max
    }

    public enum InGameUI
    {
        Pause,
        Summon,
        Encyclopedia,

        Max
    }

    public enum GameOverUI
    {
        GameOver,
        GameClear,

        Max
    }


    public enum PlayerWeapon
    {
        None,
        Bow,
        Gun,
        Ball,

        Max
    }

    public enum SkillType
    {
        None = 0,
        Skill_Ball,
        Skill_Arrow,
        Skill_Laser,
        Skill_Dash,
        Skill_Cross,
        Skill_Turn,

        Max
    }

    public enum SkillState
    {
        Init,
        Charge,
        Attack,
        Finish,

        Max
    }

    public enum MonsterType
    {
        None = 0,
        Cactus,
        Mushroom,
        Lich,
        Golem,

        Max
    }

    public enum MonsterState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Return,

        Max
    }

    public enum ProjectileType
    {
        Bullet,
        Arrow,
        Ball,

        Max
    }
}
