using Leopotam.Ecs;
using UnityEngine;

namespace UICoreECS
{
    public class ScreenSwitchSystem : IEcsRunSystem
    {
        // auto injected fields
        readonly EcsFilter<ShowScreenTag> _filter;
        readonly EcsFilter<UIScreen> _screens;
        readonly EcsWorld _world;

        private bool _screenFound;
        readonly ScreensCollection _screensCollection;
        readonly Transform _screensRoot;

        public ScreenSwitchSystem(ScreensCollection screensCollection, Transform screensRoot)
        {
            _screenFound = false;
            _screensCollection = screensCollection;
            _screensRoot = screensRoot;
        }

        public void Run()
        {
            if(_filter.IsEmpty())
                return;

            foreach (var i in _filter)
            {
                _screenFound = false;

                foreach (var j in _screens)
                {
                    if(_screens.Get1[j].Layer == _filter.Get1[i].Layer)
                    {
                        if (_screens.Get1[j].ID == _filter.Get1[i].ID)
                        {
                            _screens.Get1[j].Active = true;
                            _screens.Get1[j].Screen.Show();
                            _screenFound = true;                            
                        }
                        else if(_screens.Get1[j].Active)
                        {
                            _screens.Get1[j].Active = false;
                            _screens.Get1[j].Screen.Hide();
                        }
                    }
                }

                // spawn screen
                if(!_screenFound)
                {
                    ECSUIScreen screen = GameObject.Instantiate(_screensCollection.GetScreen(_filter.Get1[i].ID, _filter.Get1[i].Layer), _screensRoot)
                        .Init(_world, _filter.Get1[i].ID, _filter.Get1[i].Layer);
                    UIScreen entity = _world.NewEntity().Set<UIScreen>();
                    entity.ID = _filter.Get1[i].ID;
                    entity.Layer = _filter.Get1[i].Layer;
                    entity.Active = true;
                    entity.Screen = screen;
                    screen.Show();
                }

                _filter.Entities[i].Destroy(); // cleanup
            }
        }
    }
}