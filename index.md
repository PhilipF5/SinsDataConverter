---
title: Sins Data Converter
---
<img src="https://img.shields.io/github/release/philipf5/SinsDataConverter/all.svg">
<img src="https://img.shields.io/github/downloads/philipf5/SinsDataConverter/total.svg">
<img src="https://img.shields.io/github/license/philipf5/SinsDataConverter.svg">
<br>
<img src="https://img.shields.io/appveyor/ci/philipf5/SinsDataConverter/develop.svg?logo=appveyor">
<img src="https://img.shields.io/github/last-commit/philipf5/SinsDataConverter/develop.svg">
<img src="https://img.shields.io/github/issues-raw/philipf5/SinsDataConverter.svg">

# Jumpstart
**January 2018**

Well, it happened. I finally found the motivation to sit down and start a complete rewrite of the Sins Data Converter code base. This marks the beginning of beta development for **Sins Data Converter 3.0.0**, which will hopefully set the foundation for a whole new phase in the app's life.

## What Users Need to Know
`3.0.0-beta.1` is not yet feature-complete. The `README` on GitHub has the feature comparison between the classic `2.1.x` release and the new beta. The plan is to eventually have all the original features in the new project. For now, `3.x` supports converting files, converting folders, converting in-place, and converting by copying.

`3.x` supports all editions of *Sins of a Solar Empire*. It auto-detects what editions you have installed. It doesn't yet allow you to specify your own `ConvertData.exe`. It does, however, **work with the Steam edition**, relieving a major pain point of the original app over the past couple years.

If you routinely use the more advanced features of the `2.1.x` app, such as Advanced Mode, ReferenceData creation, or custom `ConvertData.exe`s, you may want to hold off on upgrading until the `3.x` branch gains those features. But if you mostly just use the Basic Mode conversion with your installed copy of *Sins*, either version should work just fine for you.

A final compatibility note: the `3.x` branch requires a newer version of .NET Framework that is not available for Windows XP/Vista. If that's your OS of choice, you're stuck with `2.1.x`. I imagine you're stuck with a lot of other things too, because those platforms are horribly outdated.

## What Developers Need to Know
I still haven't played *Sins* in years. The primary motivator for me to rewrite this code base was that this is a public project on my GitHub, and the original code was just really bad. You have my sympathies if you tried to wrangle it over the past ~10 months that it's been available. The new code base should be much easier to maintain and contribute to.

Sins Data Converter is a Windows Presentation Foundation (WPF) desktop app. The GUI for such apps is written in XAML; it's been tweaked for the new release, but is mostly faithful to the original GUI that users really liked. All the code behind the GUI, however, has been thrown out.

The new code base is written in C# instead of Visual Basic; that alone should be a big plus for prospective contributors. The new architecture is object-oriented, designed around classes, methods, and properties, with an emphasis on being DRY. To establish orthogonality, the code has been split into two projects. `SinsDataConverter.Core` is a .NET Standard library that contains all the functionality of the program. `SinsDataConverter.UI` is a WPF app which uses the `Core` library's API. The `Core` library is not dependent on the `UI` library.

---

# The Future
**March 2017**

In March 2010, I released Sins Data Converter to the public for the first time. I was an avid fan of Sins of a Solar Empire, and I enjoyed playing with various mods for the game. I simply used my programming skills (which were rather pathetic at the time) to create a tool to help make these activities a little easier for me, and I decided to share it with the community just for the heck of it.

The release was very successful, and the tool developed more of a following than I expected. I continued to maintain the project for some time, as Ironclad Games continued to release breaking changes and expansions. By September 2012, the feature set of the tool had expanded substantially. In March 2013, I announced a full code rewrite of Sins Data Converter to bring it up to speed with my greatly expanded knowledge of object-oriented programming.

**That update was never released.**

By the middle of 2013, I had lost most of my interest in Sins of a Solar Empire. I was busy in college, and the modding community for the game seemed to have hit a lull. When I realized that I was no longer getting feedback or downloads for Sins Data Converter, I decided the world had moved on, and I never finished the code rewrite.

Nothing changed for three years. But in the summer of 2016, I got a couple of emails from people who had obviously found the contact form on the old project website. A little later on, I went on the Sins of a Solar Empire forums for the first time in years, and discovered that a few people had expressed interest in my utility now and then. And something I had written on the forums back in 2010 caught my eye:

*“This program is NOT being released and then dropped.”*

And yet, I left the software in a state where the only version that worked with Rebellion—the only version of the game readily available to new players today—was a beta that didn’t even have an official download link anymore. While the software still works fine when I load it up, some others have complained about problems with the last version. I decided that it wasn’t fair for the software to just disappear the way it did, but I’m also not in a position to resume working on it daily the way I did as a teenager, or test it in all the different configurations that are out there now. So I’ve decided to do what, back then, I always told myself I’d do if I ever got tired of developing a piece of software that people actually used.

## Sins Data Converter is back, and it’s going open source.

From now on, the full source code for Sins Data Converter will be right here on GitHub for anybody who wants to play with it. I’m not gonna lie, I was 17 when I first developed this tool, and the code is **an absolute embarrassment**. It’s disorganized, not at all object-oriented, and has tons of inefficient redundancies and poorly engineered solutions. It’s not the abandoned code rewrite, since I have no idea how far I got with that. It’s the v2.1 Beta that came out in September 2012, written in Visual Basic and XAML. But it is under the Mozilla Public License now, warts and all; so, if you want to want to make any improvements or changes, whether to share with everyone or just for your personal needs, have at it.

And I won’t necessarily vanish back into the void, either. I’ll be keeping an eye on things, looking at pull requests and issues, and contributing here and there when I have time and/or interest. If this takes off, who knows, I might someday give that full code rewrite a second try from scratch and show you all how much better I’ve gotten at programming in the last seven years.

But in the meantime, if you want to contribute, or just get the last official release of the Rebellion-compatible tool for your modding adventures, head on up to the “View on GitHub” link above. If you have any problems, create a new Issue with as many details as possible so that I or someone else can evaluate it. (And if you are just coming to download the executable, make sure you check the Issues list to know what you’re getting into.)

Thanks to those who have continued to show interest in this project during the long hiatus! I hope that this new approach will provide a way for Sins Data Converter to continue to benefit however much of the community is still around to use it.
