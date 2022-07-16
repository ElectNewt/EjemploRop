using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ROP.ApiExtensions.Translations.Serializers;

namespace ROP.ApiExtensions.Translations
{
    public static class RopTranslationsExtensions
    {
        public static void AddTranslation<TTranslationType>(this JsonSerializerOptions options,
            IHttpContextAccessor httpContextAccessor)
        {
            options.Converters.Add(BuildErrorDtoSerializer<TTranslationType>(httpContextAccessor));
        }

        public static void AddTranslation<TTranslationType>(this JsonSerializerOptions options,
            IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            options.AddTranslation<TTranslationType>(serviceProvider);
        }

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