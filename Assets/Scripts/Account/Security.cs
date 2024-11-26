using UnityEngine;

public class Security : MonoBehaviour
{
    private void Start()
    {
        SRPClient.Account.CreateAccount("testid", "testpw");
    }
}
