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
        [SerializeField] public string PlayerName;

        public void Connect(EcsWorld ecsWorld, string playerName)
        {
            StartCoroutine(Login(ecsWorld, playerName));
        }

        public void ValideConnection(EcsWorld ecsWorld)
        {
            if(PlayerPrefs.HasKey("token") && PlayerPrefs.HasKey("username"))
            {
                PlayerToken = PlayerPrefs.GetString("token");
                PlayerName = PlayerPrefs.GetString("username");
                StartCoroutine(AuthCheck(ecsWorld, PlayerName, PlayerToken));
            }
        }

        public IEnumerator Login(EcsWorld world, string userName)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.ConnectEndPoint + 
            "?username=" + userName))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                Debug.Log(webRequest.downloadHandler.text);

                if (webRequest.isNetworkError)
                {
                    Debug.Log("Login " + ": Error: " + webRequest.error);
                }
                else
                {
                    JSONNode data = JSON.Parse(webRequest.downloadHandler.text);
                    PlayerToken = data["data"]["token"];
                    PlayerName = userName;
                    PlayerPrefs.SetString("token", PlayerToken);
                    PlayerPrefs.SetString("username", userName);
                    world.NewEntity().Set<LoggedInTag>();
                }
            }
        }

        public IEnumerator AuthCheck(EcsWorld world, string userName, string token)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + "/authcheck" + 
            "?username=" + userName + "&token=" + token))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                Debug.Log(webRequest.downloadHandler.text);

                if (webRequest.isNetworkError)
                {
                    Debug.Log("Login " + ": Error: " + webRequest.error);
                }
                else
                {
                    JSONNode data = JSON.Parse(webRequest.downloadHandler.text);
                    if(data["status"].AsBool)
                    {
                        world.NewEntity().Set<LoggedInTag>();
                    }
                }
            }
        }

        
        
    }
}