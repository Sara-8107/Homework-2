using System;
using Microsoft.Data.SqlClient;

namespace LibraryData
{
    public class LibraryService
    {
        private string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = MyDB; Integrated Security = True;";

        public bool IssueBook(int bookId, int memberId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                string checkStockQuery = "SELECT Stock FROM Books WHERE BookId = @bookId";
                SqlCommand cmdCheck = new SqlCommand(checkStockQuery, conn, tran);
                cmdCheck.Parameters.AddWithValue("bookId", bookId);

                object result = cmdCheck.ExecuteScalar();
                if (result == null)
                {
                    tran.Rollback();
                    conn.Close();
                    return false;
                }

                int stock = Convert.ToInt32(result);
                if (stock <= 0)
                {
                    tran.Rollback();
                    conn.Close();
                    return false;
                }

                string updateStockQuery = "UPDATE Books SET Stock = Stock - 1 WHERE BookId = @bookId";
                SqlCommand cmdUpdate = new SqlCommand(updateStockQuery, conn, tran);
                cmdUpdate.Parameters.AddWithValue("bookId", bookId);
                cmdUpdate.ExecuteNonQuery();

                string insertIssueQuery = "INSERT INTO IssuedBooks(BookId, MemberId, IssueDate, ReturnDate) VALUES(@bookId, @memberId, @issueDate, NULL)";
                SqlCommand cmdInsert = new SqlCommand(insertIssueQuery, conn, tran);
                cmdInsert.Parameters.AddWithValue("bookId", bookId);
                cmdInsert.Parameters.AddWithValue("memberId", memberId);
                cmdInsert.Parameters.AddWithValue("issueDate", DateTime.Now);
                cmdInsert.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                conn.Close();
                return false;
            }
        }

        public bool ReturnBook(int issueId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                string checkIssueQuery = "SELECT BookId FROM IssuedBooks WHERE IssueId = @issueId AND ReturnDate IS NULL";
                SqlCommand cmdCheck = new SqlCommand(checkIssueQuery, conn, tran);
                cmdCheck.Parameters.AddWithValue("issueId", issueId);

                object result = cmdCheck.ExecuteScalar();
                if (result == null)
                {
                    tran.Rollback();
                    conn.Close();
                    return false;
                }

                int bookId = Convert.ToInt32(result);

                string updateIssueQuery = "UPDATE IssuedBooks SET ReturnDate = @returnDate WHERE IssueId = @issueId";
                SqlCommand cmdUpdateIssue = new SqlCommand(updateIssueQuery, conn, tran);
                cmdUpdateIssue.Parameters.AddWithValue("returnDate", DateTime.Now);
                cmdUpdateIssue.Parameters.AddWithValue("issueId", issueId);
                cmdUpdateIssue.ExecuteNonQuery();

                string updateStockQuery = "UPDATE Books SET Stock = Stock + 1 WHERE BookId = @bookId";
                SqlCommand cmdUpdateStock = new SqlCommand(updateStockQuery, conn, tran);
                cmdUpdateStock.Parameters.AddWithValue("bookId", bookId);
                cmdUpdateStock.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                conn.Close();
                return false;
            }
        }
    }
}