using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using _2380600189.Controllers;
using _2380600189.Exams;

namespace _2380600189.Web.Controllers;

public class StudentController : _2380600189ControllerBase
{
    private readonly IExamAppService _examAppService;

    public StudentController(IExamAppService examAppService)
    {
        _examAppService = examAppService;
    }

    public async Task<IActionResult> Index()
    {
        var exams = await _examAppService.GetPublishedExamsAsync();
        return View(exams);
    }

    public async Task<IActionResult> TakeExam(int id)
    {
        var exam = await _examAppService.GetExamDetailAsync(id, forStudent: true);
        if (exam == null || !exam.IsPublished)
        {
            return NotFound("Đề thi không tồn tại hoặc chưa được xuất bản.");
        }
        return View(exam);
    }
}
