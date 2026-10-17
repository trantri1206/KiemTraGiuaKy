import 'dart:async';
import 'package:flutter/material.dart';
import '../models/exam_model.dart';
import '../models/exam_result_model.dart';
import '../services/api_service.dart';
import '../theme/app_theme.dart';
import 'exam_result_screen.dart';

class ExamRunnerScreen extends StatefulWidget {
  final ExamSummary examSummary;
  final String candidateName;
  final String candidateCode;

  const ExamRunnerScreen({
    super.key,
    required this.examSummary,
    required this.candidateName,
    required this.candidateCode,
  });

  @override
  State<ExamRunnerScreen> createState() => _ExamRunnerScreenState();
}

class _ExamRunnerScreenState extends State<ExamRunnerScreen> {
  bool _isLoading = true;
  String? _errorMessage;
  ExamDetail? _examDetail;
  List<QuestionItem> _allQuestions = [];
  int _currentIndex = 0;

  // Map of questionId -> selectedOptionId
  final Map<int, int> _selectedAnswers = {};

  // Timer
  late DateTime _startTime;
  int _secondsRemaining = 75 * 60;
  Timer? _countdownTimer;
  bool _isSubmitting = false;

  @override
  void initState() {
    super.initState();
    _startTime = DateTime.now();
    _loadExamData();
  }

  @override
  void dispose() {
    _countdownTimer?.cancel();
    super.dispose();
  }

  Future<void> _loadExamData() async {
    try {
      final detail = await ApiService.getExamDetail(widget.examSummary.id);
      final questions = detail.getAllQuestions();

      setState(() {
        _examDetail = detail;
        _allQuestions = questions;
        _secondsRemaining = detail.durationMinutes * 60;
        _isLoading = false;
      });

      _startTimer();
    } catch (e) {
      setState(() {
        _errorMessage = e.toString();
        _isLoading = false;
      });
    }
  }

  void _startTimer() {
    _countdownTimer?.cancel();
    _countdownTimer = Timer.periodic(const Duration(seconds: 1), (timer) {
      if (!mounted) return;
      if (_secondsRemaining > 0) {
        setState(() {
          _secondsRemaining--;
        });
      } else {
        timer.cancel();
        _autoSubmitOnTimeUp();
      }
    });
  }

  void _autoSubmitOnTimeUp() {
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(
        content: Text('HẾT GIỜ THI! Hệ thống đang tự động nộp bài...'),
        backgroundColor: Colors.red,
        duration: Duration(seconds: 3),
      ),
    );
    _submitExam(isAutoSubmit: true);
  }

  String _formatTime(int seconds) {
    final m = seconds ~/ 60;
    final s = seconds % 60;
    return '${m.toString().padLeft(2, '0')}:${s.toString().padLeft(2, '0')}';
  }

  void _selectOption(int questionId, int optionId) {
    setState(() {
      _selectedAnswers[questionId] = optionId;
    });
  }

  Future<void> _submitExam({bool isAutoSubmit = false}) async {
    if (_isSubmitting) return;

    if (!isAutoSubmit) {
      final answered = _selectedAnswers.length;
      final total = _allQuestions.length;
      final confirm = await showDialog<bool>(
        context: context,
        builder: (ctx) => AlertDialog(
          title: const Row(
            children: [
              Icon(Icons.help_outline, color: AppTheme.primaryBlue),
              SizedBox(width: 8),
              Text('Xác nhận nộp bài', style: TextStyle(fontWeight: FontWeight.bold)),
            ],
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text('Bạn đã hoàn thành: $answered / $total câu hỏi.'),
              if (answered < total) ...[
                const SizedBox(height: 8),
                Text(
                  'Còn ${total - answered} câu chưa chọn đáp án! Bạn có chắc muốn nộp ngay?',
                  style: const TextStyle(color: Colors.orange, fontWeight: FontWeight.bold),
                ),
              ],
              const SizedBox(height: 8),
              Text('Thời gian còn lại: ${_formatTime(_secondsRemaining)}'),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(ctx, false),
              child: const Text('Làm tiếp'),
            ),
            ElevatedButton(
              onPressed: () => Navigator.pop(ctx, true),
              style: ElevatedButton.styleFrom(backgroundColor: AppTheme.emeraldGreen),
              child: const Text('Đồng ý nộp bài'),
            ),
          ],
        ),
      );

      if (confirm != true) return;
    }

    setState(() {
      _isSubmitting = true;
    });
    _countdownTimer?.cancel();

    try {
      final submitAnswers = _allQuestions.map((q) {
        return CandidateAnswerInput(
          questionId: q.id,
          selectedOptionId: _selectedAnswers[q.id],
        );
      }).toList();

      final input = SubmitExamInput(
        examId: widget.examSummary.id,
        candidateName: widget.candidateName,
        candidateCode: widget.candidateCode,
        startTime: _startTime,
        answers: submitAnswers,
      );

      final result = await ApiService.submitExam(input);

      if (!mounted) return;
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(
          builder: (_) => ExamResultScreen(result: result, examSummary: widget.examSummary),
        ),
      );
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi khi nộp bài: $e'), backgroundColor: Colors.red),
      );
      setState(() {
        _isSubmitting = false;
      });
    }
  }

  void _showQuestionGridModal() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (ctx) {
        return Container(
          height: MediaQuery.of(context).size.height * 0.7,
          padding: const EdgeInsets.all(16),
          child: Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  const Text('Danh Sách 100 Câu Hỏi', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                  IconButton(onPressed: () => Navigator.pop(ctx), icon: const Icon(Icons.close)),
                ],
              ),
              const Divider(),
              Row(
                children: [
                  _buildLegend(AppTheme.primaryBlue, 'Đang xem'),
                  const SizedBox(width: 14),
                  _buildLegend(AppTheme.emeraldGreen, 'Đã chọn'),
                  const SizedBox(width: 14),
                  _buildLegend(Colors.grey.shade300, 'Chưa làm', textColor: Colors.black87),
                ],
              ),
              const SizedBox(height: 12),
              Expanded(
                child: GridView.builder(
                  gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                    crossAxisCount: 5,
                    mainAxisSpacing: 8,
                    crossAxisSpacing: 8,
                    childAspectRatio: 1.2,
                  ),
                  itemCount: _allQuestions.length,
                  itemBuilder: (context, i) {
                    final q = _allQuestions[i];
                    final isCurrent = i == _currentIndex;
                    final isAnswered = _selectedAnswers.containsKey(q.id);

                    Color bg = Colors.grey.shade100;
                    Color fg = Colors.black87;
                    Border? border = Border.all(color: Colors.grey.shade300);

                    if (isCurrent) {
                      bg = AppTheme.primaryBlue;
                      fg = Colors.white;
                      border = null;
                    } else if (isAnswered) {
                      bg = AppTheme.emeraldGreen;
                      fg = Colors.white;
                      border = null;
                    }

                    return InkWell(
                      onTap: () {
                        setState(() {
                          _currentIndex = i;
                        });
                        Navigator.pop(ctx);
                      },
                      borderRadius: BorderRadius.circular(8),
                      child: Container(
                        decoration: BoxDecoration(
                          color: bg,
                          border: border,
                          borderRadius: BorderRadius.circular(8),
                        ),
                        alignment: Alignment.center,
                        child: Text(
                          '${q.questionNumber}',
                          style: TextStyle(
                            color: fg,
                            fontWeight: FontWeight.bold,
                            fontSize: 13,
                          ),
                        ),
                      ),
                    );
                  },
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  Widget _buildLegend(Color color, String label, {Color textColor = Colors.black87}) {
    return Row(
      children: [
        Container(width: 14, height: 14, decoration: BoxDecoration(color: color, borderRadius: BorderRadius.circular(4))),
        const SizedBox(width: 4),
        Text(label, style: TextStyle(fontSize: 12, color: textColor)),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return Scaffold(
        appBar: AppBar(title: const Text('Đang tải bài thi...')),
        body: const Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              CircularProgressIndicator(),
              SizedBox(height: 16),
              Text('Đang nạp 100 câu hỏi đề thi chuẩn ETS TOEIC...'),
            ],
          ),
        ),
      );
    }

    if (_errorMessage != null || _allQuestions.isEmpty) {
      return Scaffold(
        appBar: AppBar(title: const Text('Lỗi')),
        body: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.error, color: Colors.red, size: 48),
              const SizedBox(height: 12),
              Text(_errorMessage ?? 'Không tìm thấy câu hỏi.'),
              const SizedBox(height: 16),
              ElevatedButton(onPressed: () => Navigator.pop(context), child: const Text('Quay lại')),
            ],
          ),
        ),
      );
    }

    final currentQuestion = _allQuestions[_currentIndex];
    final selectedOptionId = _selectedAnswers[currentQuestion.id];
    final answeredCount = _selectedAnswers.length;

    // Check if question belongs to a section with passage
    PartSectionItem? parentSection;
    if (currentQuestion.partSectionId != null && _examDetail != null) {
      for (var s in _examDetail!.sections) {
        if (s.id == currentQuestion.partSectionId) {
          parentSection = s;
          break;
        }
      }
    }

    final isTimeWarning = _secondsRemaining < 600; // less than 10 mins

    return Scaffold(
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: Row(
          children: [
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
              decoration: BoxDecoration(
                color: Colors.white.withValues(alpha: 0.15),
                borderRadius: BorderRadius.circular(6),
              ),
              child: Text(
                'Part ${currentQuestion.partNumber}',
                style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 13),
              ),
            ),
            const SizedBox(width: 10),
            Text(
              'Câu ${currentQuestion.questionNumber} / 200',
              style: const TextStyle(fontSize: 15, fontWeight: FontWeight.bold),
            ),
          ],
        ),
        actions: [
          // Timer Badge
          Container(
            margin: const EdgeInsets.symmetric(vertical: 10, horizontal: 8),
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 4),
            decoration: BoxDecoration(
              color: isTimeWarning ? AppTheme.dangerRed : Colors.white.withValues(alpha: 0.18),
              borderRadius: BorderRadius.circular(20),
            ),
            child: Row(
              children: [
                Icon(
                  Icons.timer,
                  size: 16,
                  color: isTimeWarning ? Colors.white : AppTheme.emeraldGreen,
                ),
                const SizedBox(width: 6),
                Text(
                  _formatTime(_secondsRemaining),
                  style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 14, color: Colors.white),
                ),
              ],
            ),
          ),
          IconButton(
            icon: const Icon(Icons.grid_view),
            tooltip: 'Danh sách 100 câu',
            onPressed: _showQuestionGridModal,
          ),
        ],
      ),
      body: _isSubmitting
          ? const Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  CircularProgressIndicator(),
                  SizedBox(height: 16),
                  Text('Đang nộp bài và chấm điểm theo thang ETS TOEIC...', style: TextStyle(fontWeight: FontWeight.bold)),
                ],
              ),
            )
          : Column(
              children: [
                // Progress Bar
                LinearProgressIndicator(
                  value: answeredCount / _allQuestions.length,
                  backgroundColor: Colors.grey.shade200,
                  valueColor: const AlwaysStoppedAnimation<Color>(AppTheme.emeraldGreen),
                  minHeight: 5,
                ),

                // Question Navigation Pills Bar
                Container(
                  color: Colors.white,
                  padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        'Đã làm: $answeredCount / ${_allQuestions.length} câu',
                        style: const TextStyle(fontSize: 12, fontWeight: FontWeight.bold, color: AppTheme.textMuted),
                      ),
                      InkWell(
                        onTap: _showQuestionGridModal,
                        child: const Row(
                          children: [
                            Text('Xem tất cả', style: TextStyle(fontSize: 12, color: AppTheme.primaryBlue, fontWeight: FontWeight.bold)),
                            Icon(Icons.keyboard_arrow_right, size: 16, color: AppTheme.primaryBlue),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),

                // Main Content
                Expanded(
                  child: SingleChildScrollView(
                    padding: const EdgeInsets.all(16),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        // Reading Passage (if Part 6 or Part 7)
                        if (parentSection != null && parentSection.passageContent.isNotEmpty) ...[
                          Container(
                            width: double.infinity,
                            margin: const EdgeInsets.only(bottom: 16),
                            padding: const EdgeInsets.all(16),
                            decoration: BoxDecoration(
                              color: const Color(0xFFF1F5F9),
                              borderRadius: BorderRadius.circular(12),
                              border: Border.all(color: const Color(0xFFCBD5E1)),
                            ),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Row(
                                  children: [
                                    const Icon(Icons.article_outlined, size: 18, color: AppTheme.primaryBlue),
                                    const SizedBox(width: 8),
                                    Text(
                                      parentSection.title ?? 'ĐOẠN VĂN ĐỌC HIỂU',
                                      style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 13, color: AppTheme.navyDark),
                                    ),
                                  ],
                                ),
                                const Divider(height: 20),
                                Text(
                                  parentSection.passageContent,
                                  style: const TextStyle(
                                    fontSize: 14,
                                    height: 1.6,
                                    color: AppTheme.textDark,
                                    fontFamily: 'Roboto',
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ],

                        // Question Card
                        Card(
                          child: Padding(
                            padding: const EdgeInsets.all(18),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Row(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Container(
                                      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                                      decoration: BoxDecoration(
                                        color: AppTheme.primaryBlue,
                                        borderRadius: BorderRadius.circular(6),
                                      ),
                                      child: Text(
                                        'Câu ${currentQuestion.questionNumber}',
                                        style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 13),
                                      ),
                                    ),
                                    const SizedBox(width: 12),
                                    Expanded(
                                      child: Text(
                                        currentQuestion.content,
                                        style: const TextStyle(
                                          fontSize: 15,
                                          fontWeight: FontWeight.w600,
                                          height: 1.45,
                                          color: AppTheme.textDark,
                                        ),
                                      ),
                                    ),
                                  ],
                                ),
                                const SizedBox(height: 20),

                                // Options A, B, C, D
                                ...currentQuestion.options.map((opt) {
                                  final isSelected = selectedOptionId == opt.id;
                                  return InkWell(
                                    onTap: () => _selectOption(currentQuestion.id, opt.id),
                                    borderRadius: BorderRadius.circular(10),
                                    child: Container(
                                      margin: const EdgeInsets.only(bottom: 10),
                                      padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
                                      decoration: BoxDecoration(
                                        color: isSelected ? AppTheme.primaryBlue.withValues(alpha: 0.08) : Colors.white,
                                        borderRadius: BorderRadius.circular(10),
                                        border: Border.all(
                                          color: isSelected ? AppTheme.primaryBlue : AppTheme.cardBorder,
                                          width: isSelected ? 2 : 1,
                                        ),
                                      ),
                                      child: Row(
                                        children: [
                                          Container(
                                            width: 32,
                                            height: 32,
                                            decoration: BoxDecoration(
                                              shape: BoxShape.circle,
                                              color: isSelected ? AppTheme.primaryBlue : Colors.grey.shade100,
                                              border: Border.all(
                                                color: isSelected ? AppTheme.primaryBlue : Colors.grey.shade400,
                                              ),
                                            ),
                                            alignment: Alignment.center,
                                            child: Text(
                                              opt.optionLabel,
                                              style: TextStyle(
                                                color: isSelected ? Colors.white : Colors.black87,
                                                fontWeight: FontWeight.bold,
                                                fontSize: 14,
                                              ),
                                            ),
                                          ),
                                          const SizedBox(width: 14),
                                          Expanded(
                                            child: Text(
                                              opt.content,
                                              style: TextStyle(
                                                fontSize: 14,
                                                fontWeight: isSelected ? FontWeight.w600 : FontWeight.normal,
                                                color: isSelected ? AppTheme.primaryBlue : AppTheme.textDark,
                                              ),
                                            ),
                                          ),
                                          if (isSelected)
                                            const Icon(Icons.check_circle, color: AppTheme.primaryBlue, size: 20),
                                        ],
                                      ),
                                    ),
                                  );
                                }),
                              ],
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),

                // Bottom Action Navigation Bar
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black.withValues(alpha: 0.05),
                        blurRadius: 10,
                        offset: const Offset(0, -3),
                      ),
                    ],
                  ),
                  child: Row(
                    children: [
                      // Previous Button
                      OutlinedButton.icon(
                        onPressed: _currentIndex > 0
                            ? () {
                                setState(() {
                                  _currentIndex--;
                                });
                              }
                            : null,
                        icon: const Icon(Icons.arrow_back, size: 16),
                        label: const Text('Câu trước'),
                        style: OutlinedButton.styleFrom(
                          padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
                        ),
                      ),
                      const SizedBox(width: 10),

                      // Next Button
                      if (_currentIndex < _allQuestions.length - 1)
                        Expanded(
                          child: ElevatedButton.icon(
                            onPressed: () {
                              setState(() {
                                _currentIndex++;
                              });
                            },
                            icon: const Icon(Icons.arrow_forward, size: 16),
                            label: const Text('Câu tiếp theo'),
                          ),
                        )
                      else
                        const Spacer(),

                      const SizedBox(width: 10),

                      // Submit Button
                      ElevatedButton.icon(
                        onPressed: () => _submitExam(isAutoSubmit: false),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: AppTheme.emeraldGreen,
                          padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 12),
                        ),
                        icon: const Icon(Icons.send, size: 16),
                        label: const Text('NỘP BÀI', style: TextStyle(fontWeight: FontWeight.bold)),
                      ),
                    ],
                  ),
                ),
              ],
            ),
    );
  }
}
