# Community Crawling Engine Development Manual

## Projects

### com_crawler

`com_crawler` project is the core library for com-crawler that uses `.netstandard`. 
This library implements all the major functions such as script analysis, 
configuration, download, crawling and file management.

### com_crawler.Console

`com_crawler.Console` project is cross platform console application for `Windows`, `Linux`, and `Mac OS`.
In this project, some command are implemented to properly use the main functions of the `com_crawler.Framework`.
Also, a dialog has implemented, so you can handle `com_crawler.Framework`.

### com_crawler.Tool.CustomCrawler

`com_crawler.Framework` project supports to script embedding based html-parsing command line called `Html Toolkit`.
This project will increases development productivity and eases maintenance.

[Click here for more informations.](Document/CustomCrawler.md)

## How to build?

### Linux

First, you must download and install .net-sdk for mono.

```
https://dotnet.microsoft.com/download/dotnet-core/3.0

Ubuntu 19.04 - x64
wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get install dotnet-sdk-3.0
```

Second, build project.

```
git clone https://github.com/rollrat/com-crawler
cd com-crawler/com_crawler.Console
nuget restore com_crawler.Console.csproj
dotnet build -c Release
```

Last, run program!

```
cd bin/Release/netcoreapp3
./com_crawler.Console
```

### Windows

Install VisualStudio 2019 and .NetCore 3.0.

Open `com_crawler.sln` and build solutions.

## App Provider

`App Provider` is initializer and deinitializer of `Community Crawler Framework`.
All parts of the `Community Crawler Framework` are interrelated with each components, so you need to be initialized.

### Initializing Step

```
1. Initialize Logs class for logging.
2. Create Lock file.
3. Check program crashed when last running.
4. Check invalid instance initializing.
5. Set gc to intrusive mode.
6. Initialize scheduler.
```

### Deinitializing Step

Inverse step of `Initializing Step`.

## Network

`Network` namespace is collection of core utils for downloading.

### Net Scheduler

Non-preemptive scheduler that allows you to change priorities in real-time.

### Net Task

Data structure for `Net Field` includes `URL`, `Referer`, `Cookie`, `Postprocessor`, `Events` and many other options.
Some useful methods is declared such as `NetTask.MakeDefault(url: string)`, `NetTask.MakeDefaultMobile(url: string)`.

### Net Field

A class that implements the core logic for downloading data.

### Net Tools

`NetTools` provides some useful download methods like `WebClient.DownloadStringAsync`.
All network communication is handled by the scheduler,
you should not use interfaces provided by `.Net` directly, such as `WebClient` or `HttpRequest`.

## Postprocessor

It is designed to perform tasks that are not related to downloads such as `FFmpeg` or `Zip`.
So, this class is created to define the tasks required after downloading.

## Extractor

`Extractor` gets a NetTask that directs the download task from a specific `URL`.
Seperated by `Host-Name`, all `crawling operations` are implemented here.

## Component

`Component` provide api, crawling, manufactoring, and rebuild data set funcitons.
However, `extarctor` is designed for only downloading some contents like image, movie etc...
So, you cannot extracting useful structed informations using extractor.

## Log

This class records all download progress.

## Cache

## Compiler