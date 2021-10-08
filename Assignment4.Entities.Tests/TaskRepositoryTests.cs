using System;
using Assignment4.Core;
using Xunit;
using XUnit.Project.Attributes;
using System.Data.SqlClient; 



namespace Assignment4.Entities.Tests
{
    [TestCaseOrderer("XUnit.Project.Orderers.PriorityOrderer", "XUnit.Project")]
    public class TaskRepositoryTests
    {   
        [Fact, TestPriority(5)]
        public void FindById_given_1_returns_Java_task() 
        {

            /* //TaskRepository tr = new TaskRepository(new SqlConnection("Server=localhost;Database=Kanban;User Id=sa;Password=Test1234")); 
    
            var actualResult = tr.FindById(1).Title; 

            var expectedResult = "Java";

            Assert.Equal(expectedResult, actualResult);  */

        }
    }
}
