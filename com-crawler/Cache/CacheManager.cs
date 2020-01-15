// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Crypto;
using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com_crawler.Cache
{
    public class CacheManager : ILazy<CacheManager>
    {
        public string CacheDirectory { get; set; }

        public CacheManager()
        {
            CacheDirectory = Path.Combine(AppProvider.ApplicationPath, "Cache");
            if (!Directory.Exists(CacheDirectory))
                Directory.CreateDirectory(CacheDirectory);
        }

        public void Append<T>(string cache_name, T cache_object) where T : new()
            => File.WriteAllText(Path.Combine(CacheDirectory, cache_name.GetHashMD5()), Log.Logs.SerializeObject(cache_object));

        public bool Exists(string cache_name)
            => File.Exists(Path.Combine(CacheDirectory, cache_name.GetHashMD5()));

        public string Find(string cache_name)
            => File.ReadAllText(Path.Combine(CacheDirectory, cache_name.GetHashMD5()));
    }
}
