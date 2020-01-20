// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Component
{
    public enum CommunityInclinationType
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

    public class CommunitySitemapBoard
    {
        public int Index;
        public int Category;
        public int Board;
        public string Name;
        public string Id;
        public string Description;
        public HashSet<CommunityInclinationType> Inclination;
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

    public class CommunityComment
    {
        public int Index;

        public int Article;

        public string Writer;
        public string WriteTime;
        public string WriteId;

        public string UpVote;
        public string DownVote;

        public string Body;

        public List<int> SubComment;
    }

    public class CommunityArticle
    {
        /// <summary>
        /// Hashed Identification
        /// </summary>
        public int Index;

        public int Board;

        /// <summary>
        /// This instance not contains body.
        /// </summary>
        public bool IsHeaderOnly = true;

        public string Id;

        public string Title;

        public string Writer;
        public string WriteTime;
        public string WriterId;

        public string Views;

        public string UpVote;
        public string DownVote;

        public string Body;

        public List<int> Comment;

        public virtual string MakeURL() { throw new NotImplementedException(); }
    }

    public class CommunityBoard
    {
        public int Index;
        public int BoardMap;
        public List<int> Articles;
    }

    public class CommunityLoginInfo
    {
        public string Index;
        public bool IsAnonymous;
        public string Name;

        /// <summary>
        /// If `IsAnonymous` flag is not true, then
        /// </summary>
        public string Id;
        public string Password;
        public string Token;
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

        public HashSet<CommunityInclinationType> Inclination;

        public CommunitySitemap Sitemap;

        public List<CommunitySitemapCategory> CategoryMap;
        public List<CommunitySitemapBoard> BoardMap;

        public List<CommunityBoard> Board;
        public List<CommunityArticle> Article;
        public List<CommunityComment> Comment;

        public List<CommunityLoginInfo> LoginInfo;
    }

    public abstract class CommunityModel : ComponentModel
    {
        public CommunityContext Context { get; set; }

        public CommunityModel()
        {
            Type = ComponentType.CommunitySite;
        }

        public abstract void BuildSitemap();

        /// <summary>
        /// API Implementations
        /// </summary>
        
        public abstract void WriteArticle(CommunitySitemapBoard board, string title, string body, CommunityLoginInfo loginfo);
        public abstract void DeleteArticle(CommunityArticle article, CommunityLoginInfo loginfo);

        public abstract void WriteComment(CommunityArticle article, string body, CommunityLoginInfo loginfo);
        public abstract void WriteComment(CommunityComment comment, string body, CommunityLoginInfo loginfo);
        public abstract void DeleteComment(CommunityComment article, CommunityLoginInfo loginfo);

        public abstract void UpVoteArticle(CommunityArticle article, CommunityLoginInfo loginfo);
        public abstract void DownVoteArticle(CommunityArticle article, CommunityLoginInfo loginfo);

        /// <summary>
        /// Parser Implementations
        /// </summary>

        public abstract int GetMaximumBoardPage(CommunityBoard board);

        public abstract List<int> ParseBoard(CommunityBoard board, int page);
        public abstract void ParseArticle(CommunityArticle article);
        public abstract List<int> ParseComment(CommunityArticle article);
    }
}
