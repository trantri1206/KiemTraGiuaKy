import 'package:flutter/material.dart';
import '../models/exam_result_model.dart';
import '../theme/app_theme.dart';

class ExamReviewScreen extends StatefulWidget {
  final ExamResult result;

  const ExamReviewScreen({super.key, required this.result});

  @override
  State<ExamReviewScreen> createState() => _ExamReviewScreenState();
}

class _ExamReviewScreenState extends State<ExamReviewScreen> {
  int _filterType = 0; // 0: All, 1: Wrong, 2: Skipped, 3: Correct

  List<QuestionResult> _getFilteredQuestions() {
    switch (_filterType) {
      case 1:
        return widget.result.questionResults.where((q) => !q.isCorrect && !q.isSkipped).toList();
      case 2:
        return widget.result.questionResults.where((q) => q.isSkipped).toList();
      case 3:
        return widget.result.questionResults.where((q) => q.isCorrect).toList();
      default:
        return widget.result.questionResults;
    }
  }

  @override
  Widget build(BuildContext context) {
    final filtered = _getFilteredQuestions();

    return Scaffold(
      appBar: AppBar(
        title: const Text('Xem Lại Bài Thi & Giải Thích'),
      ),
      body: Column(
        children: [
          // Filter Chips Bar
          Container(
            color: Colors.white,
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: Row(
                children: [
                  _buildFilterChip(0, 'Tất cả (${widget.result.questionResults.length})'),
                  const SizedBox(width: 8),
                  _buildFilterChip(1, 'Câu làm sai (${widget.result.totalWrong})', color: AppTheme.dangerRed),
                  const SizedBox(width: 8),
                  _buildFilterChip(2, 'Bỏ qua (${widget.result.totalSkipped})', color: Colors.grey.shade600),
                  const SizedBox(width: 8),
                  _buildFilterChip(3, 'Câu đúng (${widget.result.totalCorrect})', color: AppTheme.emeraldGreen),
                ],
              ),
            ),
          ),

          // Question List
          Expanded(
            child: filtered.isEmpty
                ? const Center(
                    child: Text('Không có câu hỏi nào trong mục này.', style: TextStyle(color: Colors.grey)),
                  )
                : ListView.builder(
                    padding: const EdgeInsets.all(16),
                    itemCount: filtered.length,
                    itemBuilder: (ctx, i) {
                      final q = filtered[i];
                      return _buildQuestionReviewCard(q);
                    },
                  ),
          ),
        ],
      ),
    );
  }

  Widget _buildFilterChip(int type, String label, {Color color = AppTheme.primaryBlue}) {
    final isSelected = _filterType == type;
    return ChoiceChip(
      label: Text(label),
      selected: isSelected,
      selectedColor: color.withValues(alpha: 0.18),
      labelStyle: TextStyle(
        color: isSelected ? color : Colors.black87,
        fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
        fontSize: 12,
      ),
      onSelected: (_) {
        setState(() {
          _filterType = type;
        });
      },
    );
  }

  Widget _buildQuestionReviewCard(QuestionResult q) {
    Color statusColor = q.isCorrect
        ? AppTheme.emeraldGreen
        : (q.isSkipped ? Colors.grey : AppTheme.dangerRed);

    String statusText = q.isCorrect
        ? 'ĐÚNG'
        : (q.isSkipped ? 'BỎ QUA' : 'SAI');

    return Card(
      margin: const EdgeInsets.only(bottom: 16),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Header
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                  decoration: BoxDecoration(
                    color: Colors.grey.shade200,
                    borderRadius: BorderRadius.circular(6),
                  ),
                  child: Text('Part ${q.partNumber}', style: const TextStyle(fontSize: 11, fontWeight: FontWeight.bold, color: Colors.black87)),
                ),
                const SizedBox(width: 8),
                Text('Câu ${q.questionNumber}', style: const TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: AppTheme.textDark)),
                const Spacer(),
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                  decoration: BoxDecoration(
                    color: statusColor.withValues(alpha: 0.12),
                    borderRadius: BorderRadius.circular(20),
                    border: Border.all(color: statusColor),
                  ),
                  child: Row(
                    children: [
                      Icon(
                        q.isCorrect ? Icons.check_circle : (q.isSkipped ? Icons.help_outline : Icons.cancel),
                        color: statusColor,
                        size: 14,
                      ),
                      const SizedBox(width: 4),
                      Text(statusText, style: TextStyle(color: statusColor, fontWeight: FontWeight.bold, fontSize: 11)),
                    ],
                  ),
                ),
              ],
            ),

            // Reading Passage if Part 6 or 7
            if (q.passageContent != null && q.passageContent!.isNotEmpty) ...[
              const SizedBox(height: 12),
              Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: const Color(0xFFF8FAFC),
                  borderRadius: BorderRadius.circular(8),
                  border: Border.all(color: const Color(0xFFE2E8F0)),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    if (q.passageTitle != null && q.passageTitle!.isNotEmpty) ...[
                      Text(q.passageTitle!, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 12, color: AppTheme.navyDark)),
                      const SizedBox(height: 6),
                    ],
                    Text(
                      q.passageContent!,
                      style: const TextStyle(fontSize: 12, height: 1.5, color: Colors.black87),
                      maxLines: 6,
                      overflow: TextOverflow.ellipsis,
                    ),
                  ],
                ),
              ),
            ],

            const SizedBox(height: 12),
            Text(q.content, style: const TextStyle(fontSize: 14, fontWeight: FontWeight.w600, color: AppTheme.textDark)),
            const SizedBox(height: 14),

            // Options
            ...q.options.map((opt) {
              final isCandidateSelected = q.selectedOptionId == opt.id;
              final isCorrectOption = opt.isCorrect;

              Color optBg = Colors.white;
              Color optBorder = AppTheme.cardBorder;
              Widget? trailingIcon;

              if (isCorrectOption) {
                optBg = AppTheme.emeraldGreen.withValues(alpha: 0.08);
                optBorder = AppTheme.emeraldGreen;
                trailingIcon = const Icon(Icons.check_circle, color: AppTheme.emeraldGreen, size: 20);
              } else if (isCandidateSelected && !isCorrectOption) {
                optBg = AppTheme.dangerRed.withValues(alpha: 0.08);
                optBorder = AppTheme.dangerRed;
                trailingIcon = const Icon(Icons.cancel, color: AppTheme.dangerRed, size: 20);
              }

              return Container(
                margin: const EdgeInsets.only(bottom: 8),
                padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                decoration: BoxDecoration(
                  color: optBg,
                  borderRadius: BorderRadius.circular(8),
                  border: Border.all(color: optBorder, width: isCorrectOption || isCandidateSelected ? 1.5 : 1),
                ),
                child: Row(
                  children: [
                    Container(
                      width: 28,
                      height: 28,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        color: isCorrectOption ? AppTheme.emeraldGreen : (isCandidateSelected ? AppTheme.dangerRed : Colors.grey.shade100),
                      ),
                      alignment: Alignment.center,
                      child: Text(
                        opt.optionLabel,
                        style: TextStyle(
                          color: isCorrectOption || isCandidateSelected ? Colors.white : Colors.black87,
                          fontWeight: FontWeight.bold,
                          fontSize: 13,
                        ),
                      ),
                    ),
                    const SizedBox(width: 12),
                    Expanded(
                      child: Text(
                        opt.content,
                        style: TextStyle(
                          fontSize: 13,
                          fontWeight: isCorrectOption || isCandidateSelected ? FontWeight.bold : FontWeight.normal,
                          color: isCorrectOption ? Colors.green.shade900 : (isCandidateSelected ? Colors.red.shade900 : AppTheme.textDark),
                        ),
                      ),
                    ),
                    ?trailingIcon,
                  ],
                ),
              );
            }),

            // Explanation Box
            if (q.explanation != null && q.explanation!.isNotEmpty) ...[
              const SizedBox(height: 10),
              Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: const Color(0xFFFEF3C7),
                  borderRadius: BorderRadius.circular(8),
                  border: Border.all(color: const Color(0xFFFDE68A)),
                ),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Icon(Icons.lightbulb_outline, color: Color(0xFFB45309), size: 20),
                    const SizedBox(width: 8),
                    Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text('Giải thích chi tiết:', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 12, color: Color(0xFF92400E))),
                          const SizedBox(height: 4),
                          Text(q.explanation!, style: const TextStyle(fontSize: 13, color: Color(0xFF78350F), height: 1.4)),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}
