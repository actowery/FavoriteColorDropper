using System;
using UnityEngine;
using Verse;

namespace FavoriteColorDropper.Tests;

public abstract class TestEnvironment : IDisposable
{
    protected TestEnvironment()
    {
        Find.CurrentMap = new Map();
        Input.Reset();
        Log.Messages.Clear();
        Log.Errors.Clear();
        Messages.ShownMessages.Clear();
    }

    public void Dispose()
    {
        Find.CurrentMap = null;
        Input.Reset();
        GC.SuppressFinalize(this);
    }

    internal static readonly IntVec3 Cell = new(10, 10);
}
