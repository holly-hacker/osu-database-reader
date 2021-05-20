[![CodeFactor](https://www.codefactor.io/repository/github/holly-hacker/osu-database-reader/badge)](https://www.codefactor.io/repository/github/holly-hacker/osu-database-reader)
[![NuGet](https://img.shields.io/nuget/v/HoLLy.osu.DatabaseReader.svg?style=flat-square)](https://www.nuget.org/packages/HoLLy.osu.DatabaseReader)
# osu-database-reader
Allows for parsing/reading osu!'s database files

## Features
- Read/Write .db files
- Read/Write replay files (excluding score checksum calculation)
- Read beatmap files

For parsing storyboards, use [osuElements](https://github.com/JasperDeSutter/osuElements) or wait for a future release.

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
[Tomlyn](https://github.com/dezhidki/Tommy) is used during code generation, and is licensed under the MIT license.
