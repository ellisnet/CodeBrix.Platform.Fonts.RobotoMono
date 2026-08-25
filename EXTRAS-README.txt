================================================================================
EXTRAS-README: CodeBrix.Platform.Fonts.RobotoMono
Samples, tools and other content in this repository that is not part of a NuGet
package
================================================================================

This repository contains no samples, demo applications, tools or scripts. It
holds two projects: the asset library that becomes the
CodeBrix.Platform.Fonts.RobotoMono.OflLicenseForever package, and its test
project.


TEST PROJECT (the only non-package content)
===========================================

  tests/CodeBrix.Platform.Fonts.RobotoMono.Tests/

The unit-test project for the package's assets. It is not packed and not
published. Run it from the repository root with:

  dotnet test CodeBrix.Platform.Fonts.RobotoMono.slnx

It needs no preparation, no environment variables and no external services: it
copies the fonts, manifests, `.uprimarker`, buildTransitive `.targets` and
CODEBRIX-DEVELOP.json next to the test assembly and inspects them as files.
See MAINTAINER-README.txt for what the suite pins.


LICENSE AND NOTICE FILES (packaged, not samples)
================================================

For completeness, the repository root also carries content that ships inside
the nupkg rather than being repository extras: LICENSE, OFL-Roboto.txt,
OFL-Noto.txt, OFL-Iosevka.txt (the SIL OFL 1.1 text once per upstream font
project, differing only in copyright header), THIRD-PARTY-NOTICES.txt (per-file
font provenance, including the Iosevka Extended-grade rename mapping) and
CODEBRIX-DEVELOP.json (the font's self-description, read by CodeBrix.Develop's
New Application experience). None of these is a sample or a tool; do not treat
them as optional repository content.
