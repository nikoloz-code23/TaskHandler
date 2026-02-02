using System;
using TaskTracker.Enums;

namespace TaskTracker.Types;

public record TodoTask
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}