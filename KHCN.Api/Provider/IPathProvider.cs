using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace KHCN.Api.Provider
{
    public interface IPathProvider
    {
        string MapPath(string path);
    }

    public class PathProvider : IPathProvider
    {
        private IWebHostEnvironment _environment;

        public PathProvider(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine($@"{_environment.ContentRootPath}\{path}");
            return filePath;
        }
    }
}