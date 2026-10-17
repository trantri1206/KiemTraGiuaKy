using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace _2380600189.Exams.Dto;

public class AnswerOptionDto : EntityDto<int>
{
    public int QuestionId { get; set; }
    public string OptionLabel { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}

public class QuestionDto : EntityDto<int>
{
    public int ExamId { get; set; }
    public int? PartSectionId { get; set; }
    public int PartNumber { get; set; }
    public int QuestionNumber { get; set; }
    public string Content { get; set; }
    public string Explanation { get; set; }
    public int OrderIndex { get; set; }
    public bool CanShuffleOptions { get; set; }
    public List<AnswerOptionDto> Options { get; set; } = new();
}

public class PartSectionDto : EntityDto<int>
{
    public int ExamId { get; set; }
    public int PartNumber { get; set; }
    public string Title { get; set; }
    public string PassageContent { get; set; }
    public string ImageUrl { get; set; }
    public int OrderIndex { get; set; }
    public bool CanShuffleQuestions { get; set; }
    public bool CanShuffleOptions { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}

public class ExamDetailDto : EntityDto<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DurationMinutes { get; set; }
    public int TotalQuestions { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedDate { get; set; }
    public bool CanShuffleQuestions { get; set; }
    public bool CanShuffleOptions { get; set; }
    public DateTime CreationTime { get; set; }

    public List<PartSectionDto> Sections { get; set; } = new();
    public List<QuestionDto> Questions { get; set; } = new();
}

public class ExamListDto : EntityDto<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int DurationMinutes { get; set; }
    public int TotalQuestions { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreationTime { get; set; }
    public int TotalAttempts { get; set; }
    public double AverageScore { get; set; }
}
