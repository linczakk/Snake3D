public static class BestScoreHandler
{
    public static int BestScore = 0;

    public static bool IsScoreBetter(int newScoreToCompare, out int result)
    {
        var isBetter = default(bool);

        if(newScoreToCompare > BestScore)
        {
            BestScore = newScoreToCompare;
            isBetter = true;
        }
        result = BestScore;
        return isBetter;
    }
}
