namespace SamweiSpiderApp2021.AppCode
{
    #region _Spaces
    using Model;
    #endregion

    public class W91MJWHelper
    {
        #region _Variables
        private const string _GetList_URL = "https://91mjw.com/category/dianying";
        #endregion

        #region List
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public static List<MovieInfo> GetList(int PageIndex)
        {
            var Results = new List<MovieInfo>() { };

            var HttpUrl = _GetList_URL + (PageIndex < 2 ? "" : string.Format("/page/{0}", PageIndex));
            var HtmlDoc = SpiderHelper.GetHtmlDocument(HttpUrl, new Dictionary<string, string>());
            if (HtmlDoc != null)
            {
                var ItemNodes = SpiderHelper.GetHtmlNodes(HtmlDoc, "//div[@class='m-movies clearfix']/article[@class='u-movie']");
                if (ItemNodes != null && ItemNodes.Count > 0)
                {
                    foreach (var item in ItemNodes)
                    {
                        #region Set-Data
                        //名称
                        var _Name = SpiderHelper.GetHtmlNode(item, "./a/h2").InnerText;

                        //Url / Id
                        var _Url = SpiderHelper.GetHtmlNode(item, "./a").Attributes["href"].Value;
                        var _Id = AppFun.GetInt32(AppFun.GetNumbericInString(_Url.Split('/').Last()));

                        //封面
                        var _ImageUrl = SpiderHelper.GetHtmlNode(item, "./a/div[@class='list-poster']/img").Attributes["data-original"].Value;

                        //评分
                        var _Score = AppFun.GetDecimal(AppFun.GetNumbericInString(SpiderHelper.GetHtmlNode(item, "./div[@class='pingfen']/span").InnerText, true));

                        //质量 (720P..)
                        var _Quality = SpiderHelper.GetHtmlNode(item, "./div[@class='zhuangtai']/span").InnerText;

                        //标签
                        var _TagNodes = SpiderHelper.GetHtmlNodes(item, "./div[@class='meta']/span/a");
                        var _TagList = new List<MovieInfo_Tag> { };
                        if(_TagNodes!=null && _TagNodes.Count > 0)
                        {
                            foreach(var tag in _TagNodes)
                            {
                                var _TagName = tag.InnerText;
                                var _TagShort = tag.Attributes["href"].Value.Split('/').Last();
                                _TagList.Add(new MovieInfo_Tag() { Name = _TagName, Short = _TagShort });
                            }
                        }

                        //Set-Value
                        Results.Add(new MovieInfo()
                        {
                            Id = 0,
                            SourceId = _Id,
                            Name = _Name,
                            FaceUrl = _ImageUrl,
                            Url = _Url,
                            Score = _Score,
                            Quality = _Quality,
                            Tags = _TagList
                        });
                        #endregion
                    }
                }
            }

            return Results;
        }
        #endregion

        #region Pages
        /// <summary>
        /// 获取页数
        /// </summary>
        /// <returns></returns>
        public static int GetPageTotal()
        {
            var Pages = 0;

            var HtmlDoc = SpiderHelper.GetHtmlDocument(_GetList_URL, new Dictionary<string, string>());
            if (HtmlDoc != null)
            {
                var PageNodes = SpiderHelper.GetHtmlNodes(HtmlDoc, "//div[@class='pagination pagination-multi']/ul/li");
                if (PageNodes != null && PageNodes.Count > 0)
                    Pages = AppFun.GetInt32(AppFun.GetNumbericInString(PageNodes.Last().InnerText));
            }

            return Pages;
        }
        #endregion
    }
}
