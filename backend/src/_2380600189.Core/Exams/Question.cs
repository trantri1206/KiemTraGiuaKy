using Abp.Domain.Entities;
using System.Collections.Generic;

namespace _2380600189.Exams;

public class Question : Entity<int>
{
    public int ExamId { get; set; }
    public virtual Exam Exam { get; set; }

    public int? PartSectionId { get; set; }
    public virtual PartSection PartSection { get; set; }

    /// <summary>
    /// Sequential question number in exam, e.g. 101 to 200
    /// </summary>
    public int QuestionNumber { get; set; }

    public string Content { get; set; }

    /// <summary>
    /// Explanation / Hint for the question
    /// </summary>
    public string Explanation { get; set; }

    public int OrderIndex { get; set; }
    public bool CanShuffleOptions { get; set; } = true;

    public virtual ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
}
