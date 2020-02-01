using Leopotam.Ecs;
using Modules.UMeshRenderer.Components;
using UnityEngine;

namespace Modules.UMeshRenderer.Systems
{
    public sealed class MatrixPositionUpdate : IEcsRunSystem
    {
        // auto injected fields
        readonly EcsFilter<URenderMatrix, UMeshData, Positioning.Components.Position, Positioning.Components.LazyPositionUpdate> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                UMeshData offset = _filter.Get2[i];
                URenderMatrix matrix = _filter.Get1[i];
                Positioning.Components.Position pos = _filter.Get3[i];
                for (int j = 0; j < offset.Offset.Length; j++)
                {
                    matrix.Matrix[j] = Matrix4x4.TRS(pos.Point + offset.Offset[j], offset.RotationOffset[j], offset.Scale[j] );
                }
            }
        }
    }
}