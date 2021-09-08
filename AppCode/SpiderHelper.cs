namespace SamweiSpiderApp2021.AppCode
{
    #region _Spaces
    using HtmlAgilityPack;
    using System.Text;
    #endregion

    public class SpiderHelper
    {
        #region HtmlDocument
        /// <summary>
        /// 文档 (URL)
        /// </summary>
        /// <param name="GetURL"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public static HtmlDocument? GetHtmlDocument(string GetURL, Dictionary<string, string> Params)
        {
            var ParamData = new StringBuilder();
            if (Params != null)
                foreach (var item in Params)
                    ParamData.AppendFormat("{0}={1}&", item.Key, item.Value);

            var HttpUrl = GetURL + (GetURL.IndexOf("?") > -1 ? "" : "?") + ParamData.ToString().TrimEnd('&');

            var IsSucceed = HttpHelper.HttpGet(HttpUrl, out int _, out string HttpMessages, out Dictionary<string, string> _);
            if (IsSucceed)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(HttpMessages);
                return htmlDocument;
            }
            return null;
        }

        /// <summary>
        /// 文档 (HtmlContent)
        /// </summary>
        /// <param name="HtmlContent"></param>
        /// <returns></returns>
        public static HtmlDocument GetHtmlDocument(string? HtmlContent)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HtmlContent);
            return htmlDocument;
        }
        #endregion

        #region HtmlNode(s)
        /// <summary>
        /// 节点列表（文档）
        /// </summary>
        /// <param name="HtmlDoc"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static HtmlNodeCollection GetHtmlNodes(HtmlDocument HtmlDoc, string XPath)
        {
            return HtmlDoc.DocumentNode.SelectNodes(XPath);
        }

        /// <summary>
        /// 节点列表（从节点）
        /// </summary>
        /// <param name="ItemNode"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static HtmlNodeCollection GetHtmlNodes(HtmlNode ItemNode, string XPath)
        {
            return ItemNode.SelectNodes(XPath);
        }

        /// <summary>
        /// 节点信息
        /// </summary>
        /// <param name="ItemNode"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static HtmlNode GetHtmlNode(HtmlNode ItemNode, string XPath)
        {
            return ItemNode.SelectSingleNode(XPath);
        }
        #endregion
    }
}
