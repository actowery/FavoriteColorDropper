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
        if (EyedropperRejectionCache.Consume(cell))
        {
            // Do not invoke vanilla's lookup: it could sample the pawn's favorite
            // color after the rejected Shift-click has already released Shift.
            __result = null;
            return false;
        }

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
                EyedropperRejectionCache.Remember(c);
                __result = "FCD_NoFavoriteColor".Translate();
                ShowRejection(__result);
                return false;
            case ColorSelectionStatus.MissingIdeoligionColor:
                EyedropperRejectionCache.Remember(c);
                __result = "FCD_NoIdeoligionColor".Translate();
                ShowRejection(__result);
                return false;
            default:
                EyedropperRejectionCache.Forget(c);
                return true;
        }
    }

    private static void ShowRejection(TaggedString message) =>
        Messages.Message(message, MessageTypeDefOf.RejectInput, false);
}
