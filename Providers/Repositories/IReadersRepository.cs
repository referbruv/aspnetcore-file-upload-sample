using System;
using System.Collections.Generic;
using System.Linq;
using ReaderStore.WebApp.Models.Entities;

namespace ReaderStore.WebApp.Providers.Repositories
{
    public interface IReadersRepository
    {
        IQueryable<Reader> Readers { get; }
        Reader GetReader(Guid id);
        Reader AddReader(Reader reader);
    }
}