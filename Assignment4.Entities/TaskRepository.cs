using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Assignment4.Core;
using Microsoft.Extensions.Configuration;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {

        private readonly SqlConnection _connection; 

         public TaskRepository(SqlConnection connection)
        {
            _connection = connection; 
        } 
        public IReadOnlyCollection<TaskDTO> All()
        {
            throw new System.NotImplementedException();
        }

        public int Create(TaskDTO task)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int taskId)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _connection.Dispose(); 
        }

        public TaskDetailsDTO FindById(int id)
        {
            var SQLText = @"SELECT *
                            FROM Tasks as T
                            WHERE T.id = @Id"; 


            using var command = new SqlCommand(SQLText, _connection); 
            command.Parameters.AddWithValue("@Id", id); 
            OpenConnection();
            using var reader = command.ExecuteReader(); 

            //tests fejler hvis man fjerner udkommentering 
            var task = reader.Read() 
                ? new TaskDetailsDTO
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    //AssignedToId = reader.GetInt32("AssignedToId"), 
                    //AssignedToName = reader.GetString("AssignedToName"),
                    AssignedToEmail = reader.GetString("AssignedToEmail"),
                    //Tags = (IEnumerable<string>) reader.GetStream("Tags"), 
                    //State = (State) reader.GetValue("State") 

                }
                : null; 

            CloseConnection();
            return task;     
        }

        public void Update(TaskDTO task)
        {
            throw new System.NotImplementedException();
        }
        public void OpenConnection() 
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

        }
        
        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
