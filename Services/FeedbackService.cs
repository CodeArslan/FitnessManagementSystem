using FitnessManagementSystem.Data;
using FitnessManagementSystem.DTOs.FeedbackDTOs;
using FitnessManagementSystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitnessManagementSystem;

public class FeedbackService(ApplicationDbContext _db) : IFeedbackService
{

    public Task ApproveFeedbackAsync(long feedbackId, string adminId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FeedbackResponseDto>> GetMemberFeedbackAsync(string memberId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FeedbackResponseDto>> GetTrainerFeedbackAsync(string trainerId, bool onlyApproved = true)
    {
        throw new NotImplementedException();
    }

    public Task<double> RecalculateTrainerAggregatesAsync(string trainerId)
    {
        throw new NotImplementedException();
    }

    public async Task<FeedbackResponseDto> SubmitFeedbackAsync(FeedbackCreateDto dto, CancellationToken ct = default)
    {
        // validation
        if (dto.Rating < 1 || dto.Rating > 5)
            throw new ArgumentException("Rating must be 1..5");

        var existing = !string.IsNullOrEmpty(dto.SessionId)
            ? await _db.Feedbacks.FirstOrDefaultAsync(f => f.MemberId == dto.MemberId && f.TrainerId == dto.TrainerId && f.SessionId == dto.SessionId,ct)
            : await _db.Feedbacks.FirstOrDefaultAsync(f => f.MemberId == dto.MemberId && f.TrainerId == dto.TrainerId && f.SessionId == null,ct);

        Feedback feedback;
        if (existing != null)
        {
            // update existing (common for per-session update)
            existing.Rating = dto.Rating;
            existing.Comment = dto.Comments;
            existing.CreatedAt = DateTime.UtcNow;
            feedback = existing;
            _db.Feedbacks.Update(existing);
        }
        else
        {
            feedback = new Feedback
            {
                MemberId = dto.MemberId,
                TrainerId = dto.TrainerId,
                Rating = dto.Rating,
                Comment = dto.Comments,
                SessionId = dto.SessionId,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false // default to require admin review
            };
            await _db.Feedbacks.AddAsync(feedback, ct);
        }

        await _db.SaveChangesAsync(ct);

        // Recalculate only using approved ratings OR if you auto-approve then include new
        //await RecalculateTrainerAggregatesAsync(dto.TrainerId);

        return new FeedbackResponseDto(feedback.FeedbackId, feedback.MemberId, feedback.TrainerId, feedback.Rating, feedback.Comment, feedback.CreatedAt, feedback.IsApproved);

    }

    public Task SubmitTrainerToMemberFeedbackAsync(string trainerId, string memberId, int rating, string? comments, string? sessionId)
    {
        throw new NotImplementedException();
    }
}
