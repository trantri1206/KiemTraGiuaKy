using Abp.Zero.EntityFrameworkCore;
using _2380600189.Authorization.Roles;
using _2380600189.Authorization.Users;
using _2380600189.MultiTenancy;
using Microsoft.EntityFrameworkCore;

using _2380600189.Exams;

namespace _2380600189.EntityFrameworkCore;

public class _2380600189DbContext : AbpZeroDbContext<Tenant, Role, User, _2380600189DbContext>
{
    /* Define a DbSet for each entity of the application */
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<PartSection> PartSections { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
    public virtual DbSet<ExamAttempt> ExamAttempts { get; set; }
    public virtual DbSet<AttemptAnswer> AttemptAnswers { get; set; }

    public _2380600189DbContext(DbContextOptions<_2380600189DbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exam>(b =>
        {
            b.Property(e => e.Title).IsRequired().HasMaxLength(Exam.MaxTitleLength);
            b.Property(e => e.Description).HasMaxLength(Exam.MaxDescriptionLength);
        });

        modelBuilder.Entity<PartSection>(b =>
        {
            b.HasOne(s => s.Exam)
                .WithMany(e => e.Sections)
                .HasForeignKey(s => s.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Question>(b =>
        {
            b.HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(q => q.PartSection)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.PartSectionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AnswerOption>(b =>
        {
            b.HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ExamAttempt>(b =>
        {
            b.HasOne(a => a.Exam)
                .WithMany(e => e.Attempts)
                .HasForeignKey(a => a.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AttemptAnswer>(b =>
        {
            b.HasOne(a => a.ExamAttempt)
                .WithMany(att => att.Answers)
                .HasForeignKey(a => a.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(a => a.SelectedOption)
                .WithMany()
                .HasForeignKey(a => a.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
