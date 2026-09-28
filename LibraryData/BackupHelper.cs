using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LibraryData
{
    public class BackupHelper
    {
        public static void SaveBooksToJson(List<Book> books, string filePath)
        {
            string json = JsonSerializer.Serialize(books);
            File.WriteAllText(filePath, json);
        }

        public static List<Book> ReadBooksFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Book>();
            }

            string json = File.ReadAllText(filePath);
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(json);
            return books;
        }
    }
}