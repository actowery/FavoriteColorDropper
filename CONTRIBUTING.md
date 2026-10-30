# Contributing

Open an issue before proposing a broad behavior change. Small bug fixes may go directly to a pull request.

## Development checks

Use the .NET SDK selected by `global.json`, then run:

```powershell
dotnet restore FavoriteColorDropper.slnx --locked-mode
dotnet build Source/FavoriteColorDropper/FavoriteColorDropper.csproj --configuration Release --no-restore
dotnet test Tests/FavoriteColorDropper.Tests/FavoriteColorDropper.Tests.csproj --configuration Release --no-restore -p:RimWorldVersion=1.6
dotnet test Tests/FavoriteColorDropper.Tests/FavoriteColorDropper.Tests.csproj --configuration Release --no-restore -p:RimWorldVersion=1.7
./scripts/validate-package.ps1 -RequireBinaries
```

Keep patches narrowly scoped, preserve vanilla eyedropper behavior for non-pawn targets, and add tests for behavior changes. Pull requests must pass all required checks and resolve review conversations before merge.
