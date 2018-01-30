# Sins Data Converter
* **2.1.x** — 2012 release written in really messy VB.NET
* **3.0.0-beta** — 2018 WIP written in object-oriented C#

## Description
Sins Data Converter is a modding tool for the classic PC strategy game Sins of Solar Empire. It provides an easy-to-use GUI for the ConvertData command line utility provided by game developer Ironclad Games. Use of ConvertData is required to translate game files from the machine-readable binary format into an human-readable raw text format for modding. Sins Data Converter boasts a full set of impressive features that take advantage of ConvertData's flexibility, none of which require any knowledge of the command line.

## Features
||2.1.x|3.x|
|-------|:-----:|:------:|
|Basic Mode for conversion of individual files and folders|•|•|
|Support for in-place conversions|•|•|
|Support for external output locations|•|•|
|Quickly switch between BIN-to-TXT and TXT-to-BIN modes|•|•|
|Automatic ConvertData support for any non-Steam installed version of Sins|•|•|
|Automatic ConvertData support for Rebellion installed through Steam||•|
|Set up a new job while the last one is still in progress||•|
|Track the start and end times of jobs to measure performance||•|
|Drag-and-drop file support|•||
|Ability to select custom ConvertData executable|•||
|Include/exclude specific subfolders at will|•||
|Advanced Mode for conversion of multiple files and folders|•||
|Set modes, outputs, and ConvertData's on a per-file basis|•||
|Manage files and folders with the Conversion Queue|•||
|Create ReferenceData from any installed version of Sins|•||
|Change the location of the Sins mod folder|•||
|Easily create entity.manifest files for your mod|•||

## System Requirements
- Microsoft Windows
	- SDC 2.1.x requires Windows XP or higher
	- SDC 3.x requires Windows 7 or higher
- .NET Framework
	- SDC 2.1.x requires .NET 3.5
	- SDC 3.x requires .NET 4.7.1
- Sins of a Solar Empire OR a `ConvertData*.exe`

## Background
Sins Data Converter is a modding tool for the classic PC strategy game Sins of Solar Empire. It provides an easy-to-use, comprehensive GUI for the ConvertData command line utility provided by game developer Ironclad Games. Use of ConvertData is required to translate game files from the machine-readable binary format into an human-readable raw text format for modding. Sins Data Converter boasts a full set of impressive features that take advantage of ConvertData's flexibility, none of which require any knowledge of the command line.

This utility was abandoned in 2013 amid an ambitious code rewrite project that seemed to attract very little interest from the community. It was resurrected in 2017 as an open-source project so that it would have a better chance at usefulness, improvement, and continued maintenance moving forward. The initial GitHub version of the project threw out the abandoned rewrite and returned to the last released version, the v2.1 Beta that added basic support for the Rebellion expansion. That means, among other things, that the code is absolutely awful. But it seems to still work despite changes to SoaSE and Windows itself over the years. This version remains available on the v2.1 branch and in the Releases section.

The code was so embarrassing that it motivated me to try again to rewrite the entire project in C#. This time, I had another five years of programming under my belt, including object-oriented practices and source control. The 3.0.0-beta releases don't have some of the original app's bells and whistles yet, but it works great for conversion, and the all-new code base is infinitely more maintainable and extendable.
