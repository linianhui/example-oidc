using System.Collections.Generic;
using System.Linq;
using AspNetCore.Filters.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Web.OAuth2.Resources.Apis
{
    [Route("books")]
    public class BooksController : Controller
    {
        private static readonly List<Book> Books = new List<Book>
        {
            new Book{Id=1,Name="SICP"},
            new Book{Id=2,Name="lnh"}
        };

        [HttpGet(Name = "books")]
        [Permission("books.read")]
        public List<Book> Get()
        {
            return Books;
        }

        [HttpGet("{bookId}")]
        [Permission("book.read")]
        public Book Get(int bookId)
        {
            return Books.FirstOrDefault(_ => _.Id == bookId);
        }

        [HttpPost]
        [Permission("book.add")]
        public Book Post(Book book)
        {
            Books.Add(book);
            return book;
        }

        [HttpPut("{bookId}")]
        [Permission("book.edit")]
        public Book Put(int bookId, Book book)
        {
            var oldBook = Books.FirstOrDefault(_ => _.Id == book.Id);
            if (oldBook != null)
            {
                oldBook.Name = book.Name;
            }
            return oldBook;
        }

        [HttpDelete("{bookId}")]
        [Permission("book.delete")]
        public Book Delete(int bookId)
        {
            var book = Books.FirstOrDefault(_ => _.Id == bookId);
            if (book != null)
            {
                Books.Remove(book);
            }
            return book;
        }
    }
}
