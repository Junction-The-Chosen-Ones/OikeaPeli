using UnityEngine;
using TMPro;
using System.Collections;
public class CardVisual : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Desc;
    public TMP_Text Cost;
    public RectTransform trans;

    public void set(string tempname,string tempdesc,string tempcost)
    {
        Name.text = tempname;
        Desc.text = tempdesc;
        Cost.text = tempcost;
    }

    IEnumerator CardUpEnum()
    {
        float timer = 0;
        while(timer < 0.5)
        {
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x,-360+Mathf.Lerp(0,100,(float)timer/0.5f));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CardDownEnum()
    {
        float timer = 0;
        while (timer < 0.5)
        {
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, -360 + Mathf.Lerp(0, 100, (0.5f-(float)timer) / 0.5f));
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public void CardUp()
    {
        StartCoroutine(CardUpEnum());
    }

    public void CardDown()
    {
        StartCoroutine(CardDownEnum());
    }

}
