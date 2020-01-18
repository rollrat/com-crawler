# Community Crawling Engine

Community is any form of website where an unspecified number of people can post and share their opinions.

This project collect the title and additional information of the post and build own search engine.
Provides fast web crawling using proxy and various add-ons.
We are looking for a very stable and scalable design based on Rust.

## How to use?

### Bot

### Components

### Downloader

Put the URL of the site you want to download.

```
./com_crawler.Console https://www.instagram.com/taylorswift/
```

If you want to see the download process, add `-p` option.

```
./com_crawler.Console https://www.instagram.com/taylorswift/ -p
```

If you want to format the download path, enter `--list-extractor` to see the supported options. 
`%(file)s` and `%(ext)s` are provided by default.

```
./com_crawler.Console --list-extractor

...
[InstagramExtractor]
[HostName] www\.instagram\.com
[Checker] ^https?://www\.instagram\.com/(?:p\/)?(?<id>.*?)/?.*?$
[Information] Instagram extactor info
   user:             Full-name.
   account:          User-name
[Options]
   --only-images               Extract only images.
   --only-thumbnail            Extract only thumbnails.
   --include-thumbnail         Include thumbnail extracting video.
   --limit-posts               Limit read posts count. [use --limit-posts <Number of post>]
...

./com_crawler.Console -o "[%(account)s] %(user)s/%(file)s.%(ext)s" https://www.instagram.com/taylorswift/
```

### Server

## Contribution

Welcome to any form of contribution!

If you are interested in this project or have any suggestions, feel free to open the issue!

## Thanks

```
dotnet: https://dotnet.microsoft.com/
youtube-dl: https://github.com/ytdl-org/youtube-dl
pixivpy: https://github.com/upbit/pixivpy
Pixeez: https://github.com/cucmberium/Pixeez
ImageSharp: https://github.com/SixLabors/ImageSharp
HtmlAgilityPack: https://html-agility-pack.net/
Json.NET: https://www.newtonsoft.com/json
FFmpeg: https://www.ffmpeg.org/
Orbot: https://www.torproject.org
Free Proxies: http://www.freeproxylists.net/, http://www.proxylists.net/, https://www.us-proxy.org/, http://free-proxy.cz/en/
```
