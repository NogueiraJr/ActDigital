using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EmployeeContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Create Employee
app.MapPost("/employees", async (EmployeeContext db, Employee employee) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/employees/{employee.Id}", employee);
})
.WithName("CreateEmployee")
.WithOpenApi();

// Read All Employees
app.MapGet("/employees", async (EmployeeContext db) =>
    await db.Employees.ToListAsync())
.WithName("GetAllEmployees")
.WithOpenApi();

// Read Employee by Id
app.MapGet("/employees/{id}", async (EmployeeContext db, int id) =>
    await db.Employees.FindAsync(id) is Employee employee
        ? Results.Ok(employee)
        : Results.NotFound())
.WithName("GetEmployeeById")
.WithOpenApi();

// Update Employee
app.MapPut("/employees/{id}", async (EmployeeContext db, int id, Employee employee) =>
{
    var existingEmployee = await db.Employees.FindAsync(id);
    if (existingEmployee is null) return Results.NotFound();
    
    existingEmployee.FirstName = employee.FirstName;
    existingEmployee.LastName = employee.LastName;
    existingEmployee.Email = employee.Email;
    existingEmployee.CPF = employee.CPF;
    existingEmployee.Phone = employee.Phone;
    existingEmployee.ManagerName = employee.ManagerName;
    existingEmployee.Password = employee.Password;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateEmployee")
.WithOpenApi();

// Delete Employee
app.MapDelete("/employees/{id}", async (EmployeeContext db, int id) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    
    db.Employees.Remove(employee);
    await db.SaveChangesAsync();
    return Results.Ok();
})
.WithName("DeleteEmployee")
.WithOpenApi();

app.Run();
