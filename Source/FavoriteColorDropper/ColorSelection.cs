using RimWorld;

namespace FavoriteColorDropper;

internal enum ColorSelectionStatus
{
    ContinueVanilla,
    Selected,
    MissingFavoriteColor,
    MissingIdeoligionColor
}

internal readonly struct ColorSelection
{
    private ColorSelection(ColorSelectionStatus status, ColorDef? color)
    {
        Status = status;
        Color = color;
    }

    internal ColorSelectionStatus Status { get; }

    internal ColorDef? Color { get; }

    internal bool HasPawnTarget => Status != ColorSelectionStatus.ContinueVanilla;

    internal bool IsSelected => Status == ColorSelectionStatus.Selected;

    internal static ColorSelection ContinueVanilla() =>
        new(ColorSelectionStatus.ContinueVanilla, null);

    internal static ColorSelection Selected(ColorDef color) =>
        new(ColorSelectionStatus.Selected, color);

    internal static ColorSelection MissingFavoriteColor() =>
        new(ColorSelectionStatus.MissingFavoriteColor, null);

    internal static ColorSelection MissingIdeoligionColor() =>
        new(ColorSelectionStatus.MissingIdeoligionColor, null);
}

