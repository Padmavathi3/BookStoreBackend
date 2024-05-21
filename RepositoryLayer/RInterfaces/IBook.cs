using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RInterfaces
{
    public interface IBook
    { 
   
        //AddBook
        public Task<int> AddBook(BookEntity re_var);

        //Get
        public Task<IEnumerable<Object>> GetAllBooks();
    }
}

