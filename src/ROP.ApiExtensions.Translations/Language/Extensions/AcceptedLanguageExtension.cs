using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ROP.ApiExtensions.Translations.Language.Extensions
{
    /// <summary>
    /// Extension to get the language from the header Accept-Language.
    /// </summary>
    public static class AcceptedLanguageExtension
    {
        /// <summary>
        /// Pick the favorite supported Language by the user browser. 
        /// if that language does not exist, the translation will pick the default one, English.
        /// based on https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language
        /// the header always follow the next pattern "ES;q=0.1", doesn't matter which country is the browser from.
        /// Need to set up the scope to a country which the decimals are dots. 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static CultureInfo GetCultureInfo(this IHeaderDictionary header)
        {
            using (new CultureScope(new CultureInfo("es")))
            {
                var languages = new List<(string, decimal)>();
                string acceptedLanguage = header["Accept-Language"];
                if (string.IsNullOrEmpty(acceptedLanguage))
                {
                    return new CultureInfo("es");
                }
                string[] acceptedLanguages = acceptedLanguage.Split(',');
                foreach (string accLang in acceptedLanguages)
                {
                    var languageDetails = accLang.Split(';');
                    if (languageDetails.Length == 1)
                    {
                        languages.Add((languageDetails[0], 1));
                    }
                    else
                    {
                        languages.Add((languageDetails[0], Convert.ToDecimal(languageDetails[1].Replace("q=", ""))));
                    }
                }
                string languageToSet = languages.OrderByDescending(a => a.Item2).First().Item1;
                return new CultureInfo(languageToSet);
            }
        }
        
    }
}