# Honeypot
## Overview
Honeypot is an online web application project for book readers. Users can view books, quotes and authors. They can also create bookshelves and delete them, rate books and add books to their own bookshelves and delete them. Admins also have the ability to create new authors, books and quotes.

## Structure
There are five main components of the system:
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
  * *Models* - all the model classes used in the application
    * `Author`, `Book`, `BookBookshelf`, `Bookshelf`, `HoneypotUser`, `Quote`, `Rating`, `Role`, `UserQuote`
* **Honeypot.Services** contains helper services for the application
  * *Abstractions*
  * *Contracts* - keeps the services interfaces
  * *Services* - classes with business logic helper methods 
    * `AuthorService`, `BookService`, `BookshelfService`, `QuoteServce`, `RatingService`, `UserService`
* **Honeypot.Tests** contains all the unit tests for the application
  * *Abstractions*
    * `BaseTest` for initializing DB context and providing Test classes with methods that clear and also fill the In Memory Database with objects
    * `BaseTestFixture` for creating and configuring the In Memory Database used for unit testing
  * *Constants*
  * *Tests* contains test classes for the services in the application 
    * `AccountServiceTests`, `AuthorServiceTests`, `BookServiceTests`, `BookshelfServiceTests`, `QuoteServiceTests`, `RatingServiceTests`
