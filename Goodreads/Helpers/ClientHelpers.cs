using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goodreads.Http;
using Goodreads.Models.Response;
using RestSharp;

namespace Goodreads.Helpers
{
    /// <summary>
    /// Helper functions
    /// </summary>
    internal class ClientHelpers
    {
        /// <summary>
        /// Executes a paged methdo the required number of times in order to return the full list of elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">Goodreads API connection object.</param>
        /// <param name="endPoint">The API endpoint.</param>
        /// <param name="expectedRoot">The name of the root element.</param>
        /// <param name="parameters">The parameters for this request.</param>
        /// <returns>The list with all elements</returns>
        public static IReadOnlyList<T> GetAllItems<T>(IConnection connection, 
                                                        string endPoint,
                                                        string expectedRoot,
                                                        List<Parameter> parameters) where T : Models.ApiResponse, new()
        {
            List<T> joinedList = new List<T>();
            if (parameters == null)
                return null;

            var pageParameter = parameters.Find(p => p.Name == "page");
            if (pageParameter == null)
                return null;

            int currentPage = 1;
            pageParameter.Value = currentPage;
            Task<PaginatedList<T>> firstList = connection.ExecuteRequest<PaginatedList<T>>(endPoint, parameters, null, expectedRoot);

            foreach (T userSummary in firstList.Result.List)
            {
                joinedList.Add(userSummary);
            }

            if (firstList.Result.Pagination.TotalItems > firstList.Result.Pagination.End)
            {
                var pageSize = firstList.Result.Pagination.End - firstList.Result.Pagination.Start;
                double totalPages = firstList.Result.Pagination.TotalItems / pageSize;
                while (currentPage <= System.Math.Ceiling(totalPages))
                {
                    currentPage++;
                    pageParameter.Value = currentPage;
                    Task<PaginatedList<T>> iteratedList = connection.ExecuteRequest<PaginatedList<T>>(endPoint,
                        parameters, null, expectedRoot);

                    foreach (T userSummary in iteratedList.Result.List)
                    {
                        joinedList.Add(userSummary);
                    }
                }

            }

            return joinedList;
        }

    }
}
