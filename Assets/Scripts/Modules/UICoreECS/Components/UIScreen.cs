using Leopotam.Ecs;

namespace UICoreECS
{
    public class UIScreen : IEcsAutoReset 
    {
        public int Layer;
        public int ID;
        public bool Active;
        public ECSUIScreen Screen;

        public void Reset()
        {
            Layer = 0;
            ID = 0;
            if(Screen != null)
                UnityEngine.GameObject.Destroy(Screen.gameObject);
            Screen = null;
        }
    }
}