using System.IO;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.Fonts.RobotoMono.Tests;

public class TargetsFileTests
{
    [Fact]
    public void Targets_file_is_present()
        => File.Exists(TestAssetPaths.TargetsFilePath).Should().BeTrue();

    [Fact]
    public void Targets_file_declares_codebrix_target_name()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("Name=\"CodeBrixRemoveUnusedRobotoMono\"");
    }

    [Fact]
    public void Targets_file_hooks_after_codebrix_add_library_assets()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("AfterTargets=\"_CodeBrixAddLibraryAssets\"");
    }

    [Fact]
    public void Targets_file_uses_net10_lib_paths()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("lib\\net10.0\\CodeBrix.Platform.Fonts.RobotoMono\\Fonts");
    }

    [Fact]
    public void Targets_file_contains_no_foreign_family_token()
    {
        //Arrange — this package was authored by mirroring the sibling
        //Merriweather package, so that is the realistic stray token. A stray
        //"Roboto" cannot be tested for; it legitimately appears in every path.
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().NotContain("Merriweather");
        content.Should().NotContain("Serif");
    }

    [Fact]
    public void Targets_file_supports_font_manifest_condition_present()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        content.Should().Contain("$(SupportsFontManifest)");
    }

    [Fact]
    public void Targets_file_never_removes_a_dash_free_font()
    {
        //Arrange
        var content = File.ReadAllText(TestAssetPaths.TargetsFilePath);

        //Assert
        // The dash-free fonts must not appear in a Remove= expression; only the
        // dash-bearing static fonts are pruned. The three companions matter
        // most here: they carry the polytonic Greek, Armenian and Georgian
        // coverage, so pruning them would silently drop scripts.
        content.Should().NotContain("Fonts\\RobotoMono.ttf\"");
        content.Should().NotContain("Fonts\\NotoSansMono.ttf\"");
        content.Should().NotContain("Fonts\\Iosevka.ttf\"");
        content.Should().NotContain("Fonts\\NotoSansGeorgian.ttf\"");
    }
}
