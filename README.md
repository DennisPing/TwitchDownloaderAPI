# Twitch Downloader API

An API around [lay295/TwitchDownloader](https://github.com/lay295/TwitchDownloader) core library

## Requirements

- .NET 7
- ASP.NET Core 7

## Keep [TwitchDownloader](https://github.com/lay295/TwitchDownloader) dependency up to date

### Using the Justfile command

   ```txt
   just
   ```

   List all Justfile commands
   ```txt
   just --list
   
   just build-debug
   just build-release
   just clean
   just update-submodule
   just commit-submodule
   ```

### Manual method

   ```txt
   cd TwitchDownloader
   git fetch origin master
   git pull --ff-only origin master
   git add .
   git commit -m "<your git commit message>"
   ```