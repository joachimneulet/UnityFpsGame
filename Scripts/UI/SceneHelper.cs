using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    public void LoadSettings(){
      SceneManager.LoadScene(1);
    }

    public void LoadLobby(){
      SceneManager.LoadScene(0);
    }

    public void QuitGameButton(){
      Application.Quit();
    }
}
