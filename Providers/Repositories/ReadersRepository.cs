using System;
using System.Collections.Generic;
using System.Linq;
using ReaderStore.WebApp.Models.Entities;

namespace ReaderStore.WebApp.Providers.Repositories
{
    public class ReadersRepository : IReadersRepository
    {
        private List<Reader> _readers;

        public ReadersRepository()
        {
            _readers = new List<Reader>();
        }

        public Reader AddReader(Reader reader)
        {
            reader.Id = Guid.NewGuid();
            reader.AddedOn = DateTime.Now;
            _readers.Add(reader);
            return reader;
        }

        public Reader GetReader(Guid id)
        {
            return _readers.Where(x => x.Id == id).FirstOrDefault();
        }

        public IQueryable<Reader> Readers => _readers.AsQueryable();
    }
}