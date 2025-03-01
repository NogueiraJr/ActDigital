using ActDigital.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ActDigital.Tests.Controllers
{
    public class EmployeeControllerGetAllTest
    {
        private readonly Mock<EmployeeContext> _mockContext;
        private readonly EmployeeController _controller;

        public EmployeeControllerGetAllTest()
        {
            _mockContext = new Mock<EmployeeContext>();
            _controller = new EmployeeController(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsOkResult_WithListOfEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Employee>>();
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.Provider);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.Expression);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.ElementType);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

            _mockContext.Setup(c => c.Employees).Returns(mockSet.Object);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnEmployees = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Equal(2, returnEmployees.Count);
        }
    }
}