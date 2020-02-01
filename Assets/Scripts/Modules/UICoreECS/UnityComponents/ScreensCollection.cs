using UnityEngine;

namespace UICoreECS
{
    [CreateAssetMenu(fileName = "ScreensCollection", menuName = "UICoreECS/ScreensCollection", order = 0)]
    public class ScreensCollection : ScriptableObject 
    {
        [SerializeField] public ScreenCollectionLayer[] Layers;

        // todo optimise
        public ECSUIScreen GetScreen(int id, int layer)
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                if(Layers[i].LayerID == layer)
                {
                    return Layers[i].GetScreen(id);
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class ScreenIDEntry
    {
        [SerializeField] public int ID;
        [SerializeField] public ECSUIScreen Screen;
    }
}