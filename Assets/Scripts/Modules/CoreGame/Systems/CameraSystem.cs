using Leopotam.Ecs;
using UnityEngine;
using Modules.ViewHub;

namespace Modules.CoreGame
{
    public class CameraSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly float MovementSpeed = 0.35f;
        readonly float Deacceleration = 0.1f;

        // auto injected
        readonly EcsWorld _world;
        private Positioning.Components.Position _camPosition;

        private Vector3 FirstPoint, SecondPoint;
        private float xPos, yPos;
        private float xPosTemp, yPosTemp;
        private float xSpeed, ySpeed;
        private float xAcc, yAcc;
        
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

                    xSpeed = (SecondPoint.x - FirstPoint.x) * MovementSpeed;
                    ySpeed = (SecondPoint.y - FirstPoint.y) * MovementSpeed;

                    xAcc = -1.0f * xSpeed * Deacceleration;
                    yAcc = -1.0f * ySpeed * Deacceleration;

                    xPos = xPosTemp - xSpeed;
                    yPos = yPosTemp - ySpeed;

                    _camPosition.Point = new Vector3(xPos, _camPosition.Point.y, yPos);
                }
            }
        }
    }
}