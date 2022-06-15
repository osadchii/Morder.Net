namespace Infrastructure.Models.Marketplaces.TaskContext;

public class RejectOrderContext
{
    public List<RejectOrderContextItem> Items { get; set; }

    public RejectOrderContext()
    {
        Items = new List<RejectOrderContextItem>();
    }

    public void Add(RejectOrderContextItem item)
    {
        Items.Add(item);
    }
}

public class RejectOrderContextItem
{
    public int ProductId { get; set; }
    public string Articul { get; set; } = null!;
    public string ItemIndex { get; set; } = null!;
    public decimal Count { get; set; }
}