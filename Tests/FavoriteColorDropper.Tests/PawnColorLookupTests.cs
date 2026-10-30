using RimWorld;
using UnityEngine;
using Verse;
using Xunit;

namespace FavoriteColorDropper.Tests;

public sealed class PawnColorLookupTests : TestEnvironment
{
    [Fact]
    public void EmptyCellContinuesVanilla()
    {
        var result = PawnColorLookup.Resolve(Cell);

        Assert.Equal(ColorSelectionStatus.ContinueVanilla, result.Status);
    }

    [Fact]
    public void OutOfBoundsCellContinuesVanilla()
    {
        var result = PawnColorLookup.Resolve(new IntVec3(-1, -1));

        Assert.Equal(ColorSelectionStatus.ContinueVanilla, result.Status);
    }

    [Fact]
    public void MissingMapContinuesVanilla()
    {
        Find.CurrentMap = null;

        var result = PawnColorLookup.Resolve(Cell);

        Assert.Equal(ColorSelectionStatus.ContinueVanilla, result.Status);
    }

    [Fact]
    public void AnimalAndUnspawnedPawnAreIgnored()
    {
        Find.CurrentMap!.ThingsAt(Cell).Add(new Pawn
        {
            story = new Pawn_StoryTracker { favoriteColor = new ColorDef("animal") }
        });
        Find.CurrentMap.ThingsAt(Cell).Add(new Pawn
        {
            Spawned = false,
            story = new Pawn_StoryTracker { favoriteColor = new ColorDef("unspawned") }
        });

        var result = PawnColorLookup.Resolve(Cell);

        Assert.Equal(ColorSelectionStatus.ContinueVanilla, result.Status);
    }

    [Fact]
    public void FirstSpawnedHumanlikePawnIsUsedDeterministically()
    {
        var firstColor = new ColorDef("first");
        Find.CurrentMap!.ThingsAt(Cell).Add(HumanlikePawn(firstColor));
        Find.CurrentMap.ThingsAt(Cell).Add(HumanlikePawn(new ColorDef("second")));

        var result = PawnColorLookup.Resolve(Cell);

        Assert.Same(firstColor, result.Color);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EitherShiftKeySelectsIdeoligionColor(bool useLeftShift)
    {
        var ideoligionColor = new ColorDef("ideo");
        var pawn = HumanlikePawn(new ColorDef("favorite"));
        pawn.Ideo = new Ideo { colorDef = ideoligionColor };
        Find.CurrentMap!.ThingsAt(Cell).Add(pawn);
        Input.SetKey(useLeftShift ? KeyCode.LeftShift : KeyCode.RightShift, true);

        var result = PawnColorLookup.Resolve(Cell);

        Assert.Same(ideoligionColor, result.Color);
        Assert.True(PawnColorLookup.IsShiftHeld());
    }

    [Fact]
    public void HumanlikePawnWithNullStoryRejectsFavoriteColor()
    {
        var pawn = HumanlikePawn(new ColorDef("unused"));
        pawn.story = null;
        Find.CurrentMap!.ThingsAt(Cell).Add(pawn);

        var result = PawnColorLookup.Resolve(Cell);

        Assert.Equal(ColorSelectionStatus.MissingFavoriteColor, result.Status);
    }

    private static Pawn HumanlikePawn(ColorDef favoriteColor)
    {
        var pawn = new Pawn
        {
            story = new Pawn_StoryTracker { favoriteColor = favoriteColor }
        };
        pawn.RaceProps.Humanlike = true;
        return pawn;
    }
}
