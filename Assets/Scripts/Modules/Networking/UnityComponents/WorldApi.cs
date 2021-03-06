using System.Collections;
using System.Collections.Generic;
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
        private Dictionary<int, Vector3> IDtoPoint;
        public Dictionary<int, string> OwnedData;


        public void GetWorldData(EcsWorld ecsWorld)
        {
            IDtoPoint = new Dictionary<int, Vector3>();
            StartCoroutine(GetWorldData(_config.ServerAddress + _config.WorldEndPoint, ecsWorld));
        }

        public void UpdateOwned(EcsWorld ecsWorld)
        {
            StartCoroutine(GetOwnedData(_config.ServerAddress + _config.OwnedList, ecsWorld));
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

        IEnumerator GetOwnedData(string uri, EcsWorld ecsWorld)
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
                    OwnedData = new Dictionary<int, string>();
                    foreach (var point in world["data"])
                    {
                        OwnedData.Add(int.Parse(point.Key), point.Value);
                    }
                    ecsWorld.NewEntity().Set<UpdateOwnersTag>();
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

                // Set Object rotation
                RotatingObject rObject = entity.Set<RotatingObject>();
                rObject.RotatingSpeedX = Random.Range(-0.2f, 0.2f);
                rObject.RotatingSpeedY = Random.Range(-0.2f, 0.2f);
                rObject.RotatingSpeedZ = Random.Range(-0.2f, 0.2f);

                ObjectOwner objOwner = entity.Set<ObjectOwner>();
                objOwner.OwnerUsername = point.Value["owned_by"];
                objOwner.IsOwned = (objOwner.OwnerUsername) == "" ? false : true;
                
                // parse position
                Positioning.Components.Position pos = entity.Set<Positioning.Components.Position>();
                pos.Point.Set(point.Value["position"]["x"].AsInt, Random.Range(-10.0f, 10.0f), point.Value["position"]["y"].AsInt);
                IDtoPoint.Add(worldPoint.PointID, pos.Point);
                
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
                        rObject.RotatingSpeedX = 0.0f;
                        rObject.RotatingSpeedZ = 0.0f;
                        break;
                }

            }

            // Generating lines
            foreach (var point in node["data"]["points"]) {
                int pID = int.Parse(point.Key);

                foreach (var neighbour in point.Value["adjacent"])
                {
                    int nKey = int.Parse(neighbour.Value);
                    if (nKey < pID) {
                        EcsEntity lineEntity = ecsWorld.NewEntity();

                        LineConnection lConnection = lineEntity.Set<LineConnection>();
                        lConnection.PositionA = IDtoPoint[pID];
                        lConnection.PositionB = IDtoPoint[nKey];

                        lineEntity.Set<AllocateView>().id = "Line";
                    }
                    
                }
            }

        }
        
        /*public void ResetWorldOwners(JSONNode node, EcsWorld ecsWorld)
        {
            var map = node["data"];
            foreach (var p in map)
            {
                foreach (var i in _ownedWorldPointFilter) {
                    if (_ownedWorldPointFilter.Get1[i].PointID == int.Parse(p.Key)) {
                        _ownedWorldPointFilter.Get2[i].OwnerUsername = p.Value;
                        _ownedWorldPointFilter.Get2[i].IsOwned = 
                            (_ownedWorldPointFilter.Get2[i].OwnerUsername == "") ? false : true;
                    }
                }
            }
        }*/

    }
}