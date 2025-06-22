using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Webapi.Domain.Entities;

namespace Webapi.Domain.Factories;

public class VoucherFactory
{
    private readonly ConcurrentDictionary<string, VoucherType> _voucherTypes = new();
    
    // Flyweight pattern implementation
    public VoucherType GetVoucherType(string name, double value, DateTime expireAt)
    {
        // Create a key for the cache
        string key = $"{name}_{value}_{expireAt:yyyyMMdd}";
        
        // Get or create the voucher type (Flyweight pattern)
        return _voucherTypes.GetOrAdd(key, _ => new VoucherType
        {
            Id = Guid.NewGuid(),
            Name = name,
            Value = value,
        });
    }
    
    // Create a new Voucher with multiple VoucherItems using the prototype pattern
    public Voucher CreateVoucher(VoucherType type, int quantity, DateTime expiredAt)
    {
        if (quantity <= 0)
        {
            quantity = 1; // Ensure we always have at least 1 voucher
        }
        
        // Create the main voucher with a proper GUID
        var voucher = new Voucher
        {
            Id = Guid.NewGuid(),
            TypeId = type.Id,
            Type = type,
            Items = new List<VoucherItem>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ExpiredAt = expiredAt,
            Quantity = quantity // Explicitly set quantity
        };
        
        // Create the prototype VoucherItem - this is the original template
        var prototypeItem = new VoucherItem
        {
            Id = Guid.NewGuid(),
            VoucherId = voucher.Id,
            Status = true,
        };
        
        // Generate items by cloning the prototype
        var items = GenerateVoucherItems(prototypeItem, quantity);
        voucher.Items = items;
        
        return voucher;
    }
    
    // Method to generate VoucherItems using the prototype pattern - public for use in update operations
    public List<VoucherItem> GenerateVoucherItems(VoucherItem prototype, int quantity)
    {
        var items = new List<VoucherItem>();
        
        for (int i = 0; i < quantity; i++)
        {
            // Clone the prototype for each new item
            var clonedItem = prototype.Clone(); // Using proper casing and the method from the entity
            
            items.Add(clonedItem);
        }
        
        return items;
    }
}