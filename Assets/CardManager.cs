using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CardManager : MonoBehaviour
{
    public card[] cards = new card[50];
    public int[] hand = new int[6] {0,0,0,0,0,0};
    public int[] cardpool = new int[] { 1, 2, 3 };
    public GameObject[] cardobjects;
    GameObject UICanvas;
    public class card
    {
        public string name;
        public string desc;
        public int cost;
        public int[] actiontype;
        public int[] damagetype;
        public int[] amount;
        
        public card(string namec, string descc, int costc, int[] actiontypec, int[] amountc, int[] damagetypec)
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
        cards[0] = new card("koko", "hihiihihih", 2, new int[] { 1 }, new int[] { 1 }, new int[] { 3 });
        cards[1] = new card("koko", "hihiihihih", 2, new int[] { 1 }, new int[] { 1 }, new int[] { 3 });
        cards[7] = new card("kokaso", "hihiihi32hih", 2, new int[] { 31 }, new int[] { 14 }, new int[] { 35 });
        cards[8] = new card("kokdasfo", "hihiihadihih", 2, new int[] { 13 }, new int[] { 11 }, new int[] { 33 });
        cards[9] = new card("koko", "hihiihihih", 2, new int[] { 1 }, new int[] { 1 }, new int[] { 3 });
        cards[2] = new card("kokaso", "hihiihi32hih", 2, new int[] { 31 }, new int[] { 14 }, new int[] { 35 });
        cards[3] = new card("kokdasfo", "hihiihadihih", 2, new int[] { 13 }, new int[] { 11 }, new int[] { 33 });
        cards[4] = new card("koko", "hihiihihih", 2, new int[] { 1 }, new int[] { 1 }, new int[] { 3 });
        cards[5] = new card("kokaso", "hihiihi32hih", 2, new int[] { 31 }, new int[] { 14 }, new int[] { 35 });
        cards[6] = new card("kokdasfo", "hihiihadihih", 2, new int[] { 13 }, new int[] { 11 }, new int[] { 33 });
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
