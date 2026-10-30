using RimWorld;
using UnityEngine;
using Verse;

namespace FavoriteColorDropper;

internal static class PawnColorLookup
{
    internal static ColorSelection Resolve(IntVec3 cell)
    {
        var pawn = FirstHumanlikePawnAt(cell);
        if (pawn is null)
        {
            return PawnColorResolver.Resolve(false, null, null, IsShiftHeld());
        }

        return PawnColorResolver.Resolve(
            true,
            pawn.story?.favoriteColor,
            pawn.Ideo?.colorDef,
            IsShiftHeld());
    }

    internal static Pawn? FirstHumanlikePawnAt(IntVec3 cell)
    {
        var map = Find.CurrentMap;
        if (map is null || !cell.InBounds(map))
        {
            return null;
        }

        var things = cell.GetThingList(map);
        for (var index = 0; index < things.Count; index++)
        {
            if (things[index] is Pawn pawn &&
                pawn.Spawned &&
                pawn.RaceProps.Humanlike)
            {
                return pawn;
            }
        }

        return null;
    }

    internal static bool IsShiftHeld() =>
        Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
}

