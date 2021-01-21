using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{

    public class Card
    {
        protected int value;
        protected string cardName;
        protected string role;
        protected string description;
        public int getValue()
        {
            return value;
        }
        public string getCardName()
        {
            return cardName;
        }
        public string getCardRole()
        {
            return role;
        }
        public string getCardDescription()
        {
            return description;
        }

        public virtual void pickUp(){}
        public virtual void play(Player accuser, Player accused) {}
        public virtual void discard(){}
    }

    public class GuardCard:Card
    {
        public GuardCard(string givenName)
        {
            value = 1;
            role = "Guard";
            cardName = givenName;
        }
        public override void play(Player accuser, Player accused){
            //Player accused = null; //replace with the accused player
            //int guessValue = 0; // replace with guessed card
            //if (guessvalue == accused.getCard().getCardValue()){
                //accused.takeOut();
            //}
        }
    }
    
    public class PriestCard:Card
    {
        public PriestCard(string givenName)
        {
            value = 2;
            role = "Priest";
            cardName = givenName;
        }
        public override void play(Player accuser, Player accused){
            //Player confessed = null; // replace with the targeted player.
            // tell the player confessed.getCard().getValue() + confessed.getCard().getRole()
        }
    }
    
    public class BaronCard:Card
    {
        public BaronCard(string givenName)
        {
            value = 3;
            role = "Baron";
            cardName = givenName;
        }
        public override void play(Player accuser, Player accused)
        {
            if (accuser.getCardFromHand().getValue() > accused.getCardFromHand().getValue())
            {
                accused.takeOut();
            }
            else if (accuser.getCardFromHand().getValue() < accused.getCardFromHand().getValue())
            {
                accuser.takeOut();
            }
            else
            {
                //tell players that they have the same card and no-one goes out
            }
        }
    }
    
    public class HandmaidCard:Card
    {
        public HandmaidCard(string givenName)
        {
            value = 4;
            role = "Handmaid";
            cardName = givenName;
        }
        public override void play(Player accuser, Player accused)
        {
            //Make player protected for 1 round
        }
    }
    
    public class PrinceCard:Card
    {
        public PrinceCard(string givenName)
        {
            value = 5;
            role = "Prince";
            cardName = givenName;
        }
        public override void play(Player accuser, Player accused)
        {
            accused.takeOut();
        }
    }
    
    public class KingCard:Card
    {
        public KingCard(string givenName)
        {
            value = 6;
            role = "King";
            cardName = givenName;
        }
        public override void play(Player accuser, Player accused)
        {
        }
    }
    
    public class CountessCard:Card
    {
        public CountessCard(string givenName)
        {
            value = 7;
            role = "Countess";
            cardName = givenName;
        }
    }
    
    public class PrincessCard:Card
    {
        public PrincessCard(string givenName)
        {
            value = 8;
            role = "Princess";
            cardName = givenName;
        }
    }

    public class GroupedCards
    {
        private List<Card> cards;
        public GroupedCards()
        {
            cards = new List<Card>();
        }
        public Card getCard(int ind)
        {
            return cards[ind];
        }
        public void addCard(Card cardToAdd)
        {
            cards.Add(cardToAdd);
        }
        public Card takeCard(int indexTaken)
        {
            Card cardTaken = cards[indexTaken];
            cards.RemoveAt(indexTaken);
            return cardTaken;
        }
        public int getCount()
        {
            return cards.Count;
        }
    }

    public class Player
    {
        private int playerNumber;
        private string playerName;
        private GroupedCards heldCards;
        private GroupedCards playedCards;

        public Player(int num, string name)
        {
            playerNumber = num;
            playerName = name;
        }
        public Card getCardFromHand()
        {
            return heldCards.getCard(0);
        }
        public string getPlayerName()
        {
            return playerName;
        }
        public void takeOut()
        {
            heldCards.getCard(0).discard();
        }
    }
}
