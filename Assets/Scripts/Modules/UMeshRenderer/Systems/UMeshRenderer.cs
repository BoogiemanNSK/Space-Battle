using Leopotam.Ecs;
using Modules.UMeshRenderer.Components;
using UnityEngine;

namespace Modules.UMeshRenderer.Systems
{
    public sealed class UMeshRenderer : IEcsRunSystem
    {
        // auto injected fields
        private EcsFilter<URenderMatrix, Positioning.Components.Position> _filter;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                for (int j = 0; j < _filter.Get1[i].Meshes.Length; j++)
                {
                    Graphics.DrawMesh(_filter.Get1[i].Meshes[j],_filter.Get1[i].Matrix[j] ,_filter.Get1[i].Materials[j],0);
                }
            }
        }
    }
}