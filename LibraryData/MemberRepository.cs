using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace LibraryData
{
    public class MemberRepository
    {
        private string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = MyDB; Integrated Security = True;";

        public void AddMember(string name)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO Members(Name) VALUES(@name)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<Member> GetAllMembers()
        {
            List<Member> list = new List<Member>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM Members";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Member m = new Member();
                m.MemberId = Convert.ToInt32(reader["MemberId"]);
                m.Name = reader["Name"].ToString();
                list.Add(m);
            }
            reader.Close();
            conn.Close();
            return list;
        }
    }
}