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


        public void GetWorldData(EcsWorld ecsWorld)
        {
            IDtoPoint = new Dictionary<int, Vector3>();
            StartCoroutine(GetWorldData(_config.ServerAddress + _config.WorldEndPoint, ecsWorld));
        }

        public void CheckOwned(EcsWorld ecsWorld)
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
                    // ResetWorldOwners(world, ecsWorld);
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
                pos.Point.Set(point.Value["position"]["x"].AsInt, 0.0f, point.Value["position"]["y"].AsInt);
                IDtoPoint.Add(worldPoint.PointID, pos.Point);

                // Generating lines
                foreach (var neighbour in point.Value["adjacent"])
                {
                    Debug.Log(worldPoint.PointID);
                    int nKey = int.Parse(neighbour.Value);
                    if (nKey < worldPoint.PointID) {
                        Debug.Log(IDtoPoint.Keys.ToString());
                        Debug.Log(nKey);

                        EcsEntity lineEntity = ecsWorld.NewEntity();

                        LineConnection lConnection = lineEntity.Set<LineConnection>();
                        lConnection.PositionA = pos.Point;
                        lConnection.PositionB = IDtoPoint[nKey];

                        lineEntity.Set<AllocateView>().id = "Line";
                    }
                }
                
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