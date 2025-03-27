using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApiWithCaching.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse?>
        where TRequest : IRequest<TResponse>
    {
        private readonly IMemoryCache _cache;

        public CachingBehavior(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
        {
            // Générer une clé unique basée sur le type de la requête et ses paramètres
            var cacheKey = request.GetType().Name + "_" + string.Join("_", request.GetType().GetProperties().Select(p => p.GetValue(request)?.ToString()));

            // Vérifier si la réponse est en cache
            if (_cache.TryGetValue(cacheKey, out TResponse? cachedResponse))
            {
                Log.Information($"CachingBehavior : Récupération des données (cacheKey : {cacheKey})");
                return cachedResponse;
            }

            // Exécuter la requête normalement si elle n'est pas en cache
            var response = await next();

            // Stocker le résultat en cache pour 10 minutes
            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
            Log.Information($"CachingBehavior : Création des données (cacheKey : {cacheKey})");
            return response;
        }
    }
}
