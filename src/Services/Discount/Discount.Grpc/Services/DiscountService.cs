using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request parameters"));
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync(context.CancellationToken);
        logger.LogInformation("Discount created successfully. Product Name : {ProductName}", coupon.ProductName);
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon is null)
        {
            coupon = new Coupon
            {
                ProductName = "No Discount",
                Description = "No Discount",
                Amount = 0
            };
        }

        logger.LogInformation($"Discount is retrieved for ProductName: {request.ProductName}");
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request parameters"));

        var existingCoupon = await dbContext.Coupons
            .FirstOrDefaultAsync(
                x => x.ProductName == request.Coupon.ProductName,
                context.CancellationToken);

        if (existingCoupon == null)
            throw new RpcException(
                new Status(StatusCode.NotFound,
                    $"Coupon not found for ProductName: {request.Coupon.ProductName}"));

        // Update fields explicitly (safe)
        existingCoupon.Description = request.Coupon.Description;
        existingCoupon.Amount = request.Coupon.Amount;

        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation(
            "Discount updated for ProductName: {ProductName}",
            existingCoupon.ProductName);

        return existingCoupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with product name = {request.ProductName} is not found"));
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount deleted successfully. Product Name : {ProductName}", coupon.ProductName);
        return new DeleteDiscountResponse { Success = true };
    }
}