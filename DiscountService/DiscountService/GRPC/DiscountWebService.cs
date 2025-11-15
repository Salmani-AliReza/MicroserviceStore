using DiscountService.Model.Services;
using DiscountService.Proto;
using Grpc.Core;

namespace DiscountService.GRPC
{
    public class DiscountWebService : DiscountServiceProto.DiscountServiceProtoBase
    {
        private readonly IDiscountService _discountService;

        public DiscountWebService(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public override Task<ResponseGetDiscountByCode> GetDiscountByCode(RequestGetDiscountByCode request, ServerCallContext context)
        {
            var data = _discountService.GetDiscountByCode(request.Code);
            return Task.FromResult(new ResponseGetDiscountByCode
            {
                Id = data.Id.ToString(),
                Code = data.Code,
                Amount = data.Amount,
                Used = data.Used
            });
        }

        public override Task<ResponseUseDiscount> UseDiscount(RequestUseDiscount request, ServerCallContext context)
        {
            var result = _discountService.UseDiscount(Guid.Parse(request.Id));
            return Task.FromResult(new ResponseUseDiscount
            {
                IsSuccess = result
            });
        }

        public override Task<ResponseAddNewDiscountCode> AddNewDiscountCode(RequestAddNewDiscountCode request, ServerCallContext context)
        {
            var result = _discountService.AddNewDiscountCode(request.Code, request.Amount);
            return Task.FromResult(new ResponseAddNewDiscountCode
            {
                IsSuccess = result
            });
        }
    }
}
