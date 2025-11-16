using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    int MaxHP = 25;
    int CurHP = 25;
    public int Shield = 0;
    int Strength = 0;
    int Endurance = 0;
    [SerializeField]
    TMP_Text text;

    public void HPHandling(int change)
    {
        CurHP += change;
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

    void UpdateText()
    {

        text.text = "Health: " + CurHP.ToString() + "/" + MaxHP.ToString() + "   Shield: " + Shield.ToString() + "   Strength: " + Strength.ToString() + "   Endurance: " + Endurance.ToString();

    }

    private void Update()
    {

    }
    void Death()
    {
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
