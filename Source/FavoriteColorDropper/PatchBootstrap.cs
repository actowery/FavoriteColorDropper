using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FavoriteColorDropper;

[StaticConstructorOnStartup]
internal static class PatchBootstrap
{
    internal const string HarmonyId = "konduct.favoritecolordropper";

    static PatchBootstrap()
    {
        TryApplyPatches(
            () =>
            {
                ValidatePatchTargets((type, name, parameters) =>
                    AccessTools.Method(type, name, parameters));
                new Harmony(HarmonyId).PatchAll(typeof(PatchBootstrap).Assembly);
            },
            message => Log.Message(message),
            message => Log.Error(message));
    }

    internal static void TryApplyPatches(Action patchAll, Action<string> logInfo, Action<string> logError)
    {
        try
        {
            patchAll();
            logInfo("[Favorite Color Dropper] Harmony patches applied.");
        }
        catch (Exception exception)
        {
            logError($"[Favorite Color Dropper] Failed to apply Harmony patches: {exception}");
        }
    }

    internal static void ValidatePatchTargets(
        Func<Type, string, Type[]?, MethodInfo?> methodResolver)
    {
        var parameters = new[] { typeof(IntVec3) };
        if (methodResolver(typeof(Designator_Eyedropper), "ColorDefAt", parameters) is null)
        {
            throw new MissingMethodException(typeof(Designator_Eyedropper).FullName, "ColorDefAt(IntVec3)");
        }

        if (methodResolver(typeof(Designator_Eyedropper), nameof(Designator_Eyedropper.CanDesignateCell), parameters) is null)
        {
            throw new MissingMethodException(
                typeof(Designator_Eyedropper).FullName,
                "CanDesignateCell(IntVec3)");
        }
    }
}
