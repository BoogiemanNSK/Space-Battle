using UnityEngine;
namespace Modules.CoreGame
{
    
    [CreateAssetMenu(fileName = "NetConfig", menuName = "Modules/Networking/NetConfig")]
    public class NetConfig : ScriptableObject 
    {
        public string ServerAddress;
        public string ConnectEndPoint;
        public string MoveEndPoint;
        public string WorldEndPoint;
        public string OwnedEndPoint;
        public string PlayersEndPoint;
        public string BuyEndPoint;
        public string DestroyEndPoint;
        public string AttackEndPoint;
        public string TradeEndPoint;
        public string OwnedList;
    }
}