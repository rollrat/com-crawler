// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Component
{
    public class CommunityInclination
    {
        public enum InclinationType
        {
            Complex, // Default

            // Keyword
            Professional,
            Anonymity,

            // Scholarship
            Politics,
            Economy,
            Programming,

            // Organism
            Cat,
            Dog,
            Plant,
            Universe,

            // Hobby
            Life,
            Game,
            Humor,
            IDOL,
            Song,

            // Motors
            Car,
            Motorcycle,

            // Device
            Smartphone,
            Computer,
            Watch,
        }

        public HashSet<InclinationType> Types;
    }

    public class CommunitySitemapBoard
    {
        public int Index;
        public int Category;
        public int Board;
        public string Name;
        public string Id;
        public string Description;
        public CommunityInclination Inclination;
        public virtual string MakeURL() { throw new NotImplementedException(); }
    }

    public class CommunitySitemapCategory
    {
        public int Index;
        public string Name;
        public List<int> SubCategories;
        public List<int> Links;
    }

    public class CommunitySitemap
    {
        public int Index;
        public int Context;
        public List<int> Categories;
        public List<int> Links;
    }

    public class CommunityArticle
    {
        /// <summary>
        /// Hashed Identification
        /// </summary>
        public int Index;

        public int Board;

        public string Id;

        public string Title;

        public string Writer;
        public string WriteTime;
        public string WriterId;

        public string Views;

        public string UpVote;
        public string DownVote;

        public string Body;

        public virtual string MakeURL() { throw new NotImplementedException(); }
    }

    public class CommunityBoard
    {
        public int Index;
        public int BoardMap;
        public List<int> Articles;
    }

    public class CommunityContext
    {
        public int Index;
        public string Name;
        public string URL;
        public string Description;

        public bool IsMobileOnly;
        public bool IsLoginable;

        /// <summary>
        /// This should be applied as false if user privacy information 
        /// is required under each country's policy.
        /// </summary>
        public bool IsCreatableAccount;

        public CommunityInclination Inclination;

        public CommunitySitemap Sitemap;

        public List<CommunitySitemapCategory> CategoryMap;
        public List<CommunitySitemapBoard> BoardMap;

        public List<CommunityBoard> Board;
        public List<CommunityArticle> Article;
    }

    public abstract class CommunityModel : ComponentModel
    {
        public CommunityContext Context { get; set; }

        public CommunityModel()
        {
            Type = ComponentType.CommunitySite;
        }


    }
}
