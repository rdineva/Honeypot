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
            CreateMap<RegisterViewModel, HoneypotUser>(MemberList.None);
            CreateMap<HoneypotUser, ProfileViewModel>(MemberList.None);

            CreateMap<CreateBookViewModel, Book>(MemberList.None);
            CreateMap<Book, BookDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.AuthorName, opt => opt
                    .MapFrom(src => src.Author.FirstName + " " + src.Author.LastName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.AverageRating()));

            CreateMap<CreateAuthorViewModel, Author>(MemberList.None);
            CreateMap<Author, AuthorDetailsViewModel>(MemberList.None);

            CreateMap<CreateQuoteViewModel, Quote>(MemberList.None);
            CreateMap<UserQuote, MyLikedQuotesViewModel>(MemberList.None)
                .ForMember(dest => dest.Quotes, opt => opt.MapFrom(src => src.Quote));

            CreateMap<Quote, QuoteDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.AuthorName, opt => opt
                    .MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));

            CreateMap<CreateBookshelfViewModel, Bookshelf>(MemberList.None);
            CreateMap<Bookshelf, BookshelfDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.UserNickname, opt => opt
                    .MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        }
    }
}