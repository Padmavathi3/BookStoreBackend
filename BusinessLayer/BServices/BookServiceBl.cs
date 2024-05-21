using BusinessLayer.BInterfaces;
using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BServices
{
    public class BookServiceBl:IBookBl
    {
        private readonly IBook _book;
        public BookServiceBl(IBook book)
        {
            _book = book;
        }
        private BookEntity MapToEntity(BookRequest request)
        {
            return new BookEntity { BookName = request.BookName, BookImage = request.BookImage, AuthorName = request.AuthorName, Description = request.Description, Quantity = request.Quantity, Price = request.Price };
        }
        public Task<int> AddBook(BookRequest requestDto)
        {

            return _book.AddBook(MapToEntity(requestDto));
        }

        public Task<IEnumerable<Object>> GetAllBooks()
        {
            return _book.GetAllBooks();
        }
    }
}
