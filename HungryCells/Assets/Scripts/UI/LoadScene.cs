using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadGame() {
        SceneManager.LoadScene("GameScene");
    }
}
