using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace _2380600189.Exams;

public class Exam : FullAuditedEntity<int>
{
    public const int MaxTitleLength = 256;
    public const int MaxDescriptionLength = 2000;

    public string Title { get; set; }
    public string Description { get; set; }
    public int DurationMinutes { get; set; } = 75;
    public int TotalQuestions { get; set; } = 100;
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedDate { get; set; }
    public bool CanShuffleQuestions { get; set; } = false;
    public bool CanShuffleOptions { get; set; } = false;

    public virtual ICollection<PartSection> Sections { get; set; } = new List<PartSection>();
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    public virtual ICollection<ExamAttempt> Attempts { get; set; } = new List<ExamAttempt>();
}
