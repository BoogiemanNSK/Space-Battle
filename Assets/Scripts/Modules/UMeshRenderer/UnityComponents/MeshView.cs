using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.UMeshRenderer.Components;
using Modules.ViewHub;
using Modules.ViewHub.Interfaces;
using UnityEngine;

namespace Modules.UMeshRenderer.UnityComponents
{
    public class MeshView : MonoBehaviour, IUViewElement
    {
        public string id;

        public ViewComponent[] _viewComponents;
        
        public void Allocate(EcsEntity entity, EcsWorld world)
        {
            ParseMeshData(entity.Set<UMeshData>(), entity.Set<URenderMatrix>());
            foreach (var component in _viewComponents)
            {
                component.EntityInit(entity, world, false);
            }
        }

        public string ID => id;

        public void ParseMeshData(UMeshData data, URenderMatrix uRenderMatrix)
        {
            List<Mesh> mesh = new List<Mesh>();
            List<Vector3> pos = new List<Vector3>();
            List<Quaternion> rot = new List<Quaternion>();
            List<Material> mat = new List<Material>();
            List<Vector3> scale = new List<Vector3>();
            
            GetRendererInfo(ref mesh, ref pos, ref rot, ref scale, ref mat, transform);
            data.Offset = pos.ToArray();
            data.RotationOffset = rot.ToArray();
            data.Scale = scale.ToArray();

            uRenderMatrix.Meshes = mesh.ToArray();
            uRenderMatrix.Materials = mat.ToArray();
            uRenderMatrix.Matrix = new Matrix4x4[mesh.Count];
        }

        void GetRendererInfo(ref List<Mesh> mesh, ref List<Vector3> position, ref List<Quaternion> rotation, ref List<Vector3> scale, ref  List<Material> mat, Transform target)
        {
            MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
            MeshFilter meshFilter = target.GetComponent<MeshFilter>();
            if (meshFilter != null && meshRenderer != null && target.gameObject.activeSelf)
            {
                mesh.Add(meshFilter.sharedMesh);
                mat.Add(meshRenderer.sharedMaterial);
                position.Add(target.position);
                rotation.Add(target.rotation);
                scale.Add(target.lossyScale);
            }

            for (int i = 0; i < target.childCount; i++)
            {
                GetRendererInfo(ref mesh, ref position, ref rotation, ref scale, ref mat, target.GetChild(i));
            }
        }
    }
}