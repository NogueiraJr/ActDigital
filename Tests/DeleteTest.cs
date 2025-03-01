using ActDigital.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ActDigital.Tests.Controllers
{
    public class EmployeeControllerDeleteTest
    {
        private readonly Mock<EmployeeContext> _mockContext;
        private readonly EmployeeController _controller;

        public EmployeeControllerDeleteTest()
        {
            _mockContext = new Mock<EmployeeContext>();
            _controller = new EmployeeController(_mockContext.Object);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 1;
            _mockContext.Setup(c => c.Employees.FindAsync(employeeId)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsOk_WhenEmployeeExists()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee { Id = employeeId };
            _mockContext.Setup(c => c.Employees.FindAsync(employeeId)).ReturnsAsync(employee);
            _mockContext.Setup(c => c.Employees.Remove(employee));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockContext.Verify(c => c.Employees.Remove(employee), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
    }
}