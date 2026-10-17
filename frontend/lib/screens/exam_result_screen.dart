import 'package:flutter/material.dart';
import '../models/exam_model.dart';
import '../models/exam_result_model.dart';
import '../theme/app_theme.dart';
import 'exam_review_screen.dart';

class ExamResultScreen extends StatelessWidget {
  final ExamResult result;
  final ExamSummary examSummary;

  const ExamResultScreen({
    super.key,
    required this.result,
    required this.examSummary,
  });

  String _getEvaluation(int score) {
    if (score >= 450) return 'Xuất sắc (CEFR C1)';
    if (score >= 400) return 'Giỏi (CEFR B2+ - Đạt chuẩn tốt nghiệp cao cấp)';
    if (score >= 350) return 'Khá (CEFR B2 - Đạt chuẩn đầu ra Đại học)';
    if (score >= 250) return 'Trung bình khá (CEFR B1)';
    return 'Cần cố gắng rèn luyện thêm';
  }

  Color _getScoreColor(int score) {
    if (score >= 400) return AppTheme.emeraldGreen;
    if (score >= 300) return AppTheme.primaryBlue;
    if (score >= 200) return AppTheme.warningOrange;
    return AppTheme.dangerRed;
  }

  @override
  Widget build(BuildContext context) {
    final scoreColor = _getScoreColor(result.score);

    return Scaffold(
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text('Kết Quả Bài Thi TOEIC'),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            // Score Banner Card
            Card(
              child: Padding(
                padding: const EdgeInsets.symmetric(vertical: 28, horizontal: 20),
                child: Column(
                  children: [
                    Container(
                      padding: const EdgeInsets.all(16),
                      decoration: BoxDecoration(
                        color: scoreColor.withValues(alpha: 0.12),
                        shape: BoxShape.circle,
                      ),
                      child: Icon(Icons.emoji_events, size: 54, color: scoreColor),
                    ),
                    const SizedBox(height: 14),
                    const Text(
                      'ĐIỂM TOEIC READING QUY ĐỔI',
                      style: TextStyle(fontSize: 12, fontWeight: FontWeight.bold, color: AppTheme.textMuted, letterSpacing: 1),
                    ),
                    const SizedBox(height: 6),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      crossAxisAlignment: CrossAxisAlignment.baseline,
                      textBaseline: TextBaseline.alphabetic,
                      children: [
                        Text(
                          '${result.score}',
                          style: TextStyle(
                            fontSize: 54,
                            fontWeight: FontWeight.w900,
                            color: scoreColor,
                          ),
                        ),
                        const SizedBox(width: 6),
                        const Text(
                          '/ 495',
                          style: TextStyle(fontSize: 20, color: AppTheme.textMuted, fontWeight: FontWeight.bold),
                        ),
                      ],
                    ),
                    const SizedBox(height: 8),
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 6),
                      decoration: BoxDecoration(
                        color: scoreColor.withValues(alpha: 0.12),
                        borderRadius: BorderRadius.circular(20),
                      ),
                      child: Text(
                        _getEvaluation(result.score),
                        style: TextStyle(color: scoreColor, fontWeight: FontWeight.bold, fontSize: 13),
                      ),
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),

            // Candidate Info
            Card(
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text('THÔNG TIN BÀI LÀM', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 13, color: AppTheme.textMuted)),
                    const Divider(height: 20),
                    _buildInfoRow('Thí sinh:', result.candidateName),
                    const SizedBox(height: 8),
                    _buildInfoRow('MSSV:', result.candidateCode),
                    const SizedBox(height: 8),
                    _buildInfoRow('Thời gian làm bài:', result.durationSpent),
                    const SizedBox(height: 8),
                    _buildInfoRow('Độ chính xác:', '${result.accuracyPercentage.toStringAsFixed(1)}%'),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),

            // Performance Statistics (3 Cards)
            Row(
              children: [
                Expanded(
                  child: _buildMetricCard(
                    'ĐÚNG',
                    '${result.totalCorrect}',
                    '/${result.totalQuestions}',
                    AppTheme.emeraldGreen,
                    Icons.check_circle_outline,
                  ),
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: _buildMetricCard(
                    'SAI',
                    '${result.totalWrong}',
                    '/${result.totalQuestions}',
                    AppTheme.dangerRed,
                    Icons.cancel_outlined,
                  ),
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: _buildMetricCard(
                    'BỎ QUA',
                    '${result.totalSkipped}',
                    '/${result.totalQuestions}',
                    Colors.grey.shade600,
                    Icons.help_outline,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),

            // Part Breakdown Card
            Card(
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text('KẾT QUẢ THEO PHẦN THI (PARTS 5 - 7)', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 13, color: AppTheme.textMuted)),
                    const Divider(height: 20),
                    _buildPartRow('Part 5: Incomplete Sentences', result.part5Correct, 30, const Color(0xFF4F46E5)),
                    const SizedBox(height: 12),
                    _buildPartRow('Part 6: Text Completion', result.part6Correct, 16, AppTheme.primaryBlue),
                    const SizedBox(height: 12),
                    _buildPartRow('Part 7: Reading Comprehension', result.part7Correct, 54, const Color(0xFF7C3AED)),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 24),

            // Actions
            SizedBox(
              width: double.infinity,
              height: 48,
              child: ElevatedButton.icon(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (_) => ExamReviewScreen(result: result),
                    ),
                  );
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppTheme.primaryBlue,
                ),
                icon: const Icon(Icons.menu_book),
                label: const Text('XEM LỜI GIẢI & ĐÁP ÁN CHI TIẾT', style: TextStyle(fontWeight: FontWeight.bold)),
              ),
            ),
            const SizedBox(height: 12),
            SizedBox(
              width: double.infinity,
              height: 46,
              child: OutlinedButton.icon(
                onPressed: () {
                  Navigator.popUntil(context, (route) => route.isFirst);
                },
                icon: const Icon(Icons.home),
                label: const Text('Về Trang Danh Sách Đề Thi'),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildInfoRow(String label, String value) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text(label, style: const TextStyle(color: AppTheme.textMuted, fontSize: 13)),
        Text(value, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 13, color: AppTheme.textDark)),
      ],
    );
  }

  Widget _buildMetricCard(String title, String count, String total, Color color, IconData icon) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.symmetric(vertical: 14, horizontal: 10),
        child: Column(
          children: [
            Icon(icon, color: color, size: 24),
            const SizedBox(height: 6),
            Text(title, style: TextStyle(fontSize: 11, fontWeight: FontWeight.bold, color: color)),
            const SizedBox(height: 4),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(count, style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold, color: color)),
                Text(total, style: const TextStyle(fontSize: 12, color: Colors.grey)),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPartRow(String title, int correct, int total, Color color) {
    final pct = total > 0 ? (correct / total) : 0.0;
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(title, style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 13)),
            Text('$correct / $total câu', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 13, color: color)),
          ],
        ),
        const SizedBox(height: 6),
        LinearProgressIndicator(
          value: pct,
          backgroundColor: Colors.grey.shade200,
          valueColor: AlwaysStoppedAnimation<Color>(color),
          minHeight: 8,
          borderRadius: BorderRadius.circular(4),
        ),
      ],
    );
  }
}
