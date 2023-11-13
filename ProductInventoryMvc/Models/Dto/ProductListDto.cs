namespace ProductInventoryMvc.Models.Dto;
public class ProductListDto
{
    public InventoryDto Inventory { get; set; }
    public ProductDto[] Products { get; set; }
}

