using System;
using System.Globalization;
using System.Threading;

namespace ROP.ApiExtensions.Translations.Language
{
    /// <summary>
    /// Represents a scope in which the current culture and UI culture are set to a specific <see cref="CultureInfo"/>.
    /// </summary>
    public class CultureScope : IDisposable
    {
        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUICulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureScope"/> class.
        /// </summary>
        /// <param name="culture"></param>
        public CultureScope(CultureInfo culture)
        {
            _originalCulture = Thread.CurrentThread.CurrentCulture;
            _originalUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Restores the original culture and UI culture.
        /// </summary>
        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = _originalCulture;
            Thread.CurrentThread.CurrentUICulture = _originalUICulture;
        }
    }
}