// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Compiler;
using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Script
{
    public class CrawlerDescriptionLanguage
    {
        static ShiftReduceParser parser;
        public static ShiftReduceParser GetParser()
        {
            if (parser != null)
                return parser;
            return null;
        }

        static Scanner scanner;
        public static Scanner GetScanner()
        {
            if (scanner != null)
                return scanner;
            return null;
        }
    }
}