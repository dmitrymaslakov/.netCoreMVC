using NewsGatheringService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsCollector.BLL.Interfaces
{
    /// <summary>
    /// Class for news evaluation.
    /// </summary>
    public interface INewsEvaluation
    {
        /// <summary>
        /// Evaluate news based on affin
        /// </summary>
        /// <param name="news">News to be evaluated</param>
        /// <returns>value in the range of -5 to 5</returns>
        Task<int> EvaluateNewsAsync(News news);
    }
}
