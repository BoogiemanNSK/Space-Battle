using Leopotam.Ecs;
using UnityEngine;
using Modules.ViewHub;

namespace Modules.CoreGame
{
    public class CameraSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly float MovementSpeed = 0.35f;

        // auto injected
        readonly EcsWorld _world;
        readonly EcsFilter<FollowPlayerTag> _follow;
        readonly EcsFilter<StopFollowingTag> _stopFollowing;
        private Positioning.Components.Position _camPosition;

        private Vector3 FirstPoint, SecondPoint;
        private float xPos, yPos;
        private float xPosTemp, yPosTemp;
        
        public void Init()
        {
            EcsEntity cameraEntity = _world.NewEntity();

            CameraComponent cameraComponent = cameraEntity.Set<CameraComponent>();
            cameraComponent.MainCamera = Camera.main;

            Positioning.Components.Position position = cameraEntity.Set<Positioning.Components.Position>();
            position.Point = cameraComponent.MainCamera.transform.position;
            position.EulerRotation = cameraComponent.MainCamera.transform.eulerAngles;

            UnityView unityView = cameraEntity.Set<UnityView>();
            unityView.GameObject = cameraComponent.MainCamera.gameObject;
            unityView.Transform = cameraComponent.MainCamera.transform;

            _camPosition = position;
        }

        public void Run()
        {
            if(!_stopFollowing.IsEmpty())
            {
                xPos = _camPosition.Point.x;
                yPos = _camPosition.Point.z;
            }

            if(!_follow.IsEmpty())
                return;

#if UNITY_EDITOR || UNITY_STANDALONE
            float acceleration = 5f;
            float shift = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
            float vertical = (Input.GetKey(KeyCode.S) ? -1f : 0f) + (Input.GetKey(KeyCode.W) ? 1f : 0f);
            float horizontal = (Input.GetKey(KeyCode.A) ? -1f : 0f) + (Input.GetKey(KeyCode.D) ? 1f : 0f);
            xPos += horizontal * MovementSpeed * acceleration * shift;
            yPos += vertical * MovementSpeed * acceleration * shift;
            _camPosition.Point = new Vector3(xPos, _camPosition.Point.y, yPos);
#else
            if (Input.touchCount > 0) 
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) 
                {
                    FirstPoint = Input.GetTouch(0).position;
                    xPosTemp = xPos;
                    yPosTemp = yPos;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    SecondPoint = Input.GetTouch(0).position;

                    xPos = xPosTemp - (SecondPoint.x - FirstPoint.x) * MovementSpeed;
                    yPos = yPosTemp - (SecondPoint.y - FirstPoint.y) * MovementSpeed;

                    _camPosition.Point = new Vector3(xPos, _camPosition.Point.y, yPos);
                }
            }
#endif
        }
    }
}