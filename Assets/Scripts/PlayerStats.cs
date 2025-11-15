using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    int MaxHP = 50;
    int CurHP = 50;
    int Strength = 0;
    int Endurance = 0;


    void HPHandling(int change) {
        CurHP += change;
        if(CurHP > MaxHP)
        {
            CurHP = MaxHP;
        } else if(CurHP <= 0)
        {
            Death();
        }
    }

    void StrengthHandling(int change)
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
    }

    void EndurancehHandling(int change)
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
    }

    private void Update()
    {

    }
    void Death()
    {
        
    }

}
