// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler
{
    public class Version
    {
        public const int MajorVersion = 2020;
        public const int MinorVersion = 01;
        public const int BuildVersion = 19;

        public const string Name = "Community Crawler";
        public static string Text { get; } = $"{MajorVersion}.{MinorVersion}.{BuildVersion}";
    }
}
