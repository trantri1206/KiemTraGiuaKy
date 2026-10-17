import 'dart:async';
import 'package:flutter/material.dart';
import '../services/url_helper.dart';

/// Khung giả lập điện thoại Android (Pixel ~ 412x892 dp).
/// Chỉ bật khi màn hình rộng hơn [breakpoint] (Chrome / Windows desktop).
/// Trên điện thoại thật hoặc emulator, trả về [child] nguyên vẹn.
class AndroidPhoneFrame extends StatelessWidget {
  final Widget child;
  static const double phoneWidth = 412;
  static const double phoneHeight = 892;
  static const double breakpoint = 600;

  const AndroidPhoneFrame({super.key, required this.child});

  @override
  Widget build(BuildContext context) {
    final mq = MediaQuery.of(context);
    if (mq.size.width < breakpoint) return child;

    // Thu nhỏ khung nếu cửa sổ trình duyệt thấp
    final availableH = mq.size.height - 32;
    final scale = (availableH / (phoneHeight + 24)).clamp(0.55, 1.0);

    const statusBarH = 28.0;
    const navBarH = 22.0;

    return ColoredBox(
      color: const Color(0xFF0B1220),
      child: Stack(
        children: [
          // Nền gradient
          const Positioned.fill(
            child: DecoratedBox(
              decoration: BoxDecoration(
                gradient: RadialGradient(
                  center: Alignment(-0.6, -0.8),
                  radius: 1.4,
                  colors: [Color(0xFF1E3A8A), Color(0xFF0B1220)],
                ),
              ),
            ),
          ),

          // THANH NÚT BẤM CỔNG GIẢNG VIÊN (NỔI TRÊN ĐỈNH MÀN HÌNH)
          Positioned(
            top: 14,
            left: 20,
            right: 20,
            child: Material(
              color: Colors.transparent,
              child: Container(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                decoration: BoxDecoration(
                  color: Colors.white.withValues(alpha: 0.12),
                  borderRadius: BorderRadius.circular(16),
                  border: Border.all(color: Colors.white.withValues(alpha: 0.2)),
                  boxShadow: const [
                    BoxShadow(color: Colors.black26, blurRadius: 16, offset: Offset(0, 4)),
                  ],
                ),
                child: Row(
                  children: [
                    Container(
                      padding: const EdgeInsets.all(7),
                      decoration: BoxDecoration(
                        color: const Color(0xFF2563EB),
                        borderRadius: BorderRadius.circular(8),
                      ),
                      child: const Icon(Icons.school, color: Colors.white, size: 18),
                    ),
                    const SizedBox(width: 10),
                    const Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Text(
                          'CỔNG DÀNH CHO GIẢNG VIÊN (WEB)',
                          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 13, letterSpacing: 0.5),
                        ),
                        Text(
                          'Hệ thống quản lý: http://localhost:5000',
                          style: TextStyle(color: Colors.white70, fontSize: 11),
                        ),
                      ],
                    ),
                    const Spacer(),

                    // Nút Quản lý bài thi
                    ElevatedButton.icon(
                      onPressed: () => UrlHelper.openUrl('http://localhost:5000/Exams'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFF2563EB),
                        foregroundColor: Colors.white,
                        elevation: 3,
                        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                      ),
                      icon: const Icon(Icons.description, size: 16),
                      label: const Text('Quản lý bài thi', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 13)),
                    ),
                    const SizedBox(width: 10),

                    // Nút Quản lý sinh viên
                    ElevatedButton.icon(
                      onPressed: () => UrlHelper.openUrl('http://localhost:5000/Students'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFF0F766E),
                        foregroundColor: Colors.white,
                        elevation: 3,
                        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                      ),
                      icon: const Icon(Icons.people_alt, size: 16),
                      label: const Text('Quản lý sinh viên', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 13)),
                    ),
                    const SizedBox(width: 10),

                    // Nút Mở Web trực tiếp
                    OutlinedButton.icon(
                      onPressed: () => UrlHelper.openUrl('http://localhost:5000/Exams'),
                      style: OutlinedButton.styleFrom(
                        foregroundColor: Colors.white,
                        side: const BorderSide(color: Colors.white54),
                        padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                      ),
                      icon: const Icon(Icons.open_in_new, size: 15),
                      label: const Text('Mở Web localhost:5000', style: TextStyle(fontSize: 12)),
                    ),
                  ],
                ),
              ),
            ),
          ),

          // KHUNG ĐIỆN THOẠI ANDROID
          Center(
            child: Transform.scale(
              scale: scale,
              child: Container(
                width: phoneWidth + 24,
                height: phoneHeight + 24,
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: const Color(0xFF111111),
                  borderRadius: BorderRadius.circular(52),
                  border: Border.all(color: const Color(0xFF3A3A3A), width: 2),
                  boxShadow: const [
                    BoxShadow(color: Colors.black54, blurRadius: 40, offset: Offset(0, 20)),
                  ],
                ),
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(40),
                  child: MediaQuery(
                    data: mq.copyWith(
                      size: const Size(phoneWidth, phoneHeight),
                      padding: const EdgeInsets.only(top: statusBarH, bottom: navBarH),
                      viewPadding: const EdgeInsets.only(top: statusBarH, bottom: navBarH),
                    ),
                    child: Stack(
                      children: [
                        Positioned.fill(child: child),
                        const Positioned(top: 0, left: 0, right: 0, height: statusBarH, child: _StatusBar()),
                        Positioned(
                          bottom: 6,
                          left: 0,
                          right: 0,
                          child: Center(
                            child: Container(
                              width: 120,
                              height: 5,
                              decoration: BoxDecoration(
                                color: Colors.black.withValues(alpha: 0.55),
                                borderRadius: BorderRadius.circular(3),
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ),

          // Nhãn chú thích góc dưới bên trái
          Positioned(
            left: 24,
            bottom: 20,
            child: Row(
              children: [
                const Text(
                  '📱 Khung App Sinh viên (Android Pixel) • ',
                  style: TextStyle(color: Colors.white70, fontSize: 12, decoration: TextDecoration.none, fontWeight: FontWeight.normal),
                ),
                MouseRegion(
                  cursor: SystemMouseCursors.click,
                  child: GestureDetector(
                    onTap: () => UrlHelper.openUrl('http://localhost:5000/Exams'),
                    child: const Text(
                      'Bấm vào đây để mở Web Giảng viên (http://localhost:5000) ↗',
                      style: TextStyle(color: Color(0xFF60A5FA), fontSize: 12, decoration: TextDecoration.underline, fontWeight: FontWeight.bold),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _StatusBar extends StatefulWidget {
  const _StatusBar();

  @override
  State<_StatusBar> createState() => _StatusBarState();
}

class _StatusBarState extends State<_StatusBar> {
  late Timer _timer;
  DateTime _now = DateTime.now();

  @override
  void initState() {
    super.initState();
    _timer = Timer.periodic(const Duration(seconds: 20), (_) => setState(() => _now = DateTime.now()));
  }

  @override
  void dispose() {
    _timer.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final t = '${_now.hour.toString().padLeft(2, '0')}:${_now.minute.toString().padLeft(2, '0')}';
    const style = TextStyle(color: Colors.white, fontSize: 12, fontWeight: FontWeight.w600, decoration: TextDecoration.none);
    return Container(
      color: const Color(0xFF0F172A),
      padding: const EdgeInsets.symmetric(horizontal: 22),
      child: Row(
        children: [
          Text(t, style: style),
          const Spacer(),
          // Camera đục lỗ
          Container(width: 10, height: 10, decoration: const BoxDecoration(color: Colors.black, shape: BoxShape.circle)),
          const Spacer(),
          const Icon(Icons.signal_cellular_alt, color: Colors.white, size: 14),
          const SizedBox(width: 4),
          const Icon(Icons.wifi, color: Colors.white, size: 14),
          const SizedBox(width: 4),
          const Icon(Icons.battery_full, color: Colors.white, size: 14),
        ],
      ),
    );
  }
}
