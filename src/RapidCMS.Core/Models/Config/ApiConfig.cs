using System;
using System.Collections.Generic;
using RapidCMS.Core.Abstractions.Config;
using RapidCMS.Core.Exceptions;
using RapidCMS.Core.Extensions;

namespace RapidCMS.Core.Models.Config
{
    internal class ApiConfig : IApiConfig
    {
        internal bool AllowAnonymousUsage { get; set; } = false;
        internal Dictionary<string, Type> Collections = new Dictionary<string, Type>();

        public IApiConfig AllowAnonymousUser()
        {
            AllowAnonymousUsage = true;
            return this;
        }

        public IApiConfig RegisterRepository<TEntity, TRepository>(string collectionAlias)
        {
            if (collectionAlias != collectionAlias.ToUrlFriendlyString())
            {
                throw new ArgumentException($"Use lowercase, hyphened strings as alias for collections, '{collectionAlias.ToUrlFriendlyString()}' instead of '{collectionAlias}'.");
            }
            if (Collections.ContainsKey(collectionAlias))
            {
                throw new NotUniqueException(nameof(collectionAlias));
            }

            Collections[collectionAlias] = typeof(TRepository);

            return this;
        }
    }
}
