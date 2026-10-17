using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using _2380600189.Controllers;
using _2380600189.Exams;
using _2380600189.Exams.Dto;

namespace _2380600189.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("api/exam")]
[EnableCors]
[IgnoreAntiforgeryToken]
public class ExamApiController : _2380600189ControllerBase
{
    private readonly IExamAppService _examAppService;

    public ExamApiController(IExamAppService examAppService)
    {
        _examAppService = examAppService;
    }

    /// <summary>
    /// Lấy danh sách đề thi đã xuất bản cho sinh viên
    /// </summary>
    [HttpGet("list")]
    public async Task<ActionResult<List<ExamListDto>>> GetPublishedExams()
    {
        var list = await _examAppService.GetPublishedExamsAsync();
        return Ok(list);
    }

    /// <summary>
    /// Lấy chi tiết đề thi cho sinh viên làm bài (ẩn đáp án đúng và gợi ý)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ExamDetailDto>> GetExamForStudent(int id)
    {
        var exam = await _examAppService.GetExamDetailAsync(id, forStudent: true);
        return Ok(exam);
    }

    /// <summary>
    /// Nộp bài thi và chấm điểm tự động
    /// </summary>
    [HttpPost("submit")]
    public async Task<ActionResult<ExamResultDto>> SubmitExam([FromBody] SubmitExamInputDto input)
    {
        if (input == null || input.ExamId <= 0)
        {
            return BadRequest("Dữ liệu nộp bài không hợp lệ.");
        }

        var result = await _examAppService.SubmitExamAsync(input);
        return Ok(result);
    }

    /// <summary>
    /// Xem lại kết quả bài thi chi tiết
    /// </summary>
    [HttpGet("result/{attemptId}")]
    public async Task<ActionResult<ExamResultDto>> GetExamResult(int attemptId)
    {
        var result = await _examAppService.GetExamResultAsync(attemptId);
        return Ok(result);
    }

    /// <summary>
    /// Thống kê đề thi (cho mobile hoặc web)
    /// </summary>
    [HttpGet("statistics/{id}")]
    public async Task<ActionResult<ExamStatisticDto>> GetStatistics(int id)
    {
        var stats = await _examAppService.GetExamStatisticsAsync(id);
        return Ok(stats);
    }

    /// <summary>
    /// Lấy toàn bộ đề thi (cho giảng viên)
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<List<ExamListDto>>> GetAllExams()
    {
        var list = await _examAppService.GetAllExamsAsync();
        return Ok(list);
    }

    /// <summary>
    /// Danh sách sinh viên và bảng điểm (cho giảng viên)
    /// </summary>
    [HttpGet("students")]
    public async Task<ActionResult<List<StudentSummaryDto>>> GetStudents([FromQuery] string searchTerm)
    {
        var list = await _examAppService.GetAllStudentsAsync(searchTerm);
        return Ok(list);
    }
}
