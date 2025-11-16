using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Fight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
