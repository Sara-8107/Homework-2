using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LibraryData
{
    public class BookRepository
    {
        private string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = MyDB; Integrated Security = True;";

        public void AddBook(string title, string author, int stock)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO Books(Title, Author, Stock) VALUES(@title, @author, @stock)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("author", author);
            cmd.Parameters.AddWithValue("stock", stock);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<Book> GetAllBooks()
        {
            List<Book> list = new List<Book>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM Books";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Book b = new Book();
                b.BookId = Convert.ToInt32(reader["BookId"]);
                b.Title = reader["Title"].ToString();
                b.Author = reader["Author"].ToString();
                b.Stock = Convert.ToInt32(reader["Stock"]);
                list.Add(b);
            }
            reader.Close();
            conn.Close();
            return list;
        }

        public void UpdateStock(int bookId, int newStock)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE Books SET Stock = @stock WHERE BookId = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("stock", newStock);
            cmd.Parameters.AddWithValue("id", bookId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void RemoveBook(int bookId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "DELETE FROM Books WHERE BookId = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", bookId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}