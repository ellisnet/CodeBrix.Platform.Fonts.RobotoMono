================================================================================
MAINTAINER-README: CodeBrix.Platform.Fonts.RobotoMono
Notes for people and agents MAINTAINING this repository — not for package
consumers
================================================================================

Consumers of the NuGet package should read AGENT-README.txt instead; everything
below is about changing this repository.


PURPOSE AND SCOPE
=================

The repository produces exactly one NuGet package:

  CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever
      Built from
      src/CodeBrix.Platform.Fonts.RobotoMono/CodeBrix.Platform.Fonts.RobotoMono.csproj.
      Consumer documentation: AGENT-README.txt (repository root), which is also
      packed into the nupkg root.

It is a font asset package: a metadata-only assembly whose job is to carry font
content files, one JSON manifest per family, a CodeBrix.Develop descriptor, a
`.uprimarker` marker file and a buildTransitive `.targets` file. There is no
managed code to maintain — the library project contains InternalsVisibleTo.cs
and nothing else — so maintenance here means font sets, manifests, the
descriptor, the targets file and the tests that pin them.

The structural shape (a primary family plus three companions that supply the
scripts the primary lacks) mirrors the sibling CodeBrix.Platform.Fonts.Roboto
and CodeBrix.Platform.Fonts.Merriweather packages. Two structural quirks are
unique to this one and must not be "fixed": Iosevka has no variable font, and
the primary family has no ExtraBold.


REPOSITORY LAYOUT
=================

  CodeBrix.Platform.Fonts.RobotoMono/
    CodeBrix.Platform.Fonts.RobotoMono.slnx
    src/CodeBrix.Platform.Fonts.RobotoMono/
      CodeBrix.Platform.Fonts.RobotoMono.csproj
      InternalsVisibleTo.cs
      CodeBrix.Platform.Fonts.RobotoMono.uprimarker      (empty marker file)
      buildTransitive/net10.0/
        CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets
      Fonts/
        RobotoMono.ttf / RobotoMono.ttf.manifest
        RobotoMono-{Light|Regular|Medium|SemiBold|Bold}{Italic?}.ttf
        NotoSansMono.ttf / .ttf.manifest / NotoSansMono-{Weight}.ttf
        Iosevka.ttf / .ttf.manifest / Iosevka-{Weight}.ttf   (no -Regular)
        NotoSansGeorgian.ttf / .ttf.manifest /
          NotoSansGeorgian-{Weight}.ttf
    tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/
      CodeBrix.Platform.Fonts.RobotoMono.Tests.csproj
      AssemblyMetadataTests.cs
      ContentFilePresenceTests.cs
      ContentManifestTests.cs
      DescriptorTests.cs
      TargetsFileTests.cs
      TestAssetPaths.cs
    AGENT-README.txt          consumer documentation (packed)
    MAINTAINER-README.txt     this file (not packed)
    EXTRAS-README.txt         non-package content (not packed)
    README-INDEX.txt          map of the README files (not packed)
    CODEBRIX-DEVELOP.json     font self-description (packed to nupkg root)
    README.md                 human-facing overview (packed)
    LICENSE                   SIL OFL 1.1, combined copyright header for all
                              four upstream font projects
    OFL-Roboto.txt            SIL OFL 1.1, Roboto Mono copyright header (packed)
    OFL-Noto.txt              SIL OFL 1.1, Noto LGC + Georgian header (packed)
    OFL-Iosevka.txt           SIL OFL 1.1, Iosevka copyright header (packed)
    THIRD-PARTY-NOTICES.txt   per-file font provenance incl. the Iosevka rename
                              mapping and release-archive URL (packed)
    icon-codebrix-128.png     package icon (packed)
    global.json               selects the Microsoft.Testing.Platform test
                              runner; pins no SDK version (not packed)
    .gitignore                (not packed)

Produced nupkg layout:

  buildTransitive/net10.0/
      CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono.dll
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono.uprimarker
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/*.ttf
  lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/*.ttf.manifest
  AGENT-README.txt, CODEBRIX-DEVELOP.json, README.md, OFL-Roboto.txt,
  OFL-Noto.txt, OFL-Iosevka.txt, THIRD-PARTY-NOTICES.txt, icon-codebrix-128.png

The `lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/` content path is
load-bearing: the `ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/...` URIs
consumers reference resolve relative to the ASSEMBLY name. If the assembly is
ever renamed, the content folder, the manifests' family_name values, the
descriptor's URIs, the targets file's prune path and every example in
AGENT-README.txt must be renamed in lockstep.


BUILDING
========

  dotnet restore CodeBrix.Platform.Fonts.RobotoMono.slnx
  dotnet build   CodeBrix.Platform.Fonts.RobotoMono.slnx

Zero warnings, zero errors. Because <GeneratePackageOnBuild> is true, a build
also produces a .nupkg (see PACKAGING AND PUBLISHING).


TESTING
=======

  dotnet test CodeBrix.Platform.Fonts.RobotoMono.slnx

THE TEST RUNNER IS Microsoft.Testing.Platform, selected by global.json at the
repository root. That file does NOT pin an SDK version, so the newest installed
.NET 10 SDK is still used; it exists solely to select the runner:

    { "test": { "runner": "Microsoft.Testing.Platform" } }

Because the setting lives in global.json rather than in the test csproj, it
applies to every `dotnet test` run anywhere in the repository. Keep the file
committed -- without it `dotnet test` silently falls back to the older VSTest
bridge. global.json and .gitignore are also carried in the .slnx Solution Items
folder, alongside the four readmes, CODEBRIX-DEVELOP.json, the icon, LICENSE,
the three OFL files and THIRD-PARTY-NOTICES.txt.

The test project's package references are Microsoft.NET.Test.Sdk, xunit.v3,
xunit.runner.visualstudio and SilverAssertions; there is no coverage collector.

xUnit v3 + SilverAssertions. No opt-in environment variables and no special
preparation. The tests do not resolve `ms-appx` URIs or render anything; they
inspect the asset files that the test project copies next to the test assembly
via <None ... CopyToOutputDirectory="PreserveNewest" Link="TestAssets\..."> —
Fonts, the `.uprimarker`, the buildTransitive `.targets` and
CODEBRIX-DEVELOP.json. All those paths are centralized in TestAssetPaths.cs
(rooted at AppContext.BaseDirectory + "TestAssets"), including the
CompanionFamilies array; add a family there rather than hard-coding paths in a
test.

What the suite pins:

  * Manifest JSON (ContentManifestTests) — all four `.ttf.manifest` files
    deserialize cleanly, carry the expected entry counts (10/6/6/6), cover the
    expected weights (300-700 for the primary, because Roboto Mono has no
    ExtraBold, and 300-800 for the companions), are Normal-stretch only, and
    every entry's family_name is rooted at
    ms-appx:///CodeBrix.Platform.Fonts.RobotoMono/Fonts/ and points at a file
    that exists on disk. Also that all three companion manifests are
    upright-only and that the Iosevka weight-400 entry points at the dash-free
    Iosevka.ttf — so those limitations stay decisions rather than accidents.
  * Descriptor (DescriptorTests) — CODEBRIX-DEVELOP.json declares
    schemaVersion 1, its packageId matches the published PackageId, its
    displayName is the typographic family name, its resourceKey follows the
    family convention, its fontFamilyUri and every fallbackFontUri carry no `#`
    fragment and point at fonts this package ships, the fallbacks are exactly
    the three companion families, and keyboardLayouts has no duplicates and
    claims the scripts the companions exist to supply.
  * Content-file presence (ContentFilePresenceTests) — all 31 `.ttf` files
    exist, the 10 static Roboto Mono faces are present, each companion's
    dash-free file is present, no `Iosevka-Regular.ttf` duplicate ships, and no
    upstream "VariableFont"/"Extended" name token survived the renames.
  * Assembly metadata (AssemblyMetadataTests) — the produced assembly is named
    CodeBrix.Platform.Fonts.RobotoMono, targets net10, exports no public types,
    and its `.uprimarker` sibling exists.
  * Targets file (TargetsFileTests) — the buildTransitive `.targets` is present,
    declares the CodeBrixRemoveUnusedRobotoMono target, hooks
    AfterTargets="_CodeBrixAddLibraryAssets", carries the SupportsFontManifest
    condition, uses net10 lib paths, contains no foreign family token, and never
    removes a dash-free font.


PACKAGING AND PUBLISHING
========================

  * <GeneratePackageOnBuild>true</GeneratePackageOnBuild> — every build of the
    library project produces a .nupkg. <IncludeContentInPack>true</...> is set.
  * Versioning is the CodeBrix date-stamped scheme computed in the csproj from
    System.DateTime.UtcNow: 1.<years since _VersionBaseYear>.<day of year>.
    <minute of day>, strictly increasing, NOT SemVer. Two builds in the same UTC
    minute produce the same version — never publish two packages from inside one
    minute. Re-baseline by changing _VersionBaseYear.
  * The buildTransitive `.targets` file name MUST equal the PackageId
    (CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever.targets). That is
    NuGet's auto-import convention; a mismatch means the file ships but is never
    imported, and NuGet warns (NU5129). It is packed with
    <None Include="buildTransitive\**\*" PackagePath="buildTransitive/" />.
  * The fonts and manifests are packed to
    lib/net10.0/CodeBrix.Platform.Fonts.RobotoMono/Fonts/, and the `.uprimarker`
    to lib/net10.0/, by explicit <None ... PackagePath> items.
  * Root-packed documents: icon-codebrix-128.png, README.md, AGENT-README.txt,
    CODEBRIX-DEVELOP.json, THIRD-PARTY-NOTICES.txt, OFL-Roboto.txt,
    OFL-Noto.txt, OFL-Iosevka.txt. MAINTAINER-README.txt, EXTRAS-README.txt and
    README-INDEX.txt are NOT packed — they describe the repository, not the
    package.
  * <PackageLicenseExpression>OFL-1.1</PackageLicenseExpression> and
    <PackageRequireLicenseAcceptance>true</...>. The <Copyright> line preserves
    every upstream font attribution:
      Copyright (c) 2026 Jeremy Ellis and contributors. Roboto Mono font (c)
      2015 The Roboto Mono Project Authors; Noto Sans Mono and Noto Sans
      Georgian fonts (c) 2022 The Noto Project Authors; Iosevka font (c)
      2015-2026 Renzhi Li; all distributed under SIL OFL 1.1.
    Keep it; SIL OFL 1.1 requires the notices to travel with the fonts.
  * Git tags are expected to match the published NuGet version.


PROVENANCE AND VENDORED SOURCES
===============================

This package is not a port of any upstream packaging project. The `.csproj`,
`.targets`, `.ttf.manifest` files, `.uprimarker` and documentation are original
CodeBrix-family files, authored by mirroring the sibling
CodeBrix.Platform.Fonts.Roboto and CodeBrix.Platform.Fonts.Merriweather
packages. The only third-party material is the font binaries, redistributed
bit-for-bit unmodified. Per-file provenance — including the Iosevka
Extended-grade rename mapping and the release-archive URL — and the SIL OFL 1.1
terms live in THIRD-PARTY-NOTICES.txt, because binary `.ttf` files cannot carry
an inline provenance comment.

Font sources:

  Roboto Mono         Google Fonts download (variable font + statics).
                      RobotoMono.ttf is upstream's
                      RobotoMono-VariableFont_wght.ttf, renamed byte-for-byte.
                      Upstream's italic variable font
                      (RobotoMono-Italic-VariableFont_wght.ttf) is deliberately
                      NOT bundled — one variable font per family, as in the
                      sibling packages; italics come from the statics. The Thin
                      (100) and ExtraLight (200) statics are likewise skipped.
  Noto Sans Mono      Google Fonts download (variable font + statics). The
                      dash-free file is the wdth+wght variable font, renamed.
  Iosevka             GitHub release v34.4.0, UNHINTED TTF package
                      (PkgTTF-Unhinted-Iosevka-34.4.0.zip). The bundled faces
                      are the EXTENDED width grade, renamed to plain
                      Iosevka/Iosevka-{Weight} names.
  Noto Sans Georgian  copied bit-for-bit from the sibling
                      CodeBrix.Platform.Fonts.Roboto repository.

Refreshing Iosevka: take the `Iosevka-Extended*` files from the UNHINTED package
of the new release, never the default-width or hinted ones, and update the
version recorded in THIRD-PARTY-NOTICES.txt. Rationale for both choices:
Iosevka's default width grade is 0.5 em where Roboto Mono and Noto Sans Mono are
0.6 em, so the Extended grade is what keeps Armenian in the character grid; and
CodeBrix.Platform renders through Skia, which does not execute TrueType hinting
instructions, so hinted builds (~10 MB per face instead of ~7 MB) would change
nothing on screen.

The `keyboardLayouts` array in CODEBRIX-DEVELOP.json is GENERATED, not
hand-written. It is computed by extracting every software-keyboard layout's
required characters (Rows / ShiftRows / AltGrRows and DisplayName from the
KeyboardLayouts.*.cs definitions in CodeBrix.Platform) and checking each against
the union of the `cmap` tables of this package's four dash-free fonts. Nothing
in this repository's build reads CodeBrix.Platform — the array is checked in as
data produced by a developer-run check. Regenerate it whenever the platform's
layout set changes or this package's font set changes. The array currently
claims the same 38 layouts as the sibling Roboto and Merriweather packages,
including `ka` and `hy`, which are delivered by the companion fonts and
therefore depend on a consuming application wiring up the fallback fonts.


CODING CONVENTIONS
==================

Standard CodeBrix family conventions apply; the ones that matter here:

  * net10.0 only. No multi-targeting.
  * Nullable reference types OFF (no <Nullable>enable</Nullable>); no `?` on
    reference types, no `!` null-forgiveness. Value-type nullables are fine.
  * No `global using` directives.
  * <GenerateDocumentationFile> is on, so every public/protected member of a
    public type needs an XML doc comment and CS1591 is fixed at source, never
    suppressed. There are no public types here, so it is trivially clean.
  * No project-level warning suppression (<NoWarn>, <WarningLevel>0</...>,
    <TreatWarningsAsErrors>false</...> are all forbidden).
  * Tests use xUnit v3 + SilverAssertions;
    TestContext.Current.CancellationToken is threaded through any cancellable
    call inside a test (xUnit1051).
  * The library project root carries InternalsVisibleTo.cs granting internals
    access to CodeBrix.Platform.Fonts.RobotoMono.Tests.
  * The whole package — wrapper and fonts alike — is SIL OFL 1.1.

For the full family convention list see CODEBRIX_LIBRARY_OBSERVATIONS.txt in the
CodeBrix.Library.Dev-private repository.


NOTES
=====

  * The `.targets` file hooks AfterTargets="_CodeBrixAddLibraryAssets", the
    asset target defined by the CodeBrix.Platform UI build tasks. If that
    internal target name changes again, this file must be updated in lockstep,
    or the conditional prune silently stops firing. TargetsFileTests pins the
    current name.
  * The prune matches dash-bearing file names
    (`...\Fonts\**-**.ttf`), which is exactly why the four fallback-capable
    fonts are named without a dash. Never introduce a dash into a dash-free
    font's name, and never add a dash-free static that is not meant to survive
    pruning.
  * Do not ship a fabricated Iosevka "variable font", and do not add an
    `Iosevka-Regular.ttf`: the manifest's weight-400 entry points at
    Iosevka.ttf on purpose, so the ~7 MB Regular face is not shipped twice.
    Tests pin both decisions.
  * Do not "complete" the primary manifest to six weights with a fake 800 entry.
    Roboto Mono's weight axis genuinely stops at 700.
  * Never add a `#FamilyName` fragment to a font URI anywhere in this repository
    — manifests, descriptor or documentation. CodeBrix.Platform strips it during
    resolution, and on DefaultTextFontFamily it silently disables the startup
    manifest preload (the appended ".manifest" lands inside the fragment and is
    dropped by Uri.PathAndQuery). DescriptorTests pins the no-fragment rule.
  * If a monospace Georgian face ever becomes available under a suitable
    license, swapping it in is a straightforward companion replacement (files,
    manifest, descriptor URIs, tests). Iosevka's Georgian coverage is a single
    punctuation mark (U+10FB), so it cannot serve that role.
  * The `.ttf` bytes must stay byte-identical to upstream. File renames are fine
    and are recorded in THIRD-PARTY-NOTICES.txt; byte edits are not.
