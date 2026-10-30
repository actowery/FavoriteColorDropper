param(
    [string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot),
    [switch]$RequireBinaries
)

$ErrorActionPreference = 'Stop'

function Assert-Condition {
    param(
        [bool]$Condition,
        [string]$Message
    )

    if (-not $Condition) {
        throw $Message
    }
}

$aboutPath = Join-Path $RepositoryRoot 'About\About.xml'
$loadFoldersPath = Join-Path $RepositoryRoot 'LoadFolders.xml'
$languagePath = Join-Path $RepositoryRoot 'Languages\English\Keyed\FavoriteColorDropper.xml'

Assert-Condition (Test-Path -LiteralPath $aboutPath) 'About/About.xml is missing.'
Assert-Condition (Test-Path -LiteralPath $loadFoldersPath) 'LoadFolders.xml is missing.'
Assert-Condition (Test-Path -LiteralPath $languagePath) 'English localization is missing.'

[xml]$about = Get-Content -Raw -LiteralPath $aboutPath
[xml]$loadFolders = Get-Content -Raw -LiteralPath $loadFoldersPath
[xml]$language = Get-Content -Raw -LiteralPath $languagePath

Assert-Condition ($about.ModMetaData.name -eq 'Favorite Color Dropper') 'Unexpected mod name.'
Assert-Condition ($about.ModMetaData.author -eq 'konduct') 'Unexpected author.'
Assert-Condition ($about.ModMetaData.packageId -eq 'konduct.favoritecolordropper') 'Unexpected package ID.'

$versions = @($about.ModMetaData.supportedVersions.li)
Assert-Condition ($versions -contains '1.6') 'RimWorld 1.6 is not declared.'
Assert-Condition ($versions -contains '1.7') 'RimWorld 1.7 is not declared.'

$dependencies = @($about.ModMetaData.modDependencies.li | ForEach-Object { $_.packageId })
Assert-Condition ($dependencies -contains 'brrainz.harmony') 'Harmony dependency is not declared.'
Assert-Condition ($loadFolders.loadFolders.'v1.6'.li[-1] -eq '1.6') 'RimWorld 1.6 load folder is incorrect.'
Assert-Condition ($loadFolders.loadFolders.'v1.7'.li[-1] -eq '1.7') 'RimWorld 1.7 load folder is incorrect.'
Assert-Condition ($null -ne $language.LanguageData.FCD_NoFavoriteColor) 'Favorite-color rejection translation is missing.'
Assert-Condition ($null -ne $language.LanguageData.FCD_NoIdeoligionColor) 'Ideoligion-color rejection translation is missing.'

if ($RequireBinaries) {
    foreach ($version in @('1.6', '1.7')) {
        $assemblyDirectory = Join-Path $RepositoryRoot "$version\Assemblies"
        $assemblyPath = Join-Path $assemblyDirectory 'FavoriteColorDropper.dll'
        Assert-Condition (Test-Path -LiteralPath $assemblyPath) "RimWorld $version assembly is missing."

        $forbidden = @(Get-ChildItem -LiteralPath $assemblyDirectory -File |
            Where-Object { $_.Name -in @('0Harmony.dll', 'Assembly-CSharp.dll', 'UnityEngine.dll', 'UnityStubs.dll') })
        Assert-Condition ($forbidden.Count -eq 0) "RimWorld $version contains bundled runtime dependencies."
    }
}

Write-Output 'Package validation passed.'
