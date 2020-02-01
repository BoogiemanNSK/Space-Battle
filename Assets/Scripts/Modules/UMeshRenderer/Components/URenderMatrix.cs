using Leopotam.Ecs;
using UnityEngine;

namespace Modules.UMeshRenderer.Components
{
    public sealed class URenderMatrix : IEcsAutoReset
    {
        public Matrix4x4[] Matrix;
        public Mesh[] Meshes;
        public Material[] Materials;

        public void Reset()
        {
            Matrix = null;
            Meshes = null;
            Materials = null;
        }
    }
}