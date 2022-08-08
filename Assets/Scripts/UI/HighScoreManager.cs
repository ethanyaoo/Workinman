using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private HighScores highScores = new HighScores();

    protected void Awake()
    {
        LoadScores();
    }

    /// <summary>
    /// Save scores to disk
    /// </summary>
    private void SaveScores()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/VeggieInfestationHighScores.dat");

        bf.Serialize(file, highScores);

        file.Close();
    }

    /// <summary>
    /// Loads scores from disk
    /// </summary>
    /// 
    private void LoadScores()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/VeggieInfestationHighScores.dat"))
        {
            ClearScoreList();

            FileStream file = File.OpenRead(Application.persistentDataPath + "/VeggieInfestationHighScores.dat");

            // Deserialize data back into highscore format
            highScores = (HighScores)bf.Deserialize(file);

            file.Close();
        }
    }

    private void ClearScoreList()
    {
        highScores.scoreList.Clear();
    }

    public void AddScore(Score score, int rank)
    {
        highScores.scoreList.Insert(rank - 1, score);

        if (highScores.scoreList.Count > Settings.numOfHighScoresTotal)
        {
            highScores.scoreList.RemoveAt(Settings.numOfHighScoresTotal);
        }

        SaveScores();
    }

    public HighScores GetHighScores()
    {
        return highScores;
    }

    public int GetRank(long playerScore)
    {
        if (highScores.scoreList.Count == 0) return 1;

        int index = 1;

        for (int i = 0; i < highScores.scoreList.Count; i++)
        {
            if (playerScore >= highScores.scoreList[i].playerScore)
            {
                return index;
            }

            index++;
        }

        if (highScores.scoreList.Count < Settings.numOfHighScoresTotal)
        {
            return index;
        }

        return 0;
    }
}
