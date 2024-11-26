using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ConditionalDebug;

namespace SRPClient
{
    internal static class Account
    {
        public class UserRecord
        {
            public string Username { get; set; }
            public byte[] Salt { get; set; }
            public BigInteger Verifier { get; set; }
        }
        
        public static UserRecord CreateAccount(string userId, string password)
        {
            // 솔트 생성 (16바이트)
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // x = H(salt || password)
            byte[] xHash = Utils.SHA256Hash(salt, Encoding.UTF8.GetBytes(password));
            BigInteger x = new BigInteger(xHash, isUnsigned: true, isBigEndian: true);

            // v = g^x % N
            BigInteger v = BigInteger.ModPow(Parameters.g, x, Parameters.N);

            // 서버로 솔트와 검증자 전송 (예시로 콘솔에 출력)
            ConDebug.Log("서버로 전송할 데이터:");
            ConDebug.Log($"사용자 이름: {userId}");
            ConDebug.Log($"솔트(salt): {Utils.ToHex(salt)}");
            ConDebug.Log($"검증자(v): {v}");

            // 서버에 사용자 정보 저장 (예시로 로컬 변수에 저장)
            return new UserRecord
            {
                Username = userId,
                Salt = salt,
                Verifier = v
            };
        }
    }

    // SRP 파라미터 클래스
    public static class Parameters
    {
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
        
        public readonly static BigInteger N = new BigInteger(
            StringToByteArray(
                "EEAF0AB9ADB38DD69C33F80AFA8FC5E86072618775FF3C0B9EA2314C9C256576"+
                "D674DF7496EA81D3383B4813D692C6E0E0D5D8E2B3C18398A0AEB9F5E8E9A0F8"+
                "9D9E3E5FBCD091FEE1A5E1DAEE59D4F681B7A78BE1E81F7F1A0F5A0F48E0C6B"+
                "A6B05B2FF524A3B2E4B1FAFF8AC13834F02E3E1BE5B848D3D73F27EAB253D"), isUnsigned: true, isBigEndian: true);

        public readonly static BigInteger g = new BigInteger(2);
        
        public readonly static BigInteger k = Calculate_k();

        private static BigInteger Calculate_k()
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] N_bytes = N.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] g_bytes = g.ToByteArray(isUnsigned: true, isBigEndian: true);

            byte[] Ng_bytes = new byte[N_bytes.Length + g_bytes.Length];
            Buffer.BlockCopy(N_bytes, 0, Ng_bytes, 0, N_bytes.Length);
            Buffer.BlockCopy(g_bytes, 0, Ng_bytes, N_bytes.Length, g_bytes.Length);

            byte[] k_bytes = sha256.ComputeHash(Ng_bytes);
                
            var k = new BigInteger(k_bytes, isUnsigned: true, isBigEndian: true);

                
            ConDebug.Log($"K: {Utils.ToHex(k.ToByteArray(isUnsigned: true, isBigEndian: true))}");
                
                
            return k;
        }
        
    }

    // 유틸리티 함수 클래스
    internal static class Utils
    {
        public static BigInteger GenerateRandomBigInteger(int numBytes)
        {
            byte[] bytes = new byte[numBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }

        public static byte[] SHA256Hash(params byte[][] data)
        {
            using SHA256 sha256 = SHA256.Create();
            foreach (var d in data)
            {
                sha256.TransformBlock(d, 0, d.Length, null, 0);
            }
            sha256.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            return sha256.Hash;
        }

        public static string ToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        public static byte[] FromHex(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}