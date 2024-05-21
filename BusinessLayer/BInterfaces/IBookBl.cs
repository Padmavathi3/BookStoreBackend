using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BInterfaces
{
    public interface IBookBl
    {
        //AddBook
        public Task<int> AddBook(BookRequest requestDto);

        //Get
        public Task<IEnumerable<Object>> GetAllBooks();
    }
}
