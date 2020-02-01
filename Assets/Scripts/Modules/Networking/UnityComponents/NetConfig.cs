using UnityEngine;
namespace Modules.CoreGame
{
    
    [CreateAssetMenu(fileName = "NetConfig", menuName = "Modules/Networking/NetConfig")]
    public class NetConfig : ScriptableObject 
    {
        public string ServerAddress;
        public string WorldEndPoint;
    }
}