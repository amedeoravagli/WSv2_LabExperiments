using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    private GameSingleton game;
    public void Awake()
    {
        game = GameSingleton.Instance;
    }

    public void StartGame(int id){
        game.Username = _username.text;
        SceneManager.LoadScene(id);
    }
    public void BackToStart()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApp(){
        Application.Quit();
    }    
}
