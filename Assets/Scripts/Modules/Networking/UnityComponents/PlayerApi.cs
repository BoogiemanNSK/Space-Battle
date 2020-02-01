using System.Collections.Generic;
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

        public Dictionary<string, Player> PlayerData = new Dictionary<string,Player>();

        public void Connect(EcsWorld ecsWorld, string playerName)
        {
            StartCoroutine(Login(ecsWorld, playerName));
        }

        public void UpdatePlayerData(EcsWorld ecsWorld)
        {
            StartCoroutine(GetPlayersData(ecsWorld));
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

        public IEnumerator GetPlayersData(EcsWorld world)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.PlayersEndPoint))
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
                    foreach (var item in data["data"])
                    {
                        Player p;

                        if(PlayerData.TryGetValue(item.Value["username"], out p))
                        {
                            p.Location = item.Value["location"].AsInt;
                            p.HP = item.Value["hp"].AsInt;
                            p.Power = item.Value["power"].AsInt;
                        }
                        else
                        {
                            p = new Player();
                            p.Name = item.Value["username"];
                            p.Location = item.Value["location"].AsInt;
                            p.HP = item.Value["hp"].AsInt;
                            p.Power = item.Value["power"].AsInt;
                            PlayerData.Add(p.Name, p);
                            world.NewEntity().Set<SpawnPlayerTag>().Player = p;
                        }
                        world.NewEntity().Set<PlayersUpdateTag>();
                    }
                }
            }
        }


        
    }
}