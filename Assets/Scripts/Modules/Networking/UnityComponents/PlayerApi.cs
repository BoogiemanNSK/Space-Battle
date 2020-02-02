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
                    if(data["status"].AsBool)
                    {
                        PlayerToken = data["data"]["token"];
                        PlayerName = userName;
                        PlayerPrefs.SetString("token", PlayerToken);
                        PlayerPrefs.SetString("username", userName);
                        PlayerPrefs.Save();
                        world.NewEntity().Set<LoggedInTag>();
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
                    }

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
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
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
                    Dictionary<string, Player> old = PlayerData;
                    PlayerData = new Dictionary<string, Player>();
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
                            if (!old.TryGetValue(p.Name, out Player rp))
                            {
                                world.NewEntity().Set<SpawnPlayerTag>().Player = p;
                            }
                        }
                        world.NewEntity().Set<PlayersUpdateTag>();
                    }
                }
            }
        }

        public void BuyAction(EcsWorld world)
        {
            StartCoroutine(Buy(world));
        }

        public IEnumerator Buy(EcsWorld world)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.BuyEndPoint + 
            "?username=" + PlayerName + "&token=" + PlayerToken))
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
                        world.NewEntity().Set<UIUpdate>();
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
                    }
                }
            }
        }

        public void DestroyAction(EcsWorld world)
        {
            StartCoroutine(Destroy(world));
        }

        public IEnumerator Destroy(EcsWorld world)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.DestroyEndPoint + 
            "?username=" + PlayerName + "&token=" + PlayerToken))
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
                        world.NewEntity().Set<UIUpdate>();
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
                    }
                }
            }
        }

        public void MoveAction(EcsWorld world, int point)
        {
            StartCoroutine(Move(world, point));
        }

        public IEnumerator Move(EcsWorld world, int point)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.MoveEndPoint + 
            "?username=" + PlayerName + "&token=" + PlayerToken + "&target=" + point))
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
                        world.NewEntity().Set<UIUpdate>();
                        UICoreECS.ShowScreenTag screen = world.NewEntity().Set<UICoreECS.ShowScreenTag>();
                        screen.ID = 1;
                        screen.Layer = 1;
                        UpdatePlayerData(world);
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
                    }
                }
            }
        }

        public void AttackAction(EcsWorld world, string target)
        {
            StartCoroutine(Attack(world, target));
        }


        public IEnumerator Attack(EcsWorld world, string target)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + _config.AttackEndPoint + 
            "?username=" + PlayerName + "&token=" + PlayerToken + "&target=" + target))
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
                        world.NewEntity().Set<UIUpdate>();
                        UICoreECS.ShowScreenTag screen = world.NewEntity().Set<UICoreECS.ShowScreenTag>();
                        screen.ID = 1;
                        screen.Layer = 1;
                        UpdatePlayerData(world);
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
                    }
                }
            }
        }


        public void HealAction(EcsWorld world)
        {
            StartCoroutine(Heal(world));
        }


        public IEnumerator Heal(EcsWorld world)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Get(_config.ServerAddress + "/heal" + 
            "?username=" + PlayerName + "&token=" + PlayerToken))
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
                        world.NewEntity().Set<UIUpdate>();
                        UICoreECS.ShowScreenTag screen = world.NewEntity().Set<UICoreECS.ShowScreenTag>();
                        screen.ID = 1;
                        screen.Layer = 1;
                        UpdatePlayerData(world);
                    }else
                    {
                        world.NewEntity().Set<ShowInfoPopUpTag>().Message = data["data"];
                    }
                }
            }
        }


        
    }
}