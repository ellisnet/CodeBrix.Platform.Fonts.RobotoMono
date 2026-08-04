using System.IO;
using System.Linq;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.RobotoMono.Tests;

public class ContentFilePresenceTests
{
    [Fact]
    public void Variable_font_RobotoMono_ttf_is_present()
        => File.Exists(TestAssetPaths.VariableFontPath).Should().BeTrue();

    [Fact]
    public void Manifest_file_is_present()
        => File.Exists(TestAssetPaths.ManifestPath).Should().BeTrue();

    [Fact]
    public void Total_ttf_count_is_31()
    {
        //Arrange/Act
        // 1 Roboto Mono variable + 10 Roboto Mono statics, then the three
        // companion families: Noto Sans Mono (1 + 6), Iosevka (1 + 5; the
        // dash-free file IS the Regular instance, so no separate Regular static
        // ships) and Noto Sans Georgian (1 + 6).
        var ttfFiles = Directory.GetFiles(TestAssetPaths.FontsFolder, "*.ttf");

        //Assert
        ttfFiles.Length.Should().Be(31);
    }

    [Fact]
    public void All_10_static_RobotoMono_fonts_are_present()
    {
        //Arrange
        // Note the static font naming convention shared across these packages:
        // the italic of the Regular weight is just "Italic" (no "Regular"
        // prefix), e.g. RobotoMono-Italic.ttf. Every other weight carries its
        // weight name in the italic filename. Roboto Mono publishes no
        // ExtraBold (800) instance upstream — its weight axis stops at Bold
        // (700) — and no Condensed stretch, so only five weights in the Normal
        // stretch ship.
        var weights = new[] { "Light", "Regular", "Medium", "SemiBold", "Bold" };
        var styles = new[] { "", "Italic" };

        //Act
        var missing = (
            from weight in weights
            from style in styles
            let weightSegment = (weight == "Regular" && style == "Italic") ? "" : weight
            let fileName = $"RobotoMono-{weightSegment}{style}.ttf"
            let path = Path.Combine(TestAssetPaths.FontsFolder, fileName)
            where !File.Exists(path)
            select fileName
        ).ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Theory]
    [InlineData("NotoSansMono")]
    [InlineData("Iosevka")]
    [InlineData("NotoSansGeorgian")]
    public void Companion_dash_free_font_is_present(string family)
        => File.Exists(TestAssetPaths.CompanionFontPath(family)).Should().BeTrue();

    [Theory]
    [InlineData("NotoSansMono")]
    [InlineData("Iosevka")]
    [InlineData("NotoSansGeorgian")]
    public void Companion_manifest_is_present(string family)
        => File.Exists(TestAssetPaths.CompanionManifestPath(family)).Should().BeTrue();

    [Theory]
    [InlineData("NotoSansMono")]
    [InlineData("NotoSansGeorgian")]
    public void All_6_static_fonts_are_present_for(string family)
    {
        //Arrange — neither family has an italic face upstream, so only the six
        //upright weights ship.
        var weights = new[] { "Light", "Regular", "Medium", "SemiBold", "Bold", "ExtraBold" };

        //Act
        var missing = weights
            .Select(weight => $"{family}-{weight}.ttf")
            .Where(fileName => !File.Exists(Path.Combine(TestAssetPaths.FontsFolder, fileName)))
            .ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Fact]
    public void All_5_dash_bearing_Iosevka_statics_are_present()
    {
        //Arrange — the Regular weight is served by the dash-free Iosevka.ttf,
        //so the dash-bearing statics are the other five weights only.
        var weights = new[] { "Light", "Medium", "SemiBold", "Bold", "ExtraBold" };

        //Act
        var missing = weights
            .Select(weight => $"Iosevka-{weight}.ttf")
            .Where(fileName => !File.Exists(Path.Combine(TestAssetPaths.FontsFolder, fileName)))
            .ToList();

        //Assert
        missing.Should().BeEmpty();
    }

    [Fact]
    public void No_separate_Iosevka_Regular_static_ships()
    {
        //Arrange — Iosevka publishes no variable-font .ttf, so the dash-free
        //Iosevka.ttf IS the (Extended-grade) Regular static instance. Shipping
        //Iosevka-Regular.ttf as well would duplicate the same bytes; its
        //manifest weight-400 entry points at Iosevka.ttf instead. This test
        //keeps that a decision rather than an accident.
        var path = Path.Combine(TestAssetPaths.FontsFolder, "Iosevka-Regular.ttf");

        //Assert
        File.Exists(path).Should().BeFalse();
    }

    [Fact]
    public void No_upstream_variable_font_name_token_survives()
    {
        //Arrange — the upstream Google Fonts variable-font files carry a
        //"-VariableFont_" token (e.g. RobotoMono-VariableFont_wght.ttf); they
        //are renamed to the dash-free family name on the way in.
        var offenders = Directory.GetFiles(TestAssetPaths.FontsFolder, "*.ttf")
            .Select(Path.GetFileName)
            .Where(name => name!.Contains("VariableFont"))
            .ToList();

        //Assert
        offenders.Should().BeEmpty();
    }

    [Fact]
    public void No_upstream_Extended_width_token_survives_in_any_Iosevka_name()
    {
        //Arrange — the bundled Iosevka files are upstream's Extended width
        //grade (0.6 em advance, matching Roboto Mono and Noto Sans Mono), but
        //they ship under plain Iosevka names; the manifest declares them as the
        //Normal stretch because they are the only width grade in the package.
        var offenders = Directory.GetFiles(TestAssetPaths.FontsFolder, "*.ttf")
            .Select(Path.GetFileName)
            .Where(name => name!.Contains("Extended"))
            .ToList();

        //Assert
        offenders.Should().BeEmpty();
    }

    [Fact]
    public void Uprimarker_file_is_present()
        => File.Exists(TestAssetPaths.UprimarkerPath).Should().BeTrue();

    [Fact]
    public void Uprimarker_file_is_empty()
    {
        //Arrange
        var info = new FileInfo(TestAssetPaths.UprimarkerPath);

        //Assert
        info.Length.Should().Be(0L);
    }

    [Fact]
    public void Variable_font_is_non_trivial_size()
    {
        //Arrange
        var info = new FileInfo(TestAssetPaths.VariableFontPath);

        //Assert
        info.Length.Should().BeGreaterThan(100_000L);
    }
}
