using UnityEngine;
using Leopotam.Ecs;

public class GameLoader : MonoBehaviour {
    EcsWorld _world;
    EcsSystems _systems;

    void OnEnable() {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems.Init();
    }

    void Update() {
        _systems.Run();
    }

    void OnDisable() {
        _world.Destroy();
        _systems.Destroy();
    }
}
