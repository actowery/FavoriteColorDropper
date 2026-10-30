using Verse;

namespace RimWorld;

internal sealed class ColorDef
{
    internal ColorDef(string id = "color")
    {
        Id = id;
    }

    internal string Id { get; }
}

internal sealed class Pawn_StoryTracker
{
    internal ColorDef? favoriteColor;
}

internal sealed class Ideo
{
    internal ColorDef? colorDef;
}

internal class Designator_Eyedropper
{
    protected virtual ColorDef? ColorDefAt(IntVec3 cell) => null;

    public virtual AcceptanceReport CanDesignateCell(IntVec3 cell) => new(string.Empty);
}

