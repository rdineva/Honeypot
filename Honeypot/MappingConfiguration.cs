using AutoMapper;
using Honeypot.Models;
using Honeypot.ViewModels;
using Honeypot.ViewModels.Account;
using Honeypot.ViewModels.Author;
using Honeypot.ViewModels.Book;
using Honeypot.ViewModels.Quote;
using CreateViewModel = Honeypot.ViewModels.Quote.CreateViewModel;

namespace Honeypot
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<RegisterViewModel, HoneypotUser>();
            CreateMap<HoneypotUser, ProfileViewModel>();
            CreateMap<Book, BookDetailsViewModel>();
            //.ForMember(x => x.Author, y => y.MapFrom(z => z.Author));

            CreateMap<ViewModels.Author.CreateViewModel, Author>();
            CreateMap<Author, AuthorDetailsViewModel>();
            CreateMap<CreateViewModel, Quote>();
            CreateMap<Quote, DetailsViewModel>();
        }
    }
}
