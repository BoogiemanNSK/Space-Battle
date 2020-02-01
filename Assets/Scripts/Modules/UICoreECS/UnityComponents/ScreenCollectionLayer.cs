using UnityEngine;

namespace UICoreECS
{
    [CreateAssetMenu(fileName = "ScreensLayer", menuName = "UICoreECS/ScreensLayer", order = 0)]
    public class ScreenCollectionLayer : ScriptableObject
    {
        [SerializeField] public int LayerID;
        [SerializeField] public ScreenIDEntry[] Screens;

        // todo optimise
        public ECSUIScreen GetScreen(int id)
        {
            for (int i = 0; i < Screens.Length; i++)
            {
                if(Screens[i].ID == id)
                {
                    return Screens[i].Screen;
                }
            }

            return null;
        }
    }
}