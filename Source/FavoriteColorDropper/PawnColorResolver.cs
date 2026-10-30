using RimWorld;

namespace FavoriteColorDropper;

internal static class PawnColorResolver
{
    internal static ColorSelection Resolve(
        bool isHumanlikePawn,
        ColorDef? favoriteColor,
        ColorDef? ideoligionColor,
        bool sampleIdeoligion)
    {
        if (!isHumanlikePawn)
        {
            return ColorSelection.ContinueVanilla();
        }

        if (sampleIdeoligion)
        {
            return ideoligionColor is null
                ? ColorSelection.MissingIdeoligionColor()
                : ColorSelection.Selected(ideoligionColor);
        }

        return favoriteColor is null
            ? ColorSelection.MissingFavoriteColor()
            : ColorSelection.Selected(favoriteColor);
    }
}

