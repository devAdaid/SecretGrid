using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SecretGridServerSettings", menuName = "Scriptable Objects/Server/SecretGridServerSettings")]
public class SecretGridServerSettings : ScriptableObject
{
    public string ServerAddr;
    
#if UNITY_EDITOR
    [Button("테스트 서버와 실서버 주소 토글")]
    private void DoSomething()
    {
        ServerAddr = ServerAddr.StartsWith("http://") ? File.ReadAllText(".serveraddress") : "http://localhost:24110";
    }
#endif
}
