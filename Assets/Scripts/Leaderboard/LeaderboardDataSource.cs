using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardDataSource", menuName = "Scriptable Objects/Leaderboard/DataSource")]
public class LeaderboardDataSource : ScriptableObject
{
    [Serializable]
    public class LeaderboardSimpleEntry
    {
        public int Rank;
        public string Nickname;
        public int Score;
    }

    public LeaderboardSimpleEntry[] Leaderboard;
}
