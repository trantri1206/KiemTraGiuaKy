using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using _2380600189.Controllers;
using _2380600189.Exams;

namespace _2380600189.Web.Controllers;

public class StudentsController : _2380600189ControllerBase
{
    private readonly IExamAppService _examAppService;

    public StudentsController(IExamAppService examAppService)
    {
        _examAppService = examAppService;
    }

    /// <summary>
    /// Danh sách tất cả sinh viên tham gia thi
    /// </summary>
    public async Task<IActionResult> Index(string search = null)
    {
        ViewBag.SearchTerm = search;
        var students = await _examAppService.GetAllStudentsAsync(search);
        return View(students);
    }

    /// <summary>
    /// Xem chi tiết lịch sử thi của sinh viên
    /// </summary>
    public async Task<IActionResult> Detail(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return RedirectToAction(nameof(Index));
        }

        var student = await _examAppService.GetStudentDetailAsync(id);
        if (student == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy dữ liệu sinh viên.";
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    /// <summary>
    /// Giảng viên xem chi tiết bài làm cụ thể của một lần thi
    /// </summary>
    public async Task<IActionResult> AttemptDetail(int id)
    {
        var result = await _examAppService.GetExamResultAsync(id);
        if (result == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy dữ liệu bài thi.";
            return RedirectToAction(nameof(Index));
        }

        return View(result);
    }
}
