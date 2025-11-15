using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CardManager : MonoBehaviour
{
    public List<card> cards = new List<card>{ };
    public int[] hand = new int[6] {0,0,0,0,0,0};
    public int[] cardpool = new int[] { 1, 2, 3 };
    public GameObject[] cardobjects;
    GameObject UICanvas;

    public enum actionType
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
        public actionType[] actiontype;
        public DamageType[] damagetype;
        public int[] amount;
        
        public card(string namec, string descc, int costc, actionType[] actiontypec, DamageType[] damagetypec, int[] amountc)
        {
            this.name = namec;
            this.desc = descc;
            this.cost = costc;
            this.actiontype = actiontypec;
            this.damagetype = damagetypec;
            this.amount = amountc;
        }

    }
    void Start()
    {
        cards.Add(new card("This shouldn't happen", "You have accessed the 0th index of the cards array", 1, new actionType[] { actionType.special }, new DamageType[] { DamageType.dark }, new int[] { 1000 }));
        cards.Add(new card("Shoot", "Deal 4 damange", 1, new actionType[] { actionType.attack }, new DamageType[] { DamageType.physical }, new int[] { 4 }));
        cards.Add(new card("Defend", "Defend for 5", 2, new actionType[] { actionType.defend }, new DamageType[] { DamageType.physical }, new int[] { 5 }));
        cards.Add(new card("Bandage", "Heal 7 hp to your character", 3, new actionType[] { actionType.heal }, new DamageType[] { DamageType.physical }, new int[] { 7 }));

        InitializeGame();

    }

    void AddCardToHand(int kortti)
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
    void AddCardToHand()
    {
        for(int i = 0; i < 6; i++) {
        if(hand[i] == 0) {
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
        for(int i = 0; i < cardobjects.Length; i++)
        {
            if(hand[i] == 0)
            {
                
                cardobjects[i].SetActive(false);
            }
            else {
                cardobjects[i].SetActive(true);
                cardobjects[i].GetComponent<CardVisual>().set(cards[hand[i]].name, cards[hand[i]].desc, cards[hand[i]].cost.ToString());
            }
        }
    }
    void Update()
    {
       
    }
}
