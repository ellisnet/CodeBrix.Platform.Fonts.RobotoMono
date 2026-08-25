================================================================================
AGENT-README: CodeBrix.Platform.Fonts.RobotoMono
A Guide for AI Coding Agents — CONSUMING the
CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever NuGet package
================================================================================


OVERVIEW
========

CodeBrix.Platform.Fonts.RobotoMono is a font ASSET package: a redistribution of
the Roboto Mono monospace font family, plus three companion families, packaged
as build-time content for CodeBrix.Platform applications and usable as a plain
content-files NuGet in any .NET 10 project that wants the font bytes.

Target framework: .NET 10 or later.

The package exposes NO public managed types. The assembly it ships is
metadata-only; the payload is font files, a font manifest per family, a
descriptor and an MSBuild targets file. Consumers reference the fonts by URI,
never by C# type.

What is in the box:

  * 31 `.ttf` files, referenced through
    `ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/<file>.ttf`.
  * 4 `.ttf.manifest` JSON files (one per family) mapping
    {font_style, font_weight, font_stretch} triples to the matching static
    font file's URI.
  * `CODEBRIX-DEVELOP.json` at the package root — the font's self-description
    (display name, primary URI, App.xaml resource key, ordered fallback URIs,
    supported software-keyboard layout ids).
  * A `.uprimarker` file that CodeBrix.Platform build pipelines use to discover
    UPRI-bearing font asset packages.
  * A `buildTransitive` MSBuild `.targets` file that prunes the redundant
    static fonts at consumer-build time on platforms that cannot use the
    manifest, while always keeping the four dash-free fonts.

Why four families and not one. Roboto Mono covers the Latin, modern Greek and
Cyrillic scripts, but NOT polytonic Greek (the Greek Extended block), Armenian
or Georgian. Three companion families supply that coverage:

  Noto Sans Mono      polytonic Greek, plus a second full Latin/Greek/Cyrillic
                      monospace set; 0.6 em advance, matching Roboto Mono.
  Iosevka             the Armenian script in a monospace design, shipped in the
                      Extended width grade so its 0.6 em advance matches.
  Noto Sans Georgian  the Georgian script — proportional, because no monospace
                      Georgian face exists under a suitable license.

Provenance: this package is not a port of anything. The `.ttf` binaries are
redistributed bit-for-bit unmodified (some are renamed on the way in; the bytes
are untouched); the packaging files are original CodeBrix-family work. Per-file
attribution is in the packaged THIRD-PARTY-NOTICES.txt.


INSTALLATION
============

NuGet PackageId:  CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever

  dotnet add package CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever

NuGet dependencies: none. The package pulls in nothing.

License: OFL-1.1 (SIL Open Font License 1.1). The WHOLE package is under it —
the wrapper assembly, the `.targets` file and all four bundled font families
alike. The license text ships three times in the nupkg root, one per upstream
font project: OFL-Roboto.txt, OFL-Noto.txt and OFL-Iosevka.txt (identical
license body, different copyright headers). None of the four families declares
a Reserved Font Name, so OFL-1.1 condition 3 restricts no name used here. The
`.ttf` bytes must not be altered; renaming files is fine.

Requirements and limits:

  * .NET 10 or later. No native libraries, no OS restrictions.
  * `ms-appx:///` URIs are resolved by the CodeBrix.Platform runtime, not by
    .NET. Outside a CodeBrix.Platform host the URIs mean nothing — see the
    plain-.NET example below for what to do instead.
  * Referencing the package is enough to get the fonts into a CodeBrix.Platform
    app's assets; the `buildTransitive` targets file is imported automatically
    by NuGet convention (its on-disk file name matches the PackageId).
  * Package payload is large by NuGet standards: about 49 MB of `.ttf`, of
    which the six Iosevka faces are about 43 MB. See PERFORMANCE TIPS.

The assembly and content-folder name is `CodeBrix.Platform.Fonts.RobotoMono`
(no `.OflLicenseForever` suffix — that exists only on the PackageId, for
license disambiguation across the CodeBrix family). The URIs are rooted at that
assembly name, so it appears verbatim in every font URI.


KEY NAMESPACES / USINGS
=======================

None. The package declares no public managed types, so no `using` directive
refers to it, and nothing in it can be `new`ed, called or subclassed. Its
"namespace" from a consumer's point of view is the content URI root:

    ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/

Everything below is a file under that root.

The one C# API that appears in the examples —
`CodeBrix.Platform.UI.FeatureConfiguration.Font` — belongs to CodeBrix.Platform,
not to this package. It is shown fully qualified (`global::CodeBrix...`) exactly
as CodeBrix.Platform applications write it, so no using is needed for it either.

Never append a `#FamilyName` fragment to these URIs (see COMMON PITFALLS).


FONT INVENTORY
==============

31 `.ttf` files and 4 `.ttf.manifest` files. Every file name below is exact.

PRIMARY FAMILY — Roboto Mono (11 files)
---------------------------------------

  RobotoMono.ttf
      The variable font, covering the weight axis. This is the dash-free file:
      it is present on every platform and is never pruned. Reference this one
      unless you have a reason not to.

  RobotoMono-Light.ttf          RobotoMono-LightItalic.ttf
  RobotoMono-Regular.ttf        RobotoMono-Italic.ttf
  RobotoMono-Medium.ttf         RobotoMono-MediumItalic.ttf
  RobotoMono-SemiBold.ttf       RobotoMono-SemiBoldItalic.ttf
  RobotoMono-Bold.ttf           RobotoMono-BoldItalic.ttf
      Ten static instances: five weights (Light 300, Regular 400, Medium 500,
      SemiBold 600, Bold 700) in two styles (Normal, Italic), Normal stretch
      only. These are what the manifest selects on platforms that resolve fonts
      through it.

  Roboto Mono publishes NO ExtraBold (800) and no Black (900) — its weight axis
  stops at Bold (700). Unlike the sibling CodeBrix font packages, the primary
  manifest here therefore has five weights, not six. Upstream's Thin (100) and
  ExtraLight (200) static instances are deliberately not bundled; those weights
  remain reachable through the variable font. There is no italic variable font
  in the package either — italics come from the static instances.

COMPANION FAMILIES (20 files)
-----------------------------

  NotoSansMono.ttf            + NotoSansMono-{Light,Regular,Medium,SemiBold,
                                Bold,ExtraBold}.ttf
      Polytonic Greek (Roboto Mono covers 1 of the 233 assigned codepoints in
      Greek Extended), plus a second full Latin/Greek/Cyrillic monospace set.
      Six weights, Normal stretch, upright only. The dash-free file is the
      upstream variable font (width + weight axes), renamed.

  Iosevka.ttf                 + Iosevka-{Light,Medium,SemiBold,Bold,
                                ExtraBold}.ttf
      The Armenian script. Six weights, upright only — but note there is no
      `Iosevka-Regular.ttf`: the dash-free `Iosevka.ttf` IS the Regular static
      instance, and the manifest's weight-400 entry points at it. See the two
      Iosevka quirks below.

  NotoSansGeorgian.ttf        + NotoSansGeorgian-{Light,Regular,Medium,
                                SemiBold,Bold,ExtraBold}.ttf
      The Georgian script. Six weights, Normal stretch, upright only.
      Proportional — see below.

  None of the three companions ships an italic face, so italic text in the
  scripts they serve renders upright.

MANIFESTS
---------

  RobotoMono.ttf.manifest        10 entries (5 weights x 2 styles, 300-700)
  NotoSansMono.ttf.manifest       6 entries (300-800, Normal style)
  Iosevka.ttf.manifest            6 entries (300-800, Normal style; the
                                   weight-400 entry points at Iosevka.ttf)
  NotoSansGeorgian.ttf.manifest   6 entries (300-800, Normal style)

Each manifest is a JSON object with a `fonts` array; each element is

    {
      "font_style":   "Normal" | "Italic",
      "font_weight":  300 | 400 | 500 | 600 | 700 | 800,
      "font_stretch": "Normal",
      "family_name":  "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/..."
    }

Every entry in every manifest declares font_stretch "Normal".

THE IOSEVKA QUIRK — NO VARIABLE FONT
------------------------------------

Iosevka publishes static TTF/TTC and webfont formats only; no variable-font
`.ttf` exists upstream. The dash-free `Iosevka.ttf` — the slot the other three
families fill with a variable font — is the static Extended-Regular instance.
Consequence for consumers: on a platform WITHOUT manifest support, where the
dash-bearing statics are pruned from the app, Armenian text renders at Regular
weight only; Bold and Light requests get whatever synthesis the platform
applies. That is a known, documented degradation, not a bug to route around.

THE IOSEVKA WIDTH GRADE — EXTENDED, DECLARED AS NORMAL
------------------------------------------------------

Roboto Mono and Noto Sans Mono both use a 0.6 em character advance. Iosevka's
default width grade is 0.5 em, which would break column alignment wherever
Armenian lands in a character grid, so the package ships upstream's EXTENDED
width grade (exactly 0.6 em) under plain `Iosevka`/`Iosevka-{Weight}` names.
The manifest declares those faces as the Normal stretch — they are the only
width grade in the package, so the Normal-stretch slot is theirs. A consumer
asking for a non-Normal FontStretch will find no manifest entry for it.

NOTO SANS GEORGIAN IS PROPORTIONAL
----------------------------------

No monospace font with Georgian letter coverage is available under a suitable
license. Georgian text therefore does NOT keep the character grid, even though
everything around it does. Plan Georgian-bearing UI accordingly (do not align
columns by character count across Georgian text).


HOW WEIGHT AND STYLE SELECT A FILE
==================================

There are two ways to put Roboto Mono on the screen, and they behave
differently:

  1. Reference the dash-free family URI
     (`.../Fonts/RobotoMono.ttf`) and let the element's FontWeight / FontStyle /
     FontStretch drive the choice. On a platform that supports the font
     manifest, those three properties are matched against the
     {font_style, font_weight, font_stretch} triples in
     `RobotoMono.ttf.manifest` and the matching static file is used. On a
     platform without manifest support, the variable font itself covers the
     weight axis. This is the recommended form: it is the only one that works
     identically on every head.

  2. Reference a specific static file directly
     (`.../Fonts/RobotoMono-Bold.ttf`). Simple, but the dash-bearing statics
     are pruned on platforms that do not support the manifest, so a direct
     static reference is not portable across heads (see COMMON PITFALLS).

What the manifest can and cannot give you, per family:

  Roboto Mono         weights 300, 400, 500, 600, 700 in Normal AND Italic.
                      Nothing above 700 exists: a request for ExtraBold (800)
                      or Black (900) cannot select a heavier face, so it
                      resolves to Bold (700) — the top of both the manifest and
                      the variable font's weight axis. Do not expect a heavier
                      rendering, and do not add a fake 800 entry.
                      Weights below 300 (Thin 100, ExtraLight 200) have no
                      static entry; they come from the variable font.
  Noto Sans Mono      weights 300-800, Normal style only. An italic request
                      finds no Italic entry and renders upright.
  Iosevka             weights 300-800, Normal style only, and only when the
                      manifest is available (see the quirk above).
  Noto Sans Georgian  weights 300-800, Normal style only.

  All four            font_stretch is "Normal" in every entry. A Condensed or
                      Expanded request has nothing to match.


FALLBACK COVERAGE — WHAT THE CONSUMER MUST DO
=============================================

Referencing this package does NOT by itself make polytonic Greek, Armenian or
Georgian text render: it makes the companion font FILES available. Something has
to tell the platform to consult them for codepoints the primary font lacks.
There are two ways, and an app usually wants the first:

  1. Register them as fallback families at startup, in the order
     `CODEBRIX-DEVELOP.json` prescribes (metric-compatible monospace families
     first, proportional Georgian last):

       global::CodeBrix.Platform.UI.FeatureConfiguration.Font.FallbackFontFamilies =
       [
           "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono.ttf",
           "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka.ttf",
           "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian.ttf",
       ];

  2. Set FontFamily explicitly on the elements that carry that script — e.g.
     bind an Armenian label's FontFamily to
     `.../Fonts/Iosevka.ttf`. Use this when only a known part of the UI is in
     the companion's script.

Whichever you choose, use the dash-free companion URIs: those four files are
never pruned, so they resolve on every platform.


CODEBRIX-DEVELOP.JSON — THE FONT'S SELF-DESCRIPTION
===================================================

`CODEBRIX-DEVELOP.json` sits at the nupkg root. CodeBrix.Develop's "New
CodeBrix.Platform Application" experience reads it to learn how to wire this
font into a generated application instead of carrying per-font logic; an agent
wiring the font by hand should read the same values rather than inventing them.

  schemaVersion     1. A consumer that does not recognise the value should
                    decline the font with a clear message rather than guess.
  packageId         CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever
  displayName       "Roboto Mono" — the typographic family name to show a user
                    and to write into generated source.
  fontFamilyUri     ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/
                    RobotoMono.ttf   (no `#` fragment)
  resourceKey       "RobotoMonoFont" — the App.xaml resource key a generated
                    application defines and pages reference with
                    {StaticResource RobotoMonoFont}.
  fallbackFontUris  The three companion URIs, in order: NotoSansMono.ttf, then
                    Iosevka.ttf, then NotoSansGeorgian.ttf. The order is
                    deliberate — metric-compatible monospace families first.
  keyboardLayouts   38 software-keyboard layout ids whose characters this
                    package's glyph coverage supports, as the UNION across the
                    primary font and its three companions:
                      en, en-GB, de, de-CH, fr, fr-BE, fr-CH, nl, es, pt, it,
                      mt, sq, tr, el, da, no, sv, fi, is, lt, lv, et, pl, cs,
                      sk, hu, ro, hr, sr-Latn, ru, uk, be, bg, sr, mk, ka, hy
                    There is deliberately no "unsupported" list: any id absent
                    from this array is not supported. `ka` (Georgian) and `hy`
                    (Armenian) are delivered by the companions, so they hold
                    only if fallbacks are wired up as described above.


CORE API REFERENCE
==================

There is no managed API. The complete consumer contract is:

1. THE CONTENT URIs
   `ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/<file>.ttf`, used as a
   FontFamily value in XAML, in code that builds XAML element trees, in an
   App.xaml resource, or as the CodeBrix.Platform default text font:

     global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
         "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf";

   The full list of 31 URIs is in the QUICK REFERENCE CARD at the end.

2. THE FONT MANIFESTS
   `<family>.ttf.manifest` beside each dash-free font, in the shape shown under
   FONT INVENTORY. Consumers do not read these files themselves; the platform
   does, when it supports manifest-based resolution. Their content is the
   contract for which weight/style combinations exist.

3. THE DESCRIPTOR
   `CODEBRIX-DEVELOP.json` at the nupkg root, described above.

4. THE MSBUILD TARGET
   `buildTransitive/net10.0/
    CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets` is imported
   automatically into a consuming build (its file name matches the PackageId,
   which is what NuGet's auto-import convention requires). It defines:

     <Target Name="CodeBrixRemoveUnusedRobotoMono"
             AfterTargets="_CodeBrixAddLibraryAssets">

   When the MSBuild property `SupportsFontManifest` is not `true`, the target
   removes the dash-bearing (static) font files from the app's assets; the four
   dash-free fonts are never removed. Nothing else in the package reacts to
   MSBuild properties, and consumers normally set nothing — `SupportsFontManifest`
   is set by the CodeBrix.Platform head being built.

If a future iteration adds managed types they will live under the
`CodeBrix.Platform.Fonts.RobotoMono` root namespace and be documented here.


COMPLETE EXAMPLES
=================

Example 1 — one element in Roboto Mono
--------------------------------------

    <TextBlock Text="Hello, world."
               FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf" />

Example 2 — weight and style, chosen through the manifest
---------------------------------------------------------

Keep the dash-free family URI and let FontWeight / FontStyle pick the face. On a
manifest-capable head these resolve to RobotoMono-Bold.ttf, RobotoMono-Italic.ttf
and RobotoMono-SemiBoldItalic.ttf respectively; on other heads the variable font
covers the weight axis.

    <StackPanel>
      <TextBlock Text="Bold"
                 FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf"
                 FontWeight="Bold" />

      <TextBlock Text="Italic"
                 FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf"
                 FontStyle="Italic" />

      <TextBlock Text="SemiBold italic"
                 FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf"
                 FontWeight="SemiBold"
                 FontStyle="Italic" />
    </StackPanel>

A request the package cannot satisfy degrades rather than failing: FontWeight
"ExtraBold" (800) on Roboto Mono has no entry above 700, so it renders as Bold.

Example 3 — App.xaml resource, using the descriptor's resource key
------------------------------------------------------------------

Define the family once as an application resource under the key
`CODEBRIX-DEVELOP.json` names (`RobotoMonoFont`), then reference it by
{StaticResource}. `m:` is Microsoft.UI.Xaml.Media from CodeBrix.Platform.UI.

    <Application x:Class="MyApp.App"
         xmlns="clr-namespace:Microsoft.UI.Xaml;assembly=CodeBrix.Platform.UI"
         xmlns:m="clr-namespace:Microsoft.UI.Xaml.Media;assembly=CodeBrix.Platform.UI"
         xmlns:c="clr-namespace:Microsoft.UI.Xaml.Controls;assembly=CodeBrix.Platform.UI.FluentTheme"
         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

      <Application.Resources>
        <ResourceDictionary>
          <ResourceDictionary.MergedDictionaries>
            <c:XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
          </ResourceDictionary.MergedDictionaries>

          <!-- Reference the .ttf file directly; no #FamilyName fragment. -->
          <m:FontFamily x:Key="RobotoMonoFont">ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf</m:FontFamily>
        </ResourceDictionary>
      </Application.Resources>

    </Application>

Using it on a page (and inheriting it for every element on that page):

    <Page x:Class="MyApp.Views.MainPage"
          xmlns="clr-namespace:Microsoft.UI.Xaml.Controls;assembly=CodeBrix.Platform.UI"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          FontFamily="{StaticResource RobotoMonoFont}">

      <StackPanel>
        <TextBlock Text="Inherits Roboto Mono" />
        <TextBlock Text="Inherits it in bold" FontWeight="Bold" />
      </StackPanel>

    </Page>

Example 4 — application-wide default plus fallbacks
---------------------------------------------------

In App.xaml.cs, before InitializeComponent():

    public App()
    {
        //Roboto Mono becomes the default font for all text in the application.
        global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
            "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf";

        //Consulted for codepoints the default font has no glyph for:
        //polytonic Greek, then Armenian, then Georgian.
        global::CodeBrix.Platform.UI.FeatureConfiguration.Font.FallbackFontFamilies =
        [
            "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono.ttf",
            "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka.ttf",
            "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian.ttf",
        ];

        InitializeComponent();
    }

The URI assigned to DefaultTextFontFamily must carry no `#FamilyName` fragment —
that one is not merely useless, it breaks the startup manifest preload.

Example 5 — a specific script on a specific element
---------------------------------------------------

When only part of the UI is in a companion's script, name the companion
directly instead of relying on fallback:

    <TextBlock Text="Հայերեն"
               FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka.ttf" />

    <TextBlock Text="ქართული"
               FontFamily="ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian.ttf" />

Example 6 — plain .NET 10, outside a CodeBrix.Platform host
-----------------------------------------------------------

`ms-appx:///` means nothing to .NET itself. In a console app, a test project or
any non-platform consumer, take the bytes from the package folder in the NuGet
cache; the fonts live under lib/net10.0 and are not copied to your output
directory automatically:

    <nuget-cache>/codebrix.platform.fonts.robotomono.ofllicenseforever/
        <version>/lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf

    using System;
    using System.IO;

    string cache = Environment.GetEnvironmentVariable("NUGET_PACKAGES")
                   ?? Path.Combine(
                          Environment.GetFolderPath(
                              Environment.SpecialFolder.UserProfile),
                          ".nuget", "packages");

    string fontPath = Path.Combine(cache,
        "codebrix.platform.fonts.robotomono.ofllicenseforever",
        packageVersion,          // whatever version you referenced
        "lib", "net10.0",
        "CodeBrix.Platform.Fonts.RobotoMono", "Fonts", "RobotoMono.ttf");

    byte[] ttf = File.ReadAllBytes(fontPath);

Copying the `.ttf` files into your own project as content is the simpler option
if you need them at a predictable path.


MINIMUM VIABLE PROJECT
======================

A CodeBrix.Platform application head references this package like any other:

    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
      </PropertyGroup>
      <ItemGroup>
        <!-- plus the CodeBrix.Platform packages the head needs -->
        <PackageReference Include="CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever" />
      </ItemGroup>
    </Project>

(Version omitted on purpose — use the current published version, or a central
package-management entry.)

Then two edits and nothing else:

  1. App.xaml — add the `m:FontFamily` resource from Example 3 (key
     `RobotoMonoFont`).
  2. App.xaml.cs — set DefaultTextFontFamily (and, if the app shows polytonic
     Greek, Armenian or Georgian, FallbackFontFamilies) as in Example 4.

Pages then either inherit the default font or opt in with
`FontFamily="{StaticResource RobotoMonoFont}"`. No build-time configuration,
no MSBuild properties and no code generation are required: the package's own
`.targets` file does the platform-dependent pruning on its own.


PERFORMANCE TIPS
================

  * Size dominates here. The `.ttf` payload is about 49 MB, and the six Iosevka
    faces are about 43 MB of it (Iosevka is a large font; each face is roughly
    7 MB). If an application will never show Armenian text, referencing this
    package still carries those bytes — there is no per-family opt-out switch.
  * On heads that do not support the font manifest, the bundled `.targets` file
    already removes the dash-bearing statics from the app's assets, leaving four
    files. You get that for free by referencing the package; do not try to prune
    the assets yourself.
  * Prefer ONE font URI per family across the app (the dash-free one) and let
    FontWeight/FontStyle select faces. Naming many different `.ttf` files
    directly means many separate typefaces to load instead of one family.
  * Fallback lookups cost something per missing glyph: keep
    FallbackFontFamilies to the fonts the app actually needs, in the descriptor's
    order (the metric-compatible monospace families first), rather than listing
    all three when only one script appears.
  * Nothing in this package executes at run time — it has no code — so there is
    no startup cost attributable to it beyond loading the font files the
    application actually references.


COMMON PITFALLS TO AVOID
========================

  1. NEVER append a `#FamilyName` fragment to a font URI from this package.
     CodeBrix.Platform strips the fragment before resolving the font, so it buys
     nothing — and on the value assigned to
     `FeatureConfiguration.Font.DefaultTextFontFamily` it actively breaks the
     startup font-manifest preload, because the ".manifest" suffix the preload
     appends lands inside the URI fragment and is dropped.

  2. Do not reference a dash-bearing static file (e.g. RobotoMono-Bold.ttf)
     from code or XAML that must work on every head. Those files are pruned at
     build time on platforms without manifest support; the reference then points
     at a file that is not there. Reference the dash-free family URI and set
     FontWeight/FontStyle instead. The four dash-free fonts are never pruned.

  3. Referencing the package does not switch any font on. Nothing renders in
     Roboto Mono until an element's FontFamily, an App.xaml resource, or
     DefaultTextFontFamily names one of its URIs.

  4. Companion coverage is not automatic either. Polytonic Greek, Armenian and
     Georgian only appear if you wire FallbackFontFamilies (or set FontFamily
     directly on the elements carrying those scripts).

  5. Roboto Mono has no ExtraBold or Black. Weight requests above 700 resolve to
     Bold — the top of both the manifest and the variable font's axis. Do not
     plan a visual hierarchy that needs an 800 or 900 monospace weight from this
     package.

  6. The companions have no italic faces. Italic Armenian, italic polytonic
     Greek and italic Georgian render upright.

  7. `Iosevka.ttf` is NOT a variable font — it is the static Extended-Regular
     instance, and there is deliberately no `Iosevka-Regular.ttf`. Do not assume
     weight-axis behaviour from it; on non-manifest heads Armenian is
     Regular-weight only.

  8. Georgian is proportional. Any layout that assumes a fixed character advance
     (column alignment, ASCII art, code-style gutters) breaks where Georgian
     text appears, even though the rest of the text is monospaced.

  9. FontStretch has exactly one value in this package: every manifest entry
     declares "Normal". Condensed/Expanded requests match nothing.

 10. `ms-appx:///` URIs resolve only inside a CodeBrix.Platform host. In a
     console app or unit test they are just strings — read the file from the
     package folder instead (Example 6).

 11. Use the file names exactly as listed. The URIs are matched against real
     file names, and the four dash-free names in particular are load-bearing:
     the build-time prune keys on the presence of a dash.

 12. Do not modify the `.ttf` bytes. OFL-1.1 permits renaming and
     redistribution, and this package already redistributes the fonts
     unmodified; editing the binaries changes the licensing story.


WHAT THIS PACKAGE DOES NOT DO
=============================

  * No managed API. No types, no methods, no MSBuild properties for you to set;
    nothing to call, subclass or configure in C#.
  * It does not install fonts into the operating system, and does not register
    them as system fonts. They are application assets.
  * It does not choose itself. It never becomes the application's font without
    an explicit FontFamily, App.xaml resource or DefaultTextFontFamily
    assignment.
  * It does not resolve `ms-appx:///` URIs — CodeBrix.Platform does. Outside a
    platform host the package is just files on disk.
  * It does not implement fallback. It supplies fallback FONTS and states their
    preferred order in the descriptor; the platform performs the lookup and only
    when the application has registered them.
  * No italic companion faces (Noto Sans Mono, Iosevka, Noto Sans Georgian are
    upright-only here), and no italic variable font for Roboto Mono.
  * No monospace Georgian: none exists under a suitable license, so the Georgian
    companion is proportional.
  * No Iosevka variable font (upstream publishes none), and no `Iosevka-Regular.
    ttf` (the dash-free file is that instance).
  * No ExtraBold (800) or Black (900) Roboto Mono, and no bundled Thin (100) or
    ExtraLight (200) statics.
  * No width grades beyond the single Normal-stretch set — no Condensed, no
    Expanded, and no way to reach Iosevka's other width grades.
  * No coverage outside the 38 layout ids the descriptor lists: CJK, Hebrew,
    Arabic, Indic and Thai scripts, and emoji, are not in this package.
  * No subsetting, instancing or font-manipulation tooling, and no way to strip
    a family you do not use from the package.
  * No .NET-generic font loading helper: reading the `.ttf` bytes outside a
    CodeBrix.Platform host is your code (Example 6).


WORKING EXAMPLES ON GITHUB
==========================

The test project pins every fact in this file — read it when you need to be
certain a file, entry or value really exists:

  https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/tree/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests

  ContentFilePresenceTests.cs
      All 31 `.ttf` files are present; there is no `Iosevka-Regular.ttf`; no
      upstream "VariableFont"/"Extended" name token survived the renames.
      https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/ContentFilePresenceTests.cs

  ContentManifestTests.cs
      All four manifests deserialize; entry counts 10/6/6/6; weights 300-700 for
      the primary and 300-800 for the companions; Normal stretch only;
      companions upright-only; every family_name URI is rooted at
      ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/ and points at a file
      that exists; the Iosevka weight-400 entry points at Iosevka.ttf.
      https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/ContentManifestTests.cs

  DescriptorTests.cs
      CODEBRIX-DEVELOP.json declares schemaVersion 1; packageId matches the
      published PackageId; the resource key follows the family convention;
      fontFamilyUri and every fallbackFontUri carry no `#` fragment and point at
      fonts this package ships; the fallbacks are the three companions;
      keyboardLayouts has no duplicates and claims the scripts the companions
      supply.
      https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/DescriptorTests.cs

  TargetsFileTests.cs
      The buildTransitive `.targets` declares CodeBrixRemoveUnusedRobotoMono,
      hooks AfterTargets="_CodeBrixAddLibraryAssets", carries the
      SupportsFontManifest condition, uses net10 lib paths, and never removes a
      dash-free font.
      https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/TargetsFileTests.cs

  AssemblyMetadataTests.cs
      The shipped assembly is named CodeBrix.Platform.Fonts.RobotoMono, targets
      net10, exports no public types, and has its `.uprimarker` sibling.
      https://github.com/ellisnet/CodeBrix.Platform.Fonts.RobotoMono/blob/main/tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/AssemblyMetadataTests.cs


QUICK REFERENCE CARD
====================

PACKAGE
  Id            CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever
  License       OFL-1.1        TFM  net10.0 or later     Dependencies  none
  Public types  none (metadata-only assembly)
  URI root      ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/
  Resource key  RobotoMonoFont          Display name  Roboto Mono
  Primary URI   ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf
  Fallbacks     NotoSansMono.ttf -> Iosevka.ttf -> NotoSansGeorgian.ttf
  Never pruned  RobotoMono.ttf, NotoSansMono.ttf, Iosevka.ttf,
                NotoSansGeorgian.ttf

THE 31 FONT URIs

PRIMARY FAMILY - Roboto Mono (11 files; Latin, modern Greek, Cyrillic)
----------------------------------------------------------------------
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Light.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-LightItalic.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Regular.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Italic.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Medium.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-MediumItalic.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-SemiBold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-SemiBoldItalic.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-BoldItalic.ttf

COMPANION - Noto Sans Mono (7 files; polytonic Greek + a second LGC set)
------------------------------------------------------------------------
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono-Light.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono-Regular.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono-Medium.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono-SemiBold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono-ExtraBold.ttf

COMPANION - Iosevka (6 files; Armenian; no -Regular by design)
--------------------------------------------------------------
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka-Light.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka-Medium.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka-SemiBold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka-ExtraBold.ttf

COMPANION - Noto Sans Georgian (7 files; Georgian; proportional)
----------------------------------------------------------------
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian-Light.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian-Regular.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian-Medium.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian-SemiBold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian-ExtraBold.ttf

MANIFEST COVERAGE
  RobotoMono         300/400/500/600/700  x  Normal + Italic   (10 entries)
  NotoSansMono       300..800             x  Normal             (6 entries)
  Iosevka            300..800             x  Normal             (6 entries)
                     (400 -> Iosevka.ttf; no -Regular file)
  NotoSansGeorgian   300..800             x  Normal             (6 entries)
  font_stretch is "Normal" everywhere. Weight > 700 on Roboto Mono -> Bold.

TYPICAL WIRING
  App.xaml    <m:FontFamily x:Key="RobotoMonoFont">ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf</m:FontFamily>
  Page        FontFamily="{StaticResource RobotoMonoFont}"
  Element     FontFamily="ms-appx:///.../RobotoMono.ttf" FontWeight="Bold"
  App.xaml.cs FeatureConfiguration.Font.DefaultTextFontFamily = "<primary URI>"
              FeatureConfiguration.Font.FallbackFontFamilies  = [ <3 URIs> ]

BUILD-TIME BEHAVIOUR
  Target      CodeBrixRemoveUnusedRobotoMono
              AfterTargets="_CodeBrixAddLibraryAssets"
  Effect      SupportsFontManifest != true  ->  dash-bearing statics removed
              dash-free fonts always kept

RULES OF THUMB
  * no `#FamilyName` fragment, ever
  * reference the dash-free URI + FontWeight/FontStyle, not a static file
  * wire FallbackFontFamilies or the companion scripts will not render
  * Georgian is proportional; Armenian is Regular-only without the manifest
  * ms-appx URIs need a CodeBrix.Platform host

================================================================================
END OF AGENT-README
================================================================================
