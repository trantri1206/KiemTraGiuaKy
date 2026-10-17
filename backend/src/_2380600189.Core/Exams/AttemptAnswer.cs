using Abp.Domain.Entities;

namespace _2380600189.Exams;

public class AttemptAnswer : Entity<int>
{
    public int ExamAttemptId { get; set; }
    public virtual ExamAttempt ExamAttempt { get; set; }

    public int QuestionId { get; set; }
    public virtual Question Question { get; set; }

    public int? SelectedOptionId { get; set; }
    public virtual AnswerOption SelectedOption { get; set; }

    public bool IsCorrect { get; set; }
}
