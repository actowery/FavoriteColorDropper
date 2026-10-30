using HarmonyLib;
using RimWorld;
using Verse;

namespace FavoriteColorDropper;

[HarmonyPatch(typeof(Designator_Eyedropper), "ColorDefAt")]
internal static class ColorDefAtPatch
{
    [HarmonyPrefix]
    internal static bool Prefix(IntVec3 cell, ref ColorDef? __result)
    {
        var selection = PawnColorLookup.Resolve(cell);
        if (!selection.HasPawnTarget)
        {
            return true;
        }

        __result = selection.Color;
        return false;
    }
}

[HarmonyPatch(typeof(Designator_Eyedropper), nameof(Designator_Eyedropper.CanDesignateCell))]
internal static class CanDesignateCellPatch
{
    [HarmonyPrefix]
    internal static bool Prefix(IntVec3 c, ref AcceptanceReport __result)
    {
        var selection = PawnColorLookup.Resolve(c);
        switch (selection.Status)
        {
            case ColorSelectionStatus.MissingFavoriteColor:
                __result = "FCD_NoFavoriteColor".Translate();
                return false;
            case ColorSelectionStatus.MissingIdeoligionColor:
                __result = "FCD_NoIdeoligionColor".Translate();
                return false;
            default:
                return true;
        }
    }
}

