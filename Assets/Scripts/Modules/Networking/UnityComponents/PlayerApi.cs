using System.Collections;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace Modules.CoreGame
{
    public class PlayerApi : MonoBehaviour
    {
        [SerializeField] private NetConfig _config;
        [SerializeField] public string PlayerToken;

        public void Connect(EcsWorld ecsWorld, string playerName)
        {
            StartCoroutine(Login(ecsWorld, playerName));
        }

        public IEnumerator Login(EcsWorld world, string userName)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.ConnectEndPoint + 
            "?username=" + userName))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.Log("Login " + ": Error: " + webRequest.error);
                }
                else
                {
                    JSONNode data = JSON.Parse(webRequest.downloadHandler.text);
                    PlayerToken = data["data"]["token"];
                    PlayerPrefs.SetString("token", PlayerToken);
                }
            }
        }
        
    }
}