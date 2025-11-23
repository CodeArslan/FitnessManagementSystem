namespace FitnessManagementSystem.DTOs.FeedbackDTOs;

public record FeedbackResponseDto(long FeedbackId, string MemberId, string TrainerId, int Rating, string? Comments, DateTime CreatedAt, bool IsApproved);

