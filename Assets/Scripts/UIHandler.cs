using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject DeathCanvas;

    public void Death()
    {
        Instantiate(DeathCanvas);
    }

}
