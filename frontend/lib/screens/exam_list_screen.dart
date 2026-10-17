import 'package:flutter/material.dart';
import '../models/exam_model.dart';
import '../services/api_service.dart';
import '../services/url_helper.dart';
import '../theme/app_theme.dart';
import 'exam_runner_screen.dart';

class ExamListScreen extends StatefulWidget {
  const ExamListScreen({super.key});

  @override
  State<ExamListScreen> createState() => _ExamListScreenState();
}

class _ExamListScreenState extends State<ExamListScreen> {
  late Future<List<ExamSummary>> _futureExams;
  final TextEditingController _candidateNameCtrl = TextEditingController(text: 'Sinh viên HuTech');
  final TextEditingController _candidateCodeCtrl = TextEditingController(text: '2380600189');

  @override
  void initState() {
    super.initState();
    _futureExams = ApiService.getPublishedExams();
  }

  void _refresh() {
    setState(() {
      _futureExams = ApiService.getPublishedExams();
    });
  }


  void _showSettingsDialog() {
    final urlCtrl = TextEditingController(text: ApiService.baseUrl);
    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Cấu hình API Backend', style: TextStyle(fontWeight: FontWeight.bold)),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text('Địa chỉ máy chủ Backend ASP.NET Core:', style: TextStyle(fontSize: 13, color: Colors.grey)),
            const SizedBox(height: 8),
            TextField(
              controller: urlCtrl,
              decoration: const InputDecoration(
                border: OutlineInputBorder(),
                hintText: 'http://localhost:5000/api/exam',
                isDense: true,
              ),
            ),
            const SizedBox(height: 12),
            const Text('Gợi ý kết nối:', style: TextStyle(fontSize: 12, fontWeight: FontWeight.bold)),
            const Text('• Windows Desktop / Web: http://localhost:5000/api/exam', style: TextStyle(fontSize: 11)),
            const Text('• Android Emulator: http://10.0.2.2:5000/api/exam', style: TextStyle(fontSize: 11)),
          ],
        ),
        actions: [
          TextButton(onPressed: () => Navigator.pop(ctx), child: const Text('Hủy')),
          ElevatedButton(
            onPressed: () {
              ApiService.setCustomUrl(urlCtrl.text.trim());
              Navigator.pop(ctx);
              _refresh();
            },
            child: const Text('Lưu & Tải lại'),
          ),
        ],
      ),
    );
  }

  void _startExam(ExamSummary exam) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (ctx) => Padding(
        padding: EdgeInsets.only(
          left: 20,
          right: 20,
          top: 24,
          bottom: MediaQuery.of(ctx).viewInsets.bottom + 24,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(10),
                  decoration: BoxDecoration(
                    color: AppTheme.primaryBlue.withValues(alpha: 0.12),
                    borderRadius: BorderRadius.circular(10),
                  ),
                  child: const Icon(Icons.timer_outlined, color: AppTheme.primaryBlue),
                ),
                const SizedBox(width: 14),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(exam.title, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                      Text('Thời lượng: ${exam.durationMinutes} phút • ${exam.totalQuestions} câu hỏi', style: const TextStyle(color: Colors.grey, fontSize: 13)),
                    ],
                  ),
                ),
              ],
            ),
            const Divider(height: 30),
            const Text('Thông tin thí sinh dự thi:', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14)),
            const SizedBox(height: 12),
            TextField(
              controller: _candidateNameCtrl,
              decoration: const InputDecoration(
                labelText: 'Họ và tên thí sinh',
                prefixIcon: Icon(Icons.person_outline),
                border: OutlineInputBorder(),
                isDense: true,
              ),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _candidateCodeCtrl,
              decoration: const InputDecoration(
                labelText: 'Mã số sinh viên (MSSV)',
                prefixIcon: Icon(Icons.badge_outlined),
                border: OutlineInputBorder(),
                isDense: true,
              ),
            ),
            const SizedBox(height: 16),
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: Colors.amber.withValues(alpha: 0.12),
                borderRadius: BorderRadius.circular(8),
                border: Border.all(color: Colors.amber.shade300),
              ),
              child: const Row(
                children: [
                  Icon(Icons.info_outline, color: Colors.orange, size: 20),
                  SizedBox(width: 10),
                  Expanded(
                    child: Text(
                      'Hệ thống sẽ đếm ngược 75 phút tự động. Hết giờ sẽ tự nộp bài.',
                      style: TextStyle(fontSize: 12, color: Colors.black87),
                    ),
                  ),
                ],
              ),
            ),
            const SizedBox(height: 20),
            SizedBox(
              width: double.infinity,
              height: 48,
              child: ElevatedButton.icon(
                onPressed: () {
                  Navigator.pop(ctx);
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (_) => ExamRunnerScreen(
                        examSummary: exam,
                        candidateName: _candidateNameCtrl.text.trim(),
                        candidateCode: _candidateCodeCtrl.text.trim(),
                      ),
                    ),
                  );
                },
                icon: const Icon(Icons.play_arrow),
                label: const Text('BẮT ĐẦU LÀM BÀI THI', style: TextStyle(fontWeight: FontWeight.bold)),
              ),
            ),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              padding: const EdgeInsets.all(6),
              decoration: BoxDecoration(
                color: AppTheme.emeraldGreen,
                borderRadius: BorderRadius.circular(8),
              ),
              child: const Icon(Icons.school, color: Colors.white, size: 20),
            ),
            const SizedBox(width: 10),
            const Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('LEXORA TOEIC', style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold, letterSpacing: 0.5)),
                Text('Cổng Thi Trực Tuyến Sinh Viên', style: TextStyle(fontSize: 10, color: AppTheme.emeraldGreen, fontWeight: FontWeight.w600)),
              ],
            ),
          ],
        ),
        actions: [
          // Nút chuyển nhanh sang Cổng Giảng viên
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10),
            child: ElevatedButton.icon(
              onPressed: () => UrlHelper.openUrl('http://localhost:5000/Exams'),
              style: ElevatedButton.styleFrom(
                backgroundColor: AppTheme.primaryBlue,
                foregroundColor: Colors.white,
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
                elevation: 2,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
              ),
              icon: const Icon(Icons.school, size: 15),
              label: const Text('Giảng viên', style: TextStyle(fontSize: 12, fontWeight: FontWeight.bold)),
            ),
          ),
          IconButton(
            icon: const Icon(Icons.refresh),
            tooltip: 'Tải lại danh sách',
            onPressed: _refresh,
          ),
          IconButton(
            icon: const Icon(Icons.settings),
            tooltip: 'Cài đặt kết nối Backend',
            onPressed: _showSettingsDialog,
          ),
          const SizedBox(width: 8),
        ],
      ),
      body: Column(
        children: [
          // Header Banner Thí sinh
          Container(
            padding: const EdgeInsets.fromLTRB(18, 16, 18, 18),
            decoration: const BoxDecoration(
              color: AppTheme.navyDark,
              borderRadius: BorderRadius.vertical(bottom: Radius.circular(20)),
            ),
            child: Container(
              padding: const EdgeInsets.all(14),
              decoration: BoxDecoration(
                color: Colors.white.withValues(alpha: 0.08),
                borderRadius: BorderRadius.circular(12),
                border: Border.all(color: Colors.white.withValues(alpha: 0.12)),
              ),
              child: Row(
                children: [
                  CircleAvatar(
                    backgroundColor: AppTheme.primaryBlue,
                    radius: 22,
                    child: const Text('SV', style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold)),
                  ),
                  const SizedBox(width: 12),
                  const Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Thí sinh: Sinh viên HuTech', style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 14)),
                        SizedBox(height: 2),
                        Text('MSSV: 2380600189 • Bài thi TOEIC Reading', style: TextStyle(color: Colors.white70, fontSize: 12)),
                      ],
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    decoration: BoxDecoration(
                      color: AppTheme.emeraldGreen.withValues(alpha: 0.2),
                      borderRadius: BorderRadius.circular(20),
                      border: Border.all(color: AppTheme.emeraldGreen),
                    ),
                    child: const Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Icon(Icons.check_circle, color: AppTheme.emeraldGreen, size: 12),
                        SizedBox(width: 4),
                        Text('Online', style: TextStyle(color: AppTheme.emeraldGreen, fontSize: 11, fontWeight: FontWeight.bold)),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),

          Padding(
            padding: const EdgeInsets.fromLTRB(18, 12, 18, 6),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const Text('DANH SÁCH ĐỀ THI ĐANG MỞ', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 13, color: AppTheme.textMuted, letterSpacing: 0.5)),
                TextButton.icon(
                  onPressed: _refresh,
                  icon: const Icon(Icons.sync, size: 15),
                  label: const Text('Cập nhật', style: TextStyle(fontSize: 12)),
                ),
              ],
            ),
          ),

          // Exam List
          Expanded(
            child: FutureBuilder<List<ExamSummary>>(
              future: _futureExams,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                }

                if (snapshot.hasError) {
                  return Center(
                    child: Padding(
                      padding: const EdgeInsets.all(24.0),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          const Icon(Icons.error_outline, color: Colors.red, size: 48),
                          const SizedBox(height: 12),
                          Text('Lỗi kết nối: ${snapshot.error}', textAlign: TextAlign.center),
                          const SizedBox(height: 16),
                          ElevatedButton(onPressed: _refresh, child: const Text('Thử lại')),
                        ],
                      ),
                    ),
                  );
                }

                final exams = snapshot.data ?? [];
                if (exams.isEmpty) {
                  return const Center(
                    child: Text('Hiện chưa có ca thi nào được kích hoạt từ Giảng viên.'),
                  );
                }

                return ListView.builder(
                  padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                  itemCount: exams.length,
                  itemBuilder: (ctx, i) {
                    final e = exams[i];
                    return Card(
                      margin: const EdgeInsets.only(bottom: 12),
                      child: Padding(
                        padding: const EdgeInsets.all(16),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Row(
                              children: [
                                Container(
                                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                                  decoration: BoxDecoration(
                                    color: AppTheme.primaryBlue.withValues(alpha: 0.1),
                                    borderRadius: BorderRadius.circular(6),
                                  ),
                                  child: const Text('TOEIC READING', style: TextStyle(color: AppTheme.primaryBlue, fontWeight: FontWeight.bold, fontSize: 11)),
                                ),
                                const Spacer(),
                                Container(
                                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                                  decoration: BoxDecoration(
                                    color: AppTheme.emeraldGreen.withValues(alpha: 0.12),
                                    borderRadius: BorderRadius.circular(6),
                                  ),
                                  child: const Row(
                                    children: [
                                      Icon(Icons.lock_open, size: 12, color: AppTheme.emeraldGreen),
                                      SizedBox(width: 4),
                                      Text('Đang mở thi', style: TextStyle(color: AppTheme.emeraldGreen, fontWeight: FontWeight.bold, fontSize: 11)),
                                    ],
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(height: 10),
                            Text(e.title, style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16, color: AppTheme.textDark)),
                            if (e.description != null && e.description!.isNotEmpty) ...[
                              const SizedBox(height: 6),
                              Text(e.description!, style: const TextStyle(color: AppTheme.textMuted, fontSize: 13), maxLines: 2, overflow: TextOverflow.ellipsis),
                            ],
                            const SizedBox(height: 14),
                            Row(
                              children: [
                                _buildBadge(Icons.timer_outlined, '${e.durationMinutes} phút'),
                                const SizedBox(width: 12),
                                _buildBadge(Icons.format_list_numbered, '${e.totalQuestions} câu hỏi'),
                                const SizedBox(width: 12),
                                _buildBadge(Icons.stars, 'Thang 5-495'),
                              ],
                            ),
                            const SizedBox(height: 16),
                            SizedBox(
                              width: double.infinity,
                              child: ElevatedButton.icon(
                                onPressed: () => _startExam(e),
                                icon: const Icon(Icons.edit_document, size: 18),
                                label: const Text('VÀO THI NGAY', style: TextStyle(fontWeight: FontWeight.bold)),
                              ),
                            ),
                          ],
                        ),
                      ),
                    );
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildBadge(IconData icon, String text) {
    return Row(
      children: [
        Icon(icon, size: 15, color: Colors.grey.shade600),
        const SizedBox(width: 4),
        Text(text, style: TextStyle(color: Colors.grey.shade700, fontSize: 12, fontWeight: FontWeight.w500)),
      ],
    );
  }
}
