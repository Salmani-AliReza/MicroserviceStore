using AutoMapper;
using BasketService.Infrastructure.Contexts;
using BasketService.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasketService.Model.Services
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string UserId);
        BasketDto GetBasket(string UserId);
        void AddItemToBasket(AddItemToBasketDto Item);
        void RemoveItemFromBasket(Guid ItemId);
        void SetBasketItemQuantity(Guid ItemId, int Quantity);
        void TransferBasket(string anonymousId, string UserId);
    }

    public class BasketService : IBasketService
    {
        private readonly BasketDataBaseContext _context;
        private readonly IMapper _mapper;

        public BasketService(BasketDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BasketDto GetBasket(string UserId)
        {
            var basket = _context.Baskets
                .Include(d => d.Items)
                .SingleOrDefault(b => b.UserID == UserId);
            if (basket is null)
            {
                return null;
            }

            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserID,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    ImageURL = item.ImageURL
                }).ToList()
            };
        }

        public BasketDto GetOrCreateBasketForUser(string UserId)
        {
            var basket = _context.Baskets
                .Include(d => d.Items)
                .SingleOrDefault(b => b.UserID == UserId);
            if (basket is null)
            {
                return CreateBasketForUSer(UserId);
            }

            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserID,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    ImageURL = item.ImageURL
                }).ToList()
            };
        }

        public void AddItemToBasket(AddItemToBasketDto Item)
        {
            var basket = _context.Baskets.FirstOrDefault(p => p.Id == Item.BasketId);
            if (basket is null)
            {
                throw new Exception("Basket not found...!");
            }

            var basketItem = _mapper.Map<BasketItem>(Item);
            basket.Items.Add(basketItem);
            _context.SaveChanges();
        }

        public void RemoveItemFromBasket(Guid ItemId)
        {
            var item = _context.BasketItems.SingleOrDefault(d => d.Id == ItemId);
            if (item is null)
            {
                throw new Exception("Basket item not found...!");
            }

            _context.BasketItems.Remove(item);
            _context.SaveChanges();
        }

        public void SetBasketItemQuantity(Guid ItemId, int Quantity)
        {
            var item = _context.BasketItems.SingleOrDefault(d => d.Id == ItemId);
            if (item is null)
            {
                throw new Exception("Basket item not found...!");
            }

            item.SetQuantity(Quantity);
            _context.SaveChanges();
        }

        public void TransferBasket(string anonymousId, string UserId)
        {
            var anonymousBasket = _context.Baskets
                .Include(d => d.Items)
                .SingleOrDefault(d => d.UserID == UserId);
            if (anonymousBasket is null)
            {
                return;
            }

            var userBasket = _context.Baskets.SingleOrDefault(d => d.UserID == UserId);
            if (userBasket is null)
            {
                userBasket = new Basket(UserId);
                _context.Baskets.Add(userBasket);
            }

            foreach (var item in anonymousBasket.Items)
            {
                userBasket.Items.Add(new BasketItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    ImageURL = item.ImageURL
                });
            }

            _context.Baskets.Remove(anonymousBasket);
            _context.SaveChanges();
        }

        private BasketDto CreateBasketForUSer(string UserId)
        {
            var basket = new Basket(UserId);
            _context.Baskets.Add(basket);
            _context.SaveChanges();
            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserID,
            };
        }
    }

    public class BasketDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        public int Total()
        {
            if (Items.Count > 0)
            {
                var Sum = Items.Sum(p => p.UnitPrice * p.Quantity);
                return Sum;
            }
            return 0;
        }
    }

    public class BasketItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageURL { get; set; }
    }

    public class AddItemToBasketDto
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageURL { get; set; }
    }
}
