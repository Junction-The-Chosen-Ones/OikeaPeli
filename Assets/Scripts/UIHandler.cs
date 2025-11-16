using UnityEngine;
using System.Collections.Generic;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject DeathCanvas;
    public List<int> SelectedCards = new List<int> { };

    bool clicktoggle = false;

    public GameObject[] cardobjects;

    public GameObject StartButton;

    private void Update()
    {
        if(Input.GetMouseButton(0) && !clicktoggle)
        {
            clicktoggle = true;
            for (int i = 0; i < cardobjects.Length; i++)
            {
                if (cardobjects[i].GetComponent<HoverAndSelection>().hover)
                {
                    if (i != -1 && Input.GetMouseButton(0))
                    {
                        if (!(cardobjects[i].GetComponent<CardVisual>().selected))
                        {
                            SelectedCards.Add(i);
                            cardobjects[i].GetComponent<CardVisual>().selected = true;
                        }
                        else if (cardobjects[i].GetComponent<CardVisual>().selected)
                        {
                            SelectedCards.Remove(i);
                            cardobjects[i].GetComponent<CardVisual>().selected = false;
                        }

                    }
                }
            }
        } else if (!Input.GetMouseButton(0))
        {
            clicktoggle = false;
        }

    }
    public void Death()
    {
        Instantiate(DeathCanvas);
    }

}
