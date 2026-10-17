using Abp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace _2380600189.Exams;

public class ExamAttempt : Entity<int>
{
    public int ExamId { get; set; }
    public virtual Exam Exam { get; set; }

    public long? UserId { get; set; }
    public string CandidateName { get; set; }
    public string CandidateCode { get; set; }

    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? SubmitTime { get; set; }

    public int TotalQuestions { get; set; }
    public int TotalCorrect { get; set; }
    public int TotalWrong { get; set; }
    public int TotalSkipped { get; set; }

    /// <summary>
    /// TOEIC Reading score (5 - 495)
    /// </summary>
    public int Score { get; set; }

    public bool IsFinished { get; set; } = false;

    public virtual ICollection<AttemptAnswer> Answers { get; set; } = new List<AttemptAnswer>();
}
