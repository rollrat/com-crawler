// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com_crawler.Component.Community
{
    public class HumorUnivSitemapBoard : CommunitySitemapBoard
    {
        public new string MakeURL()
        {
            if (!string.IsNullOrEmpty(RawURL))
                return RawURL;
            if (!string.IsNullOrEmpty(Id))
                return $"http://web.humoruniv.com/board/humor/list.html?table={Id}";
            return "";
        }
    }

    public class HumorUnivArticle : CommunityArticle
    {
        public new string MakeURL()
        {
            return "";
        }
    }

    public class HumorUnivComponent : CommunityModel
    {
        public HumorUnivComponent(int index) : base()
        {
            Context = new CommunityContext
            {
                Index = index,

                Name = "웃긴대학",
                Host = "humoruniv.com",
                Description = "",

                IsMobileOnly = false,
                IsLoginable = true,
                IsCreatableAccount = true,

                Inclination = new HashSet<CommunityInclinationType>
                {
                    CommunityInclinationType.Humor
                },

                Sitemap = -1,

                CategoryMap = null,
                BoardMap = null,

                Board = new List<int>(),
                Article = new List<int>(),
                Comment = new List<int>(),

                LoginInfo = new List<int>(),
            };
        }

        #region Build Site Map

        public override void BuildSitemap()
        {
            var sitemap = CommunityContextManager.CreateSitemap(Context);
            Context.Sitemap = sitemap;

            var category = new int[] {
                CommunityContextManager.CreateCategoryMap("웃대 스페셜"),
                CommunityContextManager.CreateCategoryMap("유머/웹툰"),
                CommunityContextManager.CreateCategoryMap("창작/예술"),
                CommunityContextManager.CreateCategoryMap("친목/휴게실"),
                CommunityContextManager.CreateCategoryMap("테마 게시판"),
                CommunityContextManager.CreateCategoryMap("웃대 서비스"),
            };

            Context.CategoryMap.AddRange(category);

            {
                var boards = new (int, int)[] {
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[0], "인기 자료", "pick", inclination: new HashSet<CommunityInclinationType> { CommunityInclinationType.Humor }),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[0], "월간베스트", raw_url: "http://web.humoruniv.com/board/best/index.html"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[0], "답글베스트", raw_url: "http://web.humoruniv.com/board/humor/comment_search.html?st=week"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[0], "너의얼굴은", "face"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[0], "웹소설", raw_url: "http://web.humoruniv.com/cr/cr_list.html"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[0], "웃파고", raw_url: "http://web.humoruniv.com/ai/hupago.html"),
                };

                Context.BoardMap.AddRange(boards.Select(x => x.Item1));
                Context.Board.AddRange(boards.Select(x => x.Item2));
                CommunityContextManager.Instance.CategoryMap[Context.CategoryMap[0]].Boards.AddRange(boards.Select(x => x.Item2));
            }

            {
                var boards = new (int, int)[] {
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "웃긴 자료", "pds", inclination: new HashSet<CommunityInclinationType> { CommunityInclinationType.Humor }),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "대기 자료", "pdswait"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "웃긴 제목", "funtitle"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "대기 제목", "titlewait"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "웃대툰", "art_toon"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "신예툰", "nova_toon"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "지식 KIN", "kin"),
                    CommunityContextManager.CreateBoardMapBoard<HumorUnivSitemapBoard>(category[1], "지식 OTL", "otl"),
                };

                Context.BoardMap.AddRange(boards.Select(x => x.Item1));
                Context.Board.AddRange(boards.Select(x => x.Item2));
                CommunityContextManager.Instance.CategoryMap[Context.CategoryMap[1]].Boards.AddRange(boards.Select(x => x.Item2));
            }
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
