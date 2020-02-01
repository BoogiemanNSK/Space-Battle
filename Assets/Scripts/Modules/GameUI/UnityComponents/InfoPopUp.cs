using UnityEngine;
using UICoreECS;
using Leopotam.Ecs;
using TMPro;

namespace Modules.CoreGame
{   
    public class InfoPopUp :  AUIEntity
    {
        [SerializeField] private TextMeshProUGUI _text;

        public override void Init(EcsWorld world)
        {
            world.NewEntity().Set<InfoPopUpComponent>().Text = _text;
        }
    }

    public class InfoPopUpComponent : IEcsAutoReset
    {
        public TextMeshProUGUI Text;

        public void Reset()
        {
            Text = null;
        }
    }

    public class ShowInfoPopUpTag : IEcsAutoReset
    {
        public string Message;

        public void Reset()
        {
            Message = null;
        }
    }

    public class InfoPopUpProcessing : IEcsRunSystem
    {
        readonly EcsFilter<ShowInfoPopUpTag> _trigger;
        readonly EcsFilter<InfoPopUpComponent> _view;
        readonly EcsWorld _world;

        public void Run()
        {
            if(_trigger.IsEmpty())
                return;
            
            foreach (var i in _view)
            {   
                foreach (var j in _trigger)
                {
                    _view.Get1[i].Text.text = _trigger.Get1[j].Message;
                    _trigger.Entities[i].Unset<ShowInfoPopUpTag>();
                }
            }

            ShowScreenTag tag = _world.NewEntity().Set<ShowScreenTag>();
            tag.ID = 0;
            tag.Layer = 2;
        }
    }
}