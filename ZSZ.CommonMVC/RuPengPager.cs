using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.CommonMVC
{
    public  class RuPengPager
    {
        /// <summary>
        /// 每一页显示的数据条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总数据条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 显示出来的页码的最多的页码数目比如说10
        /// </summary>
        public int MaxPagerCount { get; set; }
        /// <summary>
        /// 当前页码数，从第一页开始
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 当前页的页码的样式名字
        /// </summary>
        public string CurrentPageClassName { get; set; }

        /// <summary>
        /// 链接的格式，约定其中页码用{pn}占位符
        /// </summary>
        public string UrlPattern { get; set; }
        /// <summary>
        /// 上一页
        /// </summary>
        public int FrontPage { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <returns></returns>
        public int NextPage { get; set; }
        /// <summary>
        /// 首页
        /// </summary>
        public int FirstPage { get; set; }
        /// <summary>
        /// 末页
        /// </summary>
        public int LastPage { get; set; }
        public string GetPagerHtml()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<ul>");
            //总页数
            int pageCount =(int)Math.Ceiling(TotalCount*1.0f/PageSize);
            //FirstPage = 1;
            //LastPage = pageCount;
            //NextPage =(PageIndex + 1).ToString();
            //FrontPage = pageCount - 1;
            //显示出来的页码的起始页码
            int startPageIndex =(int) Math.Max(1,PageIndex-MaxPagerCount/2);//显示第一个页码
            int endPageIndex = Math.Min(pageCount, startPageIndex + MaxPagerCount );//显示出来的最后一个页码 
            for(int i = startPageIndex; i <= endPageIndex; i++)
            {
                //是当前页码
                if (i==PageIndex)
                {
                    html.Append("<li class='").Append(CurrentPageClassName).Append(i).Append(">").Append("</li>");
                    //html.Append("<li> class=''").Append("</li>")
                    
                }
                else
                {
                    string href = UrlPattern.Replace("{pn}",i.ToString());
                    html.Append("<li><a href='").Append(href).Append("'>").Append(i).Append("</a></li>");
                }
            }          
            html.Append("</ul>");
            return html.ToString(); 
        }

    }
}
