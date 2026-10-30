using Verse;

namespace FavoriteColorDropper;

/// <summary>
/// Carries a rejected Shift-click from validation to the subsequent color lookup.
/// RimWorld can query the modifier state at different points in the click lifecycle;
/// this prevents a rejected ideoligion sample from falling back to the favorite color.
/// </summary>
internal static class EyedropperRejectionCache
{
    private static IntVec3? rejectedCell;

    internal static void Remember(IntVec3 cell) => rejectedCell = cell;

    internal static void Forget(IntVec3 cell)
    {
        if (rejectedCell == cell)
        {
            rejectedCell = null;
        }
    }

    internal static bool Consume(IntVec3 cell)
    {
        if (rejectedCell != cell)
        {
            return false;
        }

        rejectedCell = null;
        return true;
    }
}
