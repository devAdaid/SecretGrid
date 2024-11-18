using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardEntry", menuName = "Scriptable Objects/Leaderboard/Entry")]
public class LeaderboardEntry : ScriptableObject
{
    public int Rank;
    public string Nickname;
    public int Score;
}
