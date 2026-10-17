using System;
using System.Collections.Generic;

namespace _2380600189.Exams.Dto;

public class CandidateAnswerInputDto
{
    public int QuestionId { get; set; }
    public int? SelectedOptionId { get; set; }
}

public class SubmitExamInputDto
{
    public int ExamId { get; set; }
    public string CandidateName { get; set; }
    public string CandidateCode { get; set; }
    public DateTime StartTime { get; set; }
    public List<CandidateAnswerInputDto> Answers { get; set; } = new();
}

public class OptionResultDto
{
    public int Id { get; set; }
    public string OptionLabel { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
}

public class QuestionResultDto
{
    public int QuestionId { get; set; }
    public int QuestionNumber { get; set; }
    public int PartNumber { get; set; }
    public string PassageTitle { get; set; }
    public string PassageContent { get; set; }
    public string ImageUrl { get; set; }
    public string Content { get; set; }
    public string Explanation { get; set; }
    public int? SelectedOptionId { get; set; }
    public string SelectedOptionLabel { get; set; }
    public int CorrectOptionId { get; set; }
    public string CorrectOptionLabel { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsSkipped { get; set; }
    public List<OptionResultDto> Options { get; set; } = new();
}

public class ExamResultDto
{
    public int AttemptId { get; set; }
    public int ExamId { get; set; }
    public string ExamTitle { get; set; }
    public string CandidateName { get; set; }
    public string CandidateCode { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime SubmitTime { get; set; }
    public string DurationSpent { get; set; }
    public int TotalQuestions { get; set; }
    public int TotalCorrect { get; set; }
    public int TotalWrong { get; set; }
    public int TotalSkipped { get; set; }
    public int Score { get; set; } // Scaled TOEIC score (5-495)
    public double Percentage { get; set; }
    public List<QuestionResultDto> Questions { get; set; } = new();
}

public class IncorrectQuestionStatDto
{
    public int QuestionId { get; set; }
    public int QuestionNumber { get; set; }
    public int PartNumber { get; set; }
    public string PassageTitle { get; set; }
    public string Content { get; set; }
    public string CorrectAnswer { get; set; }
    public string Explanation { get; set; }
    public int TotalAttempts { get; set; }
    public int IncorrectCount { get; set; }
    public double IncorrectPercentage { get; set; }
}

public class ExamStatisticDto
{
    public int ExamId { get; set; }
    public string ExamTitle { get; set; }
    public int TotalAttempts { get; set; }
    public double AverageScore { get; set; }
    public int HighestScore { get; set; }
    public int LowestScore { get; set; }
    public double AverageCorrectQuestions { get; set; }
    public List<IncorrectQuestionStatDto> TopIncorrectQuestions { get; set; } = new();
}

public class UploadDocxResultDto
{
    public bool Success { get; set; }
    public int? ExamId { get; set; }
    public string ExamTitle { get; set; }
    public int TotalQuestions { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class UpdateExamDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int DurationMinutes { get; set; }
    public bool CanShuffleQuestions { get; set; }
    public bool CanShuffleOptions { get; set; }
}

public class UpdateQuestionDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Explanation { get; set; }
    public int CorrectOptionId { get; set; }
    public List<UpdateOptionDto> Options { get; set; } = new();
}

public class UpdateOptionDto
{
    public int Id { get; set; }
    public string OptionLabel { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
}

public class StudentSummaryDto
{
    public string CandidateCode { get; set; }
    public string CandidateName { get; set; }
    public int TotalAttempts { get; set; }
    public int HighestScore { get; set; }
    public double AverageScore { get; set; }
    public DateTime LatestAttemptTime { get; set; }
    public string LatestExamTitle { get; set; }
}

public class StudentAttemptItemDto
{
    public int AttemptId { get; set; }
    public int ExamId { get; set; }
    public string ExamTitle { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? SubmitTime { get; set; }
    public string DurationSpent { get; set; }
    public int Score { get; set; }
    public int TotalCorrect { get; set; }
    public int TotalWrong { get; set; }
    public int TotalSkipped { get; set; }
    public double AccuracyPercentage { get; set; }
}

public class StudentDetailDto
{
    public string CandidateCode { get; set; }
    public string CandidateName { get; set; }
    public int TotalAttempts { get; set; }
    public int HighestScore { get; set; }
    public double AverageScore { get; set; }
    public List<StudentAttemptItemDto> Attempts { get; set; } = new();
}
