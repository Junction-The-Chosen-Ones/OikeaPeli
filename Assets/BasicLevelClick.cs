using UnityEngine;

public class BasicLevelClick : MonoBehaviour
{
    bool colliderLevel = false;
    bool click = false;
    int counter = 0;
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
                counter++;
                print("yipee"+counter.ToString());
            }
        }
        else
        {
            click = false;
        }
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
