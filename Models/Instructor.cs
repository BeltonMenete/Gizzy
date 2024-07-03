using System.ComponentModel.DataAnnotations;

namespace gizzy;

public class Instructor
{
    public Guid Id { get; set; }

    [MaxLength(30, ErrorMessage = "No more than 30 characters")]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name cannot contain special characters")]
    public required string FirstName { get; set; }
    [MaxLength(30, ErrorMessage = "No more than 30 characters")]
    [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name cannot contain special characters")]
    public required string LastName { get; set; }
}
