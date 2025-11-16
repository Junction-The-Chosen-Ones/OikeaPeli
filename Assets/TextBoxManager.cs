using UnityEngine;
using TMPro;
public class TextBoxManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text textbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textbox.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
