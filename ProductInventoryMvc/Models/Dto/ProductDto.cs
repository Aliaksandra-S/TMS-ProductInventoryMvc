namespace ProductInventoryMvc.Models.Dto;
public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Quantity { get; set; }

    public double Weight { get; set; }

    public string InventoryName { get; set; }
}
