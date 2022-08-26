<!-- # <img src="./assets/logo.png" alt="Icon" width="60" />  -->
# KQL.NET

[![CI](https://github.com/wcontayon/Adom.KQL/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/wcontayon/Adom.KQL/.github/workflows/dotnet-ci.yml) 

[![NuGet](https://img.shields.io/nuget/v/ReedSolomon.NET.svg?label=NuGet)](https://www.nuget.org/packages/ReedSolomon.NET/)


KQL (Keywords Query Language) with LINQ methods in dotnet 

## What is Keyword Query Language (KQL)?

**KQL** is a free text-keyword and property restrictions query used to search in SharePoint. You can read more about it
[KQL syntax](https://docs.microsoft.com/en-us/sharepoint/dev/general-development/keyword-query-language-kql-syntax-reference?WT.mc_id=DT-MVP-5003978)

## Motivation

We were asked in one of our project to build an free query syntax to build search over an collection of data `(IEnumerable, etc)`.
After some research, we found that [**KQL (Keyword Query Language) syntax**](https://docs.microsoft.com/en-us/sharepoint/dev/general-development/keyword-query-language-kql-syntax-reference?WT.mc_id=DT-MVP-5003978) is best suited to our need.

**`Adom.KQL`** transforms a **KQL** to a `Linq expression` to be used in an `IEnumerable`

## Installation

**Adom.KQL** can be [found here on NuGet](https://www.nuget.org/packages/) and can be installed by copying and pasting the following command into your Package Manager Console within Visual Studio (Tools > NuGet Package Manager > Package Manager Console).

```
Install-Package Adom.KQL
```

Alternatively, you can install Adom.KQL via the CLI with the following command:

```
dotnet add package Adom.KQL
```

## Usage

```cs
// Let's define an class
public class TestClass
{
    public string? StringField { get; set; }
    public int NumberField { get; set; }
    public DateTime DateTimeField { get; set; }
    public DateOnly DateOnlyField { get; set; }
}

// we can build the query linq expression using 
// the KqlEngine
var expression = KqlEngine.Parse<TestClass>("StringField = 'value' or NumberField != 10");

// we can directly filter the IEnumerable with
// the KqlEngine
var datas = new List<TestClass>();
var result = KqlEngine.ProcessQuery<TestClass>(datas, "StringField = 'value' or NumberField != 10");
```

## LINQ Methods with KQL

You can use some LINQ methods directly with a KQL query.
- Where             `IEnumerable<T>.Where<T>(string kqlQuery)`
- First             `IEnumerable<T>.First<T>(string kqlQuery)`
- FirstOrDefault    `IEnumerable<T>.FirstOrDefault<T>(string kqlQuery)`
- Any               `IEnumerable<T>.Any<T>(string kqlQuery)`
- Count             `IEnumerable<T>.First<T>(string kqlQuery)`


## Limitations
Adom.KQL works only on IEnumerable of reference object type (any class with properties like `DateTime`, `int/uint/long/ulong/decimal`, `string`, `bool`).

## Feature in development
- Query with `IEnumerable<datetime / dateonly / int / long / decimal / string / bool>`
- Support Class with nested class properties
- Contains operator -e.g. `'StringField = 'value*' (startswith) / '*value' (endswith) / '*value*' (contains)`
- math calculation on query -e.g. `(NumberField1 +-*/ NumberField...) (>=, =, !=) NumberValue`  

## Contribution

This project welcomes any contribution. Feel free to submit a PR or open an Issue.
You can read our [Code of contribution]

## Links

- [KQL syntax](https://docs.microsoft.com/en-us/sharepoint/dev/general-development/keyword-query-language-kql-syntax-reference?WT.mc_id=DT-MVP-5003978)

## License

This project is licensed under the terms of the MIT license.
