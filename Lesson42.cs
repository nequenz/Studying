using System;
using System.Collections.Generic;

namespace Tired
{
    class Program
    {
        static void Main(string[] args)
        {
            BookStorage bookStorage = new BookStorage();

            bookStorage.Add(new Book("Реквием #вторая партия", new DateTime(2011, 11, 5), "Джейми Макгвайр"));
            bookStorage.Add(new Book("Список запретных дел #первая партия", new DateTime(2013, 3, 23), "Коэти Зан"));
            bookStorage.Add(new Book("Игра в невидимку #вторая партия", new DateTime(1993, 5, 17), "Роберт Стайн"));
            bookStorage.Add(new Book("Омен #первая партия", new DateTime(1976, 2, 6), "Дэвид Зельцер"));
            bookStorage.UpdateSortedBooks();

            UserInputMenu menu = new UserInputMenu(bookStorage);

            menu.Update();
        }
    }

    class UserInputMenu
    {
        private const string WordToExit = "exit";
        private const string WordToSortByTitle = "title";
        private const string WordToSortByAuthor = "author";
        private const string WordToSortByDate = "date";
        private const string WordToAddBook = "add";
        private const string WordToRemoveBook = "remove";
        private const string WordToClearSort = "clear";

        private string _wordToRead;
        private BookStorage _bookStorage;

        public UserInputMenu(BookStorage bookStorage)
        {
            _bookStorage = bookStorage;
        }

        public void Update()
        {
            while (_bookStorage != null && _wordToRead != WordToExit)
            {
                ShowUsefulInfo();
                _bookStorage.ShowAllBooks();
                _bookStorage.ShowSortedBooks();

                Console.Write("\nВвод:");

                _wordToRead = Console.ReadLine();

                Console.WriteLine();

                switch (_wordToRead)
                {
                    case WordToExit:
                        break;

                    case WordToSortByAuthor:
                        SortByAuthorByUser();
                        break;

                    case WordToSortByTitle:
                        SortByTitleByUser();
                        break;

                    case WordToSortByDate:
                        SortByDateByUser();
                        break;

                    case WordToAddBook:
                        AddBookByUser();
                        break;

                    case WordToRemoveBook:
                        RemoveBookByUser();
                        break;

                    case WordToClearSort:
                        _bookStorage.UpdateSortedBooks();
                        break;
                }

                Console.Clear();
            }
        }

        public void ShowUsefulInfo()
        {
            Console.WriteLine("Введите команды для работы с хранилищем книг");
            Console.WriteLine("Введите exit для выхода");
            Console.WriteLine("Введите title для поиска по названию");
            Console.WriteLine("Введите author для поиска по автору");
            Console.WriteLine("Введите date для поиска по дате");
            Console.WriteLine("Введите add чтобы добавить книгу");
            Console.WriteLine("Введите remove чтобы удалить книгу по названию");
            Console.WriteLine("Введите clear чтобы очистить сортировку");
            Console.WriteLine();
        }

        public void SortByAuthorByUser()
        {
            Console.Write("Введите ФИО автора:");

            _bookStorage.SortByAuthor(Console.ReadLine());
        }

        public void SortByTitleByUser()
        {
            Console.Write("Введите название книги:");

            _bookStorage.SortByTitle(Console.ReadLine());
        }

        public void SortByDateByUser()
        {
            Console.Write("Введите дату публикации книги:");

            if (DateTime.TryParse(Console.ReadLine(), out DateTime date) == true)
            {
                _bookStorage.SortByDate(date);
            }
            else
            {
                Console.WriteLine("Ошибка ввода...");
            }
        }

        public void ReadStringBookParams(out string title, out string author)
        {
            Console.Write("Введите название книги:");

            title = Console.ReadLine();

            Console.Write("Введите автора книги:");

            author = Console.ReadLine();
        }

        public void AddBookByUser()
        {
            ReadStringBookParams(out string bookName, out string bookAuthor);
            Console.Write("Введите дату публикации книги:");

            if (DateTime.TryParse(Console.ReadLine(), out DateTime date) == true && bookName != "" && bookAuthor != "")
            {
                _bookStorage.Add(new Book(bookName, date, bookAuthor));
            }
            else
            {
                Console.WriteLine("Ошибка ввода...");
            }
        }

        public void RemoveBookByUser()
        {
            ReadStringBookParams(out string bookName, out string bookAuthor);

            if (_bookStorage.Remove(bookName, bookAuthor) == true)
            {
                Console.WriteLine("Книги удалена с хранилища!");
            }
            else
            {
                Console.WriteLine("Книгу, которую вы хотите удалить, не существует...");
            }
        }
    }

    class Book 
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public DateTime ReleaseDate { get; private set; }

        public Book(string title, DateTime date, string author = "безымянный автор")
        {
            Title = title;
            Author = author;
            ReleaseDate = date;
        }

    }

    class BookStorage 
    {
        private List<Book> _bookList = new List<Book>();
        private List<Book> _sortedBooks = new List<Book>();

        public void ClearSortedBooks() => _sortedBooks.Clear();

        public void UpdateSortedBooks()
        {
            ClearSortedBooks();

            foreach (Book book in _bookList)
            {
                _sortedBooks.Add(book);
            }
        }

        public void ShowSortedBooks()
        {
            Console.WriteLine("--Отсортированный список книг--");

            foreach (Book book in _sortedBooks)
            {
                Console.WriteLine("Книга под названием '" + book.Title + "', автор " + book.Author + " изданная в " + book.ReleaseDate.ToShortDateString());
            }
        }

        public void ShowAllBooks()
        {
            Console.WriteLine("--Полный список книг--");

            foreach (Book book in _bookList)
            {
                Console.WriteLine("Книга под названием '" + book.Title + "', автор " + book.Author + " изданная в " + book.ReleaseDate.ToShortDateString());
            }
        }

        public bool Add(Book book)
        {
            if (book != null)
            {
                _bookList.Add(book);

                return true;
            }

            return false;
        }

        public bool Remove(string title, string author)
        {
            foreach (Book book in _bookList)
            {
                if (book.Title == title && book.Author == author)
                {
                    _bookList.Remove(book);

                    return true;
                }
            }

            return false;
        }

        public void SortByTitle(string title)
        {
            _sortedBooks = _sortedBooks.FindAll(b => b.Title.Contains(title));
        }

        public void SortByAuthor(string author)
        {
            _sortedBooks = _sortedBooks.FindAll(b => b.Author.Contains(author));
        }

        public void SortByDate(DateTime date)
        {
            _sortedBooks = _sortedBooks.FindAll(b => b.ReleaseDate == date);
        }
    }
}
