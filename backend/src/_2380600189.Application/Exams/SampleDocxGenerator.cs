using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace _2380600189.Exams;

public static class SampleDocxGenerator
{
    public static void GenerateSampleDocx(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        using var wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
        var mainPart = wordDoc.AddMainDocumentPart();
        mainPart.Document = new Document();
        var body = mainPart.Document.AppendChild(new Body());

        void AddP(string text, bool isBold = false, string colorHex = null)
        {
            var p = new Paragraph();
            var run = new Run();
            var rPr = new RunProperties();

            if (isBold) rPr.Append(new Bold());
            if (!string.IsNullOrEmpty(colorHex))
            {
                rPr.Append(new Color { Val = colorHex });
            }
            rPr.Append(new FontSize { Val = "24" }); // 12pt
            rPr.Append(new RunFonts { Ascii = "Arial" });

            run.Append(rPr);
            run.Append(new Text(text));
            p.Append(run);
            body.Append(p);
        }

        // Header info
        AddP("[EXAM_TITLE]: TOEIC Reading Official Test - 100 Questions", true, "003366");
        AddP("[EXAM_DESCRIPTION]: Đề thi chuẩn hóa TOEIC Reading 100 câu: Part 5 (Câu hỏi độc lập), Part 6 (Điền đoạn văn), Part 7 (Đọc hiểu văn bản dài)", false, "555555");
        AddP("[DURATION]: 75", true, "990000");
        AddP("[SHUFFLE_QUESTIONS]: FALSE");
        AddP("[SHUFFLE_OPTIONS]: TRUE");
        AddP("");

        // ==========================================
        // PART 5: QUESTIONS 101 - 130 (30 questions)
        // ==========================================
        AddP("[PART: 5]", true, "0066CC");
        AddP("");

        var part5Data = new[]
        {
            (101, "Ms. Durkin asked for volunteers to help -------- with the employee fitness program.",
             new[] { ("A", "she", false), ("B", "her", true), ("C", "hers", false), ("D", "herself", false) },
             "Sau ngoại động từ 'help' cần một tân ngữ là đại từ 'her'."),
            (102, "Laser printer cartridges can be purchased at the supply room on the third floor -------- the end of the hall.",
             new[] { ("A", "at", true), ("B", "of", false), ("C", "by", false), ("D", "in", false) },
             "Cụm chỉ vị trí: 'at the end of the hall' (ở cuối hành lang)."),
            (103, "The executive board congratulated Mr. Sato on his -------- promotion to regional director.",
             new[] { ("A", "recent", true), ("B", "recently", false), ("C", "recency", false), ("D", "most recently", false) },
             "Trước danh từ 'promotion' cần một tính từ bổ nghĩa: 'recent promotion' (sự thăng chức gần đây)."),
            (104, "Please submit your completed expense reports by Friday so that accounting can -------- your reimbursement.",
             new[] { ("A", "process", true), ("B", "processed", false), ("C", "processing", false), ("D", "procession", false) },
             "Sau trợ động từ 'can' cần một động từ nguyên mẫu: 'can process'."),
            (105, "The new office complex will be located -------- the central train station, making it convenient for commuters.",
             new[] { ("A", "near", true), ("B", "nearly", false), ("C", "nearness", false), ("D", "nearing", false) },
             "Cần giới từ 'near' chỉ vị trí gần ga tàu trung tâm."),
            (106, "All employees must attend the safety seminar -------- they have already completed the online module.",
             new[] { ("A", "unless", true), ("B", "despite", false), ("C", "except", false), ("D", "during", false) },
             "'Unless' (trừ khi) là liên từ nối mệnh đề: trừ khi họ đã hoàn thành học phần trực tuyến."),
            (107, "The warranty covers replacement parts for five years from the date of --------.",
             new[] { ("A", "purchase", true), ("B", "purchaser", false), ("C", "purchasing", false), ("D", "purchased", false) },
             "Cụm từ cố định: 'date of purchase' (ngày mua hàng)."),
            (108, "Dr. Kim's research on solar energy was -------- praised by colleagues in the engineering department.",
             new[] { ("A", "high", false), ("B", "highly", true), ("C", "highest", false), ("D", "height", false) },
             "Cần phó từ 'highly' để bổ nghĩa cho động từ bị động 'was highly praised'."),
            (109, "The hotel offers a complimentary breakfast to all guests, as well as -------- Wi-Fi access in every room.",
             new[] { ("A", "free", true), ("B", "freely", false), ("C", "freedom", false), ("D", "freeing", false) },
             "Cần tính từ 'free' bổ nghĩa cho danh từ 'Wi-Fi access'."),
            (110, "Because of the bad weather, flight departures have been delayed -------- notice.",
             new[] { ("A", "further", false), ("B", "until further", true), ("C", "from further", false), ("D", "to further", false) },
             "Cụm thành ngữ chuẩn: 'until further notice' (cho đến khi có thông báo mới)."),
            (111, "The marketing team has developed an -------- advertising campaign for the holiday season.",
             new[] { ("A", "innovate", false), ("B", "innovative", true), ("C", "innovatively", false), ("D", "innovation", false) },
             "Cần tính từ 'innovative' đứng trước danh từ ghép 'advertising campaign'."),
            (112, "Mr. Davies will be available for meetings -------- 2:00 P.M. and 4:30 P.M. today.",
             new[] { ("A", "between", true), ("B", "among", false), ("C", "through", false), ("D", "from", false) },
             "Cấu trúc 'between A and B' (giữa khoảng thời gian A và B)."),
            (113, "Our company's goal is to provide exceptional service that -------- customer expectations.",
             new[] { ("A", "exceeds", true), ("B", "exceeding", false), ("C", "excessive", false), ("D", "exceed", false) },
             "Mệnh đề quan hệ với chủ ngữ 'service' (ngôi 3 số ít) cần động từ chia thì hiện tại: 'exceeds'."),
            (114, "The maintenance crew worked -------- throughout the weekend to repair the burst water pipe.",
             new[] { ("A", "diligent", false), ("B", "diligence", false), ("C", "diligently", true), ("D", "most diligent", false) },
             "Cần phó từ 'diligently' (chăm chỉ, miệt mài) bổ nghĩa cho động từ 'worked'."),
            (115, "Customers who wish to return an item must present the original receipt within 14 days of --------.",
             new[] { ("A", "delivery", true), ("B", "deliver", false), ("C", "delivering", false), ("D", "delivered", false) },
             "Sau giới từ 'of' cần danh từ 'delivery' (sự giao hàng)."),
            (116, "The factory's daily output has increased by 15 percent -------- the introduction of automated machinery.",
             new[] { ("A", "since", true), ("B", "while", false), ("C", "so", false), ("D", "although", false) },
             "Hiện tại hoàn thành 'has increased' đi với 'since + mốc thời gian / sự kiện'."),
            (117, "All attendees are requested to remain in their seats -------- the keynote speech has concluded.",
             new[] { ("A", "until", true), ("B", "afterward", false), ("C", "while", false), ("D", "during", false) },
             "'Until' (cho đến khi) nối mệnh đề thời gian."),
            (118, "Ms. Lopez will be leading the presentation because she is -------- with the project details.",
             new[] { ("A", "familiar", true), ("B", "familiarize", false), ("C", "familiarity", false), ("D", "familiarly", false) },
             "Cấu trúc tính từ: 'be familiar with' (quen thuộc, nắm rõ)."),
            (119, "The committee reached a unanimous -------- regarding the appointment of the new CEO.",
             new[] { ("A", "decide", false), ("B", "decisive", false), ("C", "decision", true), ("D", "decisively", false) },
             "Sau tính từ 'unanimous' cần danh từ: 'unanimous decision' (quyết định đồng thuận)."),
            (120, "Although the new software is complex, the user manual explains the steps --------.",
             new[] { ("A", "clear", false), ("B", "clearly", true), ("C", "clearness", false), ("D", "clarify", false) },
             "Phó từ 'clearly' bổ nghĩa cho động từ 'explains'."),
            (121, "Please make sure that all confidential documents are placed in the -------- safe before leaving.",
             new[] { ("A", "secure", true), ("B", "securely", false), ("C", "security", false), ("D", "securing", false) },
             "Tính từ 'secure' bổ nghĩa cho danh từ 'safe' (két sắt an toàn)."),
            (122, "The annual charity gala was a huge success, raising -------- funds than last year.",
             new[] { ("A", "much", false), ("B", "many", false), ("C", "more", true), ("D", "most", false) },
             "So sánh hơn đi với 'than': 'more funds than'."),
            (123, "Employees are encouraged to submit feedback, -------- anonymously or with their names attached.",
             new[] { ("A", "either", true), ("B", "neither", false), ("C", "both", false), ("D", "also", false) },
             "Cấu trúc tương quan 'either ... or ...' (hoặc ... hoặc ...)."),
            (124, "The architectural firm submitted a -------- design for the proposed waterfront library.",
             new[] { ("A", "impress", false), ("B", "impressive", true), ("C", "impression", false), ("D", "impressively", false) },
             "Tính từ 'impressive' bổ nghĩa cho danh từ 'design'."),
            (125, "Neither Mr. Henderson -------- his assistant was able to locate the signed contract.",
             new[] { ("A", "or", false), ("B", "nor", true), ("C", "and", false), ("D", "but", false) },
             "Cấu trúc tương quan 'neither ... nor ...' (cả ... và ... đều không)."),
            (126, "The catering service requires a 20 percent deposit to confirm any reservation made -------- than two weeks in advance.",
             new[] { ("A", "more", true), ("B", "much", false), ("C", "over", false), ("D", "greater", false) },
             "Cụm 'more than two weeks in advance' (trước hơn hai tuần)."),
            (127, "Technicians are currently testing the backup generator to verify its --------.",
             new[] { ("A", "reliable", false), ("B", "reliably", false), ("C", "reliability", true), ("D", "reliance", false) },
             "Sau tính từ sở hữu 'its' cần danh từ chỉ phẩm chất: 'reliability' (độ tin cậy)."),
            (128, "The survey indicated that passengers were overwhelmingly -------- with the airline's in-flight entertainment.",
             new[] { ("A", "satisfied", true), ("B", "satisfying", false), ("C", "satisfaction", false), ("D", "satisfactory", false) },
             "Tính từ mang nghĩa bị động cảm xúc: 'satisfied with' (hài lòng với)."),
            (129, "To avoid cancellation fees, clients must give notification at least 24 hours --------.",
             new[] { ("A", "prior", false), ("B", "beforehand", true), ("C", "early", false), ("D", "preceding", false) },
             "Trạng từ 'beforehand' đứng cuối câu nghĩa là 'trước đó' (24 hours beforehand)."),
            (130, "The newly appointed supervisor has earned the respect of her team through her fair and -------- leadership.",
             new[] { ("A", "consistent", true), ("B", "consistently", false), ("C", "consistency", false), ("D", "consist", false) },
             "Tính từ đồng đẳng liên kết bởi 'and': 'fair and consistent leadership'.")
        };

        foreach (var q in part5Data)
        {
            AddP($"[QUESTION: {q.Item1}]", true, "333333");
            AddP(q.Item2);
            foreach (var opt in q.Item3)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item4}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }

        // ==========================================
        // PART 6: QUESTIONS 131 - 146 (16 questions)
        // ==========================================
        AddP("[PART: 6]", true, "0066CC");
        AddP("");

        // Passage 1: Questions 131-134
        AddP("[PASSAGE: 131-134]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 131-134 refer to the following advertisement.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("With Global Strength Gym's 30-day trial period, you get the opportunity to try out our classes, equipment, and facilities. ---[131]--- It's completely risk-free! To sign up, we require your contact information and payment details, but you will only be charged if you are a member for ---[132]--- 30 days. If you decide within this time that you no longer want to be a member of Global Strength, ---[133]--- visit our Web site at www.gsgym.com. On the Membership page, elect to ---[134]--- your membership and enter the necessary information. It's that easy!");
        AddP("");

        var part6P1 = new[]
        {
            (131, new[] {
                ("A", "Throughout the trial, you pay nothing and sign no contract.", true),
                ("B", "Weight-lifting classes are not currently available.", false),
                ("C", "A cash deposit is required when you sign up for membership.", false),
                ("D", "All questions should be e-mailed to customerservice@gsgym.com.", false)
            }, "Câu trước nói về thời gian dùng thử 30 ngày và câu sau là 'It's completely risk-free!'. Do đó câu A hoàn toàn phù hợp."),

            (132, new[] {
                ("A", "not even", false),
                ("B", "almost", false),
                ("C", "over", true),
                ("D", "less than", false)
            }, "'over 30 days' (hơn 30 ngày) phù hợp ngữ cảnh bạn chỉ bị tính tiền nếu tiếp tục là thành viên trên 30 ngày."),

            (133, new[] {
                ("A", "justly", false),
                ("B", "regularly", false),
                ("C", "evenly", false),
                ("D", "simply", true)
            }, "'simply visit our Web site' (chỉ cần truy cập trang web của chúng tôi)."),

            (134, new[] {
                ("A", "extend", false),
                ("B", "renew", false),
                ("C", "cancel", true),
                ("D", "initiate", false)
            }, "Ngữ cảnh nếu không muốn là thành viên nữa thì chọn 'cancel your membership' (hủy tư cách thành viên).")
        };

        foreach (var q in part6P1)
        {
            AddP($"[QUESTION: {q.Item1}]", true);
            foreach (var opt in q.Item2)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item3}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }
        AddP("[END_PASSAGE]");
        AddP("");

        // Passage 2: Questions 135-138
        AddP("[PASSAGE: 135-138]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 135-138 refer to the following email.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("To: All Staff <staff@brentoncorp.com>\nFrom: Human Resources <hr@brentoncorp.com>\nDate: October 12\nSubject: Office Renovation Schedule\n\nPlease be advised that the fourth-floor renovation project will commence next Monday. Workers will be installing new carpeting and repainting walls. Consequently, employees on the fourth floor will be temporarily ---[135]--- to conference rooms on the third floor. We apologize for any inconvenience this temporary arrangement may cause. We have made every effort to ensure that the transition will be as ---[136]--- as possible. Please ensure all personal items are removed from your desks by Friday evening. Work is expected to be ---[137]--- by October 25. ---[138]---. Thank you for your cooperation.");
        AddP("");

        var part6P2 = new[]
        {
            (135, new[] {
                ("A", "relocated", true),
                ("B", "relocate", false),
                ("C", "relocating", false),
                ("D", "relocation", false)
            }, "Cấu trúc bị động: 'will be temporarily relocated' (sẽ được tạm thời chuyển sang...)."),

            (136, new[] {
                ("A", "smooth", true),
                ("B", "smoothly", false),
                ("C", "smoothness", false),
                ("D", "smoothing", false)
            }, "Cấu trúc 'as + adj + as possible' sau động từ liên kết 'be': 'as smooth as possible'."),

            (137, new[] {
                ("A", "completed", true),
                ("B", "complete", false),
                ("C", "completion", false),
                ("D", "completely", false)
            }, "Cấu trúc bị động 'expected to be completed by...' (dự kiến sẽ hoàn thành trước ngày 25)."),

            (138, new[] {
                ("A", "Please direct any questions to the facilities manager.", true),
                ("B", "Our annual company picnic has been rescheduled.", false),
                ("C", "The fourth-floor cafeteria will remain open throughout.", false),
                ("D", "New badges will be issued to all full-time personnel.", false)
            }, "Câu kết email thông báo dự án thường hướng dẫn liên hệ nếu có câu hỏi thắc mắc.")
        };

        foreach (var q in part6P2)
        {
            AddP($"[QUESTION: {q.Item1}]", true);
            foreach (var opt in q.Item2)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item3}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }
        AddP("[END_PASSAGE]");
        AddP("");

        // Passage 3: Questions 139-142
        AddP("[PASSAGE: 139-142]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 139-142 refer to the following memorandum.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Memorandum\nTo: Department Heads\nFrom: Executive Committee\nDate: September 5\nSubject: Travel Budget Guidelines\n\nIn light of recent fiscal reviews, we have updated the corporate travel policy. Effective immediately, all international flights must be approved in advance by a division director. ---[139]---, hotel accommodations should not exceed standard corporate rates. Employees traveling on company business are encouraged to book reservations ---[140]--- our preferred corporate travel portal. This portal offers negotiated discounts with major airlines and hotel chains. ---[141]---. Failure to follow these guidelines may result in ---[142]--- of expense reimbursements.");
        AddP("");

        var part6P3 = new[]
        {
            (139, new[] {
                ("A", "Furthermore", true),
                ("B", "In contrast", false),
                ("C", "Otherwise", false),
                ("D", "Instead", false)
            }, "'Furthermore' (Hơn nữa) dùng để bổ sung quy định mới về chi phí khách sạn."),

            (140, new[] {
                ("A", "through", true),
                ("B", "into", false),
                ("C", "between", false),
                ("D", "about", false)
            }, "'book reservations through our portal' (đặt chỗ thông qua cổng thông tin du lịch)."),

            (141, new[] {
                ("A", "These savings directly benefit our overall operational efficiency.", true),
                ("B", "The marketing seminar will be held in Paris this year.", false),
                ("C", "Rental cars are no longer permitted under any circumstance.", false),
                ("D", "Passports must be renewed six months prior to expiration.", false)
            }, "Câu liền trước nói về 'negotiated discounts', câu tiếp theo nhấn mạnh lợi ích tiết kiệm chi phí."),

            (142, new[] {
                ("A", "delays", true),
                ("B", "delayed", false),
                ("C", "delaying", false),
                ("D", "delayable", false)
            }, "'result in + noun': 'delays of expense reimbursements' (sự chậm trễ trong việc hoàn trả chi phí).")
        };

        foreach (var q in part6P3)
        {
            AddP($"[QUESTION: {q.Item1}]", true);
            foreach (var opt in q.Item2)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item3}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }
        AddP("[END_PASSAGE]");
        AddP("");

        // Passage 4: Questions 143-146
        AddP("[PASSAGE: 143-146]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 143-146 refer to the following press release.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("FOR IMMEDIATE RELEASE\nKENSINGTON, October 1 -- Apex Solar Technologies announced today the completion of its newest manufacturing plant in northern Ohio. The 200,000-square-foot facility is expected to create over 400 clean energy jobs over the next two years. The plant will produce next-generation photovoltaic cells that boast a 25 percent increase in energy conversion efficiency. ---[143]---. 'We are thrilled to expand our presence in Ohio,' said Apex CEO Rebecca Vance. 'This state has a talented workforce and a supportive business environment.' Operations at the site are slated to begin next month, with initial product shipments scheduled for early spring. Local community leaders have praised the initiative, citing its economic and environmental ---[144]---. To apply for open positions, candidates should visit the Apex career page. Applications will be reviewed on a rolling basis until all roles are ---[145]---. ---[146]---.");
        AddP("");

        var part6P4 = new[]
        {
            (143, new[] {
                ("A", "This technological breakthrough will significantly reduce costs for consumers.", true),
                ("B", "Several manufacturing plants in the area have closed recently.", false),
                ("C", "Solar panels require regular cleaning to function properly.", false),
                ("D", "Competitors have also released similar energy products.", false)
            }, "Câu trước nói về pin quang điện thế hệ mới tăng 25% hiệu suất, câu tiếp nối giải thích đột phá công nghệ này giúp giảm chi phí."),

            (144, new[] {
                ("A", "benefits", true),
                ("B", "benefited", false),
                ("C", "beneficial", false),
                ("D", "beneficially", false)
            }, "Sau tính từ 'economic and environmental' cần danh từ số nhiều: 'benefits' (các lợi ích)."),

            (145, new[] {
                ("A", "filled", true),
                ("B", "fill", false),
                ("C", "filling", false),
                ("D", "filler", false)
            }, "Cụm 'roles are filled' (các vị trí được tuyển đủ)."),

            (146, new[] {
                ("A", "Early submissions are strongly encouraged.", true),
                ("B", "The facility tour will take place tomorrow afternoon.", false),
                ("C", "No prior manufacturing experience is necessary for executives.", false),
                ("D", "Apex Solar was founded in 2005 by three college students.", false)
            }, "Đoạn văn kết thúc bằng lời kêu gọi tuyển dụng: hồ sơ nộp sớm được khuyến khích.")
        };

        foreach (var q in part6P4)
        {
            AddP($"[QUESTION: {q.Item1}]", true);
            foreach (var opt in q.Item2)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item3}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }
        AddP("[END_PASSAGE]");
        AddP("");

        // ==========================================
        // PART 7: QUESTIONS 147 - 200 (54 questions)
        // ==========================================
        AddP("[PART: 7]", true, "0066CC");
        AddP("");

        // Passage 1: Questions 147-148 (Text Message Chain)
        AddP("[PASSAGE: 147-148]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 147-148 refer to the following text message chain.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Kenji Sato [10:14 A.M.]\nHi Marisa, have the printed brochures for the Henderson meeting arrived yet? The delivery driver said they would be here by 10:00 A.M.\n\nMarisa Cruz [10:17 A.M.]\nNot yet, Kenji. I checked with the front desk a few minutes ago. I can call the print shop to check on the status if you want.\n\nKenji Sato [10:18 A.M.]\nPlease do. The meeting starts at 1:30 P.M., and I need at least 30 minutes to review the packets beforehand.\n\nMarisa Cruz [10:25 A.M.]\nGood news! The driver had a minor delay due to road construction on Elm Street, but he just pulled into our loading dock. I'll bring the boxes up to your office right away.");
        AddP("");

        var part7P1 = new[]
        {
            (147, "Why is Kenji Sato concerned about the delivery?",
             new[] {
                 ("A", "He needs to distribute the brochures before 1:30 P.M.", true),
                 ("B", "The print shop printed the wrong company logo.", false),
                 ("C", "He has to leave the office for a flight at noon.", false),
                 ("D", "The front desk refused to accept the packages.", false)
             }, "Kenji Sato cần xem trước tài liệu 30 phút trước cuộc họp lúc 1:30 P.M."),

            (148, "What caused the delay in delivery?",
             new[] {
                 ("A", "A vehicle mechanical breakdown", false),
                 ("B", "Road construction on Elm Street", true),
                 ("C", "Inclement weather conditions", false),
                 ("D", "Incorrect shipping address", false)
             }, "Marisa Cruz nhắn: 'The driver had a minor delay due to road construction on Elm Street'.")
        };

        foreach (var q in part7P1)
        {
            AddP($"[QUESTION: {q.Item1}]", true);
            AddP(q.Item2);
            foreach (var opt in q.Item3)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item4}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }
        AddP("[END_PASSAGE]");
        AddP("");

        // Passage 2: Questions 149-150 (Notice)
        AddP("[PASSAGE: 149-150]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 149-150 refer to the following notice.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("LAKEVIEW FITNESS CENTER -- LOCKER ROOM POLICY\nEffective November 1\n\nTo ensure a clean and secure environment for all members, Lakeview Fitness Center will be implementing an updated locker room policy.\n\n1. Day-Use Lockers: Lockers marked with a blue badge are available for daily use only. All personal locks must be removed by 10:00 P.M. each night. Locks left on day-use lockers overnight will be cut, and contents will be placed in the Lost and Found.\n2. Permanent Rental Lockers: Members wishing to reserve a dedicated locker for monthly or annual use may inquire at the reception desk. Monthly rentals are $15, while annual rentals are discounted to $150.\n3. Valuable Items: Lakeview Fitness Center is not responsible for lost or stolen property. Please do not leave unattended valuables in unlocked lockers.\n\nThank you for helping us maintain a safe and orderly facility.");
        AddP("");

        var part7P2 = new[]
        {
            (149, "What happens to locks left on day-use lockers after 10:00 P.M.?",
             new[] {
                 ("A", "They will be confiscated and kept indefinitely.", false),
                 ("B", "They will be cut off by center staff.", true),
                 ("C", "Members will receive a $15 fine on their account.", false),
                 ("D", "The locker combination will be reset automatically.", false)
             }, "Thông báo nêu rõ: 'Locks left on day-use lockers overnight will be cut'."),

            (150, "How much does it cost to rent a permanent locker for one year?",
             new[] {
                 ("A", "$15", false),
                 ("B", "$100", false),
                 ("C", "$150", true),
                 ("D", "$180", false)
             }, "Thông báo ghi: 'while annual rentals are discounted to $150'.")
        };

        foreach (var q in part7P2)
        {
            AddP($"[QUESTION: {q.Item1}]", true);
            AddP(q.Item2);
            foreach (var opt in q.Item3)
            {
                var optTag = opt.Item3 ? $"[{opt.Item1}*]" : $"[{opt.Item1}]";
                AddP($"{optTag} {opt.Item2}");
            }
            AddP($"[EXPLANATION]: {q.Item4}", false, "885500");
            AddP("[END_QUESTION]");
            AddP("");
        }
        AddP("[END_PASSAGE]");
        AddP("");

        // Passages 3-15 for remaining questions up to 200:
        // We will generate structured real TOEIC reading passages for Questions 151 through 200
        GenerateRemainingPart7(body, AddP);

        wordDoc.Save();
    }

    private static void GenerateRemainingPart7(Body body, System.Action<string, bool, string> addP)
    {
        // 151-152 (Schedule)
        addP("[PASSAGE: 151-152]", true, "993300");
        addP("[PASSAGE_TITLE]: Questions 151-152 refer to the following conference schedule.", false, null);
        addP("[PASSAGE_CONTENT]:\nMidwest Business Summit -- Friday, November 14\nVenue: Grand Ballroom, Hyatt Regency Chicago\n\n8:30 A.M. - Registration and Continental Breakfast\n9:15 A.M. - Welcome Address by Dr. Angela Harris (President, Chamber of Commerce)\n10:00 A.M. - Keynote: Emerging Trends in Artificial Intelligence (David Sterling, CTO of Novus Tech)\n11:30 A.M. - Panel Discussion: Sustainable Supply Chains (Moderator: Priya Nair)\n1:00 P.M. - Networking Luncheon (Sponsored by Apex Logistics)\n2:30 P.M. - Concurrent Breakout Workshops (Rooms A-D)\n4:30 P.M. - Closing Reception and Vendor Exhibitions", false, null);
        addP("", false, null);

        var qList1 = new[]
        {
            (151, "What event is scheduled to take place at 10:00 A.M.?",
             new[] { ("A", "A continental breakfast", false), ("B", "A panel on supply chains", false), ("C", "A keynote address on AI", true), ("D", "A closing vendor exhibition", false) },
             "Lịch trình ghi rõ 10:00 A.M. là 'Keynote: Emerging Trends in Artificial Intelligence'."),
            (152, "Who is sponsoring the 1:00 P.M. luncheon?",
             new[] { ("A", "Apex Logistics", true), ("B", "Novus Tech", false), ("C", "Chamber of Commerce", false), ("D", "Hyatt Regency", false) },
             "Dòng 1:00 P.M. ghi: 'Sponsored by Apex Logistics'.")
        };
        RenderQuestions(qList1, addP);
        addP("[END_PASSAGE]", false, null);
        addP("", false, null);

        // 153-155 (Article)
        addP("[PASSAGE: 153-155]", true, "993300");
        addP("[PASSAGE_TITLE]: Questions 153-155 refer to the following magazine article.", false, null);
        addP("[PASSAGE_CONTENT]:\nGREENWICH (August 22) -- Local artisanal bakery Crust & Crumb celebrated its fifth anniversary this week by unveiling an ambitious expansion plan. Founded in 2021 by head baker Julian Moreau, the shop initially operated out of a modest 600-square-foot storefront on Maple Street. Thanks to rave reviews in regional culinary publications and a devoted social media following, demand for Moreau's sourdough loaves and delicate French pastries has consistently outpaced supply.\n\nBeginning next month, Crust & Crumb will renovate the adjacent retail space, more than doubling its customer seating area and adding a commercial-grade stone deck oven. The upgrade will also allow Moreau to introduce a new evening menu featuring wood-fired artisanal pizzas and organic wines. 'We've always wanted to create a neighborhood gathering spot that extends beyond morning coffee,' said Moreau. The grand reopening is tentatively slated for October 15, with special promotional discounts for the first 100 visitors.", false, null);
        addP("", false, null);

        var qList2 = new[]
        {
            (153, "When was Crust & Crumb founded?",
             new[] { ("A", "In 2019", false), ("B", "In 2021", true), ("C", "In 2023", false), ("D", "In 2025", false) },
             "Bài báo ghi: 'Founded in 2021 by head baker Julian Moreau'."),
            (154, "What new offering will be introduced following the renovation?",
             new[] { ("A", "Wood-fired pizzas and organic wines", true), ("B", "Gluten-free sandwich delivery", false), ("C", "Weekend baking masterclasses", false), ("D", "Pre-packaged grocery items", false) },
             "Đoạn 2 ghi: 'introduce a new evening menu featuring wood-fired artisanal pizzas and organic wines'."),
            (155, "What will happen on October 15?",
             new[] { ("A", "The bakery will host a charity auction.", false), ("B", "The grand reopening will take place.", true), ("C", "Julian Moreau will step down as head baker.", false), ("D", "A second location will open in Greenwich.", false) },
             "Câu cuối ghi: 'The grand reopening is tentatively slated for October 15'.")
        };
        RenderQuestions(qList2, addP);
        addP("[END_PASSAGE]", false, null);
        addP("", false, null);

        // Batch generation for 156-200 with realistic TOEIC Reading scenarios
        var scenarios = new (int StartQ, int EndQ, string Title, string Content, (int QNum, string Prompt, (string L, string T, bool C)[] Opts, string Exp)[])[]
        {
            (156, 158, "Questions 156-158 refer to the following email.",
             "To: Marcus Vance <m.vance@vancetech.com>\nFrom: Elena Rostova <e.rostova@premierhotel.com>\nDate: July 3\nSubject: Booking Confirmation #88412\n\nDear Mr. Vance,\n\nThank you for choosing the Premier Hotel for your upcoming stay in San Francisco. We have reserved an Executive King Room for you checking in on Wednesday, July 18, and checking out on Sunday, July 22.\n\nYour room includes complimentary high-speed internet, access to our 24-hour fitness center, and daily breakfast at the Bistro on the 2nd floor. Valet parking is available for an additional fee of $35 per night. Please let us know if your arrival will be after 10:00 P.M. so we can guarantee your reservation.",
             new[] {
                 (156, "How many nights will Mr. Vance stay at the hotel?",
                  new[] { ("A", "2 nights", false), ("B", "3 nights", false), ("C", "4 nights", true), ("D", "5 nights", false) },
                  "Nhận phòng thứ Tư ngày 18, trả phòng Chủ Nhật ngày 22 là 4 đêm."),
                 (157, "What service requires an additional fee?",
                  new[] { ("A", "Daily breakfast", false), ("B", "Valet parking", true), ("C", "Fitness center access", false), ("D", "High-speed internet", false) },
                  "Email nêu: 'Valet parking is available for an additional fee of $35 per night'."),
                 (158, "Under what condition should Mr. Vance contact the hotel in advance?",
                  new[] { ("A", "If he arrives after 10:00 P.M.", true), ("B", "If he brings a pet", false), ("C", "If he needs airport shuttle pickup", false), ("D", "If he wishes to cancel free of charge", false) },
                  "Email nhắc: 'Please let us know if your arrival will be after 10:00 P.M.'.")
             }),

            (159, 161, "Questions 159-161 refer to the following customer review.",
             "Customer Review -- QuickClean Cordless Vacuum (Model QC-400)\nRating: 4 out of 5 stars\nReviewer: Brenda Lawson\nDate: May 19\n\nI purchased the QuickClean QC-400 three weeks ago after my old upright vacuum stopped working. Overall, I am very pleased with its performance. It is extremely lightweight, making it easy to carry up and down stairs. The suction power on hardwood floors is outstanding, picking up pet hair and dust effortlessly.\n\nMy only minor complaint is the battery life. On maximum power mode, the battery lasts approximately 18 minutes, which is slightly less than the 25 minutes advertised in the product manual. However, on standard mode, it easily cleans my entire 1,500-square-foot house on a single charge. The wall-mounted charging dock is compact and convenient.",
             new[] {
                 (159, "What feature of the vacuum does Ms. Lawson praise the most?",
                  new[] { ("A", "Its quiet motor", false), ("B", "Its lightweight design", true), ("C", "Its large dustbin capacity", false), ("D", "Its low retail price", false) },
                  "Đánh giá khen: 'It is extremely lightweight, making it easy to carry...'."),
                 (160, "What is Ms. Lawson's main criticism of the product?",
                  new[] { ("A", "The battery life on maximum power", true), ("B", "The weight of the charging dock", false), ("C", "The difficulty finding replacement filters", false), ("D", "The poor suction on hardwood floors", false) },
                  "Bà phàn nàn: 'My only minor complaint is the battery life' khi chạy ở chế độ tối đa."),
                 (161, "How long has Ms. Lawson owned the vacuum?",
                  new[] { ("A", "One week", false), ("B", "Three weeks", true), ("C", "One month", false), ("D", "Six months", false) },
                  "Câu đầu tiên: 'I purchased the QuickClean QC-400 three weeks ago'.")
             }),

            (162, 165, "Questions 162-165 refer to the following memo and table.",
             "MEMORANDUM\nTo: Sales Division Staff\nFrom: Gregory Cho, Regional Director\nDate: January 10\nSubject: Q4 Performance Awards\n\nI want to congratulate the entire sales department on an outstanding fourth quarter. Thanks to your dedication, our division surpassed its annual revenue target by 18 percent.\n\nBelow are the top-performing account managers for Q4 who will receive bonus packages and company-sponsored trips to the International Sales Expo in Tokyo:\n\nName | Territory | New Clients | Total Q4 Sales\nRachel Adams | Northwest | 24 | $1,240,000\nCarlos Mendez | Southwest | 31 | $1,420,000\nSarah Jenkins | Midwest | 19 | $980,000\nDavid Park | Southeast | 27 | $1,150,000\n\nThe award presentation ceremony will take place during our quarterly luncheon on Friday, January 26, at the Grand Hotel.",
             new[] {
                 (162, "By how much did the sales division exceed its annual target?",
                  new[] { ("A", "10 percent", false), ("B", "15 percent", false), ("C", "18 percent", true), ("D", "24 percent", false) },
                  "Bản ghi nhớ ghi rõ: 'surpassed its annual revenue target by 18 percent'."),
                 (163, "Who acquired the largest number of new clients in Q4?",
                  new[] { ("A", "Rachel Adams", false), ("B", "Carlos Mendez", true), ("C", "Sarah Jenkins", false), ("D", "David Park", false) },
                  "Bảng ghi Carlos Mendez đạt 31 New Clients (cao nhất)."),
                 (164, "Where will the award ceremony be held?",
                  new[] { ("A", "At the Grand Hotel", true), ("B", "In Tokyo, Japan", false), ("C", "At company headquarters", false), ("D", "At the Hyatt Regency", false) },
                  "Câu cuối ghi: 'at the Grand Hotel'."),
                 (165, "What will top performers receive in addition to a bonus?",
                  new[] { ("A", "A company car", false), ("B", "Stock options", false), ("C", "A trip to Tokyo", true), ("D", "An extra week of vacation", false) },
                  "Bản ghi nhớ ghi: 'company-sponsored trips to the International Sales Expo in Tokyo'.")
             }),

            (166, 169, "Questions 166-169 refer to the following policy document.",
             "SUMMIT FINANCIAL SERVICES -- REMOTE WORK POLICY\nSection 4: Equipment and Reimbursable Expenses\n\n1. Company-Provided Hardware: All full-time employees approved for remote work will receive a laptop computer, an external monitor, a keyboard, and a mouse. This hardware remains the property of Summit Financial Services and must be returned within five business days upon separation of employment.\n2. Ergonomic Stipend: Employees may claim a one-time reimbursement of up to $300 toward ergonomic office furniture, such as an adjustable desk chair or desk riser. Receipts must be submitted through the Concur expense portal within 60 days of remote work approval.\n3. Monthly Internet Allowance: A fixed monthly subsidy of $50 will be automatically added to the employee's payroll direct deposit to offset home broadband costs. Separate expense reports are not required for this subsidy.\n4. Technical Support: The IT Helpdesk is available Monday through Friday, 7:00 A.M. to 8:00 P.M. EST.",
             new[] {
                 (166, "What is the maximum amount for the ergonomic furniture reimbursement?",
                  new[] { ("A", "$100", false), ("B", "$200", false), ("C", "$300", true), ("D", "$500", false) },
                  "Mục 2 ghi rõ: 'one-time reimbursement of up to $300'."),
                 (167, "When must furniture receipts be submitted?",
                  new[] { ("A", "Within 30 days", false), ("B", "Within 60 days", true), ("C", "Within 90 days", false), ("D", "At the end of the fiscal year", false) },
                  "Mục 2 nêu: 'within 60 days of remote work approval'."),
                 (168, "How will employees receive the monthly internet allowance?",
                  new[] { ("A", "By check in the mail", false), ("B", "Through payroll direct deposit", true), ("C", "As a credit on the corporate credit card", false), ("D", "Via digital gift card", false) },
                  "Mục 3 nêu: 'automatically added to the employee's payroll direct deposit'."),
                 (169, "What must happen when an employee leaves the company?",
                  new[] { ("A", "They must repay the furniture stipend.", false), ("B", "They must return company hardware within 5 days.", true), ("C", "They must submit an exit expense report.", false), ("D", "Their internet subsidy will continue for 30 days.", false) },
                  "Mục 1 nêu: 'must be returned within five business days upon separation of employment'.")
             }),

            (170, 173, "Questions 170-173 refer to the following business proposal.",
             "PROPOSAL FOR WEBSITE REDESIGN\nPrepared for: Harrison & Cole Law Firm\nPrepared by: PixelCraft Studios\nDate: March 14\n\n1. Project Overview\nPixelCraft Studios is pleased to submit this proposal to modernize the web presence of Harrison & Cole Law Firm. The current website was built in 2017 and lacks mobile responsiveness, modern accessibility standards, and secure client intake forms.\n\n2. Proposed Scope of Work\nPhase 1 (Weeks 1-3): Stakeholder interviews, user persona creation, and wireframe development.\nPhase 2 (Weeks 4-7): Visual design, responsive layout coding, and CMS integration.\nPhase 3 (Weeks 8-9): Secure client portal setup, payment gateway integration, and SEO optimization.\nPhase 4 (Weeks 10-11): Quality assurance testing, staff training, and official launch.\n\n3. Cost and Payment Milestones\nThe total estimated project fee is $28,000, payable in four equal installments upon completion of each phase.",
             new[] {
                 (170, "What is the primary problem with Harrison & Cole's current website?",
                  new[] { ("A", "It lacks mobile responsiveness and accessibility.", true), ("B", "The domain name has expired.", false), ("C", "It was hacked by third parties.", false), ("D", "The hosting costs are excessively high.", false) },
                  "Mục 1 nêu: 'lacks mobile responsiveness, modern accessibility standards...'."),
                 (171, "What will be implemented during Phase 3?",
                  new[] { ("A", "Wireframe development", false), ("B", "Visual design mockups", false), ("C", "Secure client portal and SEO optimization", true), ("D", "Staff training and launch", false) },
                  "Mục 2 ghi rõ Phase 3 bao gồm: 'Secure client portal setup, payment gateway integration, and SEO optimization'."),
                 (172, "How long is the entire project scheduled to take?",
                  new[] { ("A", "7 weeks", false), ("B", "9 weeks", false), ("C", "11 weeks", true), ("D", "16 weeks", false) },
                  "Phase 4 ghi 'Weeks 10-11' nên tổng thời gian là 11 tuần."),
                 (173, "How much is each payment installment?",
                  new[] { ("A", "$5,000", false), ("B", "$7,000", true), ("C", "$9,000", false), ("D", "$14,000", false) },
                  "Tổng chi phí $28,000 chia 4 đợt bằng nhau: $28,000 / 4 = $7,000.")
             }),

            (174, 177, "Questions 174-177 refer to the following email and survey results.",
             "To: All Store Managers\nFrom: Natalie Simmons, VP of Customer Experience\nDate: June 8\nSubject: Quarterly Customer Satisfaction Results\n\nDear Team,\n\nThe customer satisfaction survey results for Q2 have been compiled. Across all 45 retail stores, our overall customer rating was 88 percent favorable, up 4 points from Q1.\n\nCategory | Q1 Rating | Q2 Rating | Target\nStaff Friendliness | 91% | 94% | 90%\nStore Cleanliness | 86% | 89% | 88%\nCheckout Speed | 78% | 82% | 85%\nProduct Availability | 81% | 87% | 85%\n\nWhile we achieved our targets in three categories, checkout speed remains below our benchmark of 85 percent. To address this, we will begin installing two additional self-checkout kiosks at high-volume stores next month.",
             new[] {
                 (174, "How many retail stores were included in the survey?",
                  new[] { ("A", "4", false), ("B", "45", true), ("C", "88", false), ("D", "94", false) },
                  "Đoạn 1 nêu: 'Across all 45 retail stores...'."),
                 (175, "Which category received the highest rating in Q2?",
                  new[] { ("A", "Staff Friendliness", true), ("B", "Store Cleanliness", false), ("C", "Checkout Speed", false), ("D", "Product Availability", false) },
                  "Bảng ghi Staff Friendliness đạt 94% (cao nhất)."),
                 (176, "Which category failed to meet its target in Q2?",
                  new[] { ("A", "Store Cleanliness", false), ("B", "Checkout Speed", true), ("C", "Product Availability", false), ("D", "Staff Friendliness", false) },
                  "Checkout Speed đạt 82% trong khi Target là 85%."),
                 (177, "What measure will be taken to improve checkout speed?",
                  new[] { ("A", "Hiring more cashiers", false), ("B", "Installing self-checkout kiosks", true), ("C", "Extending store operating hours", false), ("D", "Offering express lanes for cash only", false) },
                  "Đoạn cuối ghi: 'installing two additional self-checkout kiosks'.")
             }),

            (178, 180, "Questions 178-180 refer to the following letter.",
             "EVERGREEN PROPERTY MANAGEMENT\n450 Maple Avenue, Suite 200, Portland, OR\n\nSeptember 2\n\nDear Tenant,\n\nPlease be advised that annual fire alarm testing will take place throughout the building on Thursday, September 14, between 9:00 A.M. and 3:00 P.M. During this time, certified technicians from Pacific Fire & Safety will be inspecting smoke detectors, emergency exit lighting, and sprinkler valves.\n\nThe alarm horns and strobe lights will sound intermittently throughout the day. You do not need to evacuate the building during these tests unless instructed to do so by building personnel. We apologize for the loud noise and recommend that any sensitive phone conferences be scheduled outside of the testing window.\n\nSincerely,\nMarcus Brody, Property Manager",
             new[] {
                 (178, "When will the fire alarm testing occur?",
                  new[] { ("A", "On September 2", false), ("B", "On September 14", true), ("C", "On September 20", false), ("D", "On October 1", false) },
                  "Thư ghi: 'on Thursday, September 14, between 9:00 A.M. and 3:00 P.M.'."),
                 (179, "Who will perform the inspections?",
                  new[] { ("A", "Evergreen Property Management", false), ("B", "Pacific Fire & Safety", true), ("C", "The local police department", false), ("D", "The Portland City Council", false) },
                  "Thư nêu: 'certified technicians from Pacific Fire & Safety'."),
                 (180, "What are tenants advised to do regarding phone conferences?",
                  new[] { ("A", "Hold them in the parking lot", false), ("B", "Record them for later review", false), ("C", "Schedule them outside the testing window", true), ("D", "Cancel all meetings for the entire week", false) },
                  "Thư khuyên: 'recommend that any sensitive phone conferences be scheduled outside of the testing window'.")
             }),

            // Passages 181-185 (Double Passage: Email and Invoice)
            (181, 185, "Questions 181-185 refer to the following email and invoice.",
             "Document 1: Email\nTo: accounts@olympiccatering.com\nFrom: Liam Gallagher <l.gallagher@crestviewlegal.com>\nDate: November 18\nSubject: Invoice #OC-9021 Inquiry\n\nDear Olympic Catering,\n\nI am reviewing invoice #OC-9021 for our firm's annual retirement banquet held on November 15. The food and waitstaff were wonderful, but I noticed an extra charge of $250 for 'Audio/Visual Equipment Rental.' According to our contract signed on October 4, AV equipment was to be included complimentary because our food order exceeded $3,000. Could you please review this and send an amended invoice so our finance department can release the payment?\n\nSincerely, Liam Gallagher\n\nDocument 2: Invoice\nOlympic Catering Services\nInvoice Number: OC-9021 | Date: November 16\nBilled To: Crestview Legal LLP\n\nItems:\n- Buffet Dinner (80 guests @ $45/person): $3,600\n- Beverage Package: $800\n- AV Equipment Rental: $250\n- Service Gratuity (18%): $837\nTotal Amount Due: $5,487",
             new[] {
                 (181, "What event was held on November 15?",
                  new[] { ("A", "A client appreciation dinner", false), ("B", "A retirement banquet", true), ("C", "A holiday party", false), ("D", "A press conference", false) },
                  "Email ghi: 'our firm's annual retirement banquet held on November 15'."),
                 (182, "Why does Mr. Gallagher question the $250 AV charge?",
                  new[] { ("A", "The equipment did not work properly.", false), ("B", "It should have been complimentary per contract.", true), ("C", "The banquet room had its own equipment.", false), ("D", "His company never used the equipment.", false) },
                  "Email giải thích: 'AV equipment was to be included complimentary because our food order exceeded $3,000'."),
                 (183, "How many guests attended the banquet?",
                  new[] { ("A", "45", false), ("B", "80", true), ("C", "100", false), ("D", "250", false) },
                  "Hóa đơn ghi: 'Buffet Dinner (80 guests @ $45/person)'."),
                 (184, "What was the cost of the beverage package?",
                  new[] { ("A", "$250", false), ("B", "$800", true), ("C", "$837", false), ("D", "$3,600", false) },
                  "Hóa đơn mục Beverage Package ghi rõ: '$800'."),
                 (185, "What should the corrected total amount be after removing the AV charge and recalculating gratuity?",
                  new[] { ("A", "$5,237", false), ("B", "$5,192", true), ("C", "$4,800", false), ("D", "$4,400", false) },
                  "Khi trừ $250 phí AV và thuế/phí dịch vụ tương ứng, số tiền giảm khoảng $295.")
             }),

            // Passages 186-190 (Double Passage: Job Posting and Cover Letter)
            (186, 190, "Questions 186-190 refer to the following job posting and cover letter.",
             "Document 1: Job Posting\nPosition: Senior Graphic Designer\nCompany: Blue Horizon Media, Seattle, WA\n\nResponsibilities:\n- Lead creative design projects for print and digital advertising campaigns.\n- Collaborate with copywriters and marketing managers to develop brand identities.\n- Mentor junior design staff and oversee freelance contributors.\n\nQualifications:\n- Bachelor's degree in Graphic Design, Fine Arts, or related discipline.\n- Minimum of 5 years of professional design agency experience.\n- Expert proficiency with Adobe Creative Cloud (Photoshop, Illustrator, InDesign).\n- Experience with motion graphics and UI/UX design is a plus.\n\nDocument 2: Cover Letter\nDear Hiring Team,\n\nI am writing to express my enthusiastic interest in the Senior Graphic Designer position at Blue Horizon Media. For the past six years, I have served as Lead Visual Designer at Cascade Creative Agency, where I spearheaded brand redesigns for over 30 corporate clients. My team's work on the Rainier Organic Foods campaign won a Northwest Design Excellence Award last year.\n\nI hold a B.F.A. in Visual Communication from the University of Washington and am thoroughly proficient in Adobe Creative Cloud. In addition, I have extensive experience in motion graphics and video editing using After Effects.\n\nSincerely,\nDaniel Morales",
             new[] {
                 (186, "Where is Blue Horizon Media located?",
                  new[] { ("A", "Portland", false), ("B", "Seattle", true), ("C", "San Francisco", false), ("D", "Chicago", false) },
                  "Thông báo tuyển dụng ghi: 'Blue Horizon Media, Seattle, WA'."),
                 (187, "How many years of design experience does Mr. Morales have?",
                  new[] { ("A", "3 years", false), ("B", "5 years", false), ("C", "6 years", true), ("D", "10 years", false) },
                  "Thư xin việc ghi: 'For the past six years, I have served as Lead Visual Designer...'."),
                 (188, "What award did Mr. Morales's team win last year?",
                  new[] { ("A", "Northwest Design Excellence Award", true), ("B", "National Advertising Trophy", false), ("C", "Innovator of the Year", false), ("D", "Seattle Creative Merit", false) },
                  "Thư nêu: 'won a Northwest Design Excellence Award last year'."),
                 (189, "Which preferred skill mentioned in the job posting does Mr. Morales highlight?",
                  new[] { ("A", "Motion graphics and video editing", true), ("B", "3D architectural modeling", false), ("C", "Copywriting for radio", false), ("D", "Social media community management", false) },
                  "Bài đăng tìm 'motion graphics' và ông nêu: 'extensive experience in motion graphics and video editing'."),
                 (190, "What degree does Mr. Morales possess?",
                  new[] { ("A", "B.S. in Computer Science", false), ("B", "B.F.A. in Visual Communication", true), ("C", "M.B.A. in Marketing", false), ("D", "B.A. in English Literature", false) },
                  "Thư ghi rõ: 'B.F.A. in Visual Communication from the University of Washington'.")
             }),

            // Passages 191-195 (Triple Passage: Memo, Email, and Feedback Form)
            (191, 195, "Questions 191-195 refer to the following memo, email, and feedback form.",
             "Document 1: Memorandum\nTo: Branch Employees\nFrom: Operations Directorate\nDate: April 10\nSubject: Software Migration to CloudSuite\n\nNext month, our firm will transition our accounting and inventory software to CloudSuite Enterprise. Training sessions will be conducted in the second-floor computer lab between May 2 and May 6. Each employee must register for one 2-hour session via the internal training portal.\n\nDocument 2: Email\nTo: CloudSuite Support <support@cloudsuite.com>\nFrom: Teresa Huang <t.huang@beaconcorp.com>\nDate: May 7\nSubject: Training Session Completion\n\nHi Support Team, our branch has completed training for all 65 employees across 6 sessions. Instructor Brian Cole did a fantastic job explaining the inventory reconciliation modules. However, several users in the accounts payable department experienced login timeouts during hands-on exercises. Could you check server capacity before our live deployment on May 15?\n\nDocument 3: Training Feedback Summary\nInstructor: Brian Cole | Total Reviews: 65\n- Clarity of Instruction: 4.8 / 5.0\n- Course Materials: 4.7 / 5.0\n- System Responsiveness during exercises: 3.2 / 5.0\nTop Comments: 'Great trainer, but system was slow during group exercises.'",
             new[] {
                 (191, "What software is the company adopting?",
                  new[] { ("A", "QuickBooks", false), ("B", "CloudSuite Enterprise", true), ("C", "SAP Business One", false), ("D", "Microsoft Dynamics", false) },
                  "Bản ghi nhớ ghi: 'transition our accounting and inventory software to CloudSuite Enterprise'."),
                 (192, "When is the live deployment scheduled?",
                  new[] { ("A", "April 10", false), ("B", "May 2", false), ("C", "May 7", false), ("D", "May 15", true) },
                  "Email nêu: 'before our live deployment on May 15'."),
                 (193, "Who delivered the training sessions?",
                  new[] { ("A", "Teresa Huang", false), ("B", "Brian Cole", true), ("C", "Marcus Vance", false), ("D", "Angela Harris", false) },
                  "Email và Feedback form đều ghi rõ instructor là 'Brian Cole'."),
                 (194, "Which aspect of the training received the lowest rating?",
                  new[] { ("A", "Clarity of Instruction", false), ("B", "Course Materials", false), ("C", "System Responsiveness", true), ("D", "Instructor Punctuality", false) },
                  "Bảng feedback ghi System Responsiveness chỉ đạt 3.2 / 5.0 (thấp nhất)."),
                 (195, "How many employees completed the training program?",
                  new[] { ("A", "15", false), ("B", "30", false), ("C", "65", true), ("D", "100", false) },
                  "Email và bảng feedback đều xác nhận có 65 nhân viên tham gia.")
             }),

            // Passages 196-200 (Triple Passage: Web Page, Email, and Receipt)
            (196, 200, "Questions 196-200 refer to the following web page, email, and receipt.",
             "Document 1: Web Page\nApex Car Rental -- Fleet Options & Daily Rates\n- Compact (Hyundai Elantra): $45/day (Seats 5, 2 bags)\n- Standard SUV (Toyota RAV4): $65/day (Seats 5, 4 bags)\n- Minivan (Chrysler Pacifica): $85/day (Seats 7, 5 bags)\n- Luxury Sedan (Audi A6): $110/day (Seats 5, 3 bags)\nOptional Add-ons: GPS Navigation ($10/day), Child Safety Seat ($12/day), Full Collision Damage Waiver ($20/day).\nSpecial Offer: Rent for 5 or more consecutive days and receive a 15% discount on the base rental rate!\n\nDocument 2: Email\nTo: reservations@apexrental.com\nFrom: Arthur Pendelton <arthur.p@pendeltonlaw.com>\nDate: August 10\nSubject: Reservation Request\n\nHello, I need to reserve a Standard SUV for a family road trip from August 20 to August 26 (6 days total). We will pick up and drop off the vehicle at Denver International Airport. Please include the Full Collision Damage Waiver and GPS Navigation.\n\nDocument 3: Rental Receipt\nCustomer: Arthur Pendelton\nVehicle: Standard SUV (Toyota RAV4) | Rental Period: 6 Days (Aug 20 - Aug 26)\n- Base Rate (6 days @ $65/day): $390.00\n- Long-Term Rental Discount (15% off base rate): -$58.50\n- Collision Damage Waiver (6 days @ $20/day): $120.00\n- GPS Navigation (6 days @ $10/day): $60.00\n- Airport Concession Fee (10%): $51.15\nTotal Paid: $562.65",
             new[] {
                 (196, "What type of vehicle did Mr. Pendelton reserve?",
                  new[] { ("A", "Compact", false), ("B", "Standard SUV", true), ("C", "Minivan", false), ("D", "Luxury Sedan", false) },
                  "Email và Receipt ghi rõ xe là 'Standard SUV (Toyota RAV4)'."),
                 (197, "Why did Mr. Pendelton qualify for a 15% discount?",
                  new[] { ("A", "He is a corporate club member.", false), ("B", "He rented for 5 or more consecutive days.", true), ("C", "He reserved through an affiliated airline.", false), ("D", "He paid in advance with a credit card.", false) },
                  "Trang web nêu: 'Rent for 5 or more consecutive days and receive a 15% discount'."),
                 (198, "How many days did the rental last?",
                  new[] { ("A", "4 days", false), ("B", "5 days", false), ("C", "6 days", true), ("D", "7 days", false) },
                  "Email và Hóa đơn ghi rõ từ 20 đến 26 tháng 8 là 6 ngày."),
                 (199, "What was the total cost of the optional add-ons?",
                  new[] { ("A", "$120.00", false), ("B", "$180.00", true), ("C", "$200.00", false), ("D", "$240.00", false) },
                  "CDW ($120) + GPS ($60) = $180.00."),
                 (200, "Where will the vehicle be returned?",
                  new[] { ("A", "Denver Downtown", false), ("B", "Denver International Airport", true), ("C", "Colorado Springs Airport", false), ("D", "Aspen Ski Resort", false) },
                  "Email ghi: 'pick up and drop off the vehicle at Denver International Airport'.")
             })
        };

        foreach (var sc in scenarios)
        {
            addP($"[PASSAGE: {sc.StartQ}-{sc.EndQ}]", true, "993300");
            addP($"[PASSAGE_TITLE]: {sc.Title}", false, null);
            addP($"[PASSAGE_CONTENT]:\n{sc.Content}", false, null);
            addP("", false, null);
            RenderQuestions(sc.Item5, addP);
            addP("[END_PASSAGE]", false, null);
            addP("", false, null);
        }
    }

    private static void RenderQuestions((int QNum, string Prompt, (string L, string T, bool C)[] Opts, string Exp)[] qList, System.Action<string, bool, string> addP)
    {
        foreach (var q in qList)
        {
            addP($"[QUESTION: {q.QNum}]", true, null);
            addP(q.Prompt, false, null);
            foreach (var opt in q.Opts)
            {
                var optTag = opt.C ? $"[{opt.L}*]" : $"[{opt.L}]";
                addP($"{optTag} {opt.T}", false, null);
            }
            addP($"[EXPLANATION]: {q.Exp}", false, "885500");
            addP("[END_QUESTION]", false, null);
            addP("", false, null);
        }
    }
}
