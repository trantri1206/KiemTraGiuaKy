import 'package:flutter/material.dart';
import 'screens/exam_list_screen.dart';
import 'theme/app_theme.dart';
import 'widgets/android_phone_frame.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const ToeicExamApp());
}

class ToeicExamApp extends StatelessWidget {
  const ToeicExamApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Lexora TOEIC Mobile - Sinh viên',
      debugShowCheckedModeBanner: false,
      theme: AppTheme.lightTheme,
      // Khi chạy trên Chrome/Windows (màn hình rộng) -> hiển thị trong khung điện thoại Android.
      // Khi chạy trên máy Android/iOS thật hoặc emulator -> hiển thị toàn màn hình bình thường.
      builder: (context, child) => AndroidPhoneFrame(child: child ?? const SizedBox()),
      home: const ExamListScreen(),
    );
  }
}
