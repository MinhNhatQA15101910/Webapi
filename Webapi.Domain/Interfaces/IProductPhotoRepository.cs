using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Webapi.Domain.Entities;

namespace Webapi.Domain.Interfaces
{
    public interface IProductPhotoRepository
    {
        Task<ProductPhoto> GetPhotoByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductPhoto>> GetPhotosByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<ProductPhoto> GetMainPhotoForProductAsync(Guid productId, CancellationToken cancellationToken = default);
        void Add(ProductPhoto photo);
        void Remove(ProductPhoto photo);
        Task<bool> SetMainPhotoAsync(Guid photoId, Guid productId, CancellationToken cancellationToken = default);
    }
}