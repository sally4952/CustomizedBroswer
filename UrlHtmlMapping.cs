using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expolerer
{
    internal static class UrlHtmlMapping
    {
        public static readonly Dictionary<Uri, string> Mapping = new Dictionary<Uri, string>
        {
            { new Uri("http://www.example.com"), @"<html><head><title>Example Page</title></head><body><h1>This is just an example page.</h1></body></html>" },
        };
    }
}
