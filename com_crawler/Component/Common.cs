// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Extractor;
using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Component
{
    public enum ComponentType
    {
        CommunitySite,
        NewsSite,
    }

    public class ComponentQuery
    {
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;
        public DateTime EndDateTime { get; set; } = DateTime.MaxValue;

    }

    public class ComponentLoginData
    {

    }

    /// <summary>
    /// Component and Extarctor are different.
    /// 
    /// Component provide api, crawling, manufactoring, and rebuild data set funcitons.
    /// However, extarctor is designed for only downloading some contents like image, movie etc...
    /// So, you cannot extracting useful structed informations using extractor.
    /// </summary>
    public abstract class ComponentModel
    {
        public ComponentType Type { get; protected set; }


    }

    public class ComponentManager
    {

    }
}
