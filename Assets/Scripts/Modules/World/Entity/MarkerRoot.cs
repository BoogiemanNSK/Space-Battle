using Leopotam.Ecs;
using UnityEngine;

namespace Modules.CoreGame
{
    public class MarkerRoot : IEcsAutoReset
    {
        public GameObject FriendlyMarker, EnemyMarker;
        public void Reset()
        {
            this.FriendlyMarker = null;
            this.EnemyMarker = null;
        }
    }
}