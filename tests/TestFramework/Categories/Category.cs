namespace TestFramework.Categories;

public class Category
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public bool DeletionMark { get; set; }

    public Guid ExternalId { get; set; }

    public static Category Create(Guid id, Guid? parentId = null)
    {
        return new Category()
        {
            Name = id.ToString(),
            DeletionMark = false,
            ExternalId = id,
            ParentId = parentId
        };
    }
}