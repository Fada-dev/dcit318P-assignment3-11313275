using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Assignment3.Q5
{
    // a) Immutable InventoryItem record
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // b) Marker interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // c) Generic InventoryLogger<T>
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly List<T> _log = new();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item) => _log.Add(item);

        public List<T> GetAll() => new(_log);

        public void SaveToFile()
        {
            try
            {
                using var stream = new FileStream(_filePath, FileMode.Create);
                JsonSerializer.Serialize(stream, _log);
                Console.WriteLine($"Data saved to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("No file found to load.");
                    return;
                }

                using var stream = new FileStream(_filePath, FileMode.Open);
                var items = JsonSerializer.Deserialize<List<T>>(stream);
                if (items != null)
                {
                    _log.Clear();
                    _log.AddRange(items);
                }

                Console.WriteLine($"Data loaded from {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
    }

    // f) InventoryApp
    public class InventoryApp
    {
        private readonly InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            _logger = new InventoryLogger<InventoryItem>("inventory.json");
        }

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Laptop", 5, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Monitor", 8, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Keyboard", 15, DateTime.Now));
        }

        public void SaveData() => _logger.SaveToFile();

        public void LoadData() => _logger.LoadFromFile();

        public void PrintAllItems()
        {
            var items = _logger.GetAll();
            if (items.Count == 0)
            {
                Console.WriteLine("No inventory items found.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}");
            }
        }

        public void Run()
        {
            // Seed and save
            SeedSampleData();
            SaveData();

            // Simulate new session
            Console.WriteLine("\n--- Simulating New Session ---");
            var newApp = new InventoryApp();
            newApp.LoadData();
            newApp.PrintAllItems();
        }
    }
}
