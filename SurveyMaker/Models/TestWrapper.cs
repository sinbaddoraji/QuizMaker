namespace TestMaker.Models;

public class TestWrapper
{
    public Guid TestId { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? UserId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public List<Question> Questions { get; set; }
}