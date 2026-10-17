import 'package:flutter_test/flutter_test.dart';
import 'package:toeic_exam_app/main.dart';

void main() {
  testWidgets('App smoke test', (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(const ToeicExamApp());
    await tester.pump();

    // Verify that the title appears
    expect(find.text('LEXORA TOEIC'), findsOneWidget);
  });
}
