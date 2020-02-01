using UnityEngine;
using UICoreECS;
using Leopotam.Ecs;

namespace Modules.CoreGame
{
    public class ULoginBox : AUIEntity 
    {
        [SerializeField] private TMPro.TMP_InputField _InputField;
        [SerializeField] private UnityEngine.UI.Button _button;

        private EcsWorld _world;

        private void Awake() 
        {
            _button.onClick.AddListener(Login);
        }

        public override void Init(EcsWorld world)
        {
            _world = world;
        }

        public void Login()
        {
            _world.NewEntity().Set<LoginActionTag>().PlayerName = _InputField.text;
        }
    }
}