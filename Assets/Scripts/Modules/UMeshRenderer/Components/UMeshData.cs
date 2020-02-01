using Leopotam.Ecs;
using UnityEngine;

namespace Modules.UMeshRenderer.Components
{
    public sealed class UMeshData : IEcsAutoReset
    {
        //public Mesh[] Mesh;
        public Vector3[] Offset;
        public Vector3[] Scale;
        public Quaternion[] RotationOffset;
        //public Material[] Material;


        public void Reset()
        {
            Offset = null;
            RotationOffset = null;
            Scale = null;
        }
    }
}