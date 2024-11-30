package main

type LeaderboardResult struct {
	MyRank   int64              `json:"myRank,omitempty"`
	MyUserId string             `json:"myUserId,omitempty"`
	Entries  []LeaderboardEntry `json:"entries,omitempty"`
}

type LeaderboardEntry struct {
	Rank     int64   `json:"rank,omitempty"`
	UserId   string  `json:"userId,omitempty"`
	Score    float64 `json:"score,omitempty"`
	Nickname string  `json:"nickname,omitempty"`
}

