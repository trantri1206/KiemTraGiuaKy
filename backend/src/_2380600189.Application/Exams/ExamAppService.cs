using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2380600189.Exams.Dto;

namespace _2380600189.Exams;

public class ExamAppService : _2380600189AppServiceBase, IExamAppService
{
    private readonly IRepository<Exam, int> _examRepository;
    private readonly IRepository<PartSection, int> _sectionRepository;
    private readonly IRepository<Question, int> _questionRepository;
    private readonly IRepository<AnswerOption, int> _optionRepository;
    private readonly IRepository<ExamAttempt, int> _attemptRepository;
    private readonly IRepository<AttemptAnswer, int> _attemptAnswerRepository;

    public ExamAppService(
        IRepository<Exam, int> examRepository,
        IRepository<PartSection, int> sectionRepository,
        IRepository<Question, int> questionRepository,
        IRepository<AnswerOption, int> optionRepository,
        IRepository<ExamAttempt, int> attemptRepository,
        IRepository<AttemptAnswer, int> attemptAnswerRepository)
    {
        _examRepository = examRepository;
        _sectionRepository = sectionRepository;
        _questionRepository = questionRepository;
        _optionRepository = optionRepository;
        _attemptRepository = attemptRepository;
        _attemptAnswerRepository = attemptAnswerRepository;
    }

    public async Task<List<ExamListDto>> GetAllExamsAsync()
    {
        var exams = await _examRepository.GetAll()
            .Include(e => e.Attempts)
            .OrderByDescending(e => e.CreationTime)
            .ToListAsync();

        return exams.Select(e => new ExamListDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            DurationMinutes = e.DurationMinutes,
            TotalQuestions = e.TotalQuestions,
            IsPublished = e.IsPublished,
            CreationTime = e.CreationTime,
            TotalAttempts = e.Attempts?.Count ?? 0,
            AverageScore = e.Attempts != null && e.Attempts.Count > 0 ? Math.Round(e.Attempts.Average(a => a.Score), 1) : 0
        }).ToList();
    }

    public async Task<List<ExamListDto>> GetPublishedExamsAsync()
    {
        var exams = await _examRepository.GetAll()
            .Where(e => e.IsPublished)
            .Include(e => e.Attempts)
            .OrderByDescending(e => e.CreationTime)
            .ToListAsync();

        return exams.Select(e => new ExamListDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            DurationMinutes = e.DurationMinutes,
            TotalQuestions = e.TotalQuestions,
            IsPublished = e.IsPublished,
            CreationTime = e.CreationTime,
            TotalAttempts = e.Attempts?.Count ?? 0,
            AverageScore = e.Attempts != null && e.Attempts.Count > 0 ? Math.Round(e.Attempts.Average(a => a.Score), 1) : 0
        }).ToList();
    }

    public async Task<ExamDetailDto> GetExamDetailAsync(int id, bool forStudent = false)
    {
        var exam = await _examRepository.GetAll()
            .Include(e => e.Sections)
            .Include(e => e.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exam == null)
        {
            throw new Exception($"Không tìm thấy đề thi với Id = {id}");
        }

        var result = new ExamDetailDto
        {
            Id = exam.Id,
            Title = exam.Title,
            Description = exam.Description,
            DurationMinutes = exam.DurationMinutes,
            TotalQuestions = exam.TotalQuestions,
            IsPublished = exam.IsPublished,
            PublishedDate = exam.PublishedDate,
            CanShuffleQuestions = exam.CanShuffleQuestions,
            CanShuffleOptions = exam.CanShuffleOptions,
            CreationTime = exam.CreationTime
        };

        var random = new Random();

        // Map Sections
        var orderedSections = exam.Sections.OrderBy(s => s.OrderIndex).ToList();
        foreach (var sec in orderedSections)
        {
            var secDto = new PartSectionDto
            {
                Id = sec.Id,
                ExamId = sec.ExamId,
                PartNumber = sec.PartNumber,
                Title = sec.Title,
                PassageContent = sec.PassageContent,
                ImageUrl = sec.ImageUrl,
                OrderIndex = sec.OrderIndex,
                CanShuffleQuestions = sec.CanShuffleQuestions,
                CanShuffleOptions = sec.CanShuffleOptions
            };

            var secQuestions = exam.Questions.Where(q => q.PartSectionId == sec.Id).OrderBy(q => q.OrderIndex).ToList();
            if (forStudent && (sec.CanShuffleQuestions || exam.CanShuffleQuestions) && sec.PartNumber == 5)
            {
                secQuestions = secQuestions.OrderBy(_ => random.Next()).ToList();
            }

            foreach (var q in secQuestions)
            {
                var qDto = MapQuestionToDto(q, forStudent, random, exam.CanShuffleOptions || sec.CanShuffleOptions);
                secDto.Questions.Add(qDto);
            }

            result.Sections.Add(secDto);
        }

        // Map Standalone Questions (Part 5 without section)
        var standaloneQuestions = exam.Questions.Where(q => q.PartSectionId == null).OrderBy(q => q.OrderIndex).ToList();
        if (forStudent && exam.CanShuffleQuestions)
        {
            standaloneQuestions = standaloneQuestions.OrderBy(_ => random.Next()).ToList();
        }

        foreach (var q in standaloneQuestions)
        {
            result.Questions.Add(MapQuestionToDto(q, forStudent, random, exam.CanShuffleOptions));
        }

        return result;
    }

    private QuestionDto MapQuestionToDto(Question q, bool forStudent, Random random, bool canShuffleOpt)
    {
        var qDto = new QuestionDto
        {
            Id = q.Id,
            ExamId = q.ExamId,
            PartSectionId = q.PartSectionId,
            PartNumber = q.PartSection != null ? q.PartSection.PartNumber : 5,
            QuestionNumber = q.QuestionNumber,
            Content = q.Content,
            Explanation = forStudent ? null : q.Explanation,
            OrderIndex = q.OrderIndex,
            CanShuffleOptions = q.CanShuffleOptions
        };

        var options = q.Options.OrderBy(o => o.OrderIndex).ToList();
        if (forStudent && canShuffleOpt && q.CanShuffleOptions)
        {
            options = options.OrderBy(_ => random.Next()).ToList();
        }

        foreach (var opt in options)
        {
            qDto.Options.Add(new AnswerOptionDto
            {
                Id = opt.Id,
                QuestionId = opt.QuestionId,
                OptionLabel = opt.OptionLabel,
                Content = opt.Content,
                IsCorrect = forStudent ? false : opt.IsCorrect,
                OrderIndex = opt.OrderIndex
            });
        }

        return qDto;
    }

    public async Task<UploadDocxResultDto> ImportFromDocxStreamAsync(Stream stream, string fileName)
    {
        var parsed = DocxExamParser.ParseDocx(stream);

        if (!parsed.IsValid)
        {
            return new UploadDocxResultDto
            {
                Success = false,
                ExamTitle = parsed.Title,
                TotalQuestions = parsed.Questions.Count,
                Errors = parsed.ValidationErrors
            };
        }

        // ATOMIC TRANSACTION: Save to database only when 100% valid
        var exam = new Exam
        {
            Title = string.IsNullOrWhiteSpace(parsed.Title) ? Path.GetFileNameWithoutExtension(fileName) : parsed.Title,
            Description = parsed.Description,
            DurationMinutes = parsed.DurationMinutes,
            TotalQuestions = parsed.Questions.Count,
            IsPublished = false, // Must be explicitly published by teacher later
            CanShuffleQuestions = parsed.CanShuffleQuestions,
            CanShuffleOptions = parsed.CanShuffleOptions
        };

        var examId = await _examRepository.InsertAndGetIdAsync(exam);

        // Map section index to database section Id
        var sectionMap = new Dictionary<int, int>();

        for (int i = 0; i < parsed.Sections.Count; i++)
        {
            var pSec = parsed.Sections[i];
            var section = new PartSection
            {
                ExamId = examId,
                PartNumber = pSec.PartNumber,
                Title = pSec.Title,
                PassageContent = pSec.PassageContent,
                ImageUrl = pSec.ImageUrl,
                OrderIndex = pSec.OrderIndex,
                CanShuffleQuestions = pSec.CanShuffleQuestions,
                CanShuffleOptions = pSec.CanShuffleOptions
            };

            var secId = await _sectionRepository.InsertAndGetIdAsync(section);
            sectionMap[i] = secId;
        }

        // Insert questions
        foreach (var pQ in parsed.Questions)
        {
            int? secId = null;
            if (pQ.SectionIndex.HasValue && sectionMap.TryGetValue(pQ.SectionIndex.Value, out int mappedId))
            {
                secId = mappedId;
            }

            var question = new Question
            {
                ExamId = examId,
                PartSectionId = secId,
                QuestionNumber = pQ.QuestionNumber,
                Content = pQ.Content,
                Explanation = pQ.Explanation,
                OrderIndex = pQ.OrderIndex,
                CanShuffleOptions = pQ.CanShuffleOptions
            };

            var qId = await _questionRepository.InsertAndGetIdAsync(question);

            foreach (var pOpt in pQ.Options)
            {
                var option = new AnswerOption
                {
                    QuestionId = qId,
                    OptionLabel = pOpt.OptionLabel,
                    Content = pOpt.Content,
                    IsCorrect = pOpt.IsCorrect,
                    OrderIndex = pOpt.OrderIndex
                };
                await _optionRepository.InsertAsync(option);
            }
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return new UploadDocxResultDto
        {
            Success = true,
            ExamId = examId,
            ExamTitle = exam.Title,
            TotalQuestions = parsed.Questions.Count
        };
    }

    public async Task<bool> TogglePublishAsync(int id)
    {
        var exam = await _examRepository.GetAsync(id);
        exam.IsPublished = !exam.IsPublished;
        exam.PublishedDate = exam.IsPublished ? DateTime.UtcNow : null;
        await _examRepository.UpdateAsync(exam);
        return exam.IsPublished;
    }

    public async Task UpdateExamAsync(UpdateExamDto input)
    {
        var exam = await _examRepository.GetAsync(input.Id);
        exam.Title = input.Title;
        exam.Description = input.Description;
        exam.DurationMinutes = input.DurationMinutes;
        exam.CanShuffleQuestions = input.CanShuffleQuestions;
        exam.CanShuffleOptions = input.CanShuffleOptions;
        await _examRepository.UpdateAsync(exam);
    }

    public async Task UpdateQuestionAsync(UpdateQuestionDto input)
    {
        var question = await _questionRepository.GetAll()
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == input.Id);

        if (question == null)
        {
            throw new Exception($"Không tìm thấy câu hỏi với Id = {input.Id}");
        }

        question.Content = input.Content;
        question.Explanation = input.Explanation;

        foreach (var optDto in input.Options)
        {
            var opt = question.Options.FirstOrDefault(o => o.Id == optDto.Id);
            if (opt != null)
            {
                opt.Content = optDto.Content;
                opt.OptionLabel = optDto.OptionLabel;
                opt.IsCorrect = (opt.Id == input.CorrectOptionId || optDto.IsCorrect);
            }
        }

        await _questionRepository.UpdateAsync(question);
    }

    public async Task DeleteExamAsync(int id)
    {
        await _examRepository.DeleteAsync(id);
    }

    public async Task<ExamResultDto> SubmitExamAsync(SubmitExamInputDto input)
    {
        var exam = await _examRepository.GetAll()
            .Include(e => e.Questions)
                .ThenInclude(q => q.Options)
            .Include(e => e.Sections)
            .FirstOrDefaultAsync(e => e.Id == input.ExamId);

        if (exam == null)
        {
            throw new Exception($"Không tìm thấy đề thi với Id = {input.ExamId}");
        }

        var submitTime = DateTime.UtcNow;
        var answerMap = input.Answers.ToDictionary(a => a.QuestionId, a => a.SelectedOptionId);

        int totalCorrect = 0;
        int totalWrong = 0;
        int totalSkipped = 0;

        var questionResults = new List<QuestionResultDto>();
        var attemptAnswers = new List<AttemptAnswer>();

        foreach (var q in exam.Questions.OrderBy(q => q.QuestionNumber))
        {
            var correctOpt = q.Options.FirstOrDefault(o => o.IsCorrect);
            answerMap.TryGetValue(q.Id, out int? selectedOptId);

            bool isSkipped = !selectedOptId.HasValue || selectedOptId.Value == 0;
            bool isCorrect = false;

            if (isSkipped)
            {
                totalSkipped++;
            }
            else
            {
                isCorrect = correctOpt != null && selectedOptId.Value == correctOpt.Id;
                if (isCorrect) totalCorrect++;
                else totalWrong++;
            }

            var selectedOpt = q.Options.FirstOrDefault(o => o.Id == selectedOptId);

            var sec = q.PartSectionId.HasValue ? exam.Sections.FirstOrDefault(s => s.Id == q.PartSectionId.Value) : null;

            questionResults.Add(new QuestionResultDto
            {
                QuestionId = q.Id,
                QuestionNumber = q.QuestionNumber,
                PartNumber = sec?.PartNumber ?? 5,
                PassageTitle = sec?.Title,
                PassageContent = sec?.PassageContent,
                ImageUrl = sec?.ImageUrl,
                Content = q.Content,
                Explanation = q.Explanation,
                SelectedOptionId = isSkipped ? null : selectedOptId,
                SelectedOptionLabel = selectedOpt?.OptionLabel,
                CorrectOptionId = correctOpt?.Id ?? 0,
                CorrectOptionLabel = correctOpt?.OptionLabel ?? "N/A",
                IsCorrect = isCorrect,
                IsSkipped = isSkipped,
                Options = q.Options.OrderBy(o => o.OrderIndex).Select(o => new OptionResultDto
                {
                    Id = o.Id,
                    OptionLabel = o.OptionLabel,
                    Content = o.Content,
                    IsCorrect = o.IsCorrect
                }).ToList()
            });

            attemptAnswers.Add(new AttemptAnswer
            {
                QuestionId = q.Id,
                SelectedOptionId = isSkipped ? null : selectedOptId,
                IsCorrect = isCorrect
            });
        }

        int scaledScore = ToeicScoreCalculator.CalculateReadingScore(totalCorrect, exam.Questions.Count);

        var attempt = new ExamAttempt
        {
            ExamId = exam.Id,
            CandidateName = string.IsNullOrWhiteSpace(input.CandidateName) ? "Thí sinh tự do" : input.CandidateName,
            CandidateCode = string.IsNullOrWhiteSpace(input.CandidateCode) ? "SV001" : input.CandidateCode,
            StartTime = input.StartTime,
            SubmitTime = submitTime,
            TotalQuestions = exam.Questions.Count,
            TotalCorrect = totalCorrect,
            TotalWrong = totalWrong,
            TotalSkipped = totalSkipped,
            Score = scaledScore,
            IsFinished = true
        };

        var attemptId = await _attemptRepository.InsertAndGetIdAsync(attempt);

        foreach (var aa in attemptAnswers)
        {
            aa.ExamAttemptId = attemptId;
            await _attemptAnswerRepository.InsertAsync(aa);
        }

        var duration = submitTime - input.StartTime;
        var durationStr = $"{(int)duration.TotalMinutes} phút {duration.Seconds} giây";

        return new ExamResultDto
        {
            AttemptId = attemptId,
            ExamId = exam.Id,
            ExamTitle = exam.Title,
            CandidateName = attempt.CandidateName,
            CandidateCode = attempt.CandidateCode,
            StartTime = input.StartTime,
            SubmitTime = submitTime,
            DurationSpent = durationStr,
            TotalQuestions = exam.Questions.Count,
            TotalCorrect = totalCorrect,
            TotalWrong = totalWrong,
            TotalSkipped = totalSkipped,
            Score = scaledScore,
            Percentage = exam.Questions.Count > 0 ? Math.Round((double)totalCorrect / exam.Questions.Count * 100, 1) : 0,
            Questions = questionResults
        };
    }

    public async Task<ExamResultDto> GetExamResultAsync(int attemptId)
    {
        var attempt = await _attemptRepository.GetAll()
            .Include(a => a.Exam)
                .ThenInclude(e => e.Sections)
            .Include(a => a.Answers)
                .ThenInclude(ans => ans.Question)
                    .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(a => a.Id == attemptId);

        if (attempt == null)
        {
            throw new Exception($"Không tìm thấy kết quả lượt thi với Id = {attemptId}");
        }

        var questionResults = new List<QuestionResultDto>();
        var orderedAnswers = attempt.Answers.OrderBy(a => a.Question.QuestionNumber).ToList();

        foreach (var ans in orderedAnswers)
        {
            var q = ans.Question;
            var correctOpt = q.Options.FirstOrDefault(o => o.IsCorrect);
            var selectedOpt = q.Options.FirstOrDefault(o => o.Id == ans.SelectedOptionId);
            var sec = q.PartSectionId.HasValue ? attempt.Exam.Sections.FirstOrDefault(s => s.Id == q.PartSectionId.Value) : null;

            questionResults.Add(new QuestionResultDto
            {
                QuestionId = q.Id,
                QuestionNumber = q.QuestionNumber,
                PartNumber = sec?.PartNumber ?? 5,
                PassageTitle = sec?.Title,
                PassageContent = sec?.PassageContent,
                ImageUrl = sec?.ImageUrl,
                Content = q.Content,
                Explanation = q.Explanation,
                SelectedOptionId = ans.SelectedOptionId,
                SelectedOptionLabel = selectedOpt?.OptionLabel,
                CorrectOptionId = correctOpt?.Id ?? 0,
                CorrectOptionLabel = correctOpt?.OptionLabel ?? "N/A",
                IsCorrect = ans.IsCorrect,
                IsSkipped = !ans.SelectedOptionId.HasValue,
                Options = q.Options.OrderBy(o => o.OrderIndex).Select(o => new OptionResultDto
                {
                    Id = o.Id,
                    OptionLabel = o.OptionLabel,
                    Content = o.Content,
                    IsCorrect = o.IsCorrect
                }).ToList()
            });
        }

        var duration = (attempt.SubmitTime ?? DateTime.UtcNow) - attempt.StartTime;
        var durationStr = $"{(int)duration.TotalMinutes} phút {duration.Seconds} giây";

        return new ExamResultDto
        {
            AttemptId = attempt.Id,
            ExamId = attempt.ExamId,
            ExamTitle = attempt.Exam.Title,
            CandidateName = attempt.CandidateName,
            CandidateCode = attempt.CandidateCode,
            StartTime = attempt.StartTime,
            SubmitTime = attempt.SubmitTime ?? DateTime.UtcNow,
            DurationSpent = durationStr,
            TotalQuestions = attempt.TotalQuestions,
            TotalCorrect = attempt.TotalCorrect,
            TotalWrong = attempt.TotalWrong,
            TotalSkipped = attempt.TotalSkipped,
            Score = attempt.Score,
            Percentage = attempt.TotalQuestions > 0 ? Math.Round((double)attempt.TotalCorrect / attempt.TotalQuestions * 100, 1) : 0,
            Questions = questionResults
        };
    }

    public async Task<ExamStatisticDto> GetExamStatisticsAsync(int examId)
    {
        var exam = await _examRepository.GetAll()
            .Include(e => e.Questions)
                .ThenInclude(q => q.Options)
            .Include(e => e.Sections)
            .FirstOrDefaultAsync(e => e.Id == examId);

        if (exam == null)
        {
            throw new Exception($"Không tìm thấy đề thi với Id = {examId}");
        }

        var attempts = await _attemptRepository.GetAll()
            .Include(a => a.Answers)
            .Where(a => a.ExamId == examId)
            .ToListAsync();

        var result = new ExamStatisticDto
        {
            ExamId = exam.Id,
            ExamTitle = exam.Title,
            TotalAttempts = attempts.Count,
            AverageScore = attempts.Count > 0 ? Math.Round(attempts.Average(a => a.Score), 1) : 0,
            HighestScore = attempts.Count > 0 ? attempts.Max(a => a.Score) : 0,
            LowestScore = attempts.Count > 0 ? attempts.Min(a => a.Score) : 0,
            AverageCorrectQuestions = attempts.Count > 0 ? Math.Round(attempts.Average(a => a.TotalCorrect), 1) : 0
        };

        if (attempts.Count > 0)
        {
            var questionStats = new List<IncorrectQuestionStatDto>();
            var allAnswers = attempts.SelectMany(a => a.Answers).ToList();

            foreach (var q in exam.Questions)
            {
                var qAnswers = allAnswers.Where(a => a.QuestionId == q.Id).ToList();
                int qAttempts = qAnswers.Count;
                int incorrectCount = qAnswers.Count(a => !a.IsCorrect);
                double pct = qAttempts > 0 ? Math.Round((double)incorrectCount / qAttempts * 100, 1) : 0;

                var correctOpt = q.Options.FirstOrDefault(o => o.IsCorrect);
                var sec = q.PartSectionId.HasValue ? exam.Sections.FirstOrDefault(s => s.Id == q.PartSectionId.Value) : null;

                questionStats.Add(new IncorrectQuestionStatDto
                {
                    QuestionId = q.Id,
                    QuestionNumber = q.QuestionNumber,
                    PartNumber = sec?.PartNumber ?? 5,
                    PassageTitle = sec?.Title,
                    Content = q.Content,
                    CorrectAnswer = correctOpt != null ? $"{correctOpt.OptionLabel}. {correctOpt.Content}" : "N/A",
                    Explanation = q.Explanation,
                    TotalAttempts = qAttempts,
                    IncorrectCount = incorrectCount,
                    IncorrectPercentage = pct
                });
            }

            // Top 10 most missed questions
            result.TopIncorrectQuestions = questionStats
                .OrderByDescending(s => s.IncorrectCount)
                .ThenByDescending(s => s.IncorrectPercentage)
                .Take(10)
                .ToList();
        }

        return result;
    }

    public async Task<List<StudentSummaryDto>> GetAllStudentsAsync(string searchTerm = null)
    {
        var query = _attemptRepository.GetAll()
            .Include(a => a.Exam)
            .Where(a => a.IsFinished);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var s = searchTerm.Trim().ToLower();
            query = query.Where(a => a.CandidateName.ToLower().Contains(s) || a.CandidateCode.ToLower().Contains(s));
        }

        var attempts = await query.ToListAsync();

        var groups = attempts
            .GroupBy(a => string.IsNullOrWhiteSpace(a.CandidateCode) ? a.CandidateName : a.CandidateCode)
            .ToList();

        var result = new List<StudentSummaryDto>();
        foreach (var g in groups)
        {
            var latest = g.OrderByDescending(a => a.SubmitTime ?? a.StartTime).First();
            result.Add(new StudentSummaryDto
            {
                CandidateCode = latest.CandidateCode,
                CandidateName = latest.CandidateName,
                TotalAttempts = g.Count(),
                HighestScore = g.Max(a => a.Score),
                AverageScore = Math.Round(g.Average(a => a.Score), 1),
                LatestAttemptTime = latest.SubmitTime ?? latest.StartTime,
                LatestExamTitle = latest.Exam?.Title ?? "N/A"
            });
        }

        return result.OrderByDescending(s => s.LatestAttemptTime).ToList();
    }

    public async Task<StudentDetailDto> GetStudentDetailAsync(string candidateCode)
    {
        if (string.IsNullOrWhiteSpace(candidateCode)) return null;

        var attempts = await _attemptRepository.GetAll()
            .Include(a => a.Exam)
            .Where(a => a.IsFinished && (a.CandidateCode == candidateCode || a.CandidateName == candidateCode))
            .OrderByDescending(a => a.SubmitTime ?? a.StartTime)
            .ToListAsync();

        if (attempts.Count == 0) return null;

        var latest = attempts.First();
        var result = new StudentDetailDto
        {
            CandidateCode = latest.CandidateCode,
            CandidateName = latest.CandidateName,
            TotalAttempts = attempts.Count,
            HighestScore = attempts.Max(a => a.Score),
            AverageScore = Math.Round(attempts.Average(a => a.Score), 1),
            Attempts = attempts.Select(a => {
                var duration = (a.SubmitTime.HasValue && a.SubmitTime > a.StartTime)
                    ? (a.SubmitTime.Value - a.StartTime)
                    : TimeSpan.FromMinutes(45);
                var pct = a.TotalQuestions > 0 ? Math.Round((double)a.TotalCorrect / a.TotalQuestions * 100, 1) : 0;
                return new StudentAttemptItemDto
                {
                    AttemptId = a.Id,
                    ExamId = a.ExamId,
                    ExamTitle = a.Exam?.Title ?? "N/A",
                    StartTime = a.StartTime,
                    SubmitTime = a.SubmitTime,
                    DurationSpent = $"{(int)duration.TotalMinutes} phút {duration.Seconds} giây",
                    Score = a.Score,
                    TotalCorrect = a.TotalCorrect,
                    TotalWrong = a.TotalWrong,
                    TotalSkipped = a.TotalSkipped,
                    AccuracyPercentage = pct
                };
            }).ToList()
        };

        return result;
    }
}
