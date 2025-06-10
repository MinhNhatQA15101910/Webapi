using Webapi.Domain.Entities;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.DTOs.ProductSize;

namespace Webapi.Application.Builders;

public class ProductBuilder
{
    private readonly Product _product = new();
    private readonly List<Guid> _categoryIds = [];
    private readonly List<CreateProductSizeDto> _sizes = [];
    private readonly List<Guid> _existingSizeIds = [];
    
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
    
    public ProductBuilder WithExistingSizes(IEnumerable<Guid> sizeIds)
    {
        if (sizeIds != null)
        {
            _existingSizeIds.AddRange(sizeIds);
        }
        return this;
    }
    
    public ProductBuilder WithNewSizes(IEnumerable<CreateProductSizeDto> sizes)
    {
        if (sizes != null)
        {
            _sizes.AddRange(sizes);
        }
        return this;
    }
    
    public (Product Product, List<Guid> CategoryIds, List<CreateProductSizeDto> Sizes, List<Guid> ExistingSizeIds) Build()
    {
        return (_product, _categoryIds, _sizes, _existingSizeIds);
    }
    
    public static ProductBuilder FromDto(CreateProductDto dto)
    {
        return new ProductBuilder()
            .WithName(dto.Name)
            .WithDescription(dto.Description)
            .WithPrice(dto.Price)
            .WithStock(dto.InStock)
            .WithCategories(dto.CategoryIds)
            .WithNewSizes(dto.Sizes)
            .WithExistingSizes(dto.SizeIds);
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