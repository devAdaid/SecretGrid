using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SecretGridServerSettings", menuName = "Scriptable Objects/Server/SecretGridServerSettings")]
public class SecretGridServerSettings : ScriptableObject
{
    [SerializeField]
    private string serverAddr;
    
    public string ServerAddr => serverAddr.Trim();
    
#if UNITY_EDITOR
    [Button("테스트 서버와 실서버 주소 토글")]
    private void DoSomething()
    {
        serverAddr = (serverAddr?.StartsWith("http://") ?? false) ? File.ReadAllText(".serveraddress") : "http://localhost:24110";
    }
#endif
}
