using System;
using System.Collections.Generic;

namespace Assignment3.Q3
{
    // a) Marker interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // b) ElectronicItem
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string ToString() =>
            $"ElectronicItem [ID={Id}, Name={Name}, Brand={Brand}, Warranty={WarrantyMonths} months, Quantity={Quantity}]";
    }

    // b) GroceryItem
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }

        public override string ToString() =>
            $"GroceryItem [ID={Id}, Name={Name}, Expiry={ExpiryDate:d}, Quantity={Quantity}]";
    }

    // e) Custom exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // d) InventoryRepository<T>
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items = new();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.TryGetValue(id, out var item))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            return item;
        }

        public void RemoveItem(int id)
        {
            if (!_items.Remove(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
        }

        public List<T> GetAllItems() => new(_items.Values);

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidQuantityException("Quantity cannot be negative.");

            var item = GetItemById(id);
            item.Quantity = newQuantity;
        }
    }

    // f) WarehouseManager
    public class WareHouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _electronics = new();
        private readonly InventoryRepository<GroceryItem> _groceries = new();

        public void SeedData()
        {
            try
            {
                _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
                _electronics.AddItem(new ElectronicItem(2, "Smartphone", 10, "Samsung", 12));

                _groceries.AddItem(new GroceryItem(1, "Milk", 20, DateTime.Now.AddDays(10)));
                _groceries.AddItem(new GroceryItem(2, "Bread", 15, DateTime.Now.AddDays(5)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems())
                Console.WriteLine(item);
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                repo.UpdateQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Updated stock for {item.Name} to {item.Quantity}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error increasing stock: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);
                Console.WriteLine($"Removed item with ID {id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
            }
        }

        public void Run()
        {
            SeedData();

            Console.WriteLine("\n--- Grocery Items ---");
            PrintAllItems(_groceries);

            Console.WriteLine("\n--- Electronic Items ---");
            PrintAllItems(_electronics);

            Console.WriteLine("\n--- Testing Exceptions ---");
            try
            {
                _electronics.AddItem(new ElectronicItem(1, "Tablet", 3, "Apple", 12)); // duplicate
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine($"DuplicateItemException: {ex.Message}");
            }

            try
            {
                _groceries.RemoveItem(99); // non-existent
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine($"ItemNotFoundException: {ex.Message}");
            }

            try
            {
                _electronics.UpdateQuantity(2, -5); // invalid quantity
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"InvalidQuantityException: {ex.Message}");
            }
        }
    }
}
