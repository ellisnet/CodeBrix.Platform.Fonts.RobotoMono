# CodeBrix.Platform.Fonts.RobotoMono

A redistribution of the Roboto Mono monospace font family packaged as a CodeBrix-family NuGet library for .NET 10 applications.
CodeBrix.Platform.Fonts.RobotoMono is a content-files font package for CodeBrix.Platform applications — supplying the Roboto Mono variable font and its static instances as build-time assets — and is equally usable as a plain content-files NuGet in any .NET 10 project that wants the Roboto Mono font set.
Roboto Mono covers the Latin, modern Greek and Cyrillic scripts but not polytonic Greek, Armenian or Georgian, so this package also bundles three companion families that supply that coverage: Noto Sans Mono (polytonic Greek), Iosevka (the Armenian script, in a monospace design), and Noto Sans Georgian (the Georgian script).
The library has no managed dependencies other than .NET, and is provided as a .NET 10 library and associated `CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever` NuGet package.

CodeBrix.Platform.Fonts.RobotoMono supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## Installation

```
dotnet add package CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever
```

Note that the NuGet package ID and the assembly name are different - there is no package named plain `CodeBrix.Platform.Fonts.RobotoMono`:

* NuGet package ID: `CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever`
* Assembly and content-folder name: `CodeBrix.Platform.Fonts.RobotoMono` - the name that the `ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/...` URIs shown below resolve against.

The assembly carries no managed API and nothing to `using` - everything a consumer uses is a font file path or an MSBuild property. The package has no dependencies beyond .NET itself.

## CodeBrix.Platform.Fonts.RobotoMono supports:

* The Roboto Mono variable font (`RobotoMono.ttf`) covering the full weight axis (100-700), used directly on every platform.
* 10 static `.ttf` font files covering the Light/Regular/Medium/SemiBold/Bold weights in Normal and Italic styles — for platforms that resolve fonts through the static-instance manifest. (Roboto Mono is published with no ExtraBold weight; its weight axis stops at Bold.)
* Three companion font families that extend script coverage beyond what Roboto Mono itself carries:
  * **Noto Sans Mono** (`NotoSansMono.ttf` plus 6 static instances) — polytonic Greek, plus a second full Latin/Greek/Cyrillic monospace set.
  * **Iosevka** (`Iosevka.ttf` plus 5 static instances) — the Armenian script, in a monospace design. Iosevka publishes no variable-font `.ttf`, so the dash-free `Iosevka.ttf` is the static Regular instance and doubles as the manifest's weight-400 entry.
  * **Noto Sans Georgian** (`NotoSansGeorgian.ttf` plus 6 static instances) — the Georgian script. (Iosevka does not cover Georgian, so this proportional companion supplies it, exactly as in the sibling CodeBrix.Platform.Fonts.Roboto package.)
* A `.ttf.manifest` JSON file per family that maps `font_style` / `font_weight` / `font_stretch` triples to the matching static font file.
* A `CODEBRIX-DEVELOP.json` descriptor that tells CodeBrix.Develop how to wire this font into a generated application and which software-keyboard layouts the package's glyph coverage supports.
* A `buildTransitive` MSBuild `.targets` file (hooking into the CodeBrix.Platform `_CodeBrixAddLibraryAssets` target) that prunes the redundant static font files at build time on platforms that don't need them, while always keeping the four dash-free fonts available.
* The CodeBrix `.uprimarker` file so CodeBrix.Platform build pipelines discover the package as a UPRI-bearing font asset library.

## Sample Code

### Reference the font from XAML (CodeBrix.Platform app)

```xml
<TextBlock Text="Hello, world."
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf" />
```

### Reference a specific static weight

```xml
<TextBlock Text="Bold sample"
           FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Bold.ttf" />
```

### Set Roboto Mono as the default text font (CodeBrix.Platform app)

```csharp
global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
    "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf";
```

Note that the font URI carries no `#FamilyName` fragment. CodeBrix.Platform strips such a fragment before resolving the font, and leaving it on the value assigned to `DefaultTextFontFamily` prevents the startup font-manifest preload from finding the manifest.

## Monospace metrics

Roboto Mono and Noto Sans Mono both use a 0.6 em character advance (every glyph is 0.6 times the font size wide). Iosevka's default width grade is 0.5 em, which would break column alignment wherever Armenian text lands in a character grid — so this package ships Iosevka's **Extended** width grade, whose 0.6 em advance matches the other two families exactly. The Extended faces ship under plain `Iosevka` file names and are declared as the Normal stretch in the manifest, because they are the only width grade in the package. Noto Sans Georgian is a proportional face: no monospace font with Georgian coverage is available under a suitable license, so Georgian text does not keep the character grid.

The bundled Iosevka files are the unhinted builds. CodeBrix.Platform renders text through Skia, which does not execute TrueType hinting instructions, so the hinted builds would be markedly larger and change nothing in rendering.

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete reference and usage guide written for AI coding agents - point your agent at that file when it is writing code or XAML against this package. It covers the full font inventory, the manifest format, weight/style selection and the monospace-grid rules.

Additional sample code and usage examples are available in the `CodeBrix.Platform.Fonts.RobotoMono.Tests` project:
https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/tree/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests

## License

CodeBrix.Platform.Fonts.RobotoMono is licensed under the SIL Open Font License, Version 1.1 - see the
[LICENSE](https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/LICENSE) file. The licence
covers the entire package: the library code, the `.targets` file, the packaging wrapper, and the bundled Roboto
Mono, Noto Sans Mono, Iosevka and Noto Sans Georgian `.ttf` font files alike.

The full licence text is bundled with this repository three times at the repository root — `OFL-Roboto.txt`,
`OFL-Noto.txt` and `OFL-Iosevka.txt`, one per bundled font project, identical in licence body and differing only
in their copyright headers — and the same three files are packaged inside the produced NuGet. The package is
published under the SPDX expression `OFL-1.1`.

None of the four bundled families declares a Reserved Font Name, and every `.ttf` is redistributed bit-for-bit
unmodified (some files are renamed on the way in; the bytes are untouched).

For licensing and provenance information about the open source code included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/THIRD-PARTY-NOTICES.txt).
