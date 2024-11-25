using System;
using System.Collections;
using ConditionalDebug;
using UnityEngine;

public class LeaderboardAPITester : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return SecretGridServer.I.GetLeaderboardResultCoro("teststage");
        
        ConDebug.Log(SecretGridServer.I.CachedLeaderboardResult);
    }
}
