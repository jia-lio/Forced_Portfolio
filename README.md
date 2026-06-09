# Forced_Portfolio - 3D 호러 / 내러티브 게임

## 프로젝트 개요
어둠 속을 탐험하는 3D 호러 게임입니다. 챕터 기반 레벨 디자인과 몬스터 AI, 컷씬 연출이 어우러진 내러티브 게임으로, 클라이언트 2명 · 기획 2명 · 아트 4명이 팀으로 개발했습니다.

본 저장소는 포트폴리오 용도로, 제가 담당한 **세이브 / 현지화 / 공통 인프라** 시스템의 핵심 코드만 발췌해 정리한 것입니다. 비공개 팀 프로젝트의 일부 발췌본이므로 단독으로는 빌드/실행되지 않으며, 구조와 설계를 보여주기 위한 코드입니다.

## 사용 기술
- Unity 6
- C#
- UniTask, R3 (Reactive Extensions)
- Newtonsoft.Json
- FMOD (사운드)
- 커스텀 의존성 주입(DI) / 매니저 컨테이너 구조

## 담당 영역
- **세이브 시스템** : 인터페이스 기반 직렬화(IUserSaveable / IChapterSaveable)로 유저 데이터와 챕터 데이터를 분리하고, 원자적 파일 쓰기로 저장 중 손상을 방지
- **현지화(다국어)** : 시스템 언어 자동 감지 + 런타임 언어 전환(OnChanged 이벤트), 엑셀 기반 로컬라이징 테이블(ScriptableObject)
- **공통 인프라** : ISubManager 기반 중앙 매니저 컨테이너, 커스텀 DI(Inject), BaseTable / BaseScriptableObject 데이터 구조

## 프로젝트 구조
```
Assets/
└── Scripts/
    ├── Save/                          # 세이브 시스템
    │   ├── GameSaveManager.cs         # 저장/로드 총괄, 원자적 쓰기, 챕터 흐름 관리
    │   ├── ISaveable.cs               # IUserSaveable / IChapterSaveable 인터페이스 + 저장 데이터 모델
    │   ├── SaveContext.cs             # 세이브 매니저 컨텍스트
    │   └── SaveObject.cs              # 챕터 세이브 대상 오브젝트(트리거 기반 저장)
    ├── Core/
    │   ├── Localized/
    │   │   └── Localize.cs            # 현지화 - 시스템 언어 감지, 런타임 전환, 텍스트 조회
    │   ├── Table/
    │   │   └── ExcelTable.cs          # 엑셀 기반 ScriptableObject 테이블 조회
    │   ├── GameManager/
    │   │   ├── ISubManager.cs         # 서브 매니저 공통 인터페이스(Initialize/Update 루프)
    │   │   ├── GameManager.cs         # 매니저 등록 및 초기화 총괄
    │   │   ├── ManagerContext.cs      # 매니저 컨텍스트 베이스
    │   │   └── ManagerContainer.cs    # 매니저 등록/해석(Resolve) 컨테이너
    │   ├── Inject/
    │   │   ├── InjectAttribute.cs     # [Inject] 어트리뷰트 정의
    │   │   ├── InjectableMonoBehaviour.cs  # 주입 가능한 MonoBehaviour 베이스
    │   │   └── Injector.cs            # 리플렉션 기반 의존성 주입기
    │   └── EntryPoint/
    │       └── EntryPoint.cs          # 게임 진입점
    └── Table/
        ├── BaseTable.cs               # 테이블 데이터 베이스
        └── BaseScriptableObject.cs    # 테이블 ScriptableObject 베이스
```
