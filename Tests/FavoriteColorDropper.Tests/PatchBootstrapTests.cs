using System;
using System.Reflection;
using RimWorld;
using Verse;
using Xunit;

namespace FavoriteColorDropper.Tests;

public sealed class PatchBootstrapTests
{
    [Fact]
    public void SuccessfulPatchApplicationLogsSuccess()
    {
        var applied = false;
        string? info = null;
        string? error = null;

        PatchBootstrap.TryApplyPatches(
            () => applied = true,
            message => info = message,
            message => error = message);

        Assert.True(applied);
        Assert.Contains("patches applied", info, StringComparison.OrdinalIgnoreCase);
        Assert.Null(error);
    }

    [Fact]
    public void FailedPatchApplicationLogsActionableError()
    {
        string? info = null;
        string? error = null;

        PatchBootstrap.TryApplyPatches(
            () => throw new InvalidOperationException("boom"),
            message => info = message,
            message => error = message);

        Assert.Null(info);
        Assert.Contains("Failed to apply Harmony patches", error, StringComparison.Ordinal);
        Assert.Contains("boom", error, StringComparison.Ordinal);
    }

    [Fact]
    public void RealPatchTargetsPassContractValidation()
    {
        PatchBootstrap.ValidatePatchTargets(ResolveMethod);
    }

    [Theory]
    [InlineData("ColorDefAt")]
    [InlineData("CanDesignateCell")]
    public void MissingPatchTargetFailsContractValidation(string missingMethod)
    {
        MethodInfo? Resolver(Type type, string name, Type[]? parameters) =>
            name == missingMethod ? null : ResolveMethod(type, name, parameters);

        var exception = Assert.Throws<MissingMethodException>(
            () => PatchBootstrap.ValidatePatchTargets(Resolver));

        Assert.Contains(missingMethod, exception.Message, StringComparison.Ordinal);
    }

    private static MethodInfo? ResolveMethod(Type type, string name, Type[]? parameters) =>
        type.GetMethod(
            name,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
            binder: null,
            types: parameters ?? Type.EmptyTypes,
            modifiers: null);
}
