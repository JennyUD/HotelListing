using HotelListing.Data;
using HotelListing.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _Context;
        private IGenericRepository<Country> _Countries;
        private  IGenericRepository<Hotel> _Hotels;

        public UnitOfWork (DatabaseContext Context)
        {
            _Context = Context;
          
        }
        public IGenericRepository<Country> Countries => _Countries ??= new GenericRepository<Country>(_Context);
        //"??" means if it is null return a new instance object of Generic Repository of type Country base on database "_Context"

        public IGenericRepository<Hotel> Hotels => _Hotels ??= new GenericRepository<Hotel>(_Context);

        public void Dispose()
        { 
            _Context.Dispose();
            GC.SuppressFinalize(this); // GC; Garbage collector for freeing up the memory
        }

        public async Task Save()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
