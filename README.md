# Favorite Color Dropper

[![CI](https://github.com/actowery/FavoriteColorDropper/actions/workflows/ci.yml/badge.svg)](https://github.com/actowery/FavoriteColorDropper/actions/workflows/ci.yml)
[![Codecov](https://codecov.io/gh/actowery/FavoriteColorDropper/graph/badge.svg)](https://codecov.io/gh/actowery/FavoriteColorDropper)
[![CodeQL](https://github.com/actowery/FavoriteColorDropper/actions/workflows/codeql.yml/badge.svg)](https://github.com/actowery/FavoriteColorDropper/actions/workflows/codeql.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Use RimWorld's existing color eyedropper on a humanlike pawn to select the pawn's favorite color. Hold **Shift** while sampling to select the pawn's ideoligion color instead.

## Requirements

- RimWorld 1.6 or 1.7 unstable
- [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)
- Ideology is optional. Without it, favorite-color sampling still works wherever the vanilla eyedropper is available.

## Usage

1. Select a paint or colored-floor designator that supports RimWorld's eyedropper.
2. Hold the configured eyedropper key (`Ctrl` by default).
3. Click a humanlike pawn to select their favorite color.
4. Hold `Shift` as well to select their ideoligion color.

If the pawn does not have the requested color, the current selection is preserved and RimWorld displays a rejection message.

## Building

Install the .NET 10 SDK, then run:

```powershell
dotnet restore FavoriteColorDropper.slnx --use-lock-file
dotnet build Source/FavoriteColorDropper/FavoriteColorDropper.csproj -c Release
dotnet test Tests/FavoriteColorDropper.Tests/FavoriteColorDropper.Tests.csproj -c Release -p:CollectCoverage=true -p:IncludeTestAssembly=true -p:CoverletOutputFormat=cobertura
./scripts/package.ps1
```

Compiled assemblies are placed in `1.6/Assemblies` and `1.7/Assemblies`. The release archive and checksum are written to `outputs/`.

## Coverage policy

Pull requests must maintain at least 90% project coverage and 95% coverage on changed lines. Both RimWorld compatibility flags must report to Codecov. Integration seams are compiled against the real versioned reference packages and the exact production source is linked into the stubbed test harness.

## Save safety

The mod stores no data. It may be added to or removed from an existing save.

## License

MIT. See [LICENSE](LICENSE).
