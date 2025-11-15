using UnityEngine;

public class HoverAndSelection : MonoBehaviour
{
    [HideInInspector]
    public bool hover = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        hover = true;
        gameObject.GetComponent<CardVisual>().CardUp();
    }
    private void OnMouseExit()
    {
        hover = false;
        gameObject.GetComponent<CardVisual>().CardDown();
    }
}
