using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CardManager : MonoBehaviour
{
    public List<card> cards = new List<card>{ };
    public int[] hand = new int[6] {0,0,0,0,0,0};
    public int[] cardpool = new int[] { 1, 2, 3 };
    public UIHandler uihandler;
    GameObject UICanvas;

    public enum cardType
    {
        attack = 0,
        defend = 1,
        heal = 2,
        special = 3,
    }
    public enum DamageType
    {
        physical = 0,
        elemental = 1,
        holy = 2,
        dark = 3
    }
    public class card
    {
        public string name;
        public string desc;
        public int cost;
        public cardType[] cardtype;
        public DamageType[] damagetype;
        public int[] amount;
        
        public card(string namec, string descc, int costc, cardType[] cardtypec, DamageType[] damagetypec, int[] amountc)
        {
            this.name = namec;
            this.desc = descc;
            this.cost = costc;
            this.cardtype = cardtypec;
            this.damagetype = damagetypec;
            this.amount = amountc;
        }

    }
    void Start()
    {
        cards.Add(new card("This shouldn't happen", "You have accessed the 0th index of the cards array", 1, new cardType[] { cardType.special }, new DamageType[] { DamageType.dark }, new int[] { 1000 }));
        cards.Add(new card("Shoot", "Deal 4 damange", 1, new cardType[] { cardType.attack }, new DamageType[] { DamageType.physical }, new int[] { 4 }));
        cards.Add(new card("Defend", "Defend for 5", 2, new cardType[] { cardType.defend }, new DamageType[] { DamageType.physical }, new int[] { 5 }));
        cards.Add(new card("Bandage", "Heal 7 hp to your character", 3, new cardType[] { cardType.heal }, new DamageType[] { DamageType.physical }, new int[] { 7 }));

        InitializeGame();

    }

    public void CardProq(int card)
    {
        gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].cost*-1); 
        for(int i = 0; i < cards[i].cardtype.Length; i++)
        {
            switch (cards[card].cardtype[i])
            {
                case cardType.attack:
                    gameObject.GetComponentInChildren<Enemy>().HPHandling(cards[card].amount[i]*-1);
                    Debug.Log(cards[card].amount[i] * -1);
                    break;
                case cardType.special:
                    gameObject.GetComponentInChildren<Enemy>().HPHandling(-cards[card].amount[i] * -1);
                    Debug.Log(cards[card].amount[i] * -1);
                    break;
                case cardType.defend:
                    gameObject.GetComponentInChildren<PlayerStats>().Shield += (cards[card].amount[i]);
                    gameObject.GetComponentInChildren<PlayerStats>().UpdateText();
                    break;
                case cardType.heal:
                    gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].amount[i]);
                    break;
            }
        }
    }

    public void EnemyCardProq(int card)
    {
        for (int i = 0; i < cards[i].cardtype.Length; i++)
        {
            switch (cards[card].cardtype[i])
            {
                case cardType.attack:
                    gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].amount[i]*-1);
                    Debug.Log(cards[card].amount[i] * -1);
                    break;
                case cardType.special:
                    gameObject.GetComponentInChildren<PlayerStats>().HPHandling(cards[card].amount[i] * -1);
                    Debug.Log(cards[card].amount[i] * -1);
                    break;
                case cardType.defend:
                    gameObject.GetComponentInChildren<Enemy>().Shield += (cards[card].amount[i]);
                    break;
                case cardType.heal:
                    gameObject.GetComponentInChildren<Enemy>().HPHandling(cards[card].amount[i]);
                    break;
            }
        }
    }

    public void AddCardToHand(int kortti)
    {
        for (int i = 0; i < 6; i++)
        {
            if (hand[i] == 0)
            {
                hand[i] = kortti;
                break;
            }
        }
        UpdateCardVisual();
    }

    public void RemoveCardFromHand(int index)
    {
        hand[index] = 0;
        uihandler.cardobjects[index].GetComponent<HoverAndSelection>().hover = false;
        uihandler.cardobjects[index].GetComponent<CardVisual>().selected = false;
        uihandler.cardobjects[index].GetComponent<CardVisual>().trans.anchoredPosition = new Vector2(uihandler.cardobjects[index].GetComponent<CardVisual>().trans.anchoredPosition.x, 182);
        UpdateCardVisual();
    }

    public void AddCardToHand()
    {
        for(int i = 0; i < 6; i++)
        {
            if(hand[i] == 0)
            {
                hand[i] = cardpool[Random.Range(1, cardpool.Length)];
                break;
            }
        }
        UpdateCardVisual();
    }
    void InitializeGame()
    {
        for(int i = 0; i < 3; i++)
        {
            AddCardToHand();
        }
    }
    void NewTurn()
    {
        AddCardToHand();
    }

    void UpdateCardVisual()
    {
        for(int i = 0; i < uihandler.cardobjects.Length; i++)
        {
            if(hand[i] == 0)
            {
                
                uihandler.cardobjects[i].SetActive(false);
            }
            else {
                uihandler.cardobjects[i].SetActive(true);
                uihandler.cardobjects[i].GetComponent<CardVisual>().set(cards[hand[i]].name, cards[hand[i]].desc, cards[hand[i]].cost.ToString());
            }
        }
    }
    void Update()
    {
       
    }
}
