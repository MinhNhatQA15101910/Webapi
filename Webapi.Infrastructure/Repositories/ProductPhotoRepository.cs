using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Data;

namespace Webapi.Infrastructure.Repositories
{
    public class ProductPhotoRepository : IProductPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductPhotoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductPhoto> GetPhotoByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.ProductPhotos
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ProductPhoto>> GetPhotosByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.ProductPhotos
                .Where(p => p.ProductId == productId)
                .OrderByDescending(p => p.IsMain)
                .ThenBy(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductPhoto> GetMainPhotoForProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.ProductPhotos
                .Where(p => p.ProductId == productId && p.IsMain)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public void Add(ProductPhoto photo)
        {
            _context.ProductPhotos.Add(photo);
        }

        public void Remove(ProductPhoto photo)
        {
            _context.ProductPhotos.Remove(photo);
        }

        public async Task<bool> SetMainPhotoAsync(Guid photoId, Guid productId, CancellationToken cancellationToken = default)
        {
            // Clear the IsMain flag from all photos for this product
            var currentMainPhoto = await GetMainPhotoForProductAsync(productId, cancellationToken);
            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }

            // Set the new main photo
            var photo = await GetPhotoByIdAsync(photoId, cancellationToken);
            if (photo != null)
            {
                photo.IsMain = true;
                return true;
            }

            return false;
        }
    }
}