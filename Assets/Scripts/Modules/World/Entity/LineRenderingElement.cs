using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.CoreGame
{
    public class LineRenderingElement : ViewComponent
    {
        [SerializeField] private LineRenderer _root;
        private readonly int lengthOfLineRenderer = 2;
        
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld world, bool OnScene)
        {
            var points = new Vector3[lengthOfLineRenderer];
            points[0] = ecsEntity.Get<LineConnection>().PositionA;
            points[1] = ecsEntity.Get<LineConnection>().PositionB;
            _root.SetPositions(points);
            
            ecsEntity.Set<LineRendererRoot>().LineRenderer = this._root;
            if(OnScene)
                Destroy(this);
        }
    }
}