using System.Security.Cryptography;
using SecureRemotePassword;
using UnityEngine;

public class Security : MonoBehaviour
{
    void Start()
    {
        // a user enters his name and password
        var userName = "alice";
        var password = "password123";

        var srpParams = new SrpParameters {
            Hasher = new SrpHash<SHA1>(),
        };
        var client = new SrpClient(srpParams);
        var salt = client.GenerateSalt();
        var privateKey = client.DerivePrivateKey(salt, userName, password);
        var verifier = client.DeriveVerifier(privateKey);

        Debug.Log(verifier);
    }
}
