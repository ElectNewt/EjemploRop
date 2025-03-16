using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ROP.ApiExtensions.Translations.Language
{
    /// <summary>
    /// Utility class to get the localized value of an entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class LocalizationUtils<TEntity>
    {

        private static readonly IStringLocalizer _localizer;

        static LocalizationUtils()
        {
            var options = Options.Create(new LocalizationOptions());
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var type = typeof(TEntity);

            _localizer = factory.Create(type);
        }

        /// <summary>
        /// Gets the localized value of a field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetValue(string field)
        {
            return _localizer[field];
        }

        /// <summary>
        /// Gets the localized value of a field in a specific culture.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static string GetValue(string field, CultureInfo cultureInfo)
        {
            using (new CultureScope(cultureInfo))
            {
                return GetValue(field);
            }
        }
    }
}