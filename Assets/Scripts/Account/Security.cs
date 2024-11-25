using UnityEngine;

public class Security : MonoBehaviour
{
    private void Start()
    {
        SRPClient.SRPAccount.CreateAccount("testid", "testpw");
    }
}
