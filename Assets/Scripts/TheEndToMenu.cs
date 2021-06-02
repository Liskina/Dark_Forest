using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class TheEndToMenu : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    private void Start()
    {   
       
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene(0);
    }
}
