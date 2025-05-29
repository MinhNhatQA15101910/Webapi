using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.Builders;

public class ProductBuilder
{
    private readonly Product _product = new();
    private readonly List<Guid> _categoryIds = [];
    
    public ProductBuilder WithName(string name)
    {
        _product.Name = name;
        return this;
    }
    
    public ProductBuilder WithDescription(string description)
    {
        _product.Description = description;
        return this;
    }
    
    public ProductBuilder WithPrice(decimal price)
    {
        _product.Price = price;
        return this;
    }
    
    public ProductBuilder WithStock(int stock)
    {
        _product.InStock = stock;
        return this;
    }
    
    public ProductBuilder WithCategories(IEnumerable<Guid> categoryIds)
    {
        _categoryIds.AddRange(categoryIds);
        return this;
    }
    
    public (Product Product, List<Guid> CategoryIds) Build()
    {
        _product.Id = Guid.NewGuid();
        _product.CreatedAt = DateTime.UtcNow;
        _product.UpdatedAt = DateTime.UtcNow;
        return (_product, _categoryIds);
    }
    
    public static ProductBuilder FromDto(CreateProductDto dto)
    {
        return new ProductBuilder()
            .WithName(dto.Name)
            .WithDescription(dto.Description)
            .WithPrice(dto.Price)
            .WithStock(dto.InStock)
            .WithCategories(dto.CategoryIds);
    }
    
    public static ProductBuilder FromEntity(Product product)
    {
        var builder = new ProductBuilder()
            .WithName(product.Name)
            .WithDescription(product.Description)
            .WithPrice(product.Price)
            .WithStock(product.InStock);
        
        if (product.Categories.Any())
        {
            builder.WithCategories(product.Categories.Select(pc => pc.CategoryId));
        }
        
        return builder;
    }
}