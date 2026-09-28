using LibraryData;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace LibraryApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookRepository bookRepo = new BookRepository();
            MemberRepository memberRepo = new MemberRepository();
            LibraryService libraryService = new LibraryService();

            while (true)
            {
                Console.WriteLine("\n--- LIBRARY MANAGEMENT SYSTEM ---");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Add Member");
                Console.WriteLine("4. View All Members");
                Console.WriteLine("5. Issue Book to Member (Transaction)");
                Console.WriteLine("6. Return Book (Transaction)");
                Console.WriteLine("7. Export Books to JSON");
                Console.WriteLine("8. Import Books from JSON");
                Console.WriteLine("9. Exit");
                Console.Write("Enter option: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Enter Title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter Author: ");
                    string author = Console.ReadLine();
                    Console.Write("Enter Stock: ");
                    int stock = int.Parse(Console.ReadLine());

                    bookRepo.AddBook(title, author, stock);
                    Console.WriteLine("Book added successfully.");
                }
                else if (choice == "2")
                {
                    List<Book> books = bookRepo.GetAllBooks();
                    Console.WriteLine("\n--- BOOKS ---");
                    foreach (Book b in books)
                    {
                        Console.WriteLine("ID: " + b.BookId + " | Title: " + b.Title + " | Author: " + b.Author + " | Stock: " + b.Stock);
                    }
                }
                else if (choice == "3")
                {
                    Console.Write("Enter Member Name: ");
                    string name = Console.ReadLine();

                    memberRepo.AddMember(name);
                    Console.WriteLine("Member added successfully.");
                }
                else if (choice == "4")
                {
                    List<Member> members = memberRepo.GetAllMembers();
                    Console.WriteLine("\n--- MEMBERS ---");
                    foreach (Member m in members)
                    {
                        Console.WriteLine("ID: " + m.MemberId + " | Name: " + m.Name);
                    }
                }
                else if (choice == "5")
                {
                    Console.Write("Enter Book ID: ");
                    int bookId = int.Parse(Console.ReadLine());
                    Console.Write("Enter Member ID: ");
                    int memberId = int.Parse(Console.ReadLine());

                    bool success = libraryService.IssueBook(bookId, memberId);
                    if (success)
                    {
                        Console.WriteLine("Book issued successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to issue book (Out of stock or invalid ID). Transaction rolled back.");
                    }
                }
                else if (choice == "6")
                {
                    Console.Write("Enter Issue ID: ");
                    int issueId = int.Parse(Console.ReadLine());

                    bool success = libraryService.ReturnBook(issueId);
                    if (success)
                    {
                        Console.WriteLine("Book returned successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to return book (Invalid Issue ID or already returned). Transaction rolled back.");
                    }
                }
                else if (choice == "7")
                {
                    List<Book> books = bookRepo.GetAllBooks();
                    BackupHelper.SaveBooksToJson(books, "books_backup.json");
                    Console.WriteLine("Exported books to books_backup.json");
                }
                else if (choice == "8")
                {
                    List<Book> books = BackupHelper.ReadBooksFromJson("books_backup.json");
                    Console.WriteLine("\n--- BOOKS FROM JSON BACKUP ---");
                    foreach (Book b in books)
                    {
                        Console.WriteLine("Title: " + b.Title + " | Stock: " + b.Stock);
                    }
                }
                else if (choice == "9")
                {
                    break;
                }
            }
        }
    }
}