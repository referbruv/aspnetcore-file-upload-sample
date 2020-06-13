using Microsoft.Extensions.Caching.Memory;
using ReaderStore.WebApp.Models.Entities;
using System;
using System.Linq;

namespace ReaderStore.WebApp.Providers.Repositories
{
    public class CachedReadersRepository : IReadersRepository
    {
        private readonly IReadersRepository repo;
        private readonly IMemoryCache cache;

        public CachedReadersRepository(IReadersRepository repo, IMemoryCache cache)
        {
            this.repo = repo;
            this.cache = cache;
        }

        public Reader AddReader(Reader reader)
        {
            var result = repo.AddReader(reader);

            return cache.Set(result.Id, result);
        }

        public Reader GetReader(Guid id)
        {
            Reader reader;

            if (!cache.TryGetValue(id, out reader))
            {
                var record = repo.GetReader(id);

                if (record != null)
                {
                    return cache.Set(record.Id, record);
                }
            }
            
            return reader;
        }

        public IQueryable<Reader> Readers => repo.Readers;
    }
}