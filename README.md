# Honeypot
Honeypot is an online web application project for book readers. Users can view books, quotes and authors. They can also create bookshelves, rate books and add them to their own bookshelves. Moreover, Admins have the ability to create new authors, books and quotes.

## Overview
There are four components of the system:
* **Honeypot.Web** contains the core functionality of the project. 
  * *Attributes* for ViewModel data properties validation
  * *AutoMapper* for automatically mapping from user input ViewModels to Models and vice versa
  * *Constants* for keeping error messages, number constraints on model properties, etc.
  * *Controllers* for handling requests with the corresponding Class type
    * `AccountController`, `AuthorController`, `BookController`, `BookshelfController`, `HomeController`, `QuoteController` and `RatingController`  
  * *ViewModels* for partially presentng models from the database to the user
  * *Views* for the presentation layer
  * `Program` starts the application
  * `Startup` configures the aplication
* **Honeypot.Data** contains the database layer
  * *EntityConfiguration* contains the configuration for the relationships between the models and their keys
  * `ContextDbFactory`
  * `HoneypotDbContext`
* **Honeypot.Models** contains the model layer
  * *Contracts*
  * *Enums*
  * *Models*
    * `Author`, `Book`, `BookBookshelf`, `Bookshelf`, `HoneypotUser`, `Quote`, `Rating`, `Role`, `UserQuote`
* **Honeypot.Services** contains helper services for the application
  * *Abstractions*
  * *Contracts* 
  * *Services* 
    * `AuthorService`, `BookService`, `BookshelfService`, `QuoteServce`, `RatingService`, `UserService`
