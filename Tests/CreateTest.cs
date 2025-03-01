using ActDigital.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ActDigital.Tests.Controllers
{
    public class EmployeeControllerCreateTest
    {
        private readonly Mock<EmployeeContext> _mockContext;
        private readonly EmployeeController _controller;

        public EmployeeControllerCreateTest()
        {
            _mockContext = new Mock<EmployeeContext>();
            _controller = new EmployeeController(_mockContext.Object);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678901",
                Phone = "1234567890",
                ManagerName = "Jane Smith",
                Password = "password"
            };

            _mockContext.Setup(m => m.Employees.Add(It.IsAny<Employee>())).Verifiable();
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateEmployee(employee);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(EmployeeController.GetEmployeeById), createdAtActionResult.ActionName);
            Assert.Equal(employee.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(employee, createdAtActionResult.Value);
        }

        [Fact]
        public async Task CreateEmployee_AddsEmployeeToContext()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678901",
                Phone = "1234567890",
                ManagerName = "Jane Smith",
                Password = "password"
            };

            _mockContext.Setup(m => m.Employees.Add(It.IsAny<Employee>())).Verifiable();
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _controller.CreateEmployee(employee);

            // Assert
            _mockContext.Verify(m => m.Employees.Add(It.Is<Employee>(e => e == employee)), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}