using System.Collections;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace Modules.CoreGame
{
    public class WorldApi : MonoBehaviour
    {
        [SerializeField] private NetConfig _config;


        public void GetWorldData(EcsWorld ecsWorld)
        {
            StartCoroutine(GetWorldData(_config.ServerAddress + _config.WorldEndPoint, ecsWorld));
        }
        
        IEnumerator GetWorldData(string uri, EcsWorld ecsWorld)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    JSONNode world = JSON.Parse(webRequest.downloadHandler.text);
                    ParseWorldNodes(world, ecsWorld);
                }
            }
        }

        public void ParseWorldNodes(JSONNode node, EcsWorld ecsWorld)
        {
            foreach (var point in node["data"]["points"])
            {
                EcsEntity entity = ecsWorld.NewEntity();
                
                // parse worldpoint entry
                WorldPoint worldPoint = entity.Set<WorldPoint>();
                worldPoint.PointID = int.Parse(point.Key);
                worldPoint.PointType = (WorldPointType) point.Value["loc_type"].AsInt;
                
                // parse position
                Positioning.Components.Position pos = entity.Set<Positioning.Components.Position>();
                pos.Point.Set(point.Value["position"]["x"].AsInt,0.0f , point.Value["position"]["y"].AsInt);
                
                // parse point type
                switch (worldPoint.PointType)
                {
                    case WorldPointType.Asteroid:
                        entity.Set<AllocateView>().id = "Asteroid";
                        break;
                    case WorldPointType.Planet:
                        entity.Set<AllocateView>().id = "Planet";
                        break;
                    case WorldPointType.Station:
                        entity.Set<AllocateView>().id = "Station";
                        break;
                }

            }
        }
        
    }
}