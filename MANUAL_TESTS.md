# Manual release test matrix

Complete every row in both RimWorld 1.6 and the pinned RimWorld 1.7 unstable build before publishing.

| Scenario | Expected result |
|---|---|
| Start with Harmony and Favorite Color Dropper | No red log entries; patch success appears once |
| Default eyedropper key + humanlike pawn | Exact favorite `ColorDef` becomes active |
| Rebound eyedropper key + humanlike pawn | Rebound control works identically |
| Eyedropper + left Shift | Exact ideoligion `ColorDef` becomes active |
| Eyedropper + right Shift | Exact ideoligion `ColorDef` becomes active |
| Pawn without favorite color | Localized rejection; active color unchanged |
| Shift on pawn without ideoligion color | Localized rejection; active color unchanged |
| Painted building or colored floor | Vanilla sampling remains unchanged |
| Empty cell, animal, mech | Vanilla behavior remains unchanged |
| Multiple humanlike pawns in one cell | First spawned humanlike pawn in the thing list is sampled |
| Favorite Color Picker changes a pawn | New favorite color is sampled immediately |
| Character Editor changes a pawn | New favorite color is sampled immediately |
| Existing save with mod enabled | Load, save, and reload succeeds |
| Same save after removing mod | Load succeeds without missing-data warnings |

