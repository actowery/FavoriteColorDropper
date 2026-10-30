using Xunit;

namespace FavoriteColorDropper.Tests;

public sealed class CompatibilityTargetTests
{
    [Fact]
    public void ExpectedCompatibilityTargetIsCompiled()
    {
#if RIMWORLD_1_6_TEST
        Assert.Equal("1.6", "1.6");
#elif RIMWORLD_1_7_TEST
        Assert.Equal("1.7", "1.7");
#else
#error A RimWorld compatibility target must be selected.
#endif
    }
}

