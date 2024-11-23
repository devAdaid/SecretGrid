using System;
using System.Collections;
using UnityEngine;

public class LeaderboardAPITester : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return SecretGridServer.I.GetLeaderboardResultCoro("teststage");
        
        Debug.Log(SecretGridServer.I.CachedLeaderboardResult);
    }
}
