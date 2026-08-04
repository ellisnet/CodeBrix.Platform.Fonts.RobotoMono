========================================================================
AGENT-README: CodeBrix.Platform.Fonts.RobotoMono
A Comprehensive Guide for AI Coding Agents
========================================================================


OVERVIEW
========================================================================

CodeBrix.Platform.Fonts.RobotoMono is a .NET 10 redistribution of the
Roboto Mono monospace font family, packaged for the CodeBrix family. It
supplies the Roboto Mono variable font and a curated set of static
instances as build-time content assets for CodeBrix.Platform-forked
applications, and is equally usable as a plain content-files NuGet in any
.NET 10 project.

Roboto Mono covers the Latin, modern Greek and Cyrillic scripts but NOT
polytonic Greek (the Greek Extended block), Armenian or Georgian. This
package therefore also bundles three COMPANION families that supply that
coverage:

  - Noto Sans Mono   — polytonic Greek (and a second full LGC monospace
                       set), 0.6 em advance matching Roboto Mono.
  - Iosevka          — the Armenian script in a monospace design, shipped
                       in the Extended width grade so its 0.6 em advance
                       matches the other two families.
  - Noto Sans Georgian — the Georgian script (proportional; no monospace
                       Georgian font exists under a suitable license).

That three-companion structure mirrors the sibling Merriweather package
(which bundles Noto Serif, Noto Serif Armenian and Noto Serif Georgian),
and it is the thing to understand before changing anything here. Two
structural quirks are unique to this package and covered in detail below:
Iosevka has no variable font, and the primary family has no ExtraBold.

The library has effectively no managed code: the assembly is a metadata-
only .NET 10 DLL whose sole purpose is to host the bundled font content
files. The interesting payload lives in:

  - 31 `.ttf` font files (3 variable + 27 static + 1 static standing in
    the variable slot) under
    lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/ inside the nupkg.
  - Four `.ttf.manifest` JSON files (one per family) mapping
    font_style/font_weight/font_stretch triples to the matching static
    font file path.
  - A `CODEBRIX-DEVELOP.json` descriptor at the package root that tells
    CodeBrix.Develop how to wire this font into a generated application.
  - A `.uprimarker` file that CodeBrix.Platform build pipelines use to
    discover UPRI-bearing font asset packages.
  - An MSBuild `.targets` file under buildTransitive/net10.0/ that hooks
    into the CodeBrix.Platform `_CodeBrixAddLibraryAssets` target and
    prunes the redundant static fonts at consumer-build time, depending on
    the `SupportsFontManifest` MSBuild property — while always keeping the
    four dash-free fonts present.


INSTALLATION
========================================================================

NuGet package: CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever

  dotnet add package CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever

The library namespace inside the assembly is
`CodeBrix.Platform.Fonts.RobotoMono` (without the `.OflLicenseForever`
suffix; that suffix exists only on the NuGet PackageId for
license-disambiguation across the CodeBrix family).

Target framework: .NET 10.0 or higher.


KEY NAMESPACE
========================================================================

The library exposes no public managed types in its first iteration — the
assembly is metadata-only. Consumers reference the bundled font content
files via `ms-appx:///` URIs rooted at the assembly content folder:

  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono-Bold.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansMono.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/Iosevka.ttf
  ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/NotoSansGeorgian.ttf
  ...etc.

Do NOT append a `#FamilyName` fragment to these URIs. CodeBrix.Platform
strips the fragment before resolving the font, so it buys nothing — and
on the value assigned to `FeatureConfiguration.Font.DefaultTextFontFamily`
it actively breaks the startup font-manifest preload, because the
".manifest" suffix the preload appends lands inside the URI fragment and
is then dropped.


FONT INVENTORY
========================================================================

The package ships 31 `.ttf` files plus 4 `.ttf.manifest` files.

PRIMARY FAMILY — Roboto Mono (11 files)

Variable font (always present on every platform):
  RobotoMono.ttf — covers the weight axis (100-700). Renamed, byte-for-
                   byte, from the upstream variable-font file
                   `RobotoMono-VariableFont_wght.ttf`.

Static fonts (used where fonts are resolved via the static manifest):
  Five weights (Light, Regular, Medium, SemiBold, Bold) in two styles
  (Normal, Italic), Normal stretch only:
    RobotoMono-{Weight}{Italic?}.ttf   (10 files)

  Note: upstream Roboto Mono also ships Thin (100) and ExtraLight (200)
  static instances; those are intentionally NOT bundled (they remain
  reachable through the variable font). Roboto Mono publishes NO ExtraBold
  (800) or Black (900) — its weight axis STOPS at Bold (700) — so unlike
  the sibling CodeBrix font packages, the primary manifest here covers
  five weights, not six. The companions all carry the usual six.

  Upstream also publishes an italic variable font
  (`RobotoMono-Italic-VariableFont_wght.ttf`); it is not bundled, matching
  the one-variable-font-per-family convention of the sibling packages.
  Italics come from the static instances.

COMPANION FAMILIES (20 files)

  NotoSansMono.ttf + 6 statics     — supplies POLYTONIC GREEK (Greek
                                     Extended), which Roboto Mono lacks
                                     almost entirely (1 of 233 assigned
                                     codepoints), plus a second full
                                     Latin/Greek/Cyrillic monospace set.
                                     Six weights (Light..ExtraBold),
                                     Normal stretch, upright only. The
                                     dash-free file is the upstream
                                     variable font (wdth + wght axes),
                                     renamed.
  Iosevka.ttf + 5 statics          — supplies the ARMENIAN script. Six
                                     weights, upright only — but see the
                                     QUIRK below: the dash-free file IS
                                     the Regular static, so only five
                                     dash-bearing statics exist.
  NotoSansGeorgian.ttf + 6 statics — supplies the GEORGIAN script. Six
                                     weights, Normal stretch, upright
                                     only. Bit-for-bit the same files as
                                     in the sibling
                                     CodeBrix.Platform.Fonts.Roboto
                                     package. Proportional (see below).

  None of the companions ships an italic face here, so italic text in the
  scripts they serve renders upright. For the two Noto families that is an
  upstream limitation; for Iosevka (which does publish italics upstream)
  it is a deliberate packaging decision for size and consistency.

Manifests:
  RobotoMono.ttf.manifest       — 10 entries (5 weights x 2 styles)
  NotoSansMono.ttf.manifest     —  6 entries
  Iosevka.ttf.manifest          —  6 entries (the weight-400 entry points
                                   at Iosevka.ttf itself)
  NotoSansGeorgian.ttf.manifest —  6 entries

  Each is a JSON object with a `fonts` array mapping
  {font_style, font_weight, font_stretch} triples to the matching static
  font file's `ms-appx:///` URI.

THE IOSEVKA QUIRK — NO VARIABLE FONT
------------------------------------------------------------------------

Iosevka's releases publish static TTF/TTC and webfont formats ONLY — no
variable-font `.ttf` exists upstream. The dash-free `Iosevka.ttf` (the
slot every sibling package fills with a variable font) is therefore the
static Extended-Regular instance, renamed. Two consequences:

  1. On platforms WITHOUT manifest support (where the .targets prune
     removes every dash-bearing file), Armenian text renders at Regular
     weight only. Bold/Light requests fall back to whatever synthesis the
     platform applies. This is a known, documented degradation — do not
     "fix" it by shipping a fake variable font.
  2. To avoid shipping the same ~7 MB of bytes twice, there is NO
     `Iosevka-Regular.ttf`; the manifest's weight-400 entry points at
     `Iosevka.ttf` directly. Tests pin both decisions.

THE IOSEVKA WIDTH GRADE — EXTENDED, DECLARED AS NORMAL
------------------------------------------------------------------------

Roboto Mono and Noto Sans Mono both have a 0.6 em character advance.
Iosevka's DEFAULT width grade is 0.5 em, which would break column
alignment wherever Armenian lands in a character grid. The package
therefore ships upstream's EXTENDED width grade (exactly 0.6 em), renamed
to plain `Iosevka`/`Iosevka-{Weight}` file names, and the manifest
declares those faces as the Normal stretch — they are the only width
grade in the package, so the Normal-stretch slot is theirs. The upstream
file-by-file rename mapping is recorded in THIRD-PARTY-NOTICES.txt.

The bundled Iosevka files are the UNHINTED upstream builds
(PkgTTF-Unhinted-Iosevka-34.4.0.zip). CodeBrix.Platform renders text
through Skia, which does not execute TrueType hinting instructions, so the
hinted builds (~10 MB per face instead of ~7 MB) would change nothing.

NOTO SANS GEORGIAN IS PROPORTIONAL
------------------------------------------------------------------------

There is no monospace font with Georgian letter coverage under a suitable
license (Iosevka's Georgian coverage is a single punctuation mark,
U+10FB). Georgian text therefore does not keep the character grid. If a
monospace Georgian face ever becomes available under OFL, swapping it in
here would be a straightforward companion replacement.


CODEBRIX-DEVELOP.JSON
========================================================================

`CODEBRIX-DEVELOP.json` sits at the repository root and is packed to the
root of the nupkg. It is the font's self-description for CodeBrix.Develop's
"New CodeBrix.Platform Application" experience: the IDE reads it to learn
how to wire this font into a generated application, instead of carrying
per-font swap logic of its own.

  schemaVersion     Always 1 today. A consumer that does not recognise
                    the value should decline the font with a clear
                    message rather than guess.
  packageId         Must equal this package's NuGet PackageId.
  displayName       The typographic family name shown to the user
                    ("Roboto Mono"), and the authoritative value written
                    into generated source.
  fontFamilyUri     The ms-appx URI of the primary font. No `#` fragment.
  resourceKey       The App.xaml resource key a generated application
                    uses (`RobotoMonoFont`).
  fallbackFontUris  Ordered ms-appx URIs of the companion fonts, consulted
                    for codepoints the primary font lacks:
                    NotoSansMono.ttf, then Iosevka.ttf, then
                    NotoSansGeorgian.ttf. The order is deliberate — the
                    metric-compatible monospace families come first.
  keyboardLayouts   The software-keyboard layout ids this package's glyph
                    coverage supports, as the UNION across the primary
                    font and its companions. Ids absent from this list are
                    not supported; there is deliberately no "unsupported"
                    list, so the complement of the platform's layout set
                    is always the correct answer.

The keyboardLayouts array claims the same 38 layouts as the sibling Roboto
and Merriweather packages, including `ka` and `hy`, which are delivered by
the companion fonts. Those require CodeBrix.Platform to consult
`fallbackFontUris` when the primary font lacks a glyph. The claim was
verified for THIS package's fonts by extracting every layout's characters
(Rows/ShiftRows/AltGrRows and DisplayName) from the platform's
KeyboardLayouts.*.cs sources and checking each against the union of the
four dash-free fonts' cmap tables. Regenerate that check whenever the
platform's layout set changes or this package's font set changes.


CORE API REFERENCE
========================================================================

This library has no public managed API. Consumers interact with it only
through:

  1. NuGet content paths
     (`ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/...`) used as
     `FontFamily` values in XAML or in code that constructs XAML element
     trees, or by setting the CodeBrix.Platform default font:

       global::CodeBrix.Platform.UI.FeatureConfiguration.Font.DefaultTextFontFamily =
           "ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/RobotoMono.ttf";

  2. The MSBuild `.targets` file under buildTransitive/net10.0/
     `CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets`, whose
     on-disk filename matches the NuGet PackageId so that NuGet's auto-
     import convention (NU5129) picks it up in consumer builds. It
     contains the target:

       <Target Name="CodeBrixRemoveUnusedRobotoMono"
               AfterTargets="_CodeBrixAddLibraryAssets">

     On platforms that do not support the font manifest, this target
     removes the static fonts (leaving only the four dash-free fonts).
     `RobotoMono.ttf` is never removed, so the direct
     `ms-appx:///.../RobotoMono.ttf` reference resolves on every platform.

If a future iteration of this library exposes a managed API (e.g. typed
accessors that return font streams or paths for non-CodeBrix.Platform
consumers), it will live under the `CodeBrix.Platform.Fonts.RobotoMono`
root namespace and be documented in this file.


ARCHITECTURE
========================================================================

Repository layout:

  CodeBrix.Platform.Fonts.RobotoMono/
    src/CodeBrix.Platform.Fonts.RobotoMono/
      CodeBrix.Platform.Fonts.RobotoMono.csproj
      InternalsVisibleTo.cs
      CodeBrix.Platform.Fonts.RobotoMono.uprimarker  (empty file)
      buildTransitive/
        net10.0/
          CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets
      Fonts/
        RobotoMono.ttf / .ttf.manifest
        RobotoMono-{Light|Regular|Medium|SemiBold|Bold}{Italic?}.ttf
        NotoSansMono.ttf / .ttf.manifest / NotoSansMono-{Weight}.ttf
        Iosevka.ttf / .ttf.manifest / Iosevka-{Weight}.ttf  (no -Regular)
        NotoSansGeorgian.ttf / .ttf.manifest / NotoSansGeorgian-{Weight}.ttf
    tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/
      CodeBrix.Platform.Fonts.RobotoMono.Tests.csproj
      AssemblyMetadataTests.cs
      ContentFilePresenceTests.cs
      ContentManifestTests.cs
      DescriptorTests.cs
      TargetsFileTests.cs
      TestAssetPaths.cs
    AGENT-README.txt
    CODEBRIX-DEVELOP.json
    LICENSE            (SIL OFL 1.1; combined copyright header for all
                        four upstream projects)
    OFL-Roboto.txt     (SIL OFL 1.1; Roboto Mono copyright header)
    OFL-Noto.txt       (SIL OFL 1.1; Noto LGC + Georgian copyright header)
    OFL-Iosevka.txt    (SIL OFL 1.1; Iosevka copyright header)
    README.md
    THIRD-PARTY-NOTICES.txt

Inside the produced NuGet (.nupkg), the file layout is:
  buildTransitive/net10.0/CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono.dll
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono.uprimarker
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/*.ttf
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/*.ttf.manifest
  AGENT-README.txt
  CODEBRIX-DEVELOP.json
  README.md
  OFL-Roboto.txt
  OFL-Noto.txt
  OFL-Iosevka.txt
  THIRD-PARTY-NOTICES.txt
  icon-codebrix-128.png

The `lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/` content layout
is load-bearing: the `ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/...`
URIs that consumers reference resolve relative to the assembly name, so if
the assembly is renamed the content folder must be renamed in lockstep.


CODING CONVENTIONS (CodeBrix family)
========================================================================

This repository follows every CodeBrix family convention. Most are
inherited from the standard library scaffold; key points:

  * Target framework: net10.0 only. No multi-targeting.
  * Nullable reference types (NRT): OFF (do not set <Nullable>enable</Nullable>).
    No `?` annotations on reference types; no `!` null-forgiveness operator.
    Value-type nullables (`int?`, `DateOnly?`, etc.) are fine.
  * No global usings.
  * `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is on.
    Every public/protected member of a public type needs an XML doc
    comment. CS1591 is fixed at source, never suppressed. (In this
    library's first iteration there are no public types, so CS1591
    is trivially clean.)
  * Tests use xUnit v3 + SilverAssertions;
    `TestContext.Current.CancellationToken` is threaded through any
    cancellable call inside a test.
  * No project-level warning suppression (`<NoWarn>`, `<WarningLevel>0</>`,
    `<TreatWarningsAsErrors>false</>`, etc. are all forbidden).
  * The whole package — wrapper code and bundled fonts alike — is licensed
    under SIL OFL 1.1; the csproj `<PackageLicenseExpression>` is `OFL-1.1`.
    The `<Copyright>` line preserves the upstream font attributions:
      Copyright (c) 2026 Jeremy Ellis and contributors. Roboto Mono font
      (c) 2015 The Roboto Mono Project Authors; Noto Sans Mono and Noto
      Sans Georgian fonts (c) 2022 The Noto Project Authors; Iosevka font
      (c) 2015-2026 Renzhi Li; all distributed under SIL OFL 1.1.

For the full list of family conventions see CODEBRIX_LIBRARY_OBSERVATIONS.txt
in the CodeBrix.Library.Dev-private repo.


TESTING
========================================================================

Tests live under tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/. Run with:

  dotnet test CodeBrix.Platform.Fonts.RobotoMono.slnx

The test suite covers:

  * Manifest JSON: that all four `.ttf.manifest` files deserialize
    cleanly, carry the expected entry counts (10/6/6/6), cover the
    expected weights (300-700 for the primary — Roboto Mono has no
    ExtraBold — and 300-800 for the companions), are Normal-stretch only,
    and that every entry's family_name path is rooted at
    `ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/` and points at a
    file that exists on disk. Also that all three companion manifests are
    upright-only, and that the Iosevka weight-400 entry points at the
    dash-free Iosevka.ttf, so those limitations/decisions stay decisions
    rather than accidents.
  * Descriptor: that CODEBRIX-DEVELOP.json declares schemaVersion 1, its
    packageId matches the published PackageId, its fontFamilyUri and every
    fallbackFontUri carry no `#` fragment and point at fonts this package
    actually ships, and that keyboardLayouts has no duplicates and claims
    the scripts the companions exist to supply.
  * Content-file presence: that all 31 `.ttf` files exist on disk next to
    the test assembly's expected build-output font folder (resolved via
    `AppContext.BaseDirectory` + `TestAssets/Fonts/`, centralized in
    `TestAssetPaths`), that no `Iosevka-Regular.ttf` duplicate ships, and
    that no upstream "VariableFont"/"Extended" name token survived the
    renames.
  * Assembly metadata: that the produced library assembly is named
    `CodeBrix.Platform.Fonts.RobotoMono` and exports no public types, and
    that its `.uprimarker` sibling file exists.
  * .targets file: that the buildTransitive .targets file is present next
    to the test assembly, that it declares the
    `CodeBrixRemoveUnusedRobotoMono` MSBuild target, that it hooks
    `AfterTargets="_CodeBrixAddLibraryAssets"`, and that it never removes
    a dash-free font.


PROVENANCE
========================================================================

This package is not a port of any upstream packaging project. The
`.csproj`, `.targets`, `.ttf.manifest`, `.uprimarker`, and documentation
are original CodeBrix-family files, authored by mirroring the sibling
CodeBrix.Platform.Fonts.Roboto and CodeBrix.Platform.Fonts.Merriweather
packages. The only third-party material is the Roboto Mono, Noto Sans
Mono, Iosevka and Noto Sans Georgian `.ttf` font binaries, which are
redistributed bit-for-bit unmodified. Their per-file provenance (including
the Iosevka Extended-grade rename mapping and release-archive URL) and the
SIL OFL 1.1 terms are recorded in THIRD-PARTY-NOTICES.txt (binary `.ttf`
files cannot carry an inline provenance comment).

Font sources:
  - Roboto Mono:        Google Fonts download (variable + statics).
  - Noto Sans Mono:     Google Fonts download (variable + statics).
  - Iosevka:            GitHub release v34.4.0, unhinted TTF package.
  - Noto Sans Georgian: copied bit-for-bit from the sibling
                        CodeBrix.Platform.Fonts.Roboto repository.

The `keyboardLayouts` array in CODEBRIX-DEVELOP.json is GENERATED, not
hand-written: it is computed by extracting each software-keyboard layout's
required character set (from the KeyboardLayouts.*.cs definitions in
CodeBrix.Platform) and checking it against the `cmap` of the fonts this
package ships, taking the union across the primary font and its
companions. Nothing in this repository's build reads CodeBrix.Platform —
the array is computed by a developer-run check and checked in as data.
Regenerate it whenever the platform's layout set changes or this package's
font set changes.


KNOWN GOTCHAS
========================================================================

  * `ms-appx:///` URIs are resolved by the CodeBrix.Platform runtime, not
    by .NET itself. Outside a CodeBrix.Platform host, those URIs won't
    resolve. Plain .NET 10 console / test apps that reference this package
    can still access the .ttf files via the package's on-disk location
    (`<nuget-cache>/codebrix.platform.fonts.robotomono.ofllicenseforever/<version>/lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/...`),
    but they have to do that lookup themselves.

  * The .targets file hooks `AfterTargets="_CodeBrixAddLibraryAssets"` —
    the asset target defined by the CodeBrix.Platform UI build tasks. If
    that internal MSBuild target name ever changes again, this .targets
    file must be updated in lockstep — otherwise the conditional pruning
    of static fonts will silently stop firing.

  * The four dash-free fonts are deliberately never pruned. For
    RobotoMono.ttf that is the usual reason (the CodeBrix.Platform
    default-font configuration and typical consumer XAML reference it by
    its direct `ms-appx:///.../RobotoMono.ttf` path). For the three
    companions it matters MORE: they are the only source of polytonic
    Greek, Armenian and Georgian in this package, so pruning them would
    silently drop coverage. The prune matches only dash-bearing filenames,
    which is why the companion fonts are named without a dash.

  * Iosevka.ttf is NOT a variable font (see THE IOSEVKA QUIRK above). Do
    not assume weight-axis behavior from it, and do not add an
    `Iosevka-Regular.ttf` — the manifest's weight-400 entry points at
    Iosevka.ttf deliberately.

  * The bundled Iosevka faces are upstream's Extended width grade under
    plain Iosevka file names (see THE IOSEVKA WIDTH GRADE above). If you
    ever refresh Iosevka from a new upstream release, take the
    `Iosevka-Extended*` files from the UNHINTED package, not the default-
    width or hinted ones, and update the version recorded in
    THIRD-PARTY-NOTICES.txt.

  * Roboto Mono has no ExtraBold/Black. Requests for weights above 700
    resolve to Bold (or the variable font's 700 cap). Do not "complete"
    the primary manifest to six weights with a fake 800 entry.

  * NEVER add a `#FamilyName` fragment to a font URI in this package's
    documentation or descriptor. CodeBrix.Platform strips it during font
    resolution, and on `DefaultTextFontFamily` it silently disables the
    startup manifest preload (the appended ".manifest" lands inside the
    fragment and is dropped by `Uri.PathAndQuery`).

  * None of the four families' copyright statements declares a Reserved
    Font Name, so SIL OFL 1.1 condition 3 does not restrict any name used
    here. The `.ttf` binaries are nonetheless redistributed unmodified; do
    not alter the font bytes. File renames are fine (and recorded in
    THIRD-PARTY-NOTICES.txt); byte edits are not.
