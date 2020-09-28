using System;
using System.Collections.Generic;
using System.Text;

namespace NewsCollector.BLL.Helpers
{
    public class RegexPattern
    {
        public static string Pattern { get; } = "<[^>]+>|&nbsp;|&laquo;|&raquo;|&mdash;|&bdquo;|&ldquo;|&quot;|&ndash;";
    }
}
