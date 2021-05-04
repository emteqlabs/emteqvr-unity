using UnityEngine;
using UnityEngine.SceneManagement;

namespace EmteqLabs
{
    public class DemoMenu : MonoBehaviour
    {
        public void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        public void ClickTest()
        {}
        
    }
}
