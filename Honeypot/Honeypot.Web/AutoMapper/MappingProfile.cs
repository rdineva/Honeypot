using AutoMapper;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.ViewModels.Account;
using Honeypot.ViewModels.Author;
using Honeypot.ViewModels.Book;
using Honeypot.ViewModels.Bookshelf;
using Honeypot.ViewModels.Quote;

namespace Honeypot
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateAccountConfiguration();
            CreateBookConfiguration();
            CreateAuthorConfiguration();
            CreateQuoteConfiguration();
            CreateBookshelfConfiguration();
        }

        public void CreateAccountConfiguration()
        {
            CreateMap<RegisterViewModel, HoneypotUser>(MemberList.None);
            CreateMap<HoneypotUser, ProfileViewModel>(MemberList.None);
        }

        public void CreateBookConfiguration()
        {
            CreateMap<CreateBookViewModel, Book>(MemberList.None);
            CreateMap<Book, BookDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.AuthorName, opt => opt
                    .MapFrom(src => src.Author.FirstName + " " + src.Author.LastName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.AverageRating()));
        }

        public void CreateAuthorConfiguration()
        {
            CreateMap<CreateAuthorViewModel, Author>(MemberList.None);
            CreateMap<Author, AuthorDetailsViewModel>(MemberList.None);
        }

        public void CreateQuoteConfiguration()
        {
            CreateMap<CreateQuoteViewModel, Quote>(MemberList.None);
            CreateMap<UserQuote, MyLikedQuotesViewModel>(MemberList.None)
                .ForMember(dest => dest.Quotes, opt => opt.MapFrom(src => src.Quote));
            CreateMap<Quote, QuoteDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.AuthorName, opt => opt
                    .MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
        }

        public void CreateBookshelfConfiguration()
        {
            CreateMap<CreateBookshelfViewModel, Bookshelf>(MemberList.None);
            CreateMap<Bookshelf, BookshelfDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.UserNickname, opt => opt
                    .MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        }
    }
}