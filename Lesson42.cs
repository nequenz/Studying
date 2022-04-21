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
                _bookStorage.ShowItems();
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

    abstract class Storage
    {
        protected List<Item> ItemList = new List<Item>();

        public bool Add(Item item)
        {
            if (item != null)
            {
                ItemList.Add(item);

                return true;
            }

            return false;
        }

        public bool RemoveByID(int id)
        {
            Item entity = GetItemByID(id);

            if (entity != null)
            {
                ItemList.Remove(entity);

                return true;
            }

            return false;
        }

        public virtual void ShowItems()
        {
            foreach (Item entity in ItemList)
            {
                Console.WriteLine("Объект типа " + entity.ToString() + " с ид " + entity.ID);
            }
        }

        private Item GetItemByID(int id)
        {
            foreach (Item entity in ItemList)
            {
                if (entity.ID == id)
                {
                    return entity;
                }
            }

            return null;
        }
    }

    abstract class Item
    {
        private static int NextID = 0;

        public int ID { get; private set; } = 0;

        public Item()
        {
            ID = NextID;
            NextID++;
        }
    }

    class Book : Item
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

    class BookStorage : Storage
    {
        private List<Book> _sortedBooks = new List<Book>();

        public void ClearSortedBooks() => _sortedBooks.Clear();

        public void UpdateSortedBooks()
        {
            ClearSortedBooks();

            foreach (Item item in ItemList)
            {
                _sortedBooks.Add(item as Book);
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

        public override void ShowItems()
        {
            Console.WriteLine("--Полный список книг--");

            foreach (Book book in ItemList)
            {
                Console.WriteLine("Книга под названием '" + book.Title + "', автор " + book.Author + " изданная в " + book.ReleaseDate.ToShortDateString());
            }
        }

        public bool Remove(string title, string author)
        {
            foreach (Book book in ItemList)
            {
                if (book.Title == title && book.Author == author)
                {
                    ItemList.Remove(book);

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
