using UnityEngine;

namespace Modules.CoreGame
{   
    public class LoadSceneButton : MonoBehaviour 
    {
        [SerializeField] private UnityEngine.UI.Button _button;
        [SerializeField] private int gameScene;
        private void Awake() 
        {
            _button.onClick.AddListener(Load);
        }

        private void Load()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene);
        }
    }
}