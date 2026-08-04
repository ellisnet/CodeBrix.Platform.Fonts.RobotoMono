using System;
using System.IO;

namespace CodeBrix.Platform.Fonts.RobotoMono.Tests;

internal static class TestAssetPaths
{
    public static string TestAssetsRoot { get; } =
        Path.Combine(AppContext.BaseDirectory, "TestAssets");

    public static string FontsFolder { get; } =
        Path.Combine(TestAssetsRoot, "Fonts");

    public static string ManifestPath { get; } =
        Path.Combine(FontsFolder, "RobotoMono.ttf.manifest");

    public static string VariableFontPath { get; } =
        Path.Combine(FontsFolder, "RobotoMono.ttf");

    public static string UprimarkerPath { get; } =
        Path.Combine(TestAssetsRoot, "CodeBrix.Platform.Fonts.RobotoMono.uprimarker");

    public static string TargetsFilePath { get; } =
        Path.Combine(TestAssetsRoot, "buildTransitive", "net10.0", "CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets");

    public static string DescriptorPath { get; } =
        Path.Combine(TestAssetsRoot, "CODEBRIX-DEVELOP.json");

    /// <summary>
    /// The companion families that supply the coverage Roboto Mono itself does not
    /// carry: polytonic Greek (Noto Sans Mono), Armenian (Iosevka) and Georgian
    /// (Noto Sans Georgian). Each ships a dash-free font plus its own manifest.
    /// The dash-free file is a variable font for Noto Sans Mono and Noto Sans
    /// Georgian, and a static Regular instance for Iosevka (Iosevka publishes no
    /// variable-font .ttf).
    /// </summary>
    public static string[] CompanionFamilies { get; } =
        ["NotoSansMono", "Iosevka", "NotoSansGeorgian"];

    public static string CompanionFontPath(string family) =>
        Path.Combine(FontsFolder, family + ".ttf");

    public static string CompanionManifestPath(string family) =>
        Path.Combine(FontsFolder, family + ".ttf.manifest");
}
