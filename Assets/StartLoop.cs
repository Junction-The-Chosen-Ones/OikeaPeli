using UnityEngine;

public class StartLoop : MonoBehaviour
{
    [SerializeField]
    StateManager statemanager;

    public void nextstate()
    {
        statemanager.GameStateFunc();
    }


}
