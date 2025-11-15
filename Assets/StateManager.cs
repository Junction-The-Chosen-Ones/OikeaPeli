using UnityEngine;
using System.Collections;
public class StateManager : MonoBehaviour
{
    State GameState = State.PlayerTurn;
    UIHandler uihandler;
    CardManager cardmanager;

    private void Start()
    {
        uihandler = gameObject.GetComponent<UIHandler>();
        cardmanager = gameObject.GetComponent<CardManager>();
    }
    enum State
    {
        PlayerTurn = 0,
        PlayerFight = 1,
        EnemyFight = 2,

    }

    public void GameStateFunc()
    {
        StartCoroutine(GameStateFuncEnum());
    }
    public IEnumerator GameStateFuncEnum()
    {
        float timer = 0;
        switch (GameState) 
        {
            case State.PlayerTurn:
                gameObject.GetComponent<UIHandler>().StartButton.SetActive(true);
                GameState = State.PlayerFight;
                GameStateFunc();
                break;
            case State.PlayerFight:
                timer = 0;
                gameObject.GetComponent<UIHandler>().StartButton.SetActive(false);
                for(int i = 0; i < uihandler.SelectedCards.Count; i++)
                {
                    cardmanager.CardProq(cardmanager.hand[uihandler.SelectedCards[i]]);
                    cardmanager.RemoveCardFromHand(uihandler.SelectedCards[i]);
                }
                uihandler.SelectedCards = new System.Collections.Generic.List<int> { };
                while (timer < 0.5)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
                GameState = State.EnemyFight;
                GameStateFunc();

                break;
            case State.EnemyFight:
                timer = 0;
                gameObject.GetComponent<UIHandler>().StartButton.SetActive(true);
                cardmanager.AddCardToHand();
                while (timer < 1)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
                GameState = State.PlayerTurn;

                break;
        }
        yield return null;
    }
}
