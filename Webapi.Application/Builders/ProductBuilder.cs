using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;

namespace Webapi.Application.Builders;

public class ProductBuilder
{
    private readonly Product _product = new();
    private readonly List<Guid> _categoryIds = [];
    private readonly List<CreateProductSizeDto> _sizes = [];
    
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
        if (categoryIds != null)
        {
            _categoryIds.AddRange(categoryIds);
        }
        return this;
    }
    
    public ProductBuilder WithSizes(IEnumerable<CreateProductSizeDto> sizes)
    {
        if (sizes != null)
        {
            _sizes.AddRange(sizes);
        }
        return this;
    }
    
    public (Product Product, List<Guid> CategoryIds, List<CreateProductSizeDto> Sizes) Build()
    {
        return (_product, _categoryIds, _sizes);
    }
    
    public static ProductBuilder FromDto(CreateProductDto dto)
    {
        return new ProductBuilder()
            .WithName(dto.Name)
            .WithDescription(dto.Description)
            .WithPrice(dto.Price)
            .WithStock(dto.InStock)
            .WithCategories(dto.CategoryIds)
            .WithSizes(dto.Sizes);
    }
    
    public static ProductBuilder FromEntity(Product product)
    {
        return new ProductBuilder()
            .WithName(product.Name)
            .WithDescription(product.Description)
            .WithPrice(product.Price)
            .WithStock(product.InStock);
    }
}