namespace FMS.DTOs;
public record PaymentCreateDto(string UserId, int PlanId, decimal Amount, string? PaymentMethod);

