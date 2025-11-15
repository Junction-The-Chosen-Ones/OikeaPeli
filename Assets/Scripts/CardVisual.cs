using UnityEngine;
using TMPro;
using System.Collections;
public class CardVisual : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Desc;
    public TMP_Text Cost;
    public RectTransform trans;
    public Coroutine UpAnimEnum = null;
    public Coroutine DownAnimEnum = null;

    public void set(string tempname,string tempdesc,string tempcost)
    {
        Name.text = tempname;
        Desc.text = tempdesc;
        Cost.text = tempcost;
    }

    IEnumerator CardUpEnum()
    {
        print("UpEnum");
        float timer = 0;
        while(timer < 0.25)
        {
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x,-360+Mathf.Lerp(0,100,(float)timer/0.25f));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CardDownEnum()
    {
        print("DownEnum");
        float timer = 0;
        while (timer < 0.1)
        {
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, -360 + Mathf.Lerp(0, 100, (0.1f-(float)timer) / 0.1f));
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public void CardUp()
    {
        print("UpFunct");
        UpAnimEnum = StartCoroutine(CardUpEnum());
        if(DownAnimEnum != null)
        {
            StopCoroutine(DownAnimEnum);
        }

    }

    public void CardDown()
    {
        print("DownFunct");
        DownAnimEnum = StartCoroutine(CardDownEnum());
        if(UpAnimEnum != null)
        {
            StopCoroutine(UpAnimEnum);
        }

    }

}
