using RimWorld;
using UnityEngine;
using Verse;
using Xunit;

namespace FavoriteColorDropper.Tests;

public sealed class EyedropperPatchTests : TestEnvironment
{
    [Fact]
    public void ColorPatchAllowsVanillaForEmptyCell()
    {
        ColorDef? result = new("existing");

        var runOriginal = ColorDefAtPatch.Prefix(Cell, ref result);

        Assert.True(runOriginal);
        Assert.Equal("existing", result!.Id);
    }

    [Fact]
    public void ColorPatchReturnsPawnFavoriteColor()
    {
        var favorite = new ColorDef("favorite");
        AddHumanlikePawn(favorite, new ColorDef("ideo"));
        ColorDef? result = null;

        var runOriginal = ColorDefAtPatch.Prefix(Cell, ref result);

        Assert.False(runOriginal);
        Assert.Same(favorite, result);
    }

    [Fact]
    public void ColorPatchSuppressesFloorFallbackForPawnMissingColor()
    {
        AddHumanlikePawn(null, null);
        ColorDef? result = new("floor");

        var runOriginal = ColorDefAtPatch.Prefix(Cell, ref result);

        Assert.False(runOriginal);
        Assert.Null(result);
    }

    [Fact]
    public void ValidationPatchRejectsMissingFavoriteColor()
    {
        AddHumanlikePawn(null, null);
        var result = new AcceptanceReport(string.Empty);

        var runOriginal = CanDesignateCellPatch.Prefix(Cell, ref result);

        Assert.False(runOriginal);
        Assert.Equal("This pawn has no favorite color.", result.Reason);
    }

    [Fact]
    public void ValidationPatchRejectsMissingIdeoligionColorWhileShiftHeld()
    {
        AddHumanlikePawn(new ColorDef("favorite"), null);
        Input.SetKey(KeyCode.LeftShift, true);
        var result = new AcceptanceReport(string.Empty);

        var runOriginal = CanDesignateCellPatch.Prefix(Cell, ref result);

        Assert.False(runOriginal);
        Assert.Equal("This pawn has no ideoligion color.", result.Reason);
        Assert.Equal(new[] { "This pawn has no ideoligion color." }, Messages.ShownMessages);
    }

    [Fact]
    public void RejectedShiftClickCannotFallBackToFavoriteColorAfterShiftIsReleased()
    {
        var favorite = new ColorDef("favorite");
        AddHumanlikePawn(favorite, null);
        Input.SetKey(KeyCode.LeftShift, true);
        var report = new AcceptanceReport(string.Empty);

        Assert.False(CanDesignateCellPatch.Prefix(Cell, ref report));

        Input.SetKey(KeyCode.LeftShift, false);
        ColorDef? result = favorite;
        var runOriginal = ColorDefAtPatch.Prefix(Cell, ref result);

        Assert.False(runOriginal);
        Assert.Null(result);
        Assert.Equal("This pawn has no ideoligion color.", report.Reason);
    }

    [Fact]
    public void ValidationPatchAllowsVanillaForValidPawnColor()
    {
        AddHumanlikePawn(new ColorDef("favorite"), null);
        var result = new AcceptanceReport(string.Empty);

        var runOriginal = CanDesignateCellPatch.Prefix(Cell, ref result);

        Assert.True(runOriginal);
    }

    [Fact]
    public void ValidationPatchAllowsVanillaForNonPawnTarget()
    {
        var result = new AcceptanceReport(string.Empty);

        var runOriginal = CanDesignateCellPatch.Prefix(Cell, ref result);

        Assert.True(runOriginal);
    }

    private static void AddHumanlikePawn(ColorDef? favorite, ColorDef? ideoligion)
    {
        var pawn = new Pawn
        {
            story = new Pawn_StoryTracker { favoriteColor = favorite },
            Ideo = ideoligion is null ? null : new Ideo { colorDef = ideoligion }
        };
        pawn.RaceProps.Humanlike = true;
        Find.CurrentMap!.ThingsAt(Cell).Add(pawn);
    }
}
