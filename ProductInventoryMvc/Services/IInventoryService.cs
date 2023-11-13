using ProductInventoryMvc.Models;
using ProductInventoryMvc.Models.Dto;

namespace ProductInventoryMvc.Services;
public interface IInventoryService
{
    InventoryListDto GetInventories();

    CommandResultModel AddInventory(InventoryDto inventory);

    CommandResultModel DeleteInventory(InventoryDto inventoryName);

    CommandResultModel EditProduct(ProductDto product);

    ProductListDto GetProducts(InventoryDto inventoryName);

    CommandResultModel AddProduct(string inventoryName, ProductDto product);

    CommandResultModel DeleteProduct(string inventoryName, DefineProductDto product);

}
