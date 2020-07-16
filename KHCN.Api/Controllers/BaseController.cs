using KHCN.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Api.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        private Stopwatch _sw;
        private ILogger<T> _logger;
        protected ILogger<T> log => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<T>>());
        protected Stopwatch sw => _sw ?? (_sw = new Stopwatch());

        protected string GetUserName()
        {
            return this.User.Claims.First(i => i.Type == "UserName").Value;
        }

        protected async Task<PagedResults<T>> CreatePagedResults<T>(
        IQueryable<T> queryable,
        int page,
        int pageSize,
        string orderBy,
        bool ascending)
        {
            var skipAmount = pageSize * (page - 1);

            var data = queryable
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize);

            var totalNumberOfRecords = await queryable.CountAsync();
            var results = await data.ToListAsync();

            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            var nextPageUrl =
            page == totalPageCount
                ? null
                : Url?.Link("DefaultAPI", new
                {
                    page = page + 1,
                    pageSize,
                    orderBy,
                    ascending
                });

            return new PagedResults<T>
            {
                Results = results,
                PageNumber = page,
                PageSize = results.Count,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords,
                NextPageUrl = nextPageUrl
            };
        }
    }
}