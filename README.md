# PlantBook

[![.Build status](https://github.com/denxorz/PlantBook/workflows/.NET/badge.svg)](https://github.com/denxorz/PlantBook/actions) [![Coverage Status](https://coveralls.io/repos/github/denxorz/PlantBook/badge.svg?branch=master)](https://coveralls.io/github/denxorz/PlantBook?branch=master) [![NuGet](https://buildstats.info/nuget/Denxorz.PlantBook)](https://www.nuget.org/packages/Denxorz.PlantBook/) [![License](http://img.shields.io/:license-mit-blue.svg)](https://github.com/denxorz/PlantBook/blob/master/LICENSE)

## What does it do?
This library adds support for the PlantBook API. See https://plantbook.io for more info.

## Auth
You will need to create a account on https://open.plantbook.io and generate a `ClientId` and `Secret`.

## Examples

Searching for plants:

```C#
var plantbook = new PlantBook(new("auth cliend id", "auth secret"));
var searchResult = await plantbook.SearchAsync("pancake");

Console.WriteLine(searchResult.Results[0].DisplayPid);

// Output:
//   Pilea peperomioides
```

Get more info about a plant:

```C#
var plantbook = new PlantBook(new("auth cliend id", "auth secret"));
var plant = await plantbook.GetAsync("pilea peperomioides");

Console.WriteLine(plant.Alias);
Console.WriteLine(plant.MinTemp);
Console.WriteLine(plant.ImageUrl);

// Output:
//   pancake plant
//   10
//   https://opb-img.plantbook.io/pilea%20peperomioides.jpg
```

## Tools and Products Used

* [PlantBook](https://plantbook.io)
* [Microsoft Visual Studio Community](https://www.visualstudio.com)
* [NUnit](https://www.nunit.org/)
* [Flurl](https://flurl.dev/)
* [Icons8](https://icons8.com/)
* [NuGet](https://www.nuget.org/)
* [GitHub](https://github.com/)


## Versions & Release Notes

version 1.0: First version
