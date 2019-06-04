using System;
using System.Collections.Generic;
using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Services
{
    public class BookService : BaseService, IBookService
    {
        public BookService(HoneypotDbContext context) 
            : base(context)
        {
        }

        public Book GeBookById(int id)
        {
            var book = this.context
                .Books
                .Include(x => x.Author)
                .Include(x => x.Quotes)
                .Include(x => x.Ratings)
                .FirstOrDefault(x => x.Id == id);

            return book;
        }

        public bool BookTitleExists(string title, string authorFirstName, string authorLastName)
        {
            var book = this.context
                .Books
                .FirstOrDefault(x => x.Title == title 
                                  && x.Author.FirstName == authorFirstName
                                  && x.Author.LastName == authorLastName);

            var bookExists = (book != null);
            return bookExists;
        }

        public List<Book> GetAllBooks()
        {
            var books = this.context.Books.ToList();
            return books;
        }

        public Genre[] GetAllGenres()
        {
            Genre[] genres = (Genre[])Enum.GetValues(typeof(Genre));
            return genres;
        }
    }
}
