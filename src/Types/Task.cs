using System;
using TaskTracker.Enums;

namespace TaskTracker.Types;

public record TodoTask
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public TodoTaskStatus Status { get; set; }
    public string? CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }
}