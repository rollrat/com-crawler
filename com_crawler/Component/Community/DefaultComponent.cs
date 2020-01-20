// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Component.Community
{
    internal sealed class DefaultComponent : CommunityModel
    {
        #region Initialize context

        public DefaultComponent(int index) : base()
        {
            Context = new CommunityContext
            {
                Index = index,

                Name = "",
                URL = "",
                Description = "",

                IsMobileOnly = false,
                IsLoginable = true,
                IsCreatableAccount = true,

                Inclination = new HashSet<CommunityInclinationType>(),

                Sitemap = null,

                CategoryMap = new List<CommunitySitemapCategory>(),
                BoardMap = new List<CommunitySitemapBoard>(),

                Board = new List<CommunityBoard>(),
                Article = new List<CommunityArticle>(),
                Comment = new List<CommunityComment>(),

                LoginInfo = new List<CommunityLoginInfo>(),
            };
        }

        #endregion

        #region Build Site Map

        public override void BuildSitemap()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region API

        public override void WriteArticle(CommunitySitemapBoard board, string title, string body, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        public override void DeleteArticle(CommunityArticle article, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        public override void WriteComment(CommunityArticle article, string body, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        public override void WriteComment(CommunityComment comment, string body, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        public override void DeleteComment(CommunityComment article, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        public override void UpVoteArticle(CommunityArticle article, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        public override void DownVoteArticle(CommunityArticle article, CommunityLoginInfo loginfo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Parse

        public override int GetMaximumBoardPage(CommunityBoard board)
        {
            throw new NotImplementedException();
        }

        public override void ParseArticle(CommunityArticle article)
        {
            throw new NotImplementedException();
        }

        public override List<int> ParseBoard(CommunityBoard board, int page)
        {
            throw new NotImplementedException();
        }

        public override List<int> ParseComment(CommunityArticle article)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
