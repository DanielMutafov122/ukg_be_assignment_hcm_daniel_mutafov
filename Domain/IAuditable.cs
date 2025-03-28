namespace Domain;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime LastUpdatedAt { get; set; }
}
