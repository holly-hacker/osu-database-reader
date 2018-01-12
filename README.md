[![CodeFactor](https://www.codefactor.io/repository/github/holly-hacker/osu-database-reader/badge)](https://www.codefactor.io/repository/github/holly-hacker/osu-database-reader)
[![NuGet](https://img.shields.io/nuget/v/HoLLy.osu.DatabaseReader.svg?style=flat-square)](https://www.nuget.org/packages/HoLLy.osu.DatabaseReader)
# osu-database-reader
Allows for parsing/reading osu!'s database files

Find it on NuGet: [link](https://www.nuget.org/packages/HoLLy.osu.DatabaseReader)

NOTE: No parsing for storyboards, use [osuElements](https://github.com/JasperDeSutter/osuElements) for that.

### Currently finished:
* reading osu!.db
* reading collection.db
* reading scores.db
* reading presence.db
* reading replays
* reading osu! beatmaps

### Planned:
* writing osu!.db
* writing collection.db
* writing scores.db
* writing presence.db
* writing osu! beatmaps (maybe?)

## Usage
Add to your project and include the osu_database_reader namespace.
See unit tests for more detailed usage.

## Installation
I recommend installing it through NuGet. You can use the built-in package manager in Visual Studio 2017 or use the package manager console:

> Install-Package HoLLy.osu.DatabaseReader

Or, if you use .NET Core (2.0+):

> dotnet add package HoLLy.osu.DatabaseReader

## Credits
This project includes the 7zip LZMA SDK, which is released in the public domain.
