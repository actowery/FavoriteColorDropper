using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class StaticConstructorOnStartup : Attribute
{
}

internal class Thing
{
}

internal sealed class RaceProperties
{
    internal bool Humanlike { get; set; }
}

internal sealed class Pawn : Thing
{
    internal bool Spawned { get; set; } = true;

    internal RaceProperties RaceProps { get; } = new();

    internal Pawn_StoryTracker? story;

    internal Ideo? Ideo { get; set; }
}

internal sealed class Map
{
    private readonly Dictionary<IntVec3, List<Thing>> thingsByCell = new();

    internal bool Contains(IntVec3 cell) => cell.X >= 0 && cell.Z >= 0 && cell.X < 250 && cell.Z < 250;

    internal List<Thing> ThingsAt(IntVec3 cell)
    {
        if (!thingsByCell.TryGetValue(cell, out var things))
        {
            things = new List<Thing>();
            thingsByCell.Add(cell, things);
        }

        return things;
    }
}

internal readonly struct IntVec3 : IEquatable<IntVec3>
{
    internal IntVec3(int x, int z)
    {
        X = x;
        Z = z;
    }

    internal int X { get; }

    internal int Z { get; }

    internal bool InBounds(Map map) => map.Contains(this);

    internal List<Thing> GetThingList(Map map) => map.ThingsAt(this);

    public bool Equals(IntVec3 other) => X == other.X && Z == other.Z;

    public override bool Equals(object? obj) => obj is IntVec3 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Z);
}

internal static class Find
{
    internal static Map? CurrentMap { get; set; }
}

internal readonly struct TaggedString
{
    internal TaggedString(string value)
    {
        Value = value;
    }

    internal string Value { get; }
}

internal readonly struct AcceptanceReport
{
    internal AcceptanceReport(string reason)
    {
        Reason = reason;
    }

    internal string? Reason { get; }

    public static implicit operator AcceptanceReport(TaggedString value) => new(value.Value);
}

internal static class TranslationExtensions
{
    internal static TaggedString Translate(this string key) => new(key switch
    {
        "FCD_NoFavoriteColor" => "This pawn has no favorite color.",
        "FCD_NoIdeoligionColor" => "This pawn has no ideoligion color.",
        _ => key
    });
}

internal static class Log
{
    internal static List<string> Messages { get; } = new();

    internal static List<string> Errors { get; } = new();

    internal static void Message(string message) => Messages.Add(message);

    internal static void Error(string message) => Errors.Add(message);
}

internal sealed class MessageTypeDef
{
}

internal static class MessageTypeDefOf
{
    internal static MessageTypeDef RejectInput { get; } = new();
}

internal static class Messages
{
    internal static List<string> ShownMessages { get; } = new();

    internal static void Message(TaggedString message, MessageTypeDef messageType, bool historical)
    {
        ShownMessages.Add(message.Value);
    }
}
