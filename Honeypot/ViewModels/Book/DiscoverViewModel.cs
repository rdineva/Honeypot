using System.Collections.Generic;
using Honeypot.Models.Enums;

namespace Honeypot.ViewModels.Book
{
    public class DiscoverViewModel
    {
        public Dictionary<Genre, List<Models.Book>> BooksByGenre;
    }
}
