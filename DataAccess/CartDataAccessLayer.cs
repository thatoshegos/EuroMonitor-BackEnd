using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEnd.DataAccess
{
    public class CartDataAccessLayer : ICartService
    {
        readonly BookDBContext _dbContext;

        public CartDataAccessLayer(BookDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddBookToCart(int userId, int bookId)
        {
            string cartId = GetCartId(userId);
            int quantity = 1;

            var existingCartItem = _dbContext.CartItems.FirstOrDefault(x => x.ProductId == bookId && x.CartId == cartId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                _dbContext.Entry(existingCartItem).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            else
            {
                var cartItems = new CartItem
                {
                    CartId = cartId,
                    ProductId = bookId,
                    Quantity = quantity
                };
                _dbContext.CartItems.Add(cartItems);
                _dbContext.SaveChanges();
            }
        }

        public string GetCartId(int userId)
        {
            try
            {
                Cart cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == userId);

                if (cart != null)
                {
                    return cart.CartId;
                }
                else
                {
                    return CreateCart(userId);
                }

            }
            catch
            {
                throw;
            }
        }

        string CreateCart(int userId)
        {
            try
            {
                var shoppingCart = new Cart
                {
                    CartId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    DateCreated = DateTime.Now.Date
                };

                _dbContext.Carts.Add(shoppingCart);
                _dbContext.SaveChanges();

                return shoppingCart.CartId;
            }
            catch
            {
                throw;
            }
        }

        public void RemoveCartItem(int userId, int bookId)
        {
            try
            {
                string cartId = GetCartId(userId);
                var cartItem = _dbContext.CartItems.FirstOrDefault(x => x.ProductId == bookId && x.CartId == cartId);

                _dbContext.CartItems.Remove(cartItem);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteOneCartItem(int userId, int bookId)
        {
            try
            {
                string cartId = GetCartId(userId);
                var cartItem = _dbContext.CartItems.FirstOrDefault(x => x.ProductId == bookId && x.CartId == cartId);

                cartItem.Quantity -= 1;
                _dbContext.Entry(cartItem).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public int GetCartItemCount(int userId)
        {
            string cartId = GetCartId(userId);

            if (!string.IsNullOrEmpty(cartId))
            {
                int cartItemCount = _dbContext.CartItems.Where(x => x.CartId == cartId).Sum(x => x.Quantity);

                return cartItemCount;
            }
            else
            {
                return 0;
            }
        }

        public void MergeCart(int tempUserId, int permUserId)
        {
            try
            {
                if (tempUserId != permUserId && tempUserId > 0 && permUserId > 0)
                {
                    string tempCartId = GetCartId(tempUserId);
                    string permCartId = GetCartId(permUserId);

                    var tempCartItems = _dbContext.CartItems.Where(x => x.CartId == tempCartId).ToList();

                    foreach (var item in tempCartItems)
                    {
                        var cartItem = _dbContext.CartItems.FirstOrDefault(x => x.ProductId == item.ProductId && x.CartId == permCartId);

                        if (cartItem != null)
                        {
                            cartItem.Quantity += item.Quantity;
                            _dbContext.Entry(cartItem).State = EntityState.Modified;
                        }
                        else
                        {
                            var newCartItem = new CartItem
                            {
                                CartId = permCartId,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity
                            };
                            _dbContext.CartItems.Add(newCartItem);
                        }
                        _dbContext.CartItems.Remove(item);
                        _dbContext.SaveChanges();
                    }
                    DeleteCart(tempCartId);
                }
            }
            catch
            {
                throw;
            }
        }

        public int ClearCart(int userId)
        {
            try
            {
                string cartId = GetCartId(userId);
                var cartItem = _dbContext.CartItems.Where(x => x.CartId == cartId).ToList();

                if (!string.IsNullOrEmpty(cartId))
                {
                    foreach (CartItem item in cartItem)
                    {
                        _dbContext.CartItems.Remove(item);
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

        void DeleteCart(string cartId)
        {
            var cart = _dbContext.Carts.Find(cartId);
            _dbContext.Carts.Remove(cart);
            _dbContext.SaveChanges();
        }
    }
}
