using Abp.Domain.Entities;

namespace _2380600189.Exams;

public class AnswerOption : Entity<int>
{
    public int QuestionId { get; set; }
    public virtual Question Question { get; set; }

    /// <summary>
    /// Option label: "A", "B", "C", "D"
    /// </summary>
    public string OptionLabel { get; set; }

    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}
