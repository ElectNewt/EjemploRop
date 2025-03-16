using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ROP.ApiExtensions.Translations.Serializers;

namespace ROP.ApiExtensions.Translations
{
    /// <summary>
    /// Extensions to add translations to the JsonSerializerOptions.
    /// </summary>
    public static class RopTranslationsExtensions
    {
        /// <summary>
        /// Allows to detect automatic translations (with resx files) in case you have more than one language supported
        /// please see https://github.com/ElectNewt/EjemploRop#error-translations for mor details; usage:
        /// services.AddControllers().AddJsonOptions(options =>{options.JsonSerializerOptions.AddTranslation&lt;TraduccionErrores&gt;(services);} );
        /// it uses the "Accept-Language" header to define the selected language.
        /// </summary>
        /// <param name="options">Serializer options</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <typeparam name="TTranslationType">Translation class that 'references' the .resx files</typeparam>
        public static void AddTranslation<TTranslationType>(this JsonSerializerOptions options,
            IHttpContextAccessor httpContextAccessor)
        {
            options.Converters.Add(BuildErrorDtoSerializer<TTranslationType>(httpContextAccessor));
        }

        /// <summary>
        /// Allows to detect automatic translations (with resx files) in case you have more than one language supported
        /// please see https://github.com/ElectNewt/EjemploRop#error-translations for mor details; usage:
        /// services.AddControllers().AddJsonOptions(options =>{options.JsonSerializerOptions.AddTranslation&lt;TraduccionErrores&gt;(services);} );
        /// it uses the "Accept-Language" header to define the selected language.
        /// </summary>
        /// <param name="options">Serializer options</param>
        /// <param name="services">Service collection</param>
        /// <typeparam name="TTranslationType">Translation class that 'references' the .resx files</typeparam>
        public static void AddTranslation<TTranslationType>(this JsonSerializerOptions options,
            IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            options.AddTranslation<TTranslationType>(serviceProvider);
        }

        /// <summary>
        /// Allows to detect automatic translations (with resx files) in case you have more than one language supported
        /// please see https://github.com/ElectNewt/EjemploRop#error-translations for mor details; usage:
        /// services.AddControllers().AddJsonOptions(options =>{options.JsonSerializerOptions.AddTranslation&lt;TraduccionErrores&gt;(services);} );
        /// it uses the "Accept-Language" header to define the selected language.
        /// </summary>
        /// <param name="options">Serializer options</param>
        /// <param name="serviceProvider">Service Provider</param>
        /// <typeparam name="TTranslationType">Translation class that 'references' the .resx files</typeparam>
        public static void AddTranslation<TTranslationType>(this JsonSerializerOptions options,
            IServiceProvider serviceProvider)
        {
            IHttpContextAccessor httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            options.Converters.Add(BuildErrorDtoSerializer<TTranslationType>(httpContextAccessor));
        }

        private static ErrorDtoSerializer<TTranslationType> BuildErrorDtoSerializer<TTranslationType>(
            IHttpContextAccessor httpContextAccessor)
        {
            return new ErrorDtoSerializer<TTranslationType>(httpContextAccessor);
        }
    }
}