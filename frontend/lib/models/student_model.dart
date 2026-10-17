class StudentSummary {
  final String candidateCode;
  final String candidateName;
  final int totalAttempts;
  final int highestScore;
  final double averageScore;
  final String? latestExamTitle;

  StudentSummary({
    required this.candidateCode,
    required this.candidateName,
    required this.totalAttempts,
    required this.highestScore,
    required this.averageScore,
    this.latestExamTitle,
  });

  factory StudentSummary.fromJson(Map<String, dynamic> json) {
    return StudentSummary(
      candidateCode: json['candidateCode'] ?? '',
      candidateName: json['candidateName'] ?? '',
      totalAttempts: json['totalAttempts'] ?? 0,
      highestScore: json['highestScore'] ?? 0,
      averageScore: (json['averageScore'] ?? 0).toDouble(),
      latestExamTitle: json['latestExamTitle'],
    );
  }
}
