using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHelper
{
    internal static class ContextProvider
    {
        public static string ConfigRoot
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatGPT");
            }
        }
        /// <summary>
        /// Returns "" if the user has not provided an API key. If this happens,
        /// the user should be prompted to place "API Key.txt" in ConfigRoot
        /// </summary>
        public static string APIKey
        {
            get
            {
                return File.ReadAllText(Path.Combine(ConfigRoot, "API Key.txt")).Trim();
            }
        }
    }
}
