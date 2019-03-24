﻿using Honeypot.Data;
using Honeypot.Models;
using System.Collections.Generic;
using System.Linq;

namespace Honeypot.Services
{
    public class HoneypotUsersBookshelvesService : HoneypotUsersService
    {
        public HoneypotUsersBookshelvesService(HoneypotDbContext context) : base(context)
        {
        }

        public List<Bookshelf> GetUsersBookshelves(string userId) =>
            this.Context.Bookshelves.Where(x => x.UserId == userId).ToList();

        public bool CheckIfBookIsInBookshelf(int bookId, int bookshelfId) =>
            this.Context.BooksBookshelves.Any(x => x.BookId == bookId && x.BookshelfId == bookshelfId);
    }
}