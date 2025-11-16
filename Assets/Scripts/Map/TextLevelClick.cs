using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class TextLevelClick : MonoBehaviour
{
    bool colliderLevel = false;
    bool click = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (colliderLevel && !click)
            {
                click = true;
            }
        }
        else
        {
            click = false;
        }
    }
    public bool GetClick()
    {
        return click;
    }

    private void OnMouseOver()
    {
        colliderLevel = true;
    }

    private void OnMouseExit()
    {
        colliderLevel = false;
    }
}