using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public static int MaxHP = 25;
    public static int CurHP = 25;
    public int Shield = 0;
    static int Strength = 0;
    static int Endurance = 0;
    public static string Name = "010111";
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    TMP_Text nametext;

    public void HPHandling(int change)
    {
        if (change < 0)
        {
            if ((change * -1) <= Shield)
            {
                Shield += change;

            }
            else
            {
                CurHP += change + Shield;
            }
        }
        else
        {
            CurHP += change;
        }

        if (CurHP > MaxHP)
        {
            CurHP = MaxHP;
        }
        else if (CurHP <= 0)
        {
            Death();
        }
        UpdateText();
    }

    public void StrengthHandling(int change)
    {
        Strength += change;
        if (Strength > 5)
        {
            Strength = 5;
        }
        else if (Strength < -5)
        {
            Strength = -5;
        }
        UpdateText();
    }

    public void EndurancehHandling(int change)
    {
        Endurance += change;
        if (Endurance > 5)
        {
            Endurance = 5;
        }
        else if (Endurance < -5)
        {
            Endurance = -5;
        }
        UpdateText();
    }

    public void UpdateText()
    {

        text.text = "Health: " + CurHP.ToString() + "/" + MaxHP.ToString() + "   Shield: " + Shield.ToString() + "   Strength: " + Strength.ToString() + "   Endurance: " + Endurance.ToString();
        nametext.text = Name;
    }

    private void Update()
    {

    }
    void Death()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<PlayerStats>().HPHandling(20);
        CurHP = MaxHP;
        SceneManager.LoadScene("Main");
    }

    void battlestart()
    {

    }

    private void Start()
    {
        UpdateText();
    }
}
