using System.Collections.Generic;

namespace UnityEngine;

internal enum KeyCode
{
    LeftShift,
    RightShift
}

internal static class Input
{
    private static readonly HashSet<KeyCode> PressedKeys = new();

    internal static bool GetKey(KeyCode keyCode) => PressedKeys.Contains(keyCode);

    internal static void SetKey(KeyCode keyCode, bool pressed)
    {
        if (pressed)
        {
            PressedKeys.Add(keyCode);
        }
        else
        {
            PressedKeys.Remove(keyCode);
        }
    }

    internal static void Reset() => PressedKeys.Clear();
}

