# 🎮 Monster Hunter RPG

> 몬스터를 포획하고 소환하여 함께 싸우는 3D 액션 RPG

---

## 📌 프로젝트 소개

3D 오픈 월드 환경에서 플레이어가 직접 몬스터를 포획하고, 포획한 몬스터를 아군으로 소환하여 전투에 활용하는 액션 RPG입니다.
총, 활, 볼 세 가지 무기를 상황에 맞게 교체하며 전투하고, 포획한 몬스터의 도감을 완성하는 것이 목표입니다.

---

## 🛠 개발 환경

| 항목 | 내용 |
|------|------|
| 엔진 | Unity |
| 언어 | C# |
| 데이터 | JSON (세이브), CSV (데이터 테이블) |
| AI | NavMesh Agent |

---

## 🎯 핵심 기능

### ⚔️ 전투 시스템
- **총(Gun)** : 레이캐스트 기반 즉발 사격, 명중/빗나감 색상 구분 탄환 이펙트
- **활(Bow)** : 포물선 궤도 화살, 중력 영향 실제 물리 연산
- **볼(Ball)** : 몬스터를 향해 투척, 포획 확률 계산 후 성공/실패 처리

### 🐾 몬스터 포획 & 소환
- 몬스터 HP가 낮을수록 포획 확률 증가
- 포획한 몬스터는 최대 5슬롯에 저장
- E 키로 아군 몬스터 소환, 자동으로 적 추격 및 스킬 사용

### 🤖 몬스터 AI
- **상태머신** 기반 (Idle → Patrol → Chase → Attack → Return)
- 아군/적군 상태에 따라 별도 AI 동작
- NavMesh Agent로 경로 탐색

### 📖 도감 시스템
- 포획한 몬스터 종류 기록
- 모든 종류 포획 시 게임 클리어 해금

### 💾 세이브 시스템
- 슬롯 3개 지원 (신규 게임 / 불러오기)
- 플레이어 위치, 레벨, EXP, 포획 몬스터 정보 저장
- JSON 직렬화 (`JsonUtility`)

---

## 🗂 프로젝트 구조

```
Assets/2.Scripts/
├── Game/
│   ├── PlayerController.cs      # 이동, 카메라, 무기 교체, 애니메이션
│   ├── PlayerManager.cs         # 플레이어 스탯, HP, EXP, 레벨업
│   ├── CameraController.cs      # 3인칭 카메라, 줌, 카메라 쉐이크
│   ├── Monster/
│   │   ├── Monster.cs           # 몬스터 기본 클래스 (AI, 피격, 상태머신)
│   │   ├── MonsterCactus.cs
│   │   ├── MonsterMushroom.cs
│   │   ├── MonsterLich.cs
│   │   └── MonsterGolem.cs
│   └── Skill/
│       ├── Skill.cs             # 스킬 기본 클래스
│       ├── Skill_Arrow.cs       # 화살 3발 순차 발사
│       ├── Skill_Laser.cs       # 레이저 추적 공격
│       ├── Skill_Ball.cs        # 충전 후 투척 스킬
│       ├── Skill_Dash.cs        # 대시 이동
│       ├── Skill_Cross.cs       # 범위 반복 공격
│       └── Skill_Turn.cs        # 회전 근접 공격
│
├── Manager/
│   ├── InGameManager.cs         # 게임 씬 전체 초기화 및 상태 관리
│   ├── UIManager.cs             # UI 윈도우 관리
│   ├── MonsterManager.cs        # 몬스터 풀, 스폰, 전투 상태
│   ├── SkillManager.cs          # 스킬 오브젝트 풀
│   ├── ProjectileManager.cs     # 투사체 오브젝트 풀
│   ├── EffectManager.cs         # 히트 이펙트 풀
│   ├── DataManager.cs           # JSON 세이브/로드
│   ├── GameTableManager.cs      # CSV 데이터 테이블 로드
│   ├── LoadingManager.cs        # 씬 비동기 로드
│   └── TitleManager.cs          # 타이틀 화면 슬롯 관리
│
├── Weapon/
│   ├── Projectile.cs            # 투사체 기본 클래스
│   ├── GunController.cs
│   ├── BowController.cs
│   ├── BallController.cs
│   ├── BulletController.cs
│   └── ArrowController.cs
│
├── UI/                          # HUD, 슬라이더, 도감, 소환 UI 등
├── Table/                       # CSV 기반 데이터 테이블 클래스
└── Utility/
    ├── StateMachineBase.cs      # 제네릭 상태머신
    ├── ObjectPool.cs            # 제네릭 오브젝트 풀
    ├── GameObjectPool.cs        # MonoBehaviour 전용 오브젝트 풀
    ├── SingletonGameobject.cs   # 씬 내 싱글톤
    ├── SingletonDontDestroyOnLoad.cs
    ├── DefineEnums.cs           # 전체 열거형 정의
    └── DefineStructs.cs         # 전체 구조체 정의
```

---

## 🧩 기술적 특징

### 제네릭 오브젝트 풀
투사체, 스킬, 이펙트에 공통으로 사용하는 제네릭 풀을 직접 구현하여 GC 부하 최소화

### 제네릭 상태머신
`StateMachineBase<T>`를 기반으로 몬스터 AI와 스킬 모두 동일한 구조로 상태 관리. 딕셔너리에 델리게이트를 등록하는 방식으로 유연하게 확장 가능

### 데이터 테이블 시스템
CSV 파일을 런타임에 파싱하여 몬스터 스탯, 플레이어 레벨, 스킬 수치 등을 관리. 기획 데이터 수정 시 코드 변경 불필요

### 아군 몬스터 AI
포획한 몬스터가 아군으로 전환되면 별도의 `friendStateUpdate` 딕셔너리로 전환되어, 플레이어를 추종하고 적을 자동 공격

---

## 🎮 조작 방법

| 키 | 동작 |
|----|------|
| WASD | 이동 |
| 마우스 우클릭 | 조준 (줌) |
| 마우스 좌클릭 | 공격 |
| 1 / 2 / 3 | 무기 교체 (없음 / 활 / 총) |
| Q | 볼 던지기 (포획 시도) |
| E | 아군 몬스터 소환 |
| F | 소환 슬롯 UI |
| Tab | 도감 UI |
| ESC | 일시정지 |

---

## 👾 등장 몬스터

| 이름 | 특징 |
|------|------|
| Cactus | 기본형 근거리 몬스터 |
| Mushroom | 중거리 추격형 |
| Lich | 원거리 레이저/화살 스킬 사용 |
| Golem | 고체력 근거리 탱커 |

---

## 📝 개발 회고

- 상태머신과 오브젝트 풀을 제네릭으로 직접 구현하여 재사용성을 높인 것이 이번 프로젝트의 핵심 학습 포인트였습니다.
- 몬스터 AI의 아군/적군 전환 시 상태 딕셔너리를 분리하여 단일 클래스에서 두 가지 행동 패턴을 깔끔하게 관리할 수 있었습니다.
- 데이터 테이블 시스템을 도입하여 밸런스 수치를 코드 외부에서 관리하는 구조를 경험했습니다.
