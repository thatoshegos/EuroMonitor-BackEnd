using BackEnd.Interfaces;
using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEnd.DataAccess
{
    public class WishlistDataAccessLayer : IWishlistService
    {
        readonly BookDBContext _dbContext;

        public WishlistDataAccessLayer(BookDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ToggleWishlistItem(int userId, int bookId)
        {
            string wishlistId = GetWishlistId(userId);
            var existingWishlistItem = _dbContext.WishlistItems.FirstOrDefault(x => x.ProductId == bookId && x.WishlistId == wishlistId);

            if (existingWishlistItem != null)
            {
                _dbContext.WishlistItems.Remove(existingWishlistItem);
                _dbContext.SaveChanges();
            }
            else
            {
                var wishlistItem = new WishlistItem
                {
                    WishlistId = wishlistId,
                    ProductId = bookId,
                };
                _dbContext.WishlistItems.Add(wishlistItem);
                _dbContext.SaveChanges();
            }
        }

        public int ClearWishlist(int userId)
        {
            try
            {
                string wishlistId = GetWishlistId(userId);
                var wishlistItem = _dbContext.WishlistItems.Where(x => x.WishlistId == wishlistId).ToList();

                if (!string.IsNullOrEmpty(wishlistId))
                {
                    foreach (var item in wishlistItem)
                    {
                        _dbContext.WishlistItems.Remove(item);
                        _dbContext.SaveChanges();
                    }
                }
                return 0;
            }
            catch
            {
                throw;
            }
        }

        public string GetWishlistId(int userId)
        {
            try
            {
                var wishlist = _dbContext.Wishlists.FirstOrDefault(x => x.UserId == userId);

                if (wishlist != null)
                {
                    return wishlist.WishlistId;
                }
                else
                {
                    return CreateWishlist(userId);
                }

            }
            catch
            {
                throw;
            }
        }

        string CreateWishlist(int userId)
        {
            try
            {
                Wishlist wishList = new Wishlist
                {
                    WishlistId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    DateCreated = DateTime.Now.Date
                };

                _dbContext.Wishlists.Add(wishList);
                _dbContext.SaveChanges();

                return wishList.WishlistId;
            }
            catch
            {
                throw;
            }
        }
    }
}
