using Leopotam.Ecs;
using UnityEngine;

namespace Modules.CoreGame
{
    public class LineRendererRoot : IEcsAutoReset
    {
        public LineRenderer LineRenderer;
        public void Reset()
        {
            this.LineRenderer = null;
        }
    }
}