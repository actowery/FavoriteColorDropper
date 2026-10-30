using System;
using System.Reflection;

namespace HarmonyLib;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal sealed class HarmonyPatch : Attribute
{
    internal HarmonyPatch(Type declaringType, string methodName)
    {
        DeclaringType = declaringType;
        MethodName = methodName;
    }

    internal Type DeclaringType { get; }

    internal string MethodName { get; }
}

[AttributeUsage(AttributeTargets.Method)]
internal sealed class HarmonyPrefix : Attribute
{
}

internal sealed class Harmony
{
    internal Harmony(string id)
    {
        Id = id;
    }

    internal string Id { get; }

    internal void PatchAll(Assembly assembly)
    {
        LastPatchedAssembly = assembly;
    }

    internal static Assembly? LastPatchedAssembly { get; private set; }
}

internal static class AccessTools
{
    internal static MethodInfo? Method(Type type, string name, Type[]? parameters) =>
        type.GetMethod(
            name,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
            binder: null,
            types: parameters ?? Type.EmptyTypes,
            modifiers: null);
}

