class CandidateAnswerInput {
  final int questionId;
  final int? selectedOptionId;

  CandidateAnswerInput({
    required this.questionId,
    this.selectedOptionId,
  });

  Map<String, dynamic> toJson() => {
    'questionId': questionId,
    'selectedOptionId': selectedOptionId,
  };
}

class SubmitExamInput {
  final int examId;
  final String candidateName;
  final String candidateCode;
  final DateTime startTime;
  final List<CandidateAnswerInput> answers;

  SubmitExamInput({
    required this.examId,
    required this.candidateName,
    required this.candidateCode,
    required this.startTime,
    required this.answers,
  });

  Map<String, dynamic> toJson() => {
    'examId': examId,
    'candidateName': candidateName,
    'candidateCode': candidateCode,
    'startTime': startTime.toIso8601String(),
    'answers': answers.map((a) => a.toJson()).toList(),
  };
}

class OptionResult {
  final int id;
  final String optionLabel;
  final String content;
  final bool isCorrect;

  OptionResult({
    required this.id,
    required this.optionLabel,
    required this.content,
    required this.isCorrect,
  });

  factory OptionResult.fromJson(Map<String, dynamic> json) {
    return OptionResult(
      id: json['id'] ?? 0,
      optionLabel: json['optionLabel'] ?? '',
      content: json['content'] ?? '',
      isCorrect: json['isCorrect'] ?? false,
    );
  }
}

class QuestionResult {
  final int questionId;
  final int questionNumber;
  final int partNumber;
  final String? passageTitle;
  final String? passageContent;
  final String? imageUrl;
  final String content;
  final String? explanation;
  final int? selectedOptionId;
  final String? selectedOptionLabel;
  final int correctOptionId;
  final String correctOptionLabel;
  final bool isCorrect;
  final bool isSkipped;
  final List<OptionResult> options;

  QuestionResult({
    required this.questionId,
    required this.questionNumber,
    required this.partNumber,
    this.passageTitle,
    this.passageContent,
    this.imageUrl,
    required this.content,
    this.explanation,
    this.selectedOptionId,
    this.selectedOptionLabel,
    required this.correctOptionId,
    required this.correctOptionLabel,
    required this.isCorrect,
    required this.isSkipped,
    required this.options,
  });

  factory QuestionResult.fromJson(Map<String, dynamic> json) {
    var rawOpts = json['options'] as List? ?? [];
    return QuestionResult(
      questionId: json['questionId'] ?? 0,
      questionNumber: json['questionNumber'] ?? 0,
      partNumber: json['partNumber'] ?? 5,
      passageTitle: json['passageTitle'],
      passageContent: json['passageContent'],
      imageUrl: json['imageUrl'],
      content: json['content'] ?? '',
      explanation: json['explanation'],
      selectedOptionId: json['selectedOptionId'],
      selectedOptionLabel: json['selectedOptionLabel'],
      correctOptionId: json['correctOptionId'] ?? 0,
      correctOptionLabel: json['correctOptionLabel'] ?? '',
      isCorrect: json['isCorrect'] ?? false,
      isSkipped: json['isSkipped'] ?? false,
      options: rawOpts.map((o) => OptionResult.fromJson(o)).toList(),
    );
  }
}

class ExamResult {
  final int attemptId;
  final int examId;
  final String examTitle;
  final String candidateName;
  final String candidateCode;
  final DateTime startTime;
  final DateTime submitTime;
  final String durationSpent;
  final int totalQuestions;
  final int totalCorrect;
  final int totalWrong;
  final int totalSkipped;
  final int score;
  final double accuracyPercentage;
  final int part5Correct;
  final int part6Correct;
  final int part7Correct;
  final List<QuestionResult> questionResults;

  ExamResult({
    required this.attemptId,
    required this.examId,
    required this.examTitle,
    required this.candidateName,
    required this.candidateCode,
    required this.startTime,
    required this.submitTime,
    required this.durationSpent,
    required this.totalQuestions,
    required this.totalCorrect,
    required this.totalWrong,
    required this.totalSkipped,
    required this.score,
    required this.accuracyPercentage,
    required this.part5Correct,
    required this.part6Correct,
    required this.part7Correct,
    required this.questionResults,
  });

  factory ExamResult.fromJson(Map<String, dynamic> json) {
    var rawQResults = json['questionResults'] as List? ?? [];
    return ExamResult(
      attemptId: json['attemptId'] ?? 0,
      examId: json['examId'] ?? 0,
      examTitle: json['examTitle'] ?? '',
      candidateName: json['candidateName'] ?? '',
      candidateCode: json['candidateCode'] ?? '',
      startTime: DateTime.tryParse(json['startTime'] ?? '') ?? DateTime.now(),
      submitTime: DateTime.tryParse(json['submitTime'] ?? '') ?? DateTime.now(),
      durationSpent: json['durationSpent'] ?? '00:00',
      totalQuestions: json['totalQuestions'] ?? 100,
      totalCorrect: json['totalCorrect'] ?? 0,
      totalWrong: json['totalWrong'] ?? 0,
      totalSkipped: json['totalSkipped'] ?? 0,
      score: json['score'] ?? 5,
      accuracyPercentage: (json['accuracyPercentage'] ?? 0).toDouble(),
      part5Correct: json['part5Correct'] ?? 0,
      part6Correct: json['part6Correct'] ?? 0,
      part7Correct: json['part7Correct'] ?? 0,
      questionResults: rawQResults.map((q) => QuestionResult.fromJson(q)).toList(),
    );
  }
}
