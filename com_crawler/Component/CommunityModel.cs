// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
        public string WriterId;

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
        public int Index;
        public int Context;
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

        public int Sitemap;

        public List<int> CategoryMap;
        public List<int> BoardMap;

        public List<int> Board;
        public List<int> Article;
        public List<int> Comment;

        public List<int> LoginInfo;
    }

    public class CommunityContextManager : ILazy<CommunityContextManager>
    {
        List<CommunityContext> contexts;

        List<CommunitySitemap> Sitemap;

        List<CommunitySitemapCategory> CategoryMap;
        List<CommunitySitemapBoard> BoardMap;

        List<CommunityBoard> Board;
        List<CommunityArticle> Article;
        List<CommunityComment> Comment;

        List<CommunityLoginInfo> LoginInfo;

        public CommunityContextManager()
        {
            contexts = new List<CommunityContext>();
            Sitemap = new List<CommunitySitemap>();
            CategoryMap = new List<CommunitySitemapCategory>();
            BoardMap = new List<CommunitySitemapBoard>();
            Board = new List<CommunityBoard>();
            Article = new List<CommunityArticle>();
            Comment = new List<CommunityComment>();
            LoginInfo = new List<CommunityLoginInfo>();
        }

        Mutex scalar = new Mutex();

        static void Lock()
        {
            Instance.scalar.WaitOne();
        }

        static void Unlock()
        {
            Instance.scalar.ReleaseMutex();
        }

        public static int CreateContext(CommunityContext context)
        {
            Lock();

            int index = Instance.contexts.Count;

            context.Index = index;
            Instance.contexts.Add(context);

            Unlock();

            return index;
        }

        public static int CreateSitemap(int context)
        {
            Lock();

            int index = Instance.Sitemap.Count;

            Instance.Sitemap.Add(new CommunitySitemap
            {
                Index = index,
                Context = context,
                Links = new List<int>(),
                Categories = new List<int>(),
            });

            Unlock();

            return index;
        }

        public static int CreateSitemap(CommunityContext context)
        {
            return CreateSitemap(context.Index);
        }

        public static int CreateCategoryMap(string name)
        {
            Lock();

            int index = Instance.CategoryMap.Count;

            Instance.CategoryMap.Add(new CommunitySitemapCategory
            {
                Index = index,
                Name = name,
                SubCategories = new List<int>(),
                Links = new List<int>(),
            });

            Unlock();

            return index;
        }

        public static (int, int) CreateBoardMapBoard<T>(int category, string name, string id = "", 
            string description = "", HashSet<CommunityInclinationType> inclination = null)
            where T : CommunitySitemapBoard, new()
        {
            Lock();

            int index_of_boardmap = Instance.BoardMap.Count;
            int index_of_board = Instance.Board.Count;

            Instance.BoardMap.Add(new T
            {
                Index = index_of_boardmap,
                Category = category,
                Board = index_of_board,
                Name = name,
                Id = id,
                Description = description,
                Inclination = inclination,
            });

            Instance.Board.Add(new CommunityBoard
            {
                Index = index_of_board,
                BoardMap = index_of_boardmap,
                Articles = new List<int>(),
            });

            Unlock();

            return (index_of_boardmap, index_of_board);
        }

        public static (int, int) CreateBoardMapBoard<T>(CommunitySitemapCategory category, string name, string id = "",
            string description = "", HashSet<CommunityInclinationType> inclination = null)
            where T : CommunitySitemapBoard, new()
        {
            return CreateBoardMapBoard<T>(category, name, id, description, inclination);
        }

        public static int CreateArticle<T>(int board, bool is_header_only, string id, string title, string writer, string write_time, string writer_id,
            string views, string upvote, string downvote, string body)
            where T : CommunityArticle, new()
        {
            Lock();

            int index = Instance.Article.Count;

            Instance.Article.Add(new T
            {
                Index = index,
                Board = board,
                IsHeaderOnly = is_header_only,
                Id = id,
                Title = title,
                Writer = writer,
                WriteTime = write_time,
                WriterId = writer_id,
                Views = views,
                UpVote = upvote,
                DownVote = downvote,
                Body = body,
                Comment = new List<int>()
            });

            Unlock();

            return index;
        }

        public static int CreateArticle<T>(CommunityBoard board, bool is_header_only, string id, string title, string writer, string write_time, string writer_id,
            string views, string upvote, string downvote, string body)
            where T : CommunityArticle, new()
        {
            return CreateArticle<T>(board, is_header_only, id, title, writer, write_time, writer_id, views, upvote, downvote, body);
        }

        public static int CreateComment(int article, string writer, string write_time, string writer_id, string upvote, string downvote, string body)
        {
            Lock();

            int index = Instance.Comment.Count;

            Instance.Comment.Add(new CommunityComment
            {
                Index = index,
                Article = article,
                Writer = writer,
                WriteTime = write_time,
                WriterId = writer_id,
                UpVote = upvote,
                DownVote = downvote,
                Body = body,
                SubComment = new List<int>()
            });

            Unlock();

            return index;
        }

        public static int CreateComment(CommunityArticle article, string writer, string write_time, string writer_id, string upvote, string downvote, string body)
        {
            return CreateComment(article, writer, write_time, writer_id, upvote, downvote, body );
        }

        public static int CreateLoginInfo(int context, bool is_anonymous, string name, string id, string password, string token)
        {
            Lock();

            int index = Instance.LoginInfo.Count;

            Instance.LoginInfo.Add(new CommunityLoginInfo
            {
                Index = index,
                Context = context,
                IsAnonymous = is_anonymous,
                Name = name,
                Id = id,
                Password = password,
                Token = token
            });

            Unlock();

            return index;
        }

        public static int CreateLoginInfo(CommunityContext context, bool is_anonymous, string name, string id, string password, string token)
        {
            return CreateLoginInfo(context, is_anonymous, name, id, password, token);
        }
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
