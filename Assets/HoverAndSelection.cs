using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAndSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        print("OnMouseOver");
        hover = true;
        gameObject.GetComponent<CardVisual>().CardUp();
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hover = false;
        gameObject.GetComponent<CardVisual>().CardDown();
    }
}
