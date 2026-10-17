using Shouldly;
using System.IO;
using Xunit;
using _2380600189.Exams;

namespace _2380600189.Tests.Exams;

public class DocxExamParser_Tests : _2380600189TestBase
{
    [Fact]
    public void Should_Generate_And_Parse_Full_100_Question_TOEIC_Docx()
    {
        // 1. Generate docx file
        var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "TOEIC_Reading_Test_01.docx");
        var fullPath = Path.GetFullPath(targetPath);

        SampleDocxGenerator.GenerateSampleDocx(fullPath);
        File.Exists(fullPath).ShouldBeTrue();

        // Also copy to root of backend
        var backendCopy = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TOEIC_Reading_Test_01.docx");
        File.Copy(fullPath, Path.GetFullPath(backendCopy), true);

        // 2. Parse docx
        using var stream = File.OpenRead(fullPath);
        var parsed = DocxExamParser.ParseDocx(stream);

        // 3. Assertions
        parsed.ValidationErrors.Count.ShouldBe(0, string.Join("; ", parsed.ValidationErrors));
        parsed.IsValid.ShouldBeTrue();
        parsed.Questions.Count.ShouldBe(100);
        parsed.DurationMinutes.ShouldBe(75);

        // Part 5 check: 101 to 130
        parsed.Questions.Find(q => q.QuestionNumber == 101).ShouldNotBeNull();
        parsed.Questions.Find(q => q.QuestionNumber == 130).ShouldNotBeNull();

        // Part 6 check: 131 to 146
        parsed.Questions.Find(q => q.QuestionNumber == 131).ShouldNotBeNull();
        parsed.Questions.Find(q => q.QuestionNumber == 146).ShouldNotBeNull();

        // Part 7 check: 147 to 200
        parsed.Questions.Find(q => q.QuestionNumber == 147).ShouldNotBeNull();
        parsed.Questions.Find(q => q.QuestionNumber == 200).ShouldNotBeNull();

        // Every question must have exactly 4 choices and exactly 1 correct answer
        foreach (var q in parsed.Questions)
        {
            q.Options.Count.ShouldBe(4);
            q.Options.FindAll(o => o.IsCorrect).Count.ShouldBe(1);
        }
    }

    [Fact]
    public void Should_Generate_And_Parse_Official_ETS_Test02_Docx()
    {
        // 1. Generate docx file for Official ETS Test 02
        var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "TOEIC_RC_Test_02.docx");
        var fullPath = Path.GetFullPath(targetPath);

        EtsToeicTest02Generator.GenerateOfficialTest02(fullPath);
        File.Exists(fullPath).ShouldBeTrue();

        // Also copy to root of backend
        var backendCopy = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TOEIC_RC_Test_02.docx");
        File.Copy(fullPath, Path.GetFullPath(backendCopy), true);

        // 2. Parse docx
        using var stream = File.OpenRead(fullPath);
        var parsed = DocxExamParser.ParseDocx(stream);

        // 3. Assertions
        parsed.ValidationErrors.Count.ShouldBe(0, string.Join("; ", parsed.ValidationErrors));
        parsed.IsValid.ShouldBeTrue();
        parsed.Title.ShouldBe("TOEIC Reading Actual Test 02");
        parsed.Questions.Count.ShouldBe(100);
        parsed.DurationMinutes.ShouldBe(75);

        // Check each part
        parsed.Questions.FindAll(q => q.PartNumber == 5).Count.ShouldBe(30);
        parsed.Questions.FindAll(q => q.PartNumber == 6).Count.ShouldBe(16);
        parsed.Questions.FindAll(q => q.PartNumber == 7).Count.ShouldBe(54);

        foreach (var q in parsed.Questions)
        {
            q.Options.Count.ShouldBe(4);
            q.Options.FindAll(o => o.IsCorrect).Count.ShouldBe(1);
            q.Explanation.ShouldNotBeNullOrWhiteSpace();
        }
    }
}
