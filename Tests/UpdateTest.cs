using ActDigital.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ActDigital.Tests.Controllers
{
    public class EmployeeControllerUpdateTest
    {
        private readonly Mock<EmployeeContext> _mockContext;
        private readonly EmployeeController _controller;

        public EmployeeControllerUpdateTest()
        {
            _mockContext = new Mock<EmployeeContext>();
            _controller = new EmployeeController(_mockContext.Object);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            _mockContext.Setup(m => m.Employees.FindAsync(employeeId)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.UpdateEmployee(employeeId, new Employee());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNoContent_WhenEmployeeIsUpdated()
        {
            // Arrange
            var employeeId = 1;
            var existingEmployee = new Employee { Id = employeeId };
            var updatedEmployee = new Employee
            {
                Id = employeeId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "123456789",
                Phone = "1234567890",
                ManagerName = "Jane Doe",
                Password = "password"
            };

            _mockContext.Setup(m => m.Employees.FindAsync(employeeId)).ReturnsAsync(existingEmployee);
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateEmployee(employeeId, updatedEmployee);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(updatedEmployee.FirstName, existingEmployee.FirstName);
            Assert.Equal(updatedEmployee.LastName, existingEmployee.LastName);
            Assert.Equal(updatedEmployee.Email, existingEmployee.Email);
            Assert.Equal(updatedEmployee.CPF, existingEmployee.CPF);
            Assert.Equal(updatedEmployee.Phone, existingEmployee.Phone);
            Assert.Equal(updatedEmployee.ManagerName, existingEmployee.ManagerName);
            Assert.Equal(updatedEmployee.Password, existingEmployee.Password);
        }
    }
}