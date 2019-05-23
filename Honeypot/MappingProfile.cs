using AutoMapper;
using Honeypot.Models;
using Honeypot.ViewModels.Account;
using Honeypot.ViewModels.Author;
using Honeypot.ViewModels.Book;
using Honeypot.ViewModels.Quote;

namespace Honeypot
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, HoneypotUser>(MemberList.None);
            CreateMap<HoneypotUser, ProfileViewModel>(MemberList.None);

            CreateMap<Book, BookDetailsViewModel>(MemberList.None)
                .ForMember(dest => dest.AuthorName, opt => opt
                    .MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
            CreateMap<CreateBookViewModel, Book>(MemberList.None);

            CreateMap<CreateAuthorViewModel, Author>(MemberList.None);
            CreateMap<Author, AuthorDetailsViewModel>(MemberList.None);

            CreateMap<CreateQuoteViewModel, Quote>(MemberList.None);
            CreateMap<Quote, QuoteDetailsViewModel>(MemberList.None);
        }
    }
}