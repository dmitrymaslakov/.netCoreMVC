using System;
using System.Collections.Generic;
using System.Text;

namespace NewsCollector.BLL.Helpers
{
    /// <summary>
    /// Class for clearing news text
    /// </summary>
    public class RegexPattern
    {
        public static string Pattern { get; } = "<[^>]+>|&nbsp;|&laquo;|&raquo;|&mdash;|&bdquo;|&ldquo;|&quot;|&ndash;|&thinsp;|&deg;|&minus;|\\t|\\r|\\n";
    }
}
