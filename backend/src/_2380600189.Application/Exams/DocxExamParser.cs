using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2380600189.Exams;

public class ParsedExamModel
{
    public string Title { get; set; } = "TOEIC Reading Test";
    public string Description { get; set; } = "Đề thi TOEIC Reading";
    public int DurationMinutes { get; set; } = 75;
    public bool CanShuffleQuestions { get; set; } = false;
    public bool CanShuffleOptions { get; set; } = false;

    public List<ParsedSectionModel> Sections { get; set; } = new();
    public List<ParsedQuestionModel> Questions { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
    public bool IsValid => ValidationErrors.Count == 0;
}

public class ParsedSectionModel
{
    public int PartNumber { get; set; } = 5;
    public string Title { get; set; } = string.Empty;
    public string PassageContent { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool CanShuffleQuestions { get; set; }
    public bool CanShuffleOptions { get; set; }
    public List<ParsedQuestionModel> Questions { get; set; } = new();
}

public class ParsedQuestionModel
{
    public int QuestionNumber { get; set; }
    public int PartNumber { get; set; } = 5;
    public string Content { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool CanShuffleOptions { get; set; } = true;
    public List<ParsedOptionModel> Options { get; set; } = new();
    public int? SectionIndex { get; set; }
}

public class ParsedOptionModel
{
    public string OptionLabel { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}

public class DocxExamParser
{
    public static ParsedExamModel ParseDocx(Stream stream)
    {
        var result = new ParsedExamModel();
        var lines = new List<string>();

        try
        {
            using var doc = WordprocessingDocument.Open(stream, false);
            var body = doc.MainDocumentPart?.Document?.Body;
            if (body == null)
            {
                result.ValidationErrors.Add("File Word không có nội dung (Body trống).");
                return result;
            }

            foreach (var element in body.Elements())
            {
                if (element is Paragraph p)
                {
                    var text = p.InnerText?.Trim();
                    if (!string.IsNullOrEmpty(text))
                    {
                        lines.Add(text);
                    }
                }
                else if (element is Table t)
                {
                    foreach (var row in t.Elements<TableRow>())
                    {
                        var rowText = string.Join(" | ", row.Elements<TableCell>().Select(c => c.InnerText?.Trim()).Where(x => !string.IsNullOrEmpty(x)));
                        if (!string.IsNullOrEmpty(rowText))
                        {
                            lines.Add(rowText);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.ValidationErrors.Add($"Lỗi đọc file Word .docx: {ex.Message}");
            return result;
        }

        if (lines.Count == 0)
        {
            result.ValidationErrors.Add("File Word rỗng hoặc không chứa văn bản.");
            return result;
        }

        ParseLines(lines, result);
        ValidateExam(result);

        return result;
    }

    private static void ParseLines(List<string> lines, ParsedExamModel result)
    {
        int currentPart = 5;
        bool defaultShuffleQ = false;
        bool defaultShuffleOpt = false;

        ParsedSectionModel currentSection = null;
        ParsedQuestionModel currentQuestion = null;
        var passageBuilder = new StringBuilder();
        var questionContentBuilder = new StringBuilder();
        var explanationBuilder = new StringBuilder();
        bool isReadingPassage = false;
        bool isReadingExplanation = false;

        void FinalizeQuestion()
        {
            if (currentQuestion != null)
            {
                if (questionContentBuilder.Length > 0 && string.IsNullOrWhiteSpace(currentQuestion.Content))
                {
                    currentQuestion.Content = questionContentBuilder.ToString().Trim();
                }
                if (explanationBuilder.Length > 0 && string.IsNullOrWhiteSpace(currentQuestion.Explanation))
                {
                    currentQuestion.Explanation = explanationBuilder.ToString().Trim();
                }

                if (currentSection != null)
                {
                    currentSection.Questions.Add(currentQuestion);
                }
                result.Questions.Add(currentQuestion);
                currentQuestion = null;
            }
            questionContentBuilder.Clear();
            explanationBuilder.Clear();
            isReadingExplanation = false;
        }

        void FinalizeSection()
        {
            FinalizeQuestion();
            if (currentSection != null)
            {
                if (passageBuilder.Length > 0 && string.IsNullOrWhiteSpace(currentSection.PassageContent))
                {
                    currentSection.PassageContent = passageBuilder.ToString().Trim();
                }
                result.Sections.Add(currentSection);
                currentSection = null;
            }
            passageBuilder.Clear();
            isReadingPassage = false;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            var rawLine = lines[i].Trim();
            if (string.IsNullOrEmpty(rawLine)) continue;

            // 1. Exam Metadata tags
            var matchTitle = Regex.Match(rawLine, @"^\[(?:EXAM_TITLE|TITLE|TIEU_DE)\]\s*[:]?\s*(.*)$", RegexOptions.IgnoreCase);
            if (matchTitle.Success)
            {
                result.Title = matchTitle.Groups[1].Value.Trim();
                continue;
            }

            var matchDesc = Regex.Match(rawLine, @"^\[(?:EXAM_DESCRIPTION|DESCRIPTION|MO_TA)\]\s*[:]?\s*(.*)$", RegexOptions.IgnoreCase);
            if (matchDesc.Success)
            {
                result.Description = matchDesc.Groups[1].Value.Trim();
                continue;
            }

            var matchDuration = Regex.Match(rawLine, @"^\[(?:DURATION|TIME|THOI_GIAN)\]\s*[:]?\s*(\d+)", RegexOptions.IgnoreCase);
            if (matchDuration.Success && int.TryParse(matchDuration.Groups[1].Value, out int dur))
            {
                result.DurationMinutes = dur;
                continue;
            }

            var matchGlobalShuffleQ = Regex.Match(rawLine, @"^\[(?:SHUFFLE_QUESTIONS|HOAN_VI_CAU_HOI)\]\s*[:]?\s*(TRUE|FALSE|1|0|YES|NO|CO|KHONG)", RegexOptions.IgnoreCase);
            if (matchGlobalShuffleQ.Success)
            {
                var val = matchGlobalShuffleQ.Groups[1].Value.ToUpper();
                defaultShuffleQ = val is "TRUE" or "1" or "YES" or "CO";
                result.CanShuffleQuestions = defaultShuffleQ;
                continue;
            }

            var matchGlobalShuffleOpt = Regex.Match(rawLine, @"^\[(?:SHUFFLE_OPTIONS|HOAN_VI_DAP_AN)\]\s*[:]?\s*(TRUE|FALSE|1|0|YES|NO|CO|KHONG)", RegexOptions.IgnoreCase);
            if (matchGlobalShuffleOpt.Success)
            {
                var val = matchGlobalShuffleOpt.Groups[1].Value.ToUpper();
                defaultShuffleOpt = val is "TRUE" or "1" or "YES" or "CO";
                result.CanShuffleOptions = defaultShuffleOpt;
                continue;
            }

            // 2. Part tag
            var matchPart = Regex.Match(rawLine, @"^\[(?:PART)\]?\s*[:]?\s*([567])", RegexOptions.IgnoreCase);
            if (!matchPart.Success)
            {
                matchPart = Regex.Match(rawLine, @"^\[PART\s*([567])\]", RegexOptions.IgnoreCase);
            }
            if (matchPart.Success && int.TryParse(matchPart.Groups[1].Value, out int pNum))
            {
                FinalizeSection();
                currentPart = pNum;
                continue;
            }

            // 3. Passage tags for Part 6 & 7
            if (Regex.IsMatch(rawLine, @"^\[(?:END_PASSAGE)\]", RegexOptions.IgnoreCase))
            {
                FinalizeSection();
                continue;
            }

            var matchPassageTitle = Regex.Match(rawLine, @"^\[(?:PASSAGE_TITLE|TITLE_BAI_DOC)\]\s*[:]?\s*(.*)$", RegexOptions.IgnoreCase);
            if (matchPassageTitle.Success)
            {
                if (currentSection != null)
                {
                    currentSection.Title = matchPassageTitle.Groups[1].Value.Trim();
                }
                continue;
            }

            if (Regex.IsMatch(rawLine, @"^\[(?:PASSAGE_CONTENT)\]\s*[:]?\s*$", RegexOptions.IgnoreCase))
            {
                isReadingPassage = true;
                continue;
            }

            var matchImage = Regex.Match(rawLine, @"^\[(?:IMAGE|HINH_ANH)\]\s*[:]?\s*(.*)$", RegexOptions.IgnoreCase);
            if (matchImage.Success)
            {
                if (currentSection != null)
                {
                    currentSection.ImageUrl = matchImage.Groups[1].Value.Trim();
                }
                continue;
            }

            var matchPassageStart = Regex.Match(rawLine, @"^\[PASSAGE(?:\s*[:]\s*|\s+)([^\]]+)\]", RegexOptions.IgnoreCase);
            if (!matchPassageStart.Success)
            {
                matchPassageStart = Regex.Match(rawLine, @"^\[PASSAGE\]", RegexOptions.IgnoreCase);
            }
            if (matchPassageStart.Success)
            {
                FinalizeSection();
                isReadingPassage = true;
                var titleVal = matchPassageStart.Groups.Count > 1 ? matchPassageStart.Groups[1].Value.Trim() : string.Empty;
                currentSection = new ParsedSectionModel
                {
                    PartNumber = currentPart,
                    Title = titleVal,
                    OrderIndex = result.Sections.Count + 1,
                    CanShuffleQuestions = defaultShuffleQ,
                    CanShuffleOptions = defaultShuffleOpt
                };
                continue;
            }

            // 4. Question tags
            var matchQuestion = Regex.Match(rawLine, @"^\[(?:QUESTION|CAU|CAU_HOI)\]?\s*[:]?\s*(\d+)", RegexOptions.IgnoreCase);
            if (!matchQuestion.Success)
            {
                matchQuestion = Regex.Match(rawLine, @"^\[QUESTION\s*(\d+)\]", RegexOptions.IgnoreCase);
            }
            if (!matchQuestion.Success)
            {
                // Also accept: "101." or "Question 101." if standalone
                matchQuestion = Regex.Match(rawLine, @"^(?:Question\s+)?(\d{3})\.\s*(.*)$", RegexOptions.IgnoreCase);
            }

            if (matchQuestion.Success && int.TryParse(matchQuestion.Groups[1].Value, out int qNum))
            {
                isReadingPassage = false;
                FinalizeQuestion();

                currentQuestion = new ParsedQuestionModel
                {
                    QuestionNumber = qNum,
                    PartNumber = currentPart,
                    OrderIndex = result.Questions.Count + 1,
                    CanShuffleOptions = defaultShuffleOpt,
                    SectionIndex = currentSection != null ? result.Sections.Count : null
                };

                if (matchQuestion.Groups.Count > 2 && !string.IsNullOrWhiteSpace(matchQuestion.Groups[2].Value))
                {
                    questionContentBuilder.AppendLine(matchQuestion.Groups[2].Value.Trim());
                }
                continue;
            }

            if (Regex.IsMatch(rawLine, @"^\[(?:END_QUESTION)\]", RegexOptions.IgnoreCase))
            {
                FinalizeQuestion();
                continue;
            }

            // 5. Options tags: [A], [B], [C], [D] or [A*], (A), (A)*
            var matchOption = Regex.Match(rawLine, @"^\[([A-D])(\*)?\]\s*(.*)$", RegexOptions.IgnoreCase);
            if (!matchOption.Success)
            {
                matchOption = Regex.Match(rawLine, @"^\(([A-D])\)(\*)?\s*(.*)$", RegexOptions.IgnoreCase);
            }
            if (!matchOption.Success)
            {
                matchOption = Regex.Match(rawLine, @"^([A-D])[\.\)](\*)?\s+(.*)$", RegexOptions.IgnoreCase);
            }

            if (matchOption.Success && currentQuestion != null)
            {
                var label = matchOption.Groups[1].Value.ToUpper();
                bool isCorrect = matchOption.Groups[2].Value == "*";
                var optContent = matchOption.Groups[3].Value.Trim();

                // Check if asterisk is at the end of content: "her *"
                if (optContent.EndsWith("*"))
                {
                    isCorrect = true;
                    optContent = optContent.TrimEnd('*').Trim();
                }

                currentQuestion.Options.Add(new ParsedOptionModel
                {
                    OptionLabel = label,
                    Content = optContent,
                    IsCorrect = isCorrect,
                    OrderIndex = currentQuestion.Options.Count + 1
                });
                continue;
            }

            // 6. Explicit correct answer tag: [CORRECT: B] or [ANSWER: B]
            var matchCorrect = Regex.Match(rawLine, @"^\[(?:CORRECT|ANSWER|DAP_AN)\]?\s*[:]?\s*([A-D])", RegexOptions.IgnoreCase);
            if (matchCorrect.Success && currentQuestion != null)
            {
                var targetLabel = matchCorrect.Groups[1].Value.ToUpper();
                foreach (var opt in currentQuestion.Options)
                {
                    opt.IsCorrect = (opt.OptionLabel == targetLabel);
                }
                continue;
            }

            // 7. Explanation / Hint tag
            var matchExp = Regex.Match(rawLine, @"^\[(?:EXPLANATION|HINT|GIAI_THICH|GOI_Y)\]\s*[:]?\s*(.*)$", RegexOptions.IgnoreCase);
            if (matchExp.Success && currentQuestion != null)
            {
                isReadingExplanation = true;
                if (!string.IsNullOrWhiteSpace(matchExp.Groups[1].Value))
                {
                    explanationBuilder.AppendLine(matchExp.Groups[1].Value.Trim());
                }
                continue;
            }

            // 8. Accumulate content based on state
            if (isReadingExplanation && currentQuestion != null)
            {
                explanationBuilder.AppendLine(rawLine);
            }
            else if (isReadingPassage && currentSection != null)
            {
                passageBuilder.AppendLine(rawLine);
            }
            else if (currentQuestion != null && currentQuestion.Options.Count == 0)
            {
                questionContentBuilder.AppendLine(rawLine);
            }
            else if (currentSection != null && currentQuestion == null)
            {
                passageBuilder.AppendLine(rawLine);
            }
        }

        // Finalize remaining
        FinalizeSection();
    }

    private static void ValidateExam(ParsedExamModel exam)
    {
        if (string.IsNullOrWhiteSpace(exam.Title))
        {
            exam.ValidationErrors.Add("Thiếu tiêu đề đề thi ([EXAM_TITLE]).");
        }

        if (exam.Questions.Count == 0)
        {
            exam.ValidationErrors.Add("Không tìm thấy câu hỏi nào trong đề thi.");
            return;
        }

        // Validate sequential question numbering
        var seenNumbers = new HashSet<int>();
        int? previousNumber = null;

        foreach (var q in exam.Questions)
        {
            if (seenNumbers.Contains(q.QuestionNumber))
            {
                exam.ValidationErrors.Add($"Trùng số thứ tự câu hỏi: Câu {q.QuestionNumber} xuất hiện nhiều hơn 1 lần.");
            }
            seenNumbers.Add(q.QuestionNumber);

            if (previousNumber.HasValue && q.QuestionNumber != previousNumber.Value + 1)
            {
                exam.ValidationErrors.Add($"Sai thứ tự câu hỏi: Phát hiện nhảy câu từ {previousNumber.Value} sang {q.QuestionNumber}. Các câu phải liên tục.");
            }
            previousNumber = q.QuestionNumber;

            // Question prompt content check
            if (q.PartNumber == 5 && string.IsNullOrWhiteSpace(q.Content))
            {
                exam.ValidationErrors.Add($"Câu {q.QuestionNumber} (Part 5): Thiếu nội dung câu hỏi.");
            }

            // Options check: Exactly 4 options A, B, C, D
            if (q.Options.Count != 4)
            {
                exam.ValidationErrors.Add($"Câu {q.QuestionNumber}: Phải có đúng 4 đáp án (A, B, C, D). Hiện tại có {q.Options.Count} đáp án.");
            }

            var correctCount = q.Options.Count(o => o.IsCorrect);
            if (correctCount == 0)
            {
                exam.ValidationErrors.Add($"Câu {q.QuestionNumber}: Chưa được chỉ định đáp án đúng (ví dụ: [A*] hoặc [CORRECT: A]).");
            }
            else if (correctCount > 1)
            {
                exam.ValidationErrors.Add($"Câu {q.QuestionNumber}: Có nhiều hơn 1 đáp án đúng ({correctCount} đáp án).");
            }
        }

        // Validate Part 6 & 7 sections
        foreach (var sec in exam.Sections)
        {
            if (sec.PartNumber is 6 or 7)
            {
                if (string.IsNullOrWhiteSpace(sec.PassageContent))
                {
                    exam.ValidationErrors.Add($"Đoạn văn '{sec.Title}' (Part {sec.PartNumber}): Thiếu nội dung bài đọc.");
                }
                if (sec.Questions.Count == 0)
                {
                    exam.ValidationErrors.Add($"Đoạn văn '{sec.Title}' (Part {sec.PartNumber}): Không có câu hỏi nào thuộc đoạn văn này.");
                }
            }
        }
    }
}
