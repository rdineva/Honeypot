﻿using System;
using AutoMapper;
using Honeypot.Constants;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class RatingController : BaseController
    {
        private readonly IAccountService usersService;
        private readonly IRatingService ratingService;
        private readonly IBookService bookService;

        public RatingController(HoneypotDbContext context, IMapper mapper, IAccountService accountService, IRatingService ratingService, IBookService bookService)
            : base(context, mapper)
        {
            this.usersService = accountService;
            this.ratingService = ratingService;
            this.bookService = bookService;
        }

        [HttpPost]
        public IActionResult Rate(int stars, int bookId)
        {
            StarRating starRating = ValidateStars(stars);
            var book = this.bookService.GetBookById(bookId);
            if (ModelState.IsValid)
            {
                OnPostUserRateBook(bookId, starRating);
            }

            return RedirectToAction("Details", "Book", new { id = book.Id });
        }

        private void OnPostUserRateBook(int bookId, StarRating starRating)
        {
            var user = this.usersService.GetByUsername(this.User.Identity.Name);
            var userHasRatedBook = this.ratingService.HasUserRatedBook(user.Id, bookId);
            if (userHasRatedBook)
            {
                ChangeRating(user, bookId, starRating);
            }
            else
            {
                AddNewRating(user, bookId, starRating);
            }

            this.context.SaveChanges();
        }

        private void ChangeRating(HoneypotUser user, int bookId, StarRating starRating)
        {
            var rating = this.ratingService.GetUserBookRating(user.Id, bookId);
            rating.Stars = starRating;
        }

        private void AddNewRating(HoneypotUser user, int bookId, StarRating starRating)
        {
            var rating = new Rating()
            {
                Stars = starRating,
                UserId = user.Id,
                BookId = bookId
            };

            this.context.Ratings.Add(rating);
        }

        private StarRating ValidateStars(int stars)
        {
            var areStarsValid = Enum.TryParse<StarRating>(stars.ToString(), true, out StarRating starRating);
            var areStarsDefined = Enum.IsDefined(typeof(StarRating), starRating);
            if (!areStarsValid || !areStarsDefined)
            {
                var errorMessage = string.Format(ControllerConstants.InvalidRating, typeof(Book).Name);
                ModelState.AddModelError("Book", errorMessage);
            }

            return starRating;
        }

        private void ValidateBookExists(Book book)
        {
            if (book == null)
            {
                var errorMessage = string.Format(GeneralConstants.DoesntExist, typeof(Book).Name);
                ModelState.AddModelError("Book", errorMessage);
            }
        }
    }
}