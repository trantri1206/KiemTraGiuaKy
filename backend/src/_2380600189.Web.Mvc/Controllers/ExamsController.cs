using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using _2380600189.Controllers;
using _2380600189.Exams;
using _2380600189.Exams.Dto;

namespace _2380600189.Web.Controllers;

public class ExamsController : _2380600189ControllerBase
{
    private readonly IExamAppService _examAppService;

    public ExamsController(IExamAppService examAppService)
    {
        _examAppService = examAppService;
    }

    public async Task<IActionResult> Index()
    {
        var exams = await _examAppService.GetAllExamsAsync();
        if (exams.Count == 0)
        {
            await EnsureSampleExamSeededAsync();
            exams = await _examAppService.GetAllExamsAsync();
        }
        return View(exams);
    }

    private async Task EnsureSampleExamSeededAsync()
    {
        try
        {
            var candidatePaths = new[]
            {
                "TOEIC_RC_Test_02.docx",
                "../TOEIC_RC_Test_02.docx",
                "../../TOEIC_RC_Test_02.docx",
                Path.Combine(AppContext.BaseDirectory, "TOEIC_RC_Test_02.docx"),
                @"c:\Lập trình di động\KiemTra\TOEIC_RC_Test_02.docx",
                @"c:\Lập trình di động\KiemTra\backend\TOEIC_RC_Test_02.docx",
                "TOEIC_Reading_Test_01.docx",
                "../TOEIC_Reading_Test_01.docx",
                @"c:\Lập trình di động\KiemTra\TOEIC_Reading_Test_01.docx"
            };

            string docxPath = null;
            foreach (var p in candidatePaths)
            {
                if (System.IO.File.Exists(p))
                {
                    docxPath = p;
                    break;
                }
            }

            if (docxPath != null)
            {
                using var stream = System.IO.File.OpenRead(docxPath);
                var importRes = await _examAppService.ImportFromDocxStreamAsync(stream, Path.GetFileName(docxPath));
                if (importRes.Success && importRes.ExamId.HasValue)
                {
                    await _examAppService.TogglePublishAsync(importRes.ExamId.Value);
                    await SeedSampleAttemptsAsync(importRes.ExamId.Value);
                }
            }
        }
        catch
        {
            // Ignore seeding errors on fallback
        }
    }

    private async Task SeedSampleAttemptsAsync(int examId)
    {
        var exam = await _examAppService.GetExamDetailAsync(examId, forStudent: false);
        if (exam == null) return;

        var allQuestions = new List<QuestionDto>();
        foreach (var sec in exam.Sections)
        {
            allQuestions.AddRange(sec.Questions);
        }
        allQuestions.AddRange(exam.Questions);

        var students = new[]
        {
            ("Nguyễn Văn An (MSSV: 2380600189)", 92, 45), // 92% correct
            ("Trần Thị Mai", 78, 62),                     // 78% correct
            ("Lê Quốc Bảo", 60, 71)                       // 60% correct
        };

        var rnd = new Random(42);
        foreach (var (name, accuracyPct, timeSpent) in students)
        {
            var submitDto = new SubmitExamInputDto
            {
                ExamId = examId,
                CandidateName = name,
                CandidateCode = "2380600189",
                StartTime = DateTime.UtcNow.AddMinutes(-timeSpent),
                Answers = new List<CandidateAnswerInputDto>()
            };

            foreach (var q in allQuestions)
            {
                var correctOpt = q.Options.Find(o => o.IsCorrect);
                var isRight = rnd.Next(100) < accuracyPct;

                if (isRight && correctOpt != null)
                {
                    submitDto.Answers.Add(new CandidateAnswerInputDto
                    {
                        QuestionId = q.Id,
                        SelectedOptionId = correctOpt.Id
                    });
                }
                else
                {
                    var wrongOpts = q.Options.FindAll(o => !o.IsCorrect);
                    if (wrongOpts.Count > 0)
                    {
                        submitDto.Answers.Add(new CandidateAnswerInputDto
                        {
                            QuestionId = q.Id,
                            SelectedOptionId = wrongOpts[rnd.Next(wrongOpts.Count)].Id
                        });
                    }
                }
            }

            await _examAppService.SubmitExamAsync(submitDto);
        }
    }

    [HttpGet]
    public IActionResult Import()
    {
        return View(new UploadDocxResultDto { Success = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(IFormFile docxFile)
    {
        if (docxFile == null || docxFile.Length == 0)
        {
            var emptyModel = new UploadDocxResultDto
            {
                Success = false,
                Errors = new() { "Vui lòng chọn một file Word (.docx) để tải lên." }
            };
            return View(emptyModel);
        }

        var ext = Path.GetExtension(docxFile.FileName).ToLowerInvariant();
        if (ext != ".docx")
        {
            var extModel = new UploadDocxResultDto
            {
                Success = false,
                Errors = new() { "Định dạng file không hợp lệ. Chỉ chấp nhận định dạng file Word (.docx)." }
            };
            return View(extModel);
        }

        using var memoryStream = new MemoryStream();
        await docxFile.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var result = await _examAppService.ImportFromDocxStreamAsync(memoryStream, docxFile.FileName);

        if (!result.Success)
        {
            return View(result);
        }

        TempData["SuccessMessage"] = $"Đã tải lên và tạo đề thi '{result.ExamTitle}' thành công với {result.TotalQuestions} câu hỏi chuẩn 100%!";
        return RedirectToAction(nameof(Preview), new { id = result.ExamId });
    }

    public async Task<IActionResult> Preview(int id)
    {
        var exam = await _examAppService.GetExamDetailAsync(id, forStudent: false);
        return View(exam);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var exam = await _examAppService.GetExamDetailAsync(id, forStudent: false);
        var model = new UpdateExamDto
        {
            Id = exam.Id,
            Title = exam.Title,
            Description = exam.Description,
            DurationMinutes = exam.DurationMinutes,
            CanShuffleQuestions = exam.CanShuffleQuestions,
            CanShuffleOptions = exam.CanShuffleOptions
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateExamDto input)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        await _examAppService.UpdateExamAsync(input);
        TempData["SuccessMessage"] = "Cập nhật thông tin đề thi thành công!";
        return RedirectToAction(nameof(Preview), new { id = input.Id });
    }

    [HttpGet]
    public async Task<IActionResult> EditQuestion(int id, int examId)
    {
        var exam = await _examAppService.GetExamDetailAsync(examId, forStudent: false);
        var q = exam.Questions.Find(x => x.Id == id);
        if (q == null)
        {
            foreach (var sec in exam.Sections)
            {
                q = sec.Questions.Find(x => x.Id == id);
                if (q != null) break;
            }
        }

        if (q == null)
        {
            return NotFound();
        }

        ViewBag.ExamId = examId;
        ViewBag.ExamTitle = exam.Title;
        ViewBag.QuestionNumber = q.QuestionNumber;

        var model = new UpdateQuestionDto
        {
            Id = q.Id,
            Content = q.Content,
            Explanation = q.Explanation,
            CorrectOptionId = q.Options.Find(o => o.IsCorrect)?.Id ?? 0,
            Options = q.Options.ConvertAll(o => new UpdateOptionDto
            {
                Id = o.Id,
                OptionLabel = o.OptionLabel,
                Content = o.Content,
                IsCorrect = o.IsCorrect
            })
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditQuestion(UpdateQuestionDto input, int examId)
    {
        await _examAppService.UpdateQuestionAsync(input);
        TempData["SuccessMessage"] = "Cập nhật câu hỏi thành công!";
        return RedirectToAction(nameof(Preview), new { id = examId });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> TogglePublish(int id)
    {
        try
        {
            var isPublished = await _examAppService.TogglePublishAsync(id);
            return Json(new { success = true, isPublished });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    public async Task<IActionResult> Statistics(int id)
    {
        var stats = await _examAppService.GetExamStatisticsAsync(id);
        return View(stats);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _examAppService.DeleteExamAsync(id);
        TempData["SuccessMessage"] = "Đã xóa đề thi thành công.";
        return RedirectToAction(nameof(Index));
    }
}
