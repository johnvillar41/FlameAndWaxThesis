﻿using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task Add(EmployeeModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO EmployeesTable(FirstName,LastName,PhotoLink,BirthDate,HireDate,City)" +
                "VALUES(@FirstName,@LastName,@PhotoLink,@Bday,@HireDate,@City)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@FirstName", Data.FirstName);
            command.Parameters.AddWithValue("@LastName", Data.LastName);
            command.Parameters.AddWithValue("@PhotoLink", Data.PhotoLink);
            command.Parameters.AddWithValue("@Bday", Data.BirthDate);
            command.Parameters.AddWithValue("@HireDate", Data.HireDate);
            command.Parameters.AddWithValue("@City", Data.City);
            await command.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "DELETE FROM EmployeesTable WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<EmployeeModel> Fetch(int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM EmployeesTable WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new EmployeeModel
                {
                    EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    PhotoLink = reader["PhotoLink"].ToString(),
                    BirthDate = DateTime.Parse(reader["BirthDaye"].ToString()),
                    HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                    City = reader["City"].ToString()
                };
            }
            return null;
        }

        public async Task<IEnumerable<EmployeeModel>> FetchAll()
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();

            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM EmployeesTable";
            using SqlCommand command = new SqlCommand(queryString, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(
                        new EmployeeModel
                        {
                            EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            PhotoLink = reader["PhotoLink"].ToString(),
                            BirthDate = DateTime.Parse(reader["BirthDaye"].ToString()),
                            HireDate = DateTime.Parse(reader["HireDate"].ToString()),
                            City = reader["City"].ToString()
                        }
                    );
            }
            return employees;
        }

        public async Task<int> Login(EmployeeModel employee)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "SELECT * FROM EmployeesTable WHERE Username = @username AND Password = @password";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@username", employee.Username);
            command.Parameters.AddWithValue("@username", employee.Password);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return int.Parse(reader["EmployeeId"].ToString());
            }
            return -1;
        }

        public async Task ModifyEmployeeStatus(int employeeId, Constants.AccountStatus accountStatus)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE EmployeesTable SET Status = @status WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@status", nameof(accountStatus));
            command.Parameters.AddWithValue("@id", employeeId);
            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Updates the employee credentials excluding the PhotoLink property
        /// </summary>       
        public async Task Update(EmployeeModel data, int id)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE EmployeesTable SET FirstName = @firstName, LastName = @lastName, " +
                "BirthDate = @bday, HireDate = @hireDate, City = @city WHERE EmployeeId = @id";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@firstName", data.FirstName);
            command.Parameters.AddWithValue("@lastName", data.LastName);
            command.Parameters.AddWithValue("@bday", data.BirthDate);
            command.Parameters.AddWithValue("@hireDate", data.HireDate);
            command.Parameters.AddWithValue("@city", data.City);
            command.Parameters.AddWithValue("@id", data.EmployeeId);
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProfilePicture(string profileLink)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "UPDATE EmployeesTable SET PhotoLink = @link";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@firstName", profileLink);
            await command.ExecuteNonQueryAsync();
        }
    }
}
