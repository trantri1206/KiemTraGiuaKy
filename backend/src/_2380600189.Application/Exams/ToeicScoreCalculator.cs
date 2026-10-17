using System;

namespace _2380600189.Exams;

public static class ToeicScoreCalculator
{
    private static readonly int[] ReadingScale = new int[]
    {
        // 0 -> 10
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 10,
        // 11 -> 20
        15, 20, 25, 30, 35, 40, 45, 50, 55, 60,
        // 21 -> 30
        65, 70, 75, 80, 85, 90, 95, 100, 110, 115,
        // 31 -> 40
        120, 125, 130, 135, 140, 145, 150, 160, 165, 170,
        // 41 -> 50
        175, 180, 185, 190, 195, 200, 210, 215, 220, 225,
        // 51 -> 60
        230, 235, 240, 245, 250, 255, 260, 270, 275, 280,
        // 61 -> 70
        285, 290, 295, 300, 310, 315, 320, 325, 330, 340,
        // 71 -> 80
        345, 350, 355, 360, 365, 370, 380, 385, 390, 395,
        // 81 -> 90
        400, 405, 410, 415, 420, 425, 430, 435, 440, 445,
        // 91 -> 100
        450, 455, 465, 470, 475, 480, 485, 490, 495, 495
    };

    public static int CalculateReadingScore(int correctCount, int totalQuestions = 100)
    {
        if (correctCount <= 0) return 5;

        // If exam has less than 100 questions (e.g. mini test of 30 questions), normalize to 100 scale
        if (totalQuestions != 100 && totalQuestions > 0)
        {
            correctCount = (int)Math.Round((double)correctCount / totalQuestions * 100);
        }

        if (correctCount >= 100) return 495;
        if (correctCount < ReadingScale.Length)
        {
            return ReadingScale[correctCount];
        }

        return 495;
    }
}
