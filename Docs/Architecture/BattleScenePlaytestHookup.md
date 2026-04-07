# Battle Scene Playtest Hookup

This project now supports driving the existing push-your-luck battle loop from simple Unity UI controls without changing battle rules.

## Runtime pieces

- `BattleController` owns engine startup and forwards UI actions to the battle engine.
- `BattleHudPresenter` binds controller snapshots to a view and routes button requests back to the controller.
- `BattleHudView` is a pure Unity UI adapter for buttons and text readouts.

## Exact Unity editor hookup steps

1. Open `Assets/_Project/Scenes/Battle.unity`.
2. Ensure there is a GameObject with `BattleController`.
   - Keep `Start On Awake` enabled for quick playtesting.
   - Optional: adjust default values (`Player Max Hp`, `Player Dice Count`, `Enemy Max Hp`, `Enemy Attack Damage`) for test pacing.
3. Create a Canvas (if missing) and add:
   - Two `Button` objects named `RollButton` and `StopButton`.
   - Six `Text` objects for:
     - Player HP
     - Enemy HP
     - Accumulated Damage
     - Last Roll
     - Bust/Safe State
     - Current Phase
4. Add `BattleHudView` to a HUD GameObject (for example `BattleHud`).
5. Wire `BattleHudView` references in the Inspector:
   - `Roll Button` -> `RollButton`
   - `Stop Button` -> `StopButton`
   - `Player Hp Text` -> Player HP text component
   - `Enemy Hp Text` -> Enemy HP text component
   - `Accumulated Damage Text` -> Accumulated Damage text component
   - `Last Roll Text` -> Last Roll text component
   - `Bust State Text` -> Bust/Safe state text component
   - `Phase Text` -> Current phase text component
6. Add `BattleHudPresenter` to the same HUD GameObject (or another UI coordinator GameObject).
7. Wire `BattleHudPresenter` references:
   - `Battle Controller` -> scene `BattleController`
   - `Hud View` -> scene `BattleHudView`
8. Press Play.
   - Use Roll/Stop to run the loop.
   - Buttons auto-disable when the player cannot act (victory/defeat or enemy turn).

## Intentionally simple

- Uses legacy `UnityEngine.UI.Text` for broad compatibility and minimal setup.
- No animation or transitions.
- No additional gameplay systems were introduced.
