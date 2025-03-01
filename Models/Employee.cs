using System.ComponentModel.DataAnnotations;

public class Employee
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string CPF { get; set; }
    public string Phone { get; set; }
    public string ManagerName { get; set; }
    [Required]
    public string Password { get; set; }
}
