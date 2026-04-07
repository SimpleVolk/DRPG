# DRPG Initial Project Foundation (Unity 6.3 LTS)

## Reasoning (short)
- Keep core gameplay logic in plain C# to maximize testability, speed of iteration, and AI-assisted refactoring.
- Keep Unity-facing layers thin (scene composition, input wiring, UI binding, prefab references).
- Start with a small, explicit structure that can scale without introducing framework-level complexity.

---

## 1) Initial Unity Folder Structure

```text
Assets/
  _Project/
    Art/
      UI/
      VFX/
      Icons/
      Placeholders/
    Audio/
      Music/
      SFX/
    Data/
      Dice/
      Companions/
      Enemies/
      EncounterTables/
      Progression/
      Economy/
    Prefabs/
      Core/
      Battle/
      UI/
      World/
    Scenes/
      Boot.unity
      MainMenu.unity
      Hub.unity
      Battle.unity
    Scripts/
      Runtime/
        Core/
          App/
          State/
          Save/
          Time/
        Gameplay/
          Battle/
            Domain/
            Application/
            Presentation/
          Dice/
            Domain/
          Entities/
            Domain/
          Roster/
            Domain/
            Application/
        Shared/
          Math/
          RNG/
          Extensions/
      Editor/
    UI/
      UXML/
      USS/
      Sprites/
    Settings/
      Addressables/
      Input/
  Plugins/
  ThirdParty/
```

Notes:
- Keep all project-owned content in `Assets/_Project` for easy filtering and package hygiene.
- `Domain` = plain C# rules/data transforms, no Unity API.
- `Application` = orchestrates domain use cases.
- `Presentation` = MonoBehaviours/UI adapters only.

---

## 2) Core Assembly Definitions (minimum useful set)

Create asmdefs only where compile-time and ownership boundaries are clear:

1. `DRPG.Core`  
   Scope: `Scripts/Runtime/Core`, `Scripts/Runtime/Shared`.
2. `DRPG.Gameplay`  
   Scope: `Scripts/Runtime/Gameplay/*/Domain` and `Application`.
3. `DRPG.UnityAdapters`  
   Scope: `Scripts/Runtime/**/Presentation`, scene controllers, MonoBehaviours.
4. `DRPG.Editor` (Editor only)

Dependency direction:
- `DRPG.Gameplay` -> `DRPG.Core`
- `DRPG.UnityAdapters` -> `DRPG.Gameplay`, `DRPG.Core`, Unity assemblies
- `DRPG.Editor` -> any runtime asmdef as needed

Why this is enough:
- Faster compile loops for gameplay iterations.
- Prevents Unity API bleed into domain logic.
- Keeps maintenance overhead low (only 4 asmdefs).

---

## 3) First Gameplay Architecture

### 3.1 Game State Flow

State enum (small and explicit for v1):
- `Boot`
- `MainMenu`
- `Hub`
- `Battle`
- `Results`

Core pieces:
- `GameStateMachine` (plain C#): validates transitions.
- `GameFlowService` (plain C#): high-level flow operations (`StartRun`, `EnterBattle`, `ReturnToHub`).
- `GameFlowController` (MonoBehaviour): scene lifecycle + delegates to services.

Rule of thumb:
- Scene loads and UI transitions happen in adapters/controllers.
- Transition legality and run progression stay in plain C#.

### 3.2 Battle State Flow

Battle phases (deterministic, explicit):
- `BattleInit`
- `RoundStart`
- `PlayerRoll`
- `PlayerResolve`
- `EnemyTurn`
- `RoundEnd`
- `Victory`
- `Defeat`
- `CaptureAttempt` (optional branch from victory-eligible states)

Core pieces:
- `BattleStateMachine` (plain C#): phase transitions.
- `BattleContext`: immutable references + mutable battle snapshot.
- `BattleEngine`: executes one command at a time (e.g., roll, lock dice, resolve action, end turn).
- `BattleCommand` structs/classes for input events.

Keep deterministic behavior by passing RNG as dependency (`IRandomSource` or concrete `SeededRandom`).

### 3.3 Dice Resolution

Minimal resolution pipeline:
1. Roll N dice from actor dice pool.
2. Convert faces to `DiceOutcome` values.
3. Apply modifiers/effects in strict order:
   - Base face value
   - Flat bonuses
   - Multipliers
   - Conditional effects
4. Emit `ResolutionResult` (damage, shield, heal, status intents).

Key point:
- Resolution engine is plain C# (`DiceResolver`), no animation/UI concerns.
- UI reads emitted events/results and visualizes.

### 3.4 Entity Stats

Use lightweight stat model:
- `StatType` enum: HP, Attack, Defense, Speed, CaptureResist, etc.
- `StatBlock` class/struct with base values.
- `RuntimeStatBlock` for mutable combat values (current HP, temporary modifiers).

Rules:
- Permanent data comes from ScriptableObjects.
- Runtime mutations live in plain C# battle/session models.
- Clamp rules centralized in one utility (`StatRules`).

### 3.5 Companion Roster

Split authored vs runtime:
- Authored: `CompanionDefinition` ScriptableObject.
- Runtime collection: `Roster` plain C# model containing `CompanionInstance` entries.

`CompanionInstance` minimum fields:
- `InstanceId`
- `DefinitionId`
- `Level`
- `XP`
- `UnlockedDiceIds`
- `CurrentHP` (if persistent between battles)

Use cases:
- `AddCompanion`
- `RemoveCompanion`
- `SetActiveCompanion`
- `GrantXP`

### 3.6 Data Definitions (ScriptableObjects)

Minimum initial SOs:
- `DiceDefinition`
- `CompanionDefinition`
- `EnemyDefinition`
- `EncounterTableDefinition`
- `ProgressionCurveDefinition`

Design tips:
- Include stable string ID field (`Id`) in each definition.
- Reference other definitions directly where convenient; use IDs for save persistence.
- Keep SOs as authored config only (no mutable runtime state).

---

## 4) Thin MonoBehaviour Strategy

MonoBehaviours should only:
- Receive input
- Bind UI
- Forward commands to plain C# services
- Subscribe and render state/output events

MonoBehaviours should not:
- Compute battle outcomes
- Own progression rules
- Mutate core game rules directly

---

## 5) Minimum Starter Scenes and Prefabs

Scenes:
1. `Boot.unity` – initialization, save load, service wiring, first transition.
2. `MainMenu.unity` – start/load/settings entry.
3. `Hub.unity` – roster management + battle launch.
4. `Battle.unity` – isolated battle presentation scene.

Starter prefabs:
- `GameBootstrapper` (persistent root wiring)
- `BattleViewRoot` (battle UI + references)
- `DiceRollPanel`
- `CompanionCard`
- `EnemyCard`
- `PrimaryButton` (shared UI style)

Keep prefab graph shallow; avoid deep nested prefabs at project start.

---

## 6) Naming Conventions

Namespaces:
- `DRPG.Core.*`
- `DRPG.Gameplay.*`
- `DRPG.Unity.*`

Types:
- ScriptableObjects: `XxxDefinition`
- Runtime models: `XxxModel` or `XxxInstance`
- Services/use cases: `XxxService`
- State machines: `XxxStateMachine`
- MonoBehaviour adapters/controllers: `XxxController`, `XxxView`

Files:
- One public type per file.
- File name == type name.

Data IDs:
- Lowercase snake or dot-style (pick one once). Recommended: `companion.nano_drake`.

---

## 7) Starter Class List (first pass)

Core flow:
- `GameState`
- `GameStateMachine`
- `GameFlowService`
- `GameSessionModel`

Battle:
- `BattlePhase`
- `BattleStateMachine`
- `BattleContext`
- `BattleEngine`
- `BattleCommand`
- `BattleResult`

Dice:
- `DiceDefinition` (SO)
- `DiceFace`
- `DiceRollResult`
- `DiceResolver`

Entities:
- `StatType`
- `StatBlock`
- `RuntimeStatBlock`
- `CompanionDefinition` (SO)
- `EnemyDefinition` (SO)
- `CompanionInstance`
- `EnemyInstance`

Roster/Progression:
- `Roster`
- `RosterService`
- `ProgressionCurveDefinition` (SO)
- `ExperienceService`

Encounters:
- `EncounterTableDefinition` (SO)
- `EncounterService`

Unity adapters:
- `GameFlowController` (MB)
- `BattleController` (MB)
- `BattleView` (MB)
- `RosterView` (MB)

---

## 8) Implementation Order (safe iteration)

1. Create folders, namespaces, asmdefs.
2. Implement data SOs with IDs and validation helpers.
3. Implement core stat/dice domain classes (plain C#).
4. Implement battle state machine + battle engine command loop.
5. Implement roster + progression services.
6. Wire boot/main menu/hub/battle scenes with thin controllers.
7. Add basic save model for session + roster.
8. Add simple debug UI hooks to drive battle commands.

Definition of done for this phase:
- Can launch app -> start battle -> resolve deterministic round -> return result.
- No gameplay rules inside UI scripts.
- New enemy/companion/dice can be added via SO asset creation only.

---

## Risks / Tradeoffs

- Too few asmdefs can slow compile times later; too many adds overhead now. The 4-asmdef setup is the balanced start.
- Early scene separation (`Hub` vs `Battle`) adds load transitions but keeps battle module isolated and maintainable.
- Keeping v1 enums explicit is simple, but expansion to effect catalogs may require refactor once content grows.
