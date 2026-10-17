using Abp.Domain.Entities;
using System.Collections.Generic;

namespace _2380600189.Exams;

public class PartSection : Entity<int>
{
    public int ExamId { get; set; }
    public virtual Exam Exam { get; set; }

    /// <summary>
    /// Part 5, 6, or 7
    /// </summary>
    public int PartNumber { get; set; }

    public string Title { get; set; }
    public string PassageContent { get; set; }
    public string ImageUrl { get; set; }
    public int OrderIndex { get; set; }
    public bool CanShuffleQuestions { get; set; } = false;
    public bool CanShuffleOptions { get; set; } = false;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
