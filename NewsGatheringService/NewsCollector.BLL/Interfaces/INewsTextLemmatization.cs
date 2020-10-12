using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{
    /// <summary>
    /// Class for news lemmarization.
    /// </summary>
    public interface INewsTextLemmatization
    {
        /// <summary>
        /// Start lemmarization of news text
        /// </summary>
        /// <param name="text">news text</param>
        /// <returns>Returns lemmatized news text</returns>
        Task<string> RunAsync(string text);
    }
}
