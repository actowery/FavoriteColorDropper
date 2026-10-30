using System.Xml.Linq;
using Xunit;

namespace FavoriteColorDropper.Tests;

public sealed class PackageMetadataTests
{
    private static readonly string RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void AboutMetadataDeclaresIdentityVersionsAndHarmony()
    {
        var document = XDocument.Load(Path.Combine(RepositoryRoot, "About", "About.xml"));
        var root = Assert.IsType<XElement>(document.Root);

        Assert.Equal("Favorite Color Dropper", root.Element("name")?.Value);
        Assert.Equal("konduct", root.Element("author")?.Value);
        Assert.Equal("konduct.favoritecolordropper", root.Element("packageId")?.Value);

        var supportedVersions = root
            .Element("supportedVersions")!
            .Elements("li")
            .Select(element => element.Value)
            .ToArray();
        Assert.Equal(new[] { "1.6", "1.7" }, supportedVersions);

        var dependencies = root
            .Element("modDependencies")!
            .Elements("li")
            .Select(element => element.Element("packageId")?.Value)
            .ToArray();
        Assert.Contains("brrainz.harmony", dependencies);
    }

    [Fact]
    public void LoadFoldersRoutesEachSupportedGameVersion()
    {
        var document = XDocument.Load(Path.Combine(RepositoryRoot, "LoadFolders.xml"));
        var root = Assert.IsType<XElement>(document.Root);

        Assert.Equal("1.6", root.Element("v1.6")?.Elements("li").Last().Value);
        Assert.Equal("1.7", root.Element("v1.7")?.Elements("li").Last().Value);
    }

    [Theory]
    [InlineData("FCD_NoFavoriteColor")]
    [InlineData("FCD_NoIdeoligionColor")]
    public void RequiredTranslationKeyExists(string key)
    {
        var path = Path.Combine(
            RepositoryRoot,
            "Languages",
            "English",
            "Keyed",
            "FavoriteColorDropper.xml");
        var document = XDocument.Load(path);

        Assert.NotNull(document.Root?.Element(key));
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "LoadFolders.xml")) &&
                File.Exists(Path.Combine(current.FullName, "About", "About.xml")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the Favorite Color Dropper repository root.");
    }
}

