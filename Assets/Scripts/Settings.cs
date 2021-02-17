using UnityEngine;

public static class Settings
{
    private const string SCORE_TABLE = "ScoreTable";

    public static string GetSavedScores()
    {
        string result = "";

        if (PlayerPrefs.HasKey(SCORE_TABLE))
        {
            result = PlayerPrefs.GetString(SCORE_TABLE);
        }

        return result;
    }

    public static void SaveScore(string score)
    {
        if (score != null)
        {
            string scores = score.Trim() + "\n" + GetSavedScores();
            PlayerPrefs.SetString(SCORE_TABLE, scores);

            Debug.Log(string.Format("Score [{0}] saved!", score));
        }
    }

    public static void ClearScores()
    {
        if (PlayerPrefs.HasKey(SCORE_TABLE)) 
        {
            PlayerPrefs.DeleteKey(SCORE_TABLE);
        }
        
    }
}
