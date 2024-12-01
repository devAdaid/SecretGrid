// https://gist.github.com/robertwahler/b3110b3077b72b4c56199668f74978a0

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

namespace Jammer
{
    /// <summary>
    /// UnityEngine.PlayerPrefs wrapper for WebGL LocalStorage
    /// </summary>
    public static class PlayerPrefs
    {
        public static void DeleteKey(string key)
        {
            Debug.Log($"Jammer.PlayerPrefs.DeleteKey(key: {key})");

#if UNITY_WEBGL && !UNITY_EDITOR
            RemoveFromLocalStorage(key: key);
#else
            UnityEngine.PlayerPrefs.DeleteKey(key: key);
#endif
        }

        public static bool HasKey(string key)
        {
            Debug.Log($"Jammer.PlayerPrefs.HasKey(key: {key})");

#if UNITY_WEBGL && !UNITY_EDITOR
            return (HasKeyInLocalStorage(key) == 1);
#else
            return UnityEngine.PlayerPrefs.HasKey(key: key);
#endif
        }

        public static string GetString(string key)
        {
            Debug.Log($"Jammer.PlayerPrefs.GetString(key: {key})");

#if UNITY_WEBGL && !UNITY_EDITOR
            return LoadFromLocalStorage(key: key);
#else
            return UnityEngine.PlayerPrefs.GetString(key: key);
#endif
        }

        public static void SetString(string key, string value)
        {
            Debug.Log($"Jammer.PlayerPrefs.SetString(key: {key}, value: {value})");

#if UNITY_WEBGL && !UNITY_EDITOR
            SaveToLocalStorage(key: key, value: value);
#else
            UnityEngine.PlayerPrefs.SetString(key: key, value: value);
#endif

        }

        public static void Save()
        {
            Debug.Log(string.Format("Jammer.PlayerPrefs.Save()"));

#if !UNITY_WEBGL
            UnityEngine.PlayerPrefs.Save();
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
      [DllImport("__Internal")]
      private static extern void SaveToLocalStorage(string key, string value);

      [DllImport("__Internal")]
      private static extern string LoadFromLocalStorage(string key);

      [DllImport("__Internal")]
      private static extern void RemoveFromLocalStorage(string key);

      [DllImport("__Internal")]
      private static extern int HasKeyInLocalStorage(string key);
#endif
    }
}