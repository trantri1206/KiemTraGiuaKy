class ExamSummary {
  final int id;
  final String title;
  final String? description;
  final int durationMinutes;
  final int totalQuestions;
  final bool isPublished;
  final int totalAttempts;
  final double averageScore;

  ExamSummary({
    required this.id,
    required this.title,
    this.description,
    required this.durationMinutes,
    required this.totalQuestions,
    required this.isPublished,
    required this.totalAttempts,
    required this.averageScore,
  });

  factory ExamSummary.fromJson(Map<String, dynamic> json) {
    return ExamSummary(
      id: json['id'] ?? 0,
      title: json['title'] ?? '',
      description: json['description'],
      durationMinutes: json['durationMinutes'] ?? 75,
      totalQuestions: json['totalQuestions'] ?? 100,
      isPublished: json['isPublished'] ?? false,
      totalAttempts: json['totalAttempts'] ?? 0,
      averageScore: (json['averageScore'] ?? 0).toDouble(),
    );
  }
}

class AnswerOption {
  final int id;
  final int questionId;
  final String optionLabel;
  final String content;
  final bool isCorrect;

  AnswerOption({
    required this.id,
    required this.questionId,
    required this.optionLabel,
    required this.content,
    required this.isCorrect,
  });

  factory AnswerOption.fromJson(Map<String, dynamic> json) {
    return AnswerOption(
      id: json['id'] ?? 0,
      questionId: json['questionId'] ?? 0,
      optionLabel: json['optionLabel'] ?? '',
      content: json['content'] ?? '',
      isCorrect: json['isCorrect'] ?? false,
    );
  }
}

class QuestionItem {
  final int id;
  final int? partSectionId;
  final int questionNumber;
  final int partNumber;
  final String content;
  final String? explanation;
  final int orderIndex;
  final List<AnswerOption> options;

  QuestionItem({
    required this.id,
    this.partSectionId,
    required this.questionNumber,
    required this.partNumber,
    required this.content,
    this.explanation,
    required this.orderIndex,
    required this.options,
  });

  factory QuestionItem.fromJson(Map<String, dynamic> json) {
    var rawOpts = json['options'] as List? ?? [];
    return QuestionItem(
      id: json['id'] ?? 0,
      partSectionId: json['partSectionId'],
      questionNumber: json['questionNumber'] ?? 0,
      partNumber: json['partNumber'] ?? 5,
      content: json['content'] ?? '',
      explanation: json['explanation'],
      orderIndex: json['orderIndex'] ?? 0,
      options: rawOpts.map((o) => AnswerOption.fromJson(o)).toList(),
    );
  }
}

class PartSectionItem {
  final int id;
  final int examId;
  final int partNumber;
  final String? title;
  final String passageContent;
  final String? imageUrl;
  final int orderIndex;
  final List<QuestionItem> questions;

  PartSectionItem({
    required this.id,
    required this.examId,
    required this.partNumber,
    this.title,
    required this.passageContent,
    this.imageUrl,
    required this.orderIndex,
    required this.questions,
  });

  factory PartSectionItem.fromJson(Map<String, dynamic> json) {
    var rawQuestions = json['questions'] as List? ?? [];
    return PartSectionItem(
      id: json['id'] ?? 0,
      examId: json['examId'] ?? 0,
      partNumber: json['partNumber'] ?? 6,
      title: json['title'],
      passageContent: json['passageContent'] ?? '',
      imageUrl: json['imageUrl'],
      orderIndex: json['orderIndex'] ?? 0,
      questions: rawQuestions.map((q) => QuestionItem.fromJson(q)).toList(),
    );
  }
}

class ExamDetail {
  final int id;
  final String title;
  final String? description;
  final int durationMinutes;
  final int totalQuestions;
  final bool isPublished;
  final List<PartSectionItem> sections;
  final List<QuestionItem> standaloneQuestions;

  ExamDetail({
    required this.id,
    required this.title,
    this.description,
    required this.durationMinutes,
    required this.totalQuestions,
    required this.isPublished,
    required this.sections,
    required this.standaloneQuestions,
  });

  factory ExamDetail.fromJson(Map<String, dynamic> json) {
    var rawSections = json['sections'] as List? ?? [];
    var rawQuestions = json['questions'] as List? ?? [];

    return ExamDetail(
      id: json['id'] ?? 0,
      title: json['title'] ?? '',
      description: json['description'],
      durationMinutes: json['durationMinutes'] ?? 75,
      totalQuestions: json['totalQuestions'] ?? 100,
      isPublished: json['isPublished'] ?? false,
      sections: rawSections.map((s) => PartSectionItem.fromJson(s)).toList(),
      standaloneQuestions: rawQuestions.map((q) => QuestionItem.fromJson(q)).toList(),
    );
  }

  /// Get flattened list of all 100 questions ordered by QuestionNumber
  List<QuestionItem> getAllQuestions() {
    var all = <QuestionItem>[];
    for (var s in sections) {
      all.addAll(s.questions);
    }
    all.addAll(standaloneQuestions);
    all.sort((a, b) => a.questionNumber.compareTo(b.questionNumber));
    return all;
  }
}
