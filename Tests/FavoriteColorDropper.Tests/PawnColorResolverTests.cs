using RimWorld;
using Xunit;

namespace FavoriteColorDropper.Tests;

public sealed class PawnColorResolverTests
{
    private readonly ColorDef favorite = new("favorite");
    private readonly ColorDef ideoligion = new("ideoligion");

    [Fact]
    public void NonHumanlikeTargetContinuesVanilla()
    {
        var result = PawnColorResolver.Resolve(false, favorite, ideoligion, false);

        Assert.Equal(ColorSelectionStatus.ContinueVanilla, result.Status);
        Assert.False(result.HasPawnTarget);
        Assert.False(result.IsSelected);
        Assert.Null(result.Color);
    }

    [Fact]
    public void FavoriteColorIsSelectedWithoutModifier()
    {
        var result = PawnColorResolver.Resolve(true, favorite, ideoligion, false);

        Assert.Equal(ColorSelectionStatus.Selected, result.Status);
        Assert.True(result.HasPawnTarget);
        Assert.True(result.IsSelected);
        Assert.Same(favorite, result.Color);
    }

    [Fact]
    public void IdeoligionColorIsSelectedWithModifier()
    {
        var result = PawnColorResolver.Resolve(true, favorite, ideoligion, true);

        Assert.Equal(ColorSelectionStatus.Selected, result.Status);
        Assert.Same(ideoligion, result.Color);
    }

    [Fact]
    public void MissingFavoriteColorIsRejected()
    {
        var result = PawnColorResolver.Resolve(true, null, ideoligion, false);

        Assert.Equal(ColorSelectionStatus.MissingFavoriteColor, result.Status);
        Assert.Null(result.Color);
    }

    [Fact]
    public void MissingIdeoligionColorIsRejectedWithoutFallback()
    {
        var result = PawnColorResolver.Resolve(true, favorite, null, true);

        Assert.Equal(ColorSelectionStatus.MissingIdeoligionColor, result.Status);
        Assert.Null(result.Color);
    }

    [Fact]
    public void IdenticalFavoriteAndIdeoligionColorStillSucceeds()
    {
        var result = PawnColorResolver.Resolve(true, favorite, favorite, true);

        Assert.Equal(ColorSelectionStatus.Selected, result.Status);
        Assert.Same(favorite, result.Color);
    }
}

