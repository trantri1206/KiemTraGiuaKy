using Abp.Application.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using _2380600189.Exams.Dto;

namespace _2380600189.Exams;

public interface IExamAppService : IApplicationService
{
    Task<List<ExamListDto>> GetAllExamsAsync();
    Task<List<ExamListDto>> GetPublishedExamsAsync();
    Task<ExamDetailDto> GetExamDetailAsync(int id, bool forStudent = false);
    Task<UploadDocxResultDto> ImportFromDocxStreamAsync(Stream stream, string fileName);
    Task<bool> TogglePublishAsync(int id);
    Task UpdateExamAsync(UpdateExamDto input);
    Task UpdateQuestionAsync(UpdateQuestionDto input);
    Task DeleteExamAsync(int id);
    Task<ExamResultDto> SubmitExamAsync(SubmitExamInputDto input);
    Task<ExamResultDto> GetExamResultAsync(int attemptId);
    Task<ExamStatisticDto> GetExamStatisticsAsync(int examId);
    Task<List<StudentSummaryDto>> GetAllStudentsAsync(string searchTerm = null);
    Task<StudentDetailDto> GetStudentDetailAsync(string candidateCode);
}
