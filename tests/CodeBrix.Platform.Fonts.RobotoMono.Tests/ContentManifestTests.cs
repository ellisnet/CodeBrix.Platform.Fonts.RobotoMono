using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.RobotoMono.Tests;

public class ContentManifestTests
{
    private const string CodeBrixPathPrefix = "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/";

    // This package was authored by mirroring the sibling Merriweather package
    // (the other multi-companion font package), so the realistic copy-paste
    // regressions are stray "Merriweather" or "Serif" tokens. A stray "Roboto"
    // token cannot be tested for — it legitimately appears in every path here.
    private static readonly string[] ForeignFamilyTokens = ["Merriweather", "Serif"];

    // Roboto Mono publishes no ExtraBold (800) instance upstream, so the
    // primary manifest stops at Bold (700); the companions carry all six
    // family-convention weights.
    private static readonly int[] PrimaryWeights = [300, 400, 500, 600, 700];
    private static readonly int[] CompanionWeights = [300, 400, 500, 600, 700, 800];

    public static TheoryData<string, int> AllManifests => new()
    {
        { "RobotoMono", 10 },
        { "NotoSansMono", 6 },
        { "Iosevka", 6 },
        { "NotoSansGeorgian", 6 },
    };

    [Fact]
    public void Manifest_file_exists_in_test_output()
        => File.Exists(TestAssetPaths.ManifestPath).Should().BeTrue();

    [Fact]
    public void Manifest_can_be_deserialized()
    {
        //Arrange
        var json = File.ReadAllText(TestAssetPaths.ManifestPath);

        //Act
        var doc = JsonDocument.Parse(json);

        //Assert
        doc.RootElement.TryGetProperty("fonts", out var fonts).Should().BeTrue();
        fonts.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_has_the_expected_entry_count(string family, int expected)
    {
        //Arrange
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act/Assert
        entries.Count.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_every_family_name_uses_codebrix_namespace(string family, int expected)
    {
        //Arrange
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act
        var nonMatching = entries
            .Where(e => !e.FamilyName.StartsWith(CodeBrixPathPrefix))
            .ToList();

        //Assert
        nonMatching.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_every_referenced_font_file_exists_on_disk(string family, int expected)
    {
        //Arrange
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act
        var missing = entries
            .Select(e => Path.GetFileName(e.FamilyName))
            .Select(name => Path.Combine(TestAssetPaths.FontsFolder, name))
            .Where(path => !File.Exists(path))
            .ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_covers_the_expected_weights(string family, int expected)
    {
        //Arrange
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));
        var expectedWeights = family == "RobotoMono" ? PrimaryWeights : CompanionWeights;

        //Act
        var distinctWeights = entries.Select(e => e.FontWeight).Distinct().OrderBy(w => w).ToArray();

        //Assert
        distinctWeights.Should().BeEquivalentTo(expectedWeights);
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_is_normal_stretch_only(string family, int expected)
    {
        //Arrange — none of the four families ships a Condensed or SemiCondensed
        //stretch here. The Iosevka files are upstream's Extended width grade,
        //but they are the only width grade in the package, so the manifest
        //declares them as the Normal stretch.
        _ = expected;
        var entries = ReadManifestEntries(ManifestFor(family));

        //Act
        var distinctStretches = entries.Select(e => e.FontStretch).Distinct().ToArray();

        //Assert
        distinctStretches.Should().BeEquivalentTo(new[] { "Normal" });
    }

    [Theory]
    [MemberData(nameof(AllManifests))]
    public void Manifest_contains_no_foreign_family_tokens(string family, int expected)
    {
        //Arrange
        _ = expected;
        var json = File.ReadAllText(ManifestFor(family));

        //Act
        var offenders = ForeignFamilyTokens.Where(json.Contains).ToList();

        //Assert
        offenders.Should().BeEmpty();
    }

    [Fact]
    public void RobotoMono_manifest_covers_normal_and_italic_styles()
    {
        //Arrange
        var entries = ReadManifestEntries(TestAssetPaths.ManifestPath);

        //Act
        var distinctStyles = entries.Select(e => e.FontStyle).Distinct().OrderBy(s => s).ToArray();

        //Assert
        distinctStyles.Should().BeEquivalentTo(new[] { "Italic", "Normal" });
    }

    [Theory]
    [InlineData("NotoSansMono")]
    [InlineData("Iosevka")]
    [InlineData("NotoSansGeorgian")]
    public void Companion_manifest_is_upright_only(string family)
    {
        //Arrange — Noto Sans Mono and Noto Sans Georgian have no italic face
        //upstream. Iosevka does publish italics, but the companion exists only
        //to supply the Armenian script and ships upright-only like the other
        //companions, so italic text in those scripts renders upright. Asserting
        //it here keeps that a decision rather than an accident.
        var entries = ReadManifestEntries(TestAssetPaths.CompanionManifestPath(family));

        //Act
        var distinctStyles = entries.Select(e => e.FontStyle).Distinct().ToArray();

        //Assert
        distinctStyles.Should().BeEquivalentTo(new[] { "Normal" });
    }

    [Fact]
    public void Iosevka_manifest_regular_entry_points_at_the_dash_free_font()
    {
        //Arrange — Iosevka publishes no variable-font .ttf, so the dash-free
        //Iosevka.ttf IS the Regular static instance; the weight-400 manifest
        //entry references it directly rather than a duplicate
        //Iosevka-Regular.ttf.
        var entries = ReadManifestEntries(TestAssetPaths.CompanionManifestPath("Iosevka"));

        //Act
        var regular = entries.Single(e => e.FontWeight == 400);

        //Assert
        Path.GetFileName(regular.FamilyName).Should().Be("Iosevka.ttf");
    }

    private static string ManifestFor(string family) =>
        family == "RobotoMono" ? TestAssetPaths.ManifestPath : TestAssetPaths.CompanionManifestPath(family);

    private static List<ManifestEntry> ReadManifestEntries(string manifestPath)
    {
        var json = File.ReadAllText(manifestPath);
        using var doc = JsonDocument.Parse(json);
        var fonts = doc.RootElement.GetProperty("fonts");

        var list = new List<ManifestEntry>(fonts.GetArrayLength());
        foreach (var entry in fonts.EnumerateArray())
        {
            list.Add(new ManifestEntry(
                entry.GetProperty("font_style").GetString() ?? string.Empty,
                entry.GetProperty("font_weight").GetInt32(),
                entry.GetProperty("font_stretch").GetString() ?? string.Empty,
                entry.GetProperty("family_name").GetString() ?? string.Empty));
        }
        return list;
    }

    private readonly record struct ManifestEntry(
        string FontStyle,
        int FontWeight,
        string FontStretch,
        string FamilyName);
}
