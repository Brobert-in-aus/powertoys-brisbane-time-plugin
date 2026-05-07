# Brisbane Time PowerToys Run Plugin

PowerToys Run plugin that converts times from common source timezones to Brisbane time.

## Usage

Use the `bt` action keyword in PowerToys Run:

```text
bt 10:30 PM ET
bt 22:30 EST
bt ten thirty CST
bt 10:00 CET
bt 10:00 GMT+2
bt May 10 10:30 PM ET
bt 2026-05-10 22:30 EST
bt next Friday ten thirty pm PT
bt tomorrow quarter past ten pm PT
bt noon UTC
```

Brisbane does not observe daylight saving, so the output is always shown as AEST.

## Supported Input

The parser supports:

- Numeric times: `10:30 PM`, `22:30`, `1030pm`
- Word times: `ten thirty`, `quarter past ten`, `quarter to eleven`, `noon`, `midnight`
- Dates: `May 10`, `10 May`, `2026-05-10`, `10/05/2026`
- Relative dates: `today`, `tomorrow`, `yesterday`, `Friday`, `next Friday`, `last Friday`
- Common zones: North America, Europe, UK, Africa, India, East/Southeast Asia, Australia, New Zealand, and `UTC`/`GMT` offsets such as `UTC+2` or `GMT-0530`

Region names such as `ET`, `Central European Time`, `UK Time`, `India Time`, and `New Zealand Time` use Windows timezone rules where applicable, so daylight saving is applied for the selected date. Abbreviations such as `EST`, `CST`, `CET`, and `CEST` are treated as fixed offsets.

Numeric dates without a month name use your Windows culture settings. Month-name dates and ISO dates are preferred when you want no ambiguity.

## Build

Install the .NET 9 SDK, then run:

```powershell
dotnet build .\BrisbaneTime.slnx -c Release /p:Platform=x64
dotnet test .\BrisbaneTime.slnx /p:Platform=x64
```

## Deploy

From an elevated PowerShell prompt:

```powershell
.\deploy.ps1 -Platform x64
```

The script copies the built plugin to:

```text
%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\BrisbaneTime
```

Restart PowerToys after deploying if it is already running.
