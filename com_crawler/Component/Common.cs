// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Extractor;
using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Component
{
    /// <summary>
    /// Component and Extarctor are different.
    /// 
    /// Component provide api, crawling, manufactoring, and rebuild data set funcitons.
    /// However, extarctor is designed for only downloading some contents like image, movie etc...
    /// So, you cannot extracting useful structed informations using extractor.
    /// </summary>
    public abstract class ComponentModel
    {
    }
}
