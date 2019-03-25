using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal
{
    public static class JavascriptExtension
    {
        public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string filename)
        {
            string str = ConfigurationManager.AppSettings["SiteDirectory"].ToString();
            if (str.Length > 0)
                filename = "/" + str + filename;
            string version = helper.GetVersion(filename);
            return MvcHtmlString.Create("<script type='text/javascript' src='" + filename + version + "'></script>");
        }

        private static string GetVersion(this HtmlHelper helper, string filename)
        {
            HttpContextBase httpContext = helper.ViewContext.RequestContext.HttpContext;
            if (httpContext.Cache[filename] != null)
                return httpContext.Cache[filename] as string;
            string str = string.Format("?v={0}", (object)new FileInfo(httpContext.Server.MapPath(filename)).LastWriteTime.ToString("MMddHHmmss"));
            httpContext.Cache.Add(filename, (object)str, (CacheDependency)null, DateTime.Now.AddMinutes(5.0), TimeSpan.Zero, CacheItemPriority.Normal, (CacheItemRemovedCallback)null);
            return str;
        }
    }
}