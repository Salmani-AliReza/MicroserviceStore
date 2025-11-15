using AutoMapper;
using DiscountService.Infrastructure.Contexts;
using DiscountService.Model.Entites;

namespace DiscountService.Model.Services
{
    public interface IDiscountService
    {
        DiscountDto GetDiscountByCode(string code);
        bool UseDiscount(Guid Id);
        bool AddNewDiscountCode(string Code, int Amount);
    }

    public class DiscountService : IDiscountService
    {
        private readonly DiscountDataBaseContext _context;
        private readonly IMapper _mapper;

        public DiscountService(DiscountDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public DiscountDto GetDiscountByCode(string code)
        {
            var discountCode = _context.DiscountCodes.SingleOrDefault(x => x.Code.Equals(code));
            if (discountCode is null)
            {
                throw new Exception("Discount not found...!");
            }

            var result = _mapper.Map<DiscountDto>(discountCode);
            return result;
        }

        public bool UseDiscount(Guid Id)
        {
            var discountCode = _context.DiscountCodes.Find(Id);
            if (discountCode is null)
            {
                throw new Exception("Discount not found...!");
            }

            discountCode.Used = true;
            _context.SaveChanges();
            return true;
        }

        public bool AddNewDiscountCode(string Code, int Amount)
        {
            var discountCode = new DiscountCode
            {
                Code = Code,
                Amount = Amount,
                Used = false
            };
            _context.DiscountCodes.Add(discountCode);
            _context.SaveChanges();
            return true;
        }
    }

    public class DiscountDto
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public bool Used { get; set; }
    }
}
