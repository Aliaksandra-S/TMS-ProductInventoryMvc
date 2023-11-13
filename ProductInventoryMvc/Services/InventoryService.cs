using ProductInventoryMvc.Data;
using ProductInventoryMvc.Exceptions;
using ProductInventoryMvc.Models;
using ProductInventoryMvc.Models.Dto;

namespace ProductInventoryMvc.Services;
public class InventoryService : IInventoryService
{
    private readonly DataContext _context;

    public InventoryService(DataContext context)
    {
        _context = context;
    }
    public CommandResultModel AddInventory(InventoryDto inventory)
    {
        if (string.IsNullOrWhiteSpace(inventory.Name))
        {
            throw new InvalidInputException(new CommandResultModel
            {
                Success = false,
                Message = "Name is empty",
            });
        }

        var model = new InventoryModel
        {
            Name = inventory.Name,
        };

        _context.Inventories.Add(model);
        _context.SaveChanges();

        return new CommandResultModel
        {
            Success = true,
            Message = "ok"
        };
    }

    public CommandResultModel DeleteInventory(InventoryDto inventory)
    {
        var inv = FindInventory(inventory.Name);
        _context.Inventories.Remove(inv);
        _context.SaveChanges();

        return new CommandResultModel
        {
            Success = true,
            Message = "ok"
        };
    }

    public InventoryListDto GetInventories()
    {
        return new InventoryListDto
        {
            Inventories = _context.Inventories
                .Select(x => new GetInventoryDto
                {
                    Name = x.Name,
                    TotalQuantity = x.TotalQuantity,
                    TotalWeight = x.TotalWeight,
                })
                .ToArray()
        };
    }

    public ProductListDto GetProducts(InventoryDto inventoryName)
    {
        var inventory = FindInventory(inventoryName.Name);

        if (inventory != null)
        {
            return new ProductListDto
            {
                Inventory = inventoryName,
                Products = _context.Products
                    .Where(inv => inv.InventoryId == inventory.Id)
                    .Select(x => new ProductDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        Weight = x.Weight,
                        InventoryName = inventory.Name
                    })
                    .ToArray()
            };
        }

        return null;
    }

    public CommandResultModel AddProduct(string inventoryName, ProductDto product)
    {
        CheckProduct(product);
        var inventory = FindInventory(inventoryName);

        _context.Products.Add(new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Weight = product.Weight,
            InventoryId = inventory.Id,
            Inventory = inventory,
        });

        inventory.TotalQuantity += product.Quantity;
        inventory.TotalWeight += product.Weight;

        _context.SaveChanges();

        return new CommandResultModel
        {
            Success = true,
            Message = "ok",
        };
    }

    public CommandResultModel DeleteProduct(string inventoryName, DefineProductDto productToDelete)
    {
        var inventory = FindInventory(inventoryName);

        var product = _context.Products.FirstOrDefault(x => x.Id == productToDelete.Id);

        if (product == null)
        {
            throw new ProductInventoryNotFoundException(new CommandResultModel
            {
                Success = false,
                Message = "product not found",
            });
        }

        _context.Products.Remove(product);

        inventory.TotalWeight -= product.Weight;
        inventory.TotalQuantity -= product.Quantity;

        _context.SaveChanges();

        return new CommandResultModel
        {
            Success = true,
            Message = "ok",
        };
    }

    public CommandResultModel EditProduct(ProductDto product)
    {
        var productToEdit = _context.Products.Where(x => x.Id == product.Id)
            .Where(y => y.Inventory.Name == product.InventoryName)
            .FirstOrDefault();
        
        if (productToEdit == null)
        {
            throw new ProductInventoryNotFoundException(new CommandResultModel
            {
                Success = false,
                Message = "product not found",
            });
        }

        CheckProduct(product);
        productToEdit.Name = product.Name;
        productToEdit.Weight = product.Weight;
        productToEdit.Quantity = product.Quantity;

        _context.SaveChanges();

        return new CommandResultModel
        {
            Success = true,
            Message = "ok",
        };
    }

    public InventoryModel FindInventory(string inventoryName)
    {
        var inventory = _context.Inventories.Where(x => x.Name == inventoryName).FirstOrDefault();

        if (inventory == null)
        {
            throw new ProductInventoryNotFoundException(new CommandResultModel
            {
                Success = false,
                Message = "inventory not found",
            });
        }

        return inventory;
    }

    private void CheckProduct(ProductDto product)
    {
        if (product.Quantity < 0)
        {
            throw new InvalidInputException(new CommandResultModel
            {
                Success = false,
                Message = "Quantity is negative",
            });
        }

        if (product.Weight <= 0)
        {
            throw new InvalidInputException(new CommandResultModel
            {
                Success = false,
                Message = "Weight is negative",
            });
        }
    }
}

