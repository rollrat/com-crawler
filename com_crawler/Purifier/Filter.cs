// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace com_crawler.Purifier
{
    public class Filter
    {
        static Regex regex_email = new Regex(@"[\w!#$%&'*+/=?`{|}~^.-]+@[A-Z0-9.-]+");

        public static string PersonalInformation(string str)
        {
            return regex_email.Replace(str, "*@*.*");
        }
    }
}
