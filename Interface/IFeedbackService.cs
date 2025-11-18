
using FitnessManagementSystem.DTOs.FeedbackDTOs;

namespace FitnessManagementSystem.Interface;

public interface IFeedbackService
{
    Task<FeedbackResponseDto> SubmitFeedbackAsync(FeedbackCreateDto dto, CancellationToken ct = default);
    Task<IEnumerable<FeedbackResponseDto>> GetTrainerFeedbackAsync(string trainerId, bool onlyApproved = true);
    Task<IEnumerable<FeedbackResponseDto>> GetMemberFeedbackAsync(string memberId);
    Task ApproveFeedbackAsync(long feedbackId, string adminId);
    Task<double> RecalculateTrainerAggregatesAsync(string trainerId);
    // trainer -> member rating method
    Task SubmitTrainerToMemberFeedbackAsync(string trainerId, string memberId, int rating, string? comments, string? sessionId);
}
