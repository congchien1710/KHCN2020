using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KHCN.Api.Provider
{
    public static class AssemblyHelper
    {
        public static List<ControllerActionAssembly> GetAllControllerAction()
        {
            var result = Assembly.GetExecutingAssembly().GetTypes()
                   .Where(type => typeof(Controller).IsAssignableFrom(type))
                   .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                   .Where(method => method.IsPublic
                   && !method.IsDefined(typeof(NonControllerAttribute))
                   && !method.IsDefined(typeof(NonActionAttribute)))
                   .Select(x => new ControllerActionAssembly
                   {
                       Api = x.DeclaringType.Name,
                       Action = x.Name
                   }).ToList();

            return result;
        }
    }

    public class ControllerActionAssembly
    {
        public string Api { get; set; }
        public string Action { get; set; }
    }
}
