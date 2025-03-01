using ActDigital.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ActDigital.Tests.Controllers
{
    public class EmployeeControllerGetByIdTest
    {
        private readonly Mock<EmployeeContext> _mockContext;
        private readonly EmployeeController _controller;

        public EmployeeControllerGetByIdTest()
        {
            _mockContext = new Mock<EmployeeContext>();
            _controller = new EmployeeController(_mockContext.Object);
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsOkResult_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, FirstName = "John", LastName = "Doe" };
            _mockContext.Setup(c => c.Employees.FindAsync(employeeId)).ReturnsAsync(employee);

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployee = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employeeId, returnedEmployee.Id);
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsNotFoundResult_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            _mockContext.Setup(c => c.Employees.FindAsync(employeeId)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}