using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KHCN.Web
{
    public class Utils
    {
        private static Random _random = new Random();

        public static class ConfigurationManager
        {
            public static IConfiguration AppSetting { get; }

            static ConfigurationManager()
            {
                AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
            }
        }

        public static string GenerateRandomReferCode()
        {
            return _random.Next(1000, 9999).ToString("D4");
        }

        public static long TimeInEpoch(DateTime? dt = null)
        {
            if (dt.HasValue)
                return (long)(dt.Value.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public static DateTimeOffset EpochToTime(long ep)
        {
            return DateTimeOffset.FromUnixTimeSeconds(ep).ToLocalTime();
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ep);
        }

        public static string EpochToTimeStringShortFomart(long ep)
        {
            return EpochToTime(ep).ToString("dd/MM/yyyy HH:mm");
        }

        public static string EpochToTimeString(long ep, string format = "dd/MM/yyyy HH:mm:ss")
        {
            return EpochToTime(ep).ToString(format);
        }
    }

    public static class ExtendMethod
    {
        public static string GetHashString(this String s)
        {
            int n = s.Length;
            int h = 0;
            for (int i = 0; i < n; i++)
            {
                h += s[i];
                h *= 123123123;
            }
            return h.ToString();
        }

        public static int GetHashInt(this String s)
        {
            int n = s.Length;
            int h = 0;
            for (int i = 0; i < n; i++)
            {
                h += s[i];
                h *= 123123123;
            }
            return h;
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static string GetDescription(this Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }

    public class GenericCompare<T> : IEqualityComparer<T> where T : class
    {
        private Func<T, object> _expr { get; set; }

        public GenericCompare(Func<T, object> expr)
        {
            this._expr = expr;
        }

        public bool Equals(T x, T y)
        {
            var first = _expr.Invoke(x);
            var sec = _expr.Invoke(y);
            if (first != null && first.Equals(sec))
                return true;
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            return _expr.Invoke(obj).GetHashCode();
        }
    }
}
