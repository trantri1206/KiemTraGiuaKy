using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace _2380600189.Exams;

public static class EtsToeicTest02Generator
{
    public static void GenerateOfficialTest02(string filePath)
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

        // Exam Metadata
        AddP("[EXAM_TITLE]: TOEIC Reading Actual Test 02", true, "0F1E36");
        AddP("[EXAM_DESCRIPTION]: Đề thi chính thức TOEIC Reading RC 02 chuẩn ETS gồm đầy đủ 100 câu: Part 5 (Câu 101-130), Part 6 (Câu 131-146), Part 7 (Câu 147-200)", false, "555555");
        AddP("[DURATION]: 75", true, "10B981");
        AddP("[SHUFFLE_QUESTIONS]: FALSE");
        AddP("[SHUFFLE_OPTIONS]: TRUE");
        AddP("");

        // =========================================================================
        // PART 5: QUESTIONS 101 - 130 (Incomplete Sentences - 30 câu hỏi độc lập)
        // =========================================================================
        AddP("[PART: 5]", true, "2563EB");
        AddP("");

        var part5 = new[]
        {
            (101, "Ms. Carpenter will be attending the conference with ------- marketing team.",
             new[] { ("A", "she", false), ("B", "her", true), ("C", "hers", false), ("D", "herself", false) },
             "Trước cụm danh từ 'marketing team' cần tính từ sở hữu 'her' để chỉ đội ngũ tiếp thị của cô ấy."),

            (102, "------- the last ten years, Bay City's population has grown by about 27 percent.",
             new[] { ("A", "As", false), ("B", "Against", false), ("C", "During", true), ("D", "Below", false) },
             "Giới từ 'During' (Trong suốt) đi với khoảng thời gian 'the last ten years' để diễn tả quá trình tăng trưởng dân số."),

            (103, "Please congratulate Alan Schmit, ------- of the Leadership Award in Nursing at Knoll Hospital.",
             new[] { ("A", "won", false), ("B", "wins", false), ("C", "winning", false), ("D", "winner", true) },
             "Vị trí sau dấu phẩy là cụm đồng vị ngữ chỉ người, cần danh từ 'winner' (người chiến thắng giải thưởng)."),

            (104, "Ireland's largest software producer will ------- be opening a large facility in Cork.",
             new[] { ("A", "soon", true), ("B", "such", false), ("C", "ever", false), ("D", "like", false) },
             "Phó từ 'soon' (sớm) đứng giữa trợ động từ 'will' và động từ 'be opening' để chỉ hành động sắp diễn ra trong tương lai gần."),

            (105, "Ashburn Bank's online service has been in high demand -------.",
             new[] { ("A", "lateness", false), ("B", "later", false), ("C", "lately", true), ("D", "latest", false) },
             "Trạng từ thời gian 'lately' (gần đây) thường đứng cuối câu và dùng với thì hiện tại hoàn thành 'has been'."),

            (106, "The ------- of videos to electronic press releases can help companies showcase their products.",
             new[] { ("A", "content", false), ("B", "addition", true), ("C", "pictures", false), ("D", "promotion", false) },
             "Cụm danh từ 'The addition of A to B' (Việc bổ sung/thêm video vào các thông cáo báo chí điện tử)."),

            (107, "Ms. Hyun is reviewing the training manual to see if updates -------.",
             new[] { ("A", "have need", false), ("B", "needing", false), ("C", "are needed", true), ("D", "to be needed", false) },
             "Chủ ngữ 'updates' (các bản cập nhật) mang nghĩa bị động: 'are needed' (được cần đến/có cần thiết hay không)."),

            (108, "When leaving the auditorium, please exit ------- the doors on the lower level.",
             new[] { ("A", "except", false), ("B", "inside", false), ("C", "without", false), ("D", "through", true) },
             "Giới từ 'through' chỉ sự xuyên qua/đi qua: 'exit through the doors' (đi ra qua các cánh cửa ở tầng dưới)."),

            (109, "The judges for this year's screenplay competition include ------- from Hanovi Studios.",
             new[] { ("A", "represents", false), ("B", "representatives", true), ("C", "represented", false), ("D", "represent", false) },
             "Sau ngoại động từ 'include' cần danh từ làm tân ngữ: 'representatives' (những người đại diện từ Hanovi Studios)."),

            (110, "I have attached my résumé detailing my ------- experience in the hotel industry.",
             new[] { ("A", "extensive", true), ("B", "punctual", false), ("C", "prospective", false), ("D", "accepted", false) },
             "Tính từ 'extensive' bổ nghĩa cho 'experience': 'extensive experience' (kinh nghiệm sâu rộng/dày dặn trong ngành khách sạn)."),

            (111, "Remember to check the spelling of Mr. Kamashi's name when ------- the document.",
             new[] { ("A", "revising", true), ("B", "revises", false), ("C", "revised", false), ("D", "revise", false) },
             "Rút gọn mệnh đề trạng ngữ cùng chủ ngữ với liên từ 'when': 'when revising the document' (khi chỉnh sửa tài liệu)."),

            (112, "Residents visited City Hall to ask ------- developers will preserve the historic properties.",
             new[] { ("A", "although", false), ("B", "since", false), ("C", "whether", true), ("D", "both", false) },
             "Động từ 'ask' đi với liên từ 'whether' mang nghĩa hỏi xem liệu các nhà phát triển có bảo tồn công trình lịch sử hay không."),

            (113, "The Ford Group's proposed advertising campaign is by far the most ------- we have seen so far.",
             new[] { ("A", "innovate", false), ("B", "innovative", true), ("C", "innovations", false), ("D", "innovatively", false) },
             "Cấu trúc so sánh nhất 'the most + adj': 'the most innovative' (sáng tạo nhất)."),

            (114, "Solei Landscaping announced that the design for the Cherry Hill building complex is ------- complete.",
             new[] { ("A", "almost", true), ("B", "nearby", false), ("C", "anytime", false), ("D", "yet", false) },
             "Phó từ chỉ mức độ 'almost' bổ nghĩa cho tính từ 'complete': 'almost complete' (gần như hoàn tất)."),

            (115, "Daniel Nishida, the chief supply officer, asked that ------- be given full responsibility for approving all invoices.",
             new[] { ("A", "he", true), ("B", "him", false), ("C", "his", false), ("D", "himself", false) },
             "Cấu trúc giả định thức sau 'asked that + S + (should) be given': vị trí chủ ngữ của mệnh đề con cần đại từ nhân xưng 'he'."),

            (116, "In preparation for Mr. Kumar's retirement at the end of March, the Carolex Corporation will need to ------- a new facilities director.",
             new[] { ("A", "resume", false), ("B", "compete", false), ("C", "recruit", true), ("D", "conduct", false) },
             "Động từ 'recruit' (tuyển dụng) phù hợp với ngữ cảnh chuẩn bị người thay thế cho nhân sự về hưu."),

            (117, "The team's contributions to the Ripton Group's marketing plan were very ------- acknowledged.",
             new[] { ("A", "favor", false), ("B", "favorably", true), ("C", "favorable", false), ("D", "favored", false) },
             "Phó từ 'favorably' (một cách tích cực/thuận lợi) bổ nghĩa cho động từ bị động 'acknowledged'."),

            (118, "------- receiving the engineering award, Ms. Kwon made a point of thanking longtime mentors.",
             new[] { ("A", "Onto", false), ("B", "Unlike", false), ("C", "About", false), ("D", "Upon", true) },
             "Giới từ 'Upon + V-ing' mang nghĩa 'Ngay khi nhận được giải thưởng kĩ thuật, bà Kwon đã gửi lời cảm ơn...'."),

            (119, "Please read the list of ------- qualifications to ensure that you have the necessary education and experience for the position.",
             new[] { ("A", "slight", false), ("B", "equal", false), ("C", "obliged", false), ("D", "essential", true) },
             "Tính từ 'essential' (thiết yếu/cần thiết) bổ nghĩa cho 'qualifications' (các tiêu chuẩn bằng cấp cần thiết)."),

            (120, "Choosing the best software tool to eliminate computer viruses is rarely simple, ------- it is important to seek expert advice.",
             new[] { ("A", "why", false), ("B", "then", false), ("C", "nor", false), ("D", "so", true) },
             "Liên từ 'so' (vì vậy) nối mệnh đề chỉ kết quả: việc chọn phần mềm diệt virus không đơn giản, vì vậy việc xin ý kiến chuyên gia là rất quan trọng."),

            (121, "Ms. Delgado would like to meet with all loan officers ------- reviewing any more loan applications.",
             new[] { ("A", "now that", false), ("B", "as though", false), ("C", "before", true), ("D", "often", false) },
             "Giới từ 'before + V-ing': 'before reviewing any more loan applications' (trước khi xét duyệt thêm hồ sơ vay vốn)."),

            (122, "Cedar Branch Hill has a ------- as a business-friendly environment that provides efficient licensing and other legal processes.",
             new[] { ("A", "prediction", false), ("B", "courtesy", false), ("C", "reputation", true), ("D", "statement", false) },
             "Cụm từ 'have a reputation as...' (có danh tiếng/tiếng tăm là một môi trường thân thiện với doanh nghiệp)."),

            (123, "The actors held an additional rehearsal ------- perfect their performance in the final scene.",
             new[] { ("A", "considerably", false), ("B", "in order to", true), ("C", "nevertheless", false), ("D", "as a result of", false) },
             "Cụm chỉ mục đích 'in order to + V_bare': 'in order to perfect their performance' (để hoàn thiện màn biểu diễn của họ)."),

            (124, "RZT Technology will double the size of its Toronto laboratory to ------- the organization's rapid growth.",
             new[] { ("A", "assign", false), ("B", "investigate", false), ("C", "experience", false), ("D", "accommodate", true) },
             "Động từ 'accommodate' (đáp ứng/chứa đựng): mở rộng gấp đôi diện tích phòng thí nghiệm để đáp ứng sự tăng trưởng nhanh chóng."),

            (125, "Even though Smithton Electronics' second quarter was not -------, the company plans to invest large sums on research.",
             new[] { ("A", "profitable", true), ("B", "profiting", false), ("C", "profitability", false), ("D", "profitably", false) },
             "Sau động từ to-be 'was not' cần tính từ làm vị ngữ: 'profitable' (có lợi nhuận/sinh lời)."),

            (126, "The Williamsport Hotel is an ideal venue for the conference because of its ------- to the airport.",
             new[] { ("A", "achievement", false), ("B", "proximity", true), ("C", "competence", false), ("D", "exception", false) },
             "Danh từ 'proximity to...' nghĩa là 'sự gần kề/khoảng cách gần với sân bay'."),

            (127, "The second training session is for employees ------- responsibilities include processing payroll forms.",
             new[] { ("A", "whose", true), ("B", "which", false), ("C", "what", false), ("D", "who", false) },
             "Đại từ quan hệ sở hữu 'whose + noun': 'whose responsibilities' (những người có trách nhiệm bao gồm...)."),

            (128, "------- poorly the high-speed printer may be functioning, it is still making copies that are adequate for our purposes.",
             new[] { ("A", "Rather", false), ("B", "Seldom", false), ("C", "However", true), ("D", "Thoroughly", false) },
             "Cấu trúc nhượng bộ: 'However + adv + S + V' (Dù cho máy in tốc độ cao có hoạt động kém đến mức nào đi nữa...)."),

            (129, "The long-awaited Weka 2XG digital camera will finally be ------- at a product exhibition on August 16.",
             new[] { ("A", "reduced", false), ("B", "unveiled", true), ("C", "consulted", false), ("D", "resolved", false) },
             "Động từ bị động 'be unveiled' nghĩa là 'được ra mắt/công bố/trình làng' tại triển lãm sản phẩm."),

            (130, "Northeast Community Finance ------- an online system in order to shorten service lines at branch locations.",
             new[] { ("A", "has implemented", true), ("B", "to be implementing", false), ("C", "to have been implementing", false), ("D", "is implemented", false) },
             "Câu cần động từ chính có chủ ngữ số ít và mang nghĩa chủ động: 'has implemented' (đã triển khai một hệ thống trực tuyến).")
        };

        foreach (var q in part5)
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

        // =========================================================================
        // PART 6: QUESTIONS 131 - 146 (Text Completion - 16 câu hỏi theo bài đọc)
        // =========================================================================
        AddP("[PART: 6]", true, "2563EB");
        AddP("");

        // Part 6 - Passage 1: Questions 131-134
        AddP("[PASSAGE: 131-134]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 131-134 refer to the following advertisement.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Garden Shade Tree Landscaping\n\nGarden Shade creates tree landscapes and hedges to suit every garden, no matter the size. Our designs have ---[131]--- small urban gardens as well as large-scale projects commissioned by architects and property developers. ---[132]---. However, no single nursery can offer trees of all species and sizes. That is why Garden Shade has developed close relationships with many specialist growers to provide us with the trees we need. Such resources give us the variety necessary to complete any ---[133]--- order. In other words, whatever your landscape design dream, we can make it happen.");
        AddP("");

        var p6q1 = new[]
        {
            (131, new[] { ("A", "transformed", true), ("B", "related", false), ("C", "collected", false), ("D", "planted", false) },
             "Động từ 'transformed' (biến đổi/làm thay đổi hoàn toàn diện mạo các khu vườn nhỏ đô thị)."),
            (132, new[] {
                ("A", "We are here to answer your landscaping questions.", false),
                ("B", "For most projects, we use trees from our own nurseries.", true),
                ("C", "Some trees have specific growing requirements.", false),
                ("D", "Under normal conditions, nursery stock is guaranteed for one year.", false)
            }, "Câu B nối tiếp hợp lý với câu sau: 'Tuy nhiên, không có vườn ươm đơn lẻ nào có thể cung cấp đủ mọi loài cây...'."),
            (133, new[] { ("A", "readiness", false), ("B", "readies", false), ("C", "readiest", false), ("D", "ready", false), ("D*", "ready", false) },
             "Tính từ hoặc ngữ cảnh đơn hàng hoàn chỉnh."),
            (134, new[] { ("A", "study", false), ("B", "form", false), ("C", "order", true), ("D", "survey", false) },
             "Danh từ 'order' (đơn đặt hàng): hoàn thành bất kỳ đơn đặt hàng nào.")
        };
        // Fix 133
        AddP("[QUESTION: 131]", true);
        AddP("[A*] transformed"); AddP("[B] related"); AddP("[C] collected"); AddP("[D] planted");
        AddP("[EXPLANATION]: Động từ 'transformed' (làm thay đổi diện mạo cảnh quan).");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 132]", true);
        AddP("[A] We are here to answer your landscaping questions.");
        AddP("[B*] For most projects, we use trees from our own nurseries.");
        AddP("[C] Some trees have specific growing requirements.");
        AddP("[D] Under normal conditions, nursery stock is guaranteed for one year.");
        AddP("[EXPLANATION]: Câu trước nói về dự án, câu sau dùng 'However, no single nursery...' nên B là sự liên kết hoàn hảo.");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 133]", true);
        AddP("[A] readiness"); AddP("[B] readies"); AddP("[C] readiest"); AddP("[D*] ready");
        AddP("[EXPLANATION]: 'ready to provide us' hoặc tính từ chỉ trạng thái sẵn sàng.");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 134]", true);
        AddP("[A] study"); AddP("[B] form"); AddP("[C*] order"); AddP("[D] survey");
        AddP("[EXPLANATION]: 'complete any order' (hoàn thành bất kỳ đơn đặt hàng nào).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // Part 6 - Passage 2: Questions 135-138
        AddP("[PASSAGE: 135-138]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 135-138 refer to the following e-mail.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("To: Staff\nFrom: Amy Henwith\nDate: 15 January\nSubject: Exciting news\n\nDear Staff,\nThanks for a great year! In case you haven't heard, Henwith Home Supply will be ---[135]--- our second store this spring. This additional retail site will be located in the shopping centre at the corner of Aberton Parkway and Sutton Avenue in Derbyshire.\n\nWe will be accepting applications for cashiers and sales positions ---[136]--- 1 April. The personnel director will review applicants' qualifications from 2 April to 6 April, and ---[137]--- is scheduled to begin one week later. ---[138]---.\n\nBest,\nAmy Henwith, CEO\nHenwith Home Supply");
        AddP("");

        AddP("[QUESTION: 135]", true);
        AddP("[A] moving"); AddP("[B] renovating"); AddP("[C] expanding"); AddP("[D*] opening");
        AddP("[EXPLANATION]: 'will be opening our second store' (sẽ khai trương cửa hàng thứ hai).");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 136]", true);
        AddP("[A*] until"); AddP("[B] following"); AddP("[C] according to"); AddP("[D] for");
        AddP("[EXPLANATION]: 'accepting applications until 1 April' (nhận hồ sơ ứng tuyển cho đến ngày 1 tháng 4).");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 137]", true);
        AddP("[A] trainer"); AddP("[B*] training"); AddP("[C] train"); AddP("[D] trained");
        AddP("[EXPLANATION]: Chủ ngữ chỉ hoạt động đào tạo nhân viên mới: 'training is scheduled to begin...'.");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 138]", true);
        AddP("[A*] Feel free to share this news with any interested friends.");
        AddP("[B] Make sure you have received all of the material.");
        AddP("[C] Contact Henwith Home Supply if you are still waiting for a response.");
        AddP("[D] Access to the main entrance will be blocked by construction.");
        AddP("[EXPLANATION]: Email nội bộ thông báo tuyển dụng thường kết thúc bằng lời kêu gọi nhân viên giới thiệu bạn bè.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // Part 6 - Passage 3: Questions 139-142
        AddP("[PASSAGE: 139-142]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 139-142 refer to the following article.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Amon Donates to Music School in Grenel City\n\nA spokesperson for Brenda Amon ---[139]--- that the pianist made a sizeable donation toward the expansion of the Grenel City Conservatory of Music. 'Without her generous support,' said Marc Diaz, director of facility planning, 'we would have been limited in our renovation plans going forward.'\n\n---[140]---. Now, a new wing will be constructed on the south end of the ---[141]--- conservatory. Once completed, the building will boast a 700-seat auditorium, state-of-the-art recording studios, and new faculty and administrative offices. Additionally, private practice rooms will be located ---[142]--- the current student lounge.");
        AddP("");

        AddP("[QUESTION: 139]", true);
        AddP("[A] confirm"); AddP("[B] confirmation"); AddP("[C*] has confirmed"); AddP("[D] will confirm");
        AddP("[EXPLANATION]: Chủ ngữ 'A spokesperson' ngôi 3 số ít, động từ chia thì hiện tại hoàn thành 'has confirmed'.");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 140]", true);
        AddP("[A] Ms. Amon's performance at the conservatory was outstanding.");
        AddP("[B*] The project had been delayed because of budget cuts.");
        AddP("[C] Student enrollment has decreased over the past few years.");
        AddP("[D] The original conservatory is being converted into student housing.");
        AddP("[EXPLANATION]: Câu trước nói nếu không có khoản tài trợ thì kế hoạch bị hạn chế, câu B giải thích dự án từng bị trì hoãn do cắt giảm ngân sách.");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 141]", true);
        AddP("[A] temporary"); AddP("[B*] existing"); AddP("[C] corrected"); AddP("[D] proposed");
        AddP("[EXPLANATION]: 'the existing conservatory' (khu nhạc viện hiện hữu / đang có sẵn).");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 142]", true);
        AddP("[A*] adjacent to"); AddP("[B] even though"); AddP("[C] instead of"); AddP("[D] as well as");
        AddP("[EXPLANATION]: Giới từ chỉ vị trí 'adjacent to' (nằm liền kề với phòng chờ sinh viên hiện tại).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // Part 6 - Passage 4: Questions 143-146
        AddP("[PASSAGE: 143-146]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 143-146 refer to the following e-mail.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Date: 11 January\nTo: Mitchell Parker <mparker@allmail.co.za>\nFrom: Inez Lofaro <ilofaro@daqtex.co.za>\nSubject: Product recall\n\nDear Mr. Parker,\nThank you for your recent ---[143]--- of the Daqtex Mini-V camera. We are contacting everyone who has recently bought this product to inform them that certain models are being recalled for repair. In these models, the electronic chip that enables the digital conversion of light is faulty. ---[144]---. Please ---[145]--- whether your camera has this problem by checking the serial number on the bottom of the camera. If it ends with the letters TVX, a repair will be required. Daqtex will pay all shipping costs for sending your Mini-V back to us. In addition, we will repair ---[146]--- free of charge.\n\nThank you,\nInez Lofaro, Customer Service Manager\nDaqtex Industries");
        AddP("");

        AddP("[QUESTION: 143]", true);
        AddP("[A*] purchase"); AddP("[B] review"); AddP("[C] gift"); AddP("[D] demonstration");
        AddP("[EXPLANATION]: 'your recent purchase' (việc mua hàng gần đây của quý khách).");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 144]", true);
        AddP("[A] We hope you will enjoy the product for many years to come.");
        AddP("[B] It is covered in the troubleshooting section of the manual.");
        AddP("[C*] This defect will eventually interfere with the clarity of your images.");
        AddP("[D] This special feature is unavailable on some older models.");
        AddP("[EXPLANATION]: Câu trước nói về chip bị lỗi, câu C giải thích hậu quả của lỗi này là làm ảnh hưởng đến độ rõ nét của hình ảnh.");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 145]", true);
        AddP("[A] verification"); AddP("[B] verified"); AddP("[C*] verify"); AddP("[D] verifies");
        AddP("[EXPLANATION]: Sau 'Please' là động từ nguyên mẫu chỉ mệnh lệnh lịch sự: 'Please verify' (Vui lòng kiểm tra/xác minh).");
        AddP("[END_QUESTION]"); AddP("");

        AddP("[QUESTION: 146]", true);
        AddP("[A] mine"); AddP("[B*] it"); AddP("[C] theirs"); AddP("[D] these");
        AddP("[EXPLANATION]: Tân ngữ thay thế cho chiếc máy ảnh (danh từ số ít): 'repair it free of charge' (sửa chữa nó miễn phí).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // =========================================================================
        // PART 7: QUESTIONS 147 - 200 (Reading Comprehension - 54 câu hỏi)
        // =========================================================================
        AddP("[PART: 7]", true, "2563EB");
        AddP("");

        // 147-148
        AddP("[PASSAGE: 147-148]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 147-148 refer to the following notice.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Fastest Fleet\n\nRATE OUR SERVICES AND WIN FREE TRAVEL!\n\nVisit our Web site at www.fastestfleet.com/feedback and fill out the 5-minute questionnaire. You could win one of 10 FREE round-trip bus trips to any destination within the continental United States!\n\nYou must be 18 or older to participate. Only customers who have previously traveled with Fastest Fleet are eligible to enter.");
        AddP("");
        AddP("[QUESTION: 147]", true);
        AddP("Where would the notice likely be seen?");
        AddP("[A] In an airport"); AddP("[B*] In a bus station"); AddP("[C] In a train station"); AddP("[D] In a ferry terminal");
        AddP("[EXPLANATION]: Bài viết tặng 'FREE round-trip bus trips' nên thông báo này xuất hiện ở trạm xe buýt (bus station).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 148]", true);
        AddP("What are customers asked to do?");
        AddP("[A*] Provide feedback"); AddP("[B] Buy a round-trip ticket"); AddP("[C] Take advantage of a discount"); AddP("[D] Join a customer loyalty program");
        AddP("[EXPLANATION]: Tiêu đề yêu cầu: 'RATE OUR SERVICES... fill out the 5-minute questionnaire' (cung cấp phản hồi đánh giá).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 149-150
        AddP("[PASSAGE: 149-150]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 149-150 refer to the following online chat discussion.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Daniel Mather (9:38 A.M.)\nHi Sue, are you working at home today?\n\nSusan Maldini (9:39 A.M.)\nNo, but I'm at an off-site meeting. What's up?\n\nDaniel Mather (9:41 A.M.)\nThe manager asked me to make hard copies of some contract agreements for our records. I forgot my printer card at home. Could I borrow yours?\n\nSusan Maldini (9:42 A.M.)\nSure thing. It's in the business card holder on my desk.\n\nDaniel Mather (9:55 A.M.)\nOkay, done. Thank you! I put it back on your desk.");
        AddP("");
        AddP("[QUESTION: 149]", true);
        AddP("What task was Mr. Mather given?");
        AddP("[A] To send out contracts"); AddP("[B] To repair the photocopier"); AddP("[C] To monitor employee attendance"); AddP("[D*] To make copies of some documents");
        AddP("[EXPLANATION]: Daniel Mather nói: 'The manager asked me to make hard copies of some contract agreements' (in sao tài liệu hợp đồng).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 150]", true);
        AddP("At 9:42 A.M., what does Ms. Maldini most likely mean when she writes 'Sure thing'?");
        AddP("[A*] She can lend Mr. Mather her card."); AddP("[B] She will return to the office."); AddP("[C] She will contact the manager."); AddP("[D] She can copy the agreements.");
        AddP("[EXPLANATION]: Daniel hỏi mượn thẻ in 'Could I borrow yours?', Susan trả lời 'Sure thing' đồng ý cho mượn.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 151-152
        AddP("[PASSAGE: 151-152]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 151-152 refer to the following memo.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("MEMO\nTo: Westhauser Laboratories Staff\nFrom: Area manager\nDate: March 22\nRe: Break-time beverages\n\nBeginning immediately, staff members are asked to contribute toward the cost of hot beverages available in our kitchenette. For each cup of coffee or tea you pour for yourself, we are requesting that you leave fifty cents. Please note that this amount is still far less than what you would pay at the café next to our building.\n\nEach time you prepare a beverage, just place your payment in the marked can on the counter next to the sink. As in the past, a variety of high-quality tea and coffee supplies will be made available, and products will be restocked monthly. Your understanding is appreciated so that everyone can continue to enjoy conveniently located hot beverages throughout the workday.");
        AddP("");
        AddP("[QUESTION: 151]", true);
        AddP("What is the purpose of the memo?");
        AddP("[A*] To explain a new policy"); AddP("[B] To discourage long breaks"); AddP("[C] To provide a budget update"); AddP("[D] To address staff complaints");
        AddP("[EXPLANATION]: Thông báo về chính sách mới yêu cầu đóng góp 50 cents cho mỗi tách trà/cà phê.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 152]", true);
        AddP("What are staff members asked to do?");
        AddP("[A] Bring their own beverages to work"); AddP("[B*] Leave payments in a container"); AddP("[C] Submit requests for supplies"); AddP("[D] Keep the kitchenette tidy");
        AddP("[EXPLANATION]: Văn bản ghi: 'place your payment in the marked can on the counter next to the sink' (bỏ tiền vào hộp chứa).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 153-155
        AddP("[PASSAGE: 153-155]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 153-155 refer to the following notice.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Attention Milwaukee Modern Art Museum Visitors\n\nThe Meacham Room is closed temporarily as we prepare the space for the Modern Artist Showcase, which will run from April 1 to June 30.\n\nWe encourage you to return to experience this much-anticipated exhibit that will feature paintings, sculpture, and multimedia works by acclaimed international artists such as Sally Acosta, Frank Kember, Kimberly Hong, and Matilda Breeland. Of note is newcomer Theodore Carmody's sculpture collection that was dubbed 'most exciting debut of the decade' by Ethan Lerner, renowned and respected critic of the Ireland Arts Chronicle. More information about the featured artists and the exhibit can be found at www.milwaukeemodernart.org. Or download our museum app to keep current with all upcoming events.");
        AddP("");
        AddP("[QUESTION: 153]", true);
        AddP("What is indicated about the museum?");
        AddP("[A*] It is promoting an upcoming exhibit on its Web site."); AddP("[B] It is known for its sculpture exhibits."); AddP("[C] It will be closed for renovations in April."); AddP("[D] Its upcoming exhibit is free for museum members.");
        AddP("[EXPLANATION]: Đoạn cuối ghi: 'More information about the featured artists and the exhibit can be found at www.milwaukeemodernart.org'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 154]", true);
        AddP("Who is Ms. Breeland?");
        AddP("[A] A journalist"); AddP("[B*] An artist"); AddP("[C] A museum curator"); AddP("[D] An art critic");
        AddP("[EXPLANATION]: Văn bản liệt kê: 'acclaimed international artists such as Sally Acosta, Frank Kember, Kimberly Hong, and Matilda Breeland'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 155]", true);
        AddP("Who was praised by Mr. Lerner?");
        AddP("[A] Ms. Acosta"); AddP("[B] Mr. Kember"); AddP("[C] Ms. Hong"); AddP("[D*] Mr. Carmody");
        AddP("[EXPLANATION]: Văn bản nêu: 'newcomer Theodore Carmody's sculpture collection that was dubbed \"most exciting debut of the decade\" by Ethan Lerner'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 156-157
        AddP("[PASSAGE: 156-157]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 156-157 refer to the following form.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Change of Work Order\nGeneral Contractor: Howard Kleiber, Mercrest Construction, 106 Pickens Way, Columbus, OH 43211\nProperty Owner: Oscar Copeland, 866 Andell Road, Columbus, OH 43215\nProject Start Date: July 10 | Project End Date: July 15\n\nDescription of Work Added/Deleted:\nClient will now be supplying the countertops and faucet in addition to the kitchen cabinets. Therefore, the total project cost will be adjusted to only include the cost of installation.\nOriginal Contract Price: $3,496\nNet Reduction from Previous Contract: -$2,412\nTotal Project Cost with Approved Changes: $1,084\n\nRevised Payment Schedule:\n15% of total project cost ($162) is due upfront to secure contractor, $222 is due on project start date, and the remainder ($700) is due upon project completion.");
        AddP("");
        AddP("[QUESTION: 156]", true);
        AddP("Why has the price of a project been adjusted?");
        AddP("[A*] The client is providing all the materials."); AddP("[B] The client wants to match a competitor's price."); AddP("[C] The contractor installed countertops incorrectly."); AddP("[D] The contractor overestimated the cost of labor.");
        AddP("[EXPLANATION]: Văn bản ghi: 'Client will now be supplying the countertops and faucet in addition to the kitchen cabinets' (khách tự cung cấp vật liệu).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 157]", true);
        AddP("How much money will Mr. Copeland most likely give Mr. Kleiber on July 15 ?");
        AddP("[A] $162"); AddP("[B] $222"); AddP("[C*] $700"); AddP("[D] $1,084");
        AddP("[EXPLANATION]: July 15 là ngày hoàn thành dự án ('Project End Date: July 15'), khoản thanh toán khi hoàn thành là 'the remainder ($700) is due upon project completion'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 158-160
        AddP("[PASSAGE: 158-160]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 158-160 refer to the following e-mail.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("To: Dahlia Pawar <dpawar@bronsonco.ca>\nFrom: Anna Bondell <abondell@noaaa.com>\nSubject: Information\nDate: August 25\n\nDear Ms. Pawar:\nThank you for your continued membership in the North American Architects Association. I'm writing to remind you that preregistration for the annual conference closes on September 15. ---[1]---. This year's conference features more than 100 sessions, an exhibit hall, and special workshops. ---[2]---.\n\nPreregistration will save you $30 off the regular registration fee. ---[3]---. At the same time, you'll be able to sign up in advance for a group tour of the city and for the VIP Banquet, which always fills up quickly. Visit our Web site to register online. ---[4]---. If you prefer to register by telephone, contact us at 1-249-555-0177. Please have your membership number available.\n\nLooking forward to seeing you in Mexico City!\nSincerely, Anna Bondell, Membership Coordinator");
        AddP("");
        AddP("[QUESTION: 158]", true);
        AddP("What most likely is Ms. Pawar's occupation?");
        AddP("[A*] Architect"); AddP("[B] Writer"); AddP("[C] Travel agent"); AddP("[D] Conference organizer");
        AddP("[EXPLANATION]: Thư gửi thành viên của hiệp hội 'North American Architects Association' (Hiệp hội Kiến trúc sư Bắc Mỹ).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 159]", true);
        AddP("What is NOT a benefit of conference preregistration?");
        AddP("[A] A tour of Mexico City"); AddP("[B*] A hotel room upgrade"); AddP("[C] Admission to a dinner"); AddP("[D] Reduced registration fees");
        AddP("[EXPLANATION]: Quyền lợi bao gồm: giảm $30 (D), tour tham quan (A), tiệc VIP banquet (C). Không đề cập đến việc nâng cấp phòng khách sạn (B).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 160]", true);
        AddP("In which of the positions marked [1], [2], [3], and [4] does the following sentence best belong? 'You will also find the complete conference program there.'");
        AddP("[A] [1]"); AddP("[B] [2]"); AddP("[C] [3]"); AddP("[D*] [4]");
        AddP("[EXPLANATION]: Trước vị trí [4] là câu 'Visit our Web site to register online', từ 'there' chỉ trang web nơi người đọc có thể tìm thấy toàn bộ chương trình hội thảo.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 161-164
        AddP("[PASSAGE: 161-164]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 161-164 refer to the following online chat discussion.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Kurt Webber (4:30 P.M.)\nJake and I are grabbing a bite to eat after work, around 5:30. Anyone care to join us?\n\nDavi Avril (4:31 P.M.)\nMaybe. I still have some work to do on the midyear report. Where are you planning to go?\n\nKurt Webber (4:32 P.M.)\nWe're thinking of trying the new Mexican place on Slate Road. It's called Ocho Coronas.\n\nTim Foxx (4:33 P.M.)\nYou're out of luck. That place closed a few days ago.\n\nKurt Webber (4:34 P.M.)\nThat's too bad. I heard great things about it.\n\nTim Foxx (4:36 P.M.)\nHow about Lotus Tacos around the corner? They always have a special menu on Fridays.\n\nKurt Webber (4:37 P.M.)\nThat would be great. Do you want to go to Lotus Tacos, Jake?\n\nDavi Avril (4:38 P.M.)\nOK, but I won't be able to get there till about six.\n\nJake Rivas (4:39 P.M.)\nSounds good to me. And Davi, I just sent you the updated figures for the report.");
        AddP("");
        AddP("[QUESTION: 161]", true);
        AddP("What are the writers discussing?");
        AddP("[A] Where to hold a company banquet"); AddP("[B] What restaurant serves the best food"); AddP("[C] What is on the menu at Ocho Coronas"); AddP("[D*] Where to go for dinner");
        AddP("[EXPLANATION]: Họ bàn luận việc đi ăn tối sau giờ làm: 'grabbing a bite to eat after work'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 162]", true);
        AddP("What information does Mr. Foxx provide about Ocho Coronas?");
        AddP("[A] People have recommended it to him."); AddP("[B] It usually opens early on Fridays."); AddP("[C] The food there is not very authentic."); AddP("[D*] It is no longer in business.");
        AddP("[EXPLANATION]: Tim Foxx cho biết: 'That place closed a few days ago' (quán đã đóng cửa vài ngày trước).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 163]", true);
        AddP("At 4:34 P.M., why most likely does Mr. Webber write, 'That's too bad'?");
        AddP("[A*] He wanted to try a new restaurant."); AddP("[B] Mr. Foxx cannot complete a project."); AddP("[C] Slate Road is too far away."); AddP("[D] He has a scheduling conflict.");
        AddP("[EXPLANATION]: Kurt tiếc nuối vì quán mới mà anh ấy định thử đã đóng cửa.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 164]", true);
        AddP("What does Mr. Avril decide to do?");
        AddP("[A] Research nearby restaurants"); AddP("[B] Work extra hours tomorrow"); AddP("[C*] Join his coworkers for a meal"); AddP("[D] Have food delivered to the office");
        AddP("[EXPLANATION]: Davi Avril đồng ý tham gia: 'OK, but I won't be able to get there till about six'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 165-167
        AddP("[PASSAGE: 165-167]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 165-167 refer to the following letter.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Multinational Voice Magazine\nPO Box 17999, Greenlane, Auckland 1546\n\nTuata Wehi, 16 Page Street, Wellington 6023\nDear Mr. Wehi,\n\nOn 25 March, your three-month trial subscription to Multinational Voice Magazine will expire. To continue to receive this vital news resource, please fill out the postage-paid renewal card included with this letter and mail it in before the end of this month. You may select from a three-month, six-month, one-year, or two-year subscription. Do note that our two-year offer is by far the most economical. Also, the one- and two-year options include the 'Notable People of the Year' special edition.\n\nThank you once again for choosing Multinational Voice Magazine. We continually strive to provide the best commentary on global affairs from a New Zealand perspective. To provide your feedback, please visit www.mvmagazine.co.nz and fill out our online comment form.\n\nSincerely,\nEstelle Pearson, Customer Service Representative");
        AddP("");
        AddP("[QUESTION: 165]", true);
        AddP("Why was the letter sent to Mr. Wehi?");
        AddP("[A] To report a late payment"); AddP("[B] To offer him a refund"); AddP("[C] To promote a new service"); AddP("[D*] To remind him to resubscribe");
        AddP("[EXPLANATION]: Thư thông báo thời gian dùng thử sắp hết hạn và nhắc nhở gia hạn đăng ký (resubscribe).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 166]", true);
        AddP("For about how long has Mr. Wehi been receiving Multinational Voice Magazine?");
        AddP("[A*] For three months"); AddP("[B] For six months"); AddP("[C] For one year"); AddP("[D] For two years");
        AddP("[EXPLANATION]: Thư ghi: 'your three-month trial subscription... will expire'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 167]", true);
        AddP("What is one thing that Mr. Wehi is encouraged to do?");
        AddP("[A] Call Ms. Pearson directly"); AddP("[B*] Offer his opinion"); AddP("[C] Attend an event"); AddP("[D] Confirm his address");
        AddP("[EXPLANATION]: Thư khuyến khích: 'To provide your feedback, please visit www.mvmagazine.co.nz...' (đóng góp ý kiến).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 168-171
        AddP("[PASSAGE: 168-171]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 168-171 refer to the following e-mail.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("From: info@morganairportshuttle.com\nTo: tgrant@tivimail.com\nSubject: re: Suitcase on board\nDate: January 19\n\nDear Mr. Grant,\n\nThanks for your inquiry. There are a number of suitcases in our storeroom that match the description you provided, but we have not found one bearing a name tag identifying you as the owner. You will need to come over and find yours on-site at 620 Baker St. Please note that you will be asked to indicate what time your bus left the airport terminal or, better yet, to present your ticket if you still have it.\n\nWe are open 6 A.M.-10 P.M. every day. Please do not delay. Due to the large number of items found by our drivers, we have no choice but to limit storage time to 7 days only, after which we dispose of the item.\n\nRegards,\nGina Steiner, Morgan Airport Shuttle");
        AddP("");
        AddP("[QUESTION: 168]", true);
        AddP("Where does Ms. Steiner work?");
        AddP("[A*] At a transportation service"); AddP("[B] At an insurance company"); AddP("[C] At a car rental company"); AddP("[D] At a travel agency");
        AddP("[EXPLANATION]: Tên công ty là 'Morgan Airport Shuttle' (dịch vụ xe đưa đón sân bay).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 169]", true);
        AddP("What did Mr. Grant most likely inquire about?");
        AddP("[A] Reservation options"); AddP("[B] Luggage limitations"); AddP("[C] Travel expenses"); AddP("[D*] Lost property");
        AddP("[EXPLANATION]: Tiêu đề 'Suitcase on board' và nội dung nói về tìm lại vali bị bỏ quên trên xe (hành lý thất lạc).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 170]", true);
        AddP("What information will Mr. Grant be asked to provide?");
        AddP("[A] His address"); AddP("[B] His phone number"); AddP("[C*] His departure time"); AddP("[D] His final destination");
        AddP("[EXPLANATION]: Thư nêu: 'you will be asked to indicate what time your bus left the airport terminal' (giờ xe buýt xuất phát).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 171]", true);
        AddP("What is Mr. Grant advised to do?");
        AddP("[A] Take advantage of a discount"); AddP("[B] Compare payment options"); AddP("[C*] Visit the office as soon as possible"); AddP("[D] Print out an electronic ticket");
        AddP("[EXPLANATION]: Thư nhắc: 'Please do not delay... limit storage time to 7 days only' (đến văn phòng càng sớm càng tốt).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 172-175
        AddP("[PASSAGE: 172-175]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 172-175 refer to the following article.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Spectacular Bridge Created with Super Cranes\nBy Marcia Brunon\nMarch 14\n\nFive years ago, the town of Stonewell, situated at the foot of the Marshall Mountains, decided to finance the building of a bridge across the Stonewell River. It was planned as a much-needed northern route for the town. - [1] - . The efficiency of the project was directly attributable to two RWC451 cranes, manufactured by Ronsonworks, a company based in England.\n\nThe two identical tower cranes were owned by the construction firm Redding Builders. One crane was assembled at the south bank of the river, while the other one was placed on a concrete pylon in the center of the Stonewell River. - [2] -. The assembly of the second crane was particularly challenging, as it required the use of another crane on a floating barge.\n\n'Any large project is going to require unforeseen adjustments that may disrupt an official construction schedule,' said Roger Lee, chief engineer of Redding Builders. - [3] - . In this case, the metal anchors that connected the bridge to the pylons were determined to be too small. The new anchors weighed 22 tons, which is within the safe lifting capacity of an RWC451 crane. - [4] - . Engineers reinforced the existing cranes for extra security, and the cranes performed well.\n\nToday, this postcard-worthy bridge is the pride of Stonewell, featuring viewing platforms for pedestrians, hikers, and visitors at the base of each pier overlooking the mountainous valley. Construction materials and colors harmonize beautifully with the local environment. All design elements, including the piers, viewing platforms, and railings, complement the natural landscape, making the bridge a hit with everyone from hikers to the truck drivers who now include it in their regular route.");
        AddP("");
        AddP("[QUESTION: 172]", true);
        AddP("What is mentioned about the cranes?");
        AddP("[A] They are owned by Ronsonworks.");
        AddP("[B] They were designed specifically for the Stonewell Bridge project.");
        AddP("[C*] They were assembled on the Stonewell Bridge construction site.");
        AddP("[D] They weigh more than 22 tons.");
        AddP("[EXPLANATION]: Bài báo mô tả việc lắp ráp cần cẩu trực tiếp tại bờ sông và trên trụ giữa sông.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 173]", true);
        AddP("What is indicated by Mr. Lee?");
        AddP("[A] His professional specialty is bridge construction.");
        AddP("[B*] He believes that projects like the Stonewell Bridge always present problems.");
        AddP("[C] He delayed the Stonewell Bridge project for budgetary reasons.");
        AddP("[D] His first project as a chief engineer was the Stonewell Bridge.");
        AddP("[EXPLANATION]: Ông Lee nói: 'Any large project is going to require unforeseen adjustments that may disrupt an official construction schedule'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 174]", true);
        AddP("What is NOT suggested about the Stonewell Bridge?");
        AddP("[A] It provides a northward exit out of town.");
        AddP("[B] It was designed for both pedestrians and vehicles.");
        AddP("[C] It was designed to blend with the natural setting.");
        AddP("[D*] It required more workers than first estimated.");
        AddP("[EXPLANATION]: Không có chi tiết nào nói rằng cây cầu cần nhiều công nhân hơn ước tính ban đầu.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 175]", true);
        AddP("In which of the positions marked [1], [2], [3], and [4] does the following sentence best belong? 'The project, which was expected to take four years, was completed in less than three.'");
        AddP("[A*] [1]"); AddP("[B] [2]"); AddP("[C] [3]"); AddP("[D] [4]");
        AddP("[EXPLANATION]: Vị trí [1] theo sau bởi câu 'The efficiency of the project was directly attributable to two RWC451 cranes', giải thích lý do hoàn thành trước thời hạn.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 176-180
        AddP("[PASSAGE: 176-180]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 176-180 refer to the following memo and schedule.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("To: Absalom and Twigg Law Firm employees\nFrom: Shawna Montgomery, Office Manager\nSubject: March plans | Sent: February 12 | Attachment: Schedule\n\nAs most of you are aware, our schedule will be a bit challenging during the first week in March. Various rooms and offices will need to be vacated for certain periods to allow work crews to repaint, recarpet, and replace old furniture. Affected employees will need to box up all their office items by 3 P.M. on the day before their room is scheduled for work (please see the attached schedule). Two teams of workers will be on-site, so more than one room at a time will need to be vacated. Note that any rooms due for work on Monday must be packed up and vacated by Friday afternoon, February 27.\n\nBoxes will be provided. Leave your boxes in the rooms for the work crews to remove. Please label them with your name and office number so that the crews can return them to the correct offices once the work is complete. Please make arrangements to continue working on your assignments while the work crews are in your rooms. The conference room (Room 409) will remain available to be used as a workspace during the entire week. Another possible option is to request permission from your supervisor to telecommute for one or two days.\n\nWORK SCHEDULE - March 2 to March 6\nMONDAY, March 2: Room 401 (Meeting room) & Room 403 (Allie Stevens & Matt Beale)\nTUESDAY, March 3: Room 401 & Room 403\nWEDNESDAY, March 4: Room 402 (Marlene Asbury & Luke Roe) & Room 408 (Meeting room)\nTHURSDAY, March 5: Room 402 & Room 408\nFRIDAY, March 6: Room 407 (Jung Li) & Room 404 (Elliot Hagburg & Ana Keller)");
        AddP("");
        AddP("[QUESTION: 176]", true);
        AddP("Why was the memo sent to employees?");
        AddP("[A*] To alert them to upcoming renovations"); AddP("[B] To announce that the firm will be relocating"); AddP("[C] To request feedback about new workplace facilities"); AddP("[D] To address their complaints about building maintenance");
        AddP("[EXPLANATION]: Thông báo kế hoạch sơn sửa, thay thảm và đổi nội thất văn phòng (renovations).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 177]", true);
        AddP("What are employees instructed to do?");
        AddP("[A] Report for work early"); AddP("[B] Schedule a meeting with a manager"); AddP("[C*] Indicate which office supplies are theirs"); AddP("[D] Update their contact information online");
        AddP("[EXPLANATION]: Thư hướng dẫn: 'Please label them with your name and office number' (đánh dấu đồ đạc của mình).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 178]", true);
        AddP("What is stated about Room 409 ?");
        AddP("[A] It will be available for videoconferencing."); AddP("[B*] Employees may gather there for work."); AddP("[C] A scheduling meeting will be held there."); AddP("[D] Its furniture will be removed temporarily.");
        AddP("[EXPLANATION]: 'The conference room (Room 409) will remain available to be used as a workspace during the entire week'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 179]", true);
        AddP("When should Ms. Asbury be ready to vacate her office?");
        AddP("[A] On February 12"); AddP("[B] On February 27"); AddP("[C*] On March 3"); AddP("[D] On March 4");
        AddP("[EXPLANATION]: Phòng của Ms. Asbury làm việc vào Wednesday March 4, nên phải đóng gói trước 3 P.M. ngày hôm trước tức March 3.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 180]", true);
        AddP("What is suggested about Mr. Hagburg?");
        AddP("[A*] He shares an office with a colleague."); AddP("[B] He will work off-site on March 5."); AddP("[C] He is the head of a department."); AddP("[D] He requested the use of a conference room.");
        AddP("[EXPLANATION]: Bảng lịch làm việc ghi: 'Room 404 (Elliot Hagburg & Ana Keller)' cùng chung phòng.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 181-185
        AddP("[PASSAGE: 181-185]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 181-185 refer to the following flyer and form.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Livrou Farm -- Fresh Organic Produce-From Our Farm to Your Home\n\nLivrou Farm in Bromont, Quebec, invites you to participate in its community-supported agriculture program. Members enjoy fresh farm produce during our growing season from June to November.\nSign up for a farm share and receive these benefits:\n- More than 30 varieties of in-season vegetables, fruits, and herbs, harvested by our farm staff and prepared for you to pick up at our barn\n- A selection of pick-your-own strawberries, apples, and other fruits\n- Access to our member Web site with recipes, farm updates, and a farm newsletter\n- Discounts on events at the farm, such as the annual summer music festival. Events typically cost $15, but members pay $10.\nMembers pick up their shares once a week at the farm. A full-size share is $700 per season, and a half-size share is $350. Half-size shareholders receive half the amount of produce each week. All other benefits remain the same. Our farm produce is grown without the use of pesticides or herbicides, and we use only naturally occurring fertilizers. See www.livroufarm.ca.\n\nForm: Registration\nName: Julien Bernard | Address: 78 Dorion Street, Bromont, QC J2L 2K7\nFirst choice pick-up day: Thursday | Second choice pick-up day: Friday\nSize of share: Half | NOTE: Pick-up is between 3 P.M. and 6 P.M.\nNames of other household individuals permitted to pick up: Margaux Bernard, Laurent Bernard");
        AddP("");
        AddP("[QUESTION: 181]", true);
        AddP("What is the purpose of the flyer?");
        AddP("[A] To invite people to a farm festival"); AddP("[B] To promote community service opportunities"); AddP("[C] To share recipes for healthy eating"); AddP("[D*] To advertise a farm program");
        AddP("[EXPLANATION]: Tờ rơi quảng cáo chương trình nông nghiệp chia sẻ cộng đồng của trang trại.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 182]", true);
        AddP("What is suggested about the workers at Livrou Farm?");
        AddP("[A*] They collect farm produce from June to November."); AddP("[B] They update the farm's Web site once a week."); AddP("[C] They create meals using the farm's products."); AddP("[D] They sell farm products at several local markets.");
        AddP("[EXPLANATION]: Tờ rơi nêu: 'growing season from June to November... harvested by our farm staff'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 183]", true);
        AddP("What is NOT indicated about Livrou Farm?");
        AddP("[A] It publishes a newsletter."); AddP("[B] It uses natural fertilizers."); AddP("[C] It hosts musical performances."); AddP("[D*] It offers cooking classes.");
        AddP("[EXPLANATION]: Không có đề cập đến các lớp dạy nấu ăn (cooking classes).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 184]", true);
        AddP("What is true about Mr. Bernard's membership?");
        AddP("[A] He must pick up his produce on Sundays."); AddP("[B] He is the only person allowed to pick up his farm share."); AddP("[C*] He will be allowed to pick some of his own fruit."); AddP("[D] He will be able to plant and grow his own vegetables.");
        AddP("[EXPLANATION]: Thành viên có quyền lợi: 'A selection of pick-your-own strawberries, apples, and other fruits'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 185]", true);
        AddP("How much should Mr. Bernard pay for the membership?");
        AddP("[A] $10"); AddP("[B] $15"); AddP("[C*] $350"); AddP("[D] $700");
        AddP("[EXPLANATION]: Form đăng ký của ông Bernard chọn 'Size of share: Half', và tờ rơi ghi rõ 'a half-size share is $350'.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 186-190
        AddP("[PASSAGE: 186-190]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 186-190 refer to the following advertisements and e-mail.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Advertisement 1: Professional Seeking Apartment in Burlington\nJorge Janssen <jjanssen@blitzer.com> | Date: June 23\nI accepted a new position in Burlington and need to relocate near the downtown area before my start date on August 15. I'm seeking a simple, clean, one-bedroom rental or larger, depending on price. A relaxing location with outdoor seating for entertaining friends or family would be a plus. I do have a car, but I'd appreciate having good access to public transportation. I have a budget of around $1,400 monthly to cover all housing costs, including utilities.\n\nAdvertisement 2: Apartment for Rent in Burlington\nEloise McMahon <mcmahonrental@hmail.com> | Date: June 23\nBe the first to rent this two-bedroom apartment upon completion of extensive renovations. This property is anticipated to be move-in ready on August 1. It will feature a clean modern look, new floors throughout, and all-new appliances. The apartment is situated downtown, and students are welcome as it's less than ten minutes by bus to Denton University. On-street parking is available with a decal from the City Transportation Office. Cats or small dogs are potentially permitted, but with conditions, so please inquire. $1,400 rent also pays for water, sewer, garbage pickups, and general upkeep of the property. Electricity and natural gas will be the responsibility of the tenant. A one-time security deposit equal to one month's rent should be paid upon signing.\n\nEmail:\nTo: Eloise McMahon | From: Jorge Janssen | Date: June 24 | Re: Apartment\nDear Ms. McMahon, I noticed your rental listing on communitylinkforum.org. From the description it sounds as if it may be just what I've been looking for. I'm eager to look over the apartment, and I just happen to be in Burlington all this week. My last day in town will be Sunday, June 30. If the place suits me, I'd want to move in the same day that it's expected to be available. The timing would be perfect! I hope to hear from you soon. Jorge Janssen");
        AddP("");
        AddP("[QUESTION: 186]", true);
        AddP("Why is Mr. Janssen relocating?");
        AddP("[A*] To begin a new job"); AddP("[B] To return to his hometown"); AddP("[C] To study part-time"); AddP("[D] To start his retirement");
        AddP("[EXPLANATION]: Ông viết: 'I accepted a new position in Burlington and need to relocate...' (bắt đầu công việc mới).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 187]", true);
        AddP("What aspect of the property does NOT match Mr. Janssen's preferences?");
        AddP("[A] The location"); AddP("[B*] The monthly costs"); AddP("[C] The parking availability"); AddP("[D] The size");
        AddP("[EXPLANATION]: Ngân sách của ông là $1,400 bao gồm tiền điện nước, nhưng tiền thuê nhà $1,400 của bà McMahon chưa bao gồm điện và gas tự nhiên.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 188]", true);
        AddP("For what situation does Ms. McMahon mention that she will need additional information?");
        AddP("[A] When changes to the decor are preferred"); AddP("[B] When a tenant is ready to pay a security deposit"); AddP("[C] When repairs to the apartment are needed"); AddP("[D*] When someone wants to keep an indoor pet");
        AddP("[EXPLANATION]: Bài đăng nêu: 'Cats or small dogs are potentially permitted, but with conditions, so please inquire' (khi nuôi thú cưng).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 189]", true);
        AddP("Why does Mr. Janssen send the e-mail?");
        AddP("[A] To agree to the terms of a rental contract"); AddP("[B] To change the details of a residential advertisement"); AddP("[C] To ask about a feature of an apartment"); AddP("[D*] To make arrangements to view a property");
        AddP("[EXPLANATION]: Ông viết: 'I'm eager to look over the apartment, and I just happen to be in Burlington all this week' (hẹn xem căn hộ).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 190]", true);
        AddP("When does Mr. Janssen wish to start living in the residence?");
        AddP("[A] On June 24"); AddP("[B] On June 30"); AddP("[C*] On August 1"); AddP("[D] On August 15");
        AddP("[EXPLANATION]: Ông nói muốn chuyển đến đúng ngày căn hộ sẵn sàng: 'move in the same day that it's expected to be available', đó là ngày August 1.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 191-195
        AddP("[PASSAGE: 191-195]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 191-195 refer to the following product description, customer review, and online response.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Document 1: Kitchenware Utopia Food Processor-Model C3\nYou will never need to buy another food processor! Our best-selling model, the C3, is made of high-quality plastic and easy-to-clean stainless steel.\nFEATURES: The unique blade design and powerful motor make this a professional-grade appliance, ideal for busy restaurants of any size.\nWARRANTY: We include a seven-year warranty on all parts and labor- a reassurance to you that our food processor will last a long time.\nSpecial purchase price: $319.00 / KU Club Members: $299.00\n\nDocument 2: Customer Review\nRating: 5 stars | Posted by Eli Perles on March 27\nReview: This product is amazing! I work as a caterer, and I've used a lot of different food processors. Kitchenware Utopia's C3 is by far the best I've found. Its motor is very powerful, and the many different blades and settings make it extremely versatile. The settings are also straightforward. It's expensive but well worth the investment, and since I'm a loyalty club member, the price was reasonable. My only complaint would be that it's very heavy and therefore not as portable as I'd hoped. Overall, though, I'm exceptionally satisfied with this product.\n\nDocument 3: Customer Service Response\nPosted by Kitchenware Utopia Customer Service on March 28\nWe are very glad to hear you are happy with our C3 food processor. Customer satisfaction is our number one priority. We would like to respond to your complaint and provide a suggestion regarding your concerns. Our C2 processor might be better suited to your professional needs. The C2 offers the same motor size as the C3, but it is much smaller than the C3 in general. However, this model does cost slightly more than the C3. You can view the product description by visiting www.kitchenwareutopia.com/C2.");
        AddP("");
        AddP("[QUESTION: 191]", true);
        AddP("What is NOT mentioned in the product description as a feature of the C3 food processor?");
        AddP("[A] It is very durable."); AddP("[B] It is suitable for commercial kitchens."); AddP("[C*] It is larger than competitors' food processors."); AddP("[D] It is a popular model.");
        AddP("[EXPLANATION]: Bài mô tả nói bền (7-year warranty), phù hợp nhà hàng bận rộn, bán chạy nhất (best-selling). Không hề so sánh kích thước lớn hơn đối thủ.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 192]", true);
        AddP("What is indicated in the customer review?");
        AddP("[A] The C3 comes with detailed instructions."); AddP("[B*] Mr. Perles is pleased with his purchase."); AddP("[C] Kitchenware Utopia's customer service is very good."); AddP("[D] Users find the C3 difficult to clean.");
        AddP("[EXPLANATION]: Đánh giá 5 sao và nhận xét: 'I'm exceptionally satisfied with this product' (rất hài lòng với sản phẩm).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 193]", true);
        AddP("What is suggested about Mr. Perles?");
        AddP("[A*] He paid $299 for the food processor."); AddP("[B] He purchased some optional parts for the processor."); AddP("[C] He catered a large-scale event on March 27."); AddP("[D] He has never used a food processor before.");
        AddP("[EXPLANATION]: Ông cho biết: 'since I'm a loyalty club member...', giá dành cho hội viên KU Club là $299.00.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 194]", true);
        AddP("Why would the C2 processor likely be recommended as more suitable for Mr. Perles?");
        AddP("[A] It is inexpensive."); AddP("[B] It is dishwasher safe."); AddP("[C] It is easy to assemble."); AddP("[D*] It is lightweight.");
        AddP("[EXPLANATION]: Ông phàn nàn C3 quá nặng ('very heavy and therefore not as portable'), và dịch vụ phản hồi C2 'much smaller than the C3 in general' (nhẹ hơn, gọn hơn).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 195]", true);
        AddP("In the online response, the word 'regarding' in paragraph 1, line 3, is closest in meaning to");
        AddP("[A] looking after"); AddP("[B*] about"); AddP("[C] in comparison"); AddP("[D] admiring");
        AddP("[EXPLANATION]: 'regarding' có nghĩa tương đương với 'about' (liên quan đến / về vấn đề của bạn).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        // 196-200
        AddP("[PASSAGE: 196-200]", true, "993300");
        AddP("[PASSAGE_TITLE]: Questions 196-200 refer to the following article, schedule, and news update.");
        AddP("[PASSAGE_CONTENT]:");
        AddP("Document 1: Ment Hall to Host Piasek Cup Final\nMent Hall, which is currently undergoing extensive renovation, will be ready to host the final game of the Piasek Cup Volleyball Championship to be held in Warsaw this June. Although the project has been in the works for some time, it was the opportunity to host this championship that provided the economic incentive to push the project to its completion. City officials have confirmed that the construction is progressing according to schedule and will be complete well before the finals of the championship. Since Ment Hall is the only major arena in the country that has never hosted a Piasek Cup event, the hall's owners were proud to have been chosen to host this year's finale. The final game will inaugurate the refurbished hall. The event is expected to draw thousands of volleyball fans, and the organizers are confident that Ment Hall will live up to the occasion. Seating 19,000 spectators, Ment Hall will be double its previous size.\n\nDocument 2: Piasek Cup Volleyball Championship Schedule\nQuarterfinal Games:\n- Venezuela-Australia: 10 June, 3:00 P.M., Loave Arena, Bydgoszcz\n- Poland-France: 10 June, 6:00 P.M., Timpani Hall, Katowice\n- Brazil-USA: 11 June, 3:00 P.M., Polana Center, Cracow\n- Cuba-Egypt: 11 June, 6:00 P.M., Mistrz Arena, Wroclaw\nSemifinal Games:\n- Winners of 10 June games: 13 June, 3:00 P.M., Timpani Hall, Katowice\n- Winners of 11 June games: 13 June, 3:00 P.M., Polana Center, Cracow\nFinal Game:\n- 16 June, 6:00 P.M., Ment Hall, Warsaw\nNOTE: Tickets for the final game are selling fast, so don't wait until after the semifinal games to order. Reserved seating only, no general admission. No refunds.\n\nDocument 3: News Update -- HALLAX RADIO 108.3 FM\nSports-Volleyball, 10 June\nAt 3:00 P.M., Venezuela will take on Australia for qualification to the semifinals. Our guest announcer will be Vin Cote, retired coach of last year's winning team from Canada. At 6:00 P.M., host team Poland will face off against France. The game will be covered live by Jeremy Bosko.");
        AddP("");
        AddP("[QUESTION: 196]", true);
        AddP("In the article, the word 'draw' in paragraph 2, line 7, is closest in meaning to");
        AddP("[A] promote"); AddP("[B*] attract"); AddP("[C] sketch"); AddP("[D] remove");
        AddP("[EXPLANATION]: 'draw thousands of volleyball fans' nghĩa là 'thu hút hàng ngàn người hâm mộ' (attract).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 197]", true);
        AddP("What will be changed about Ment Hall?");
        AddP("[A] Its ownership"); AddP("[B] Its ticket prices"); AddP("[C*] Its seating capacity"); AddP("[D] Its location");
        AddP("[EXPLANATION]: Bài báo ghi: 'Seating 19,000 spectators, Ment Hall will be double its previous size' (tăng sức chứa gấp đôi).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 198]", true);
        AddP("When will the first game be held at a new host venue?");
        AddP("[A*] On June 10"); AddP("[B] On June 13"); AddP("[C] On June 14"); AddP("[D] On June 16");
        AddP("[EXPLANATION]: Trận chung kết tại Ment Hall (địa điểm mới chưa từng tổ chức sự kiện này) diễn ra ngày 16 June. Tuy nhiên câu hỏi 'the first game held at a new host venue' theo lịch thi đấu chính thức bắt đầu từ tứ kết ngày 10 June hoặc chung kết 16 June.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 199]", true);
        AddP("In the schedule, what are people advised to do?");
        AddP("[A] Request a refund for tickets that they do not plan to use");
        AddP("[B] Order tickets immediately after the semifinal games");
        AddP("[C*] Buy tickets for the final game as soon as possible");
        AddP("[D] Arrive at the final game early");
        AddP("[EXPLANATION]: Ghi chú ghi: 'Tickets for the final game are selling fast, so don't wait until after the semifinal games to order' (hãy mua vé càng sớm càng tốt).");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[QUESTION: 200]", true);
        AddP("Where will Mr. Cote be reporting from?");
        AddP("[A*] Bydgoszcz"); AddP("[B] Katowice"); AddP("[C] Cracow"); AddP("[D] Wroclaw");
        AddP("[EXPLANATION]: Vin Cote bình luận trận Venezuela vs Australia lúc 3:00 P.M. ngày 10 June, trận đấu này diễn ra tại Loave Arena, Bydgoszcz.");
        AddP("[END_QUESTION]"); AddP("");
        AddP("[END_PASSAGE]"); AddP("");

        wordDoc.Save();
    }
}
