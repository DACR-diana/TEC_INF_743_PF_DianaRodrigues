using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.WebApp.Helpers
{
    public interface ILocalizationHelper
    {
        Task<object> GetContent(string lang, string controller,string url);
    }
}
