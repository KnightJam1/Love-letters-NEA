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

        public virtual void pickUp(Player picker){}
        public virtual void play(Player accuser, Player accused) {}
        public virtual void discard(Player discarder) {}
    }

    public class GuardCard:Card
    {
        public GuardCard(string givenName)
        {
            value = 1;
            role = "Guard";
            cardName = givenName;
            description = "Accuse another player of having a certain type of card, other than a guard. If you are correct they will go out.";
        }
        public override void play(Player accuser, Player accused){
            int guessValue = 0;
            if (guessValue == accused.getCardFromHand(0).getValue()){
                Debug.Log("Your assumption was correct");
                accused.takeOut();
            }
        }
    }
    
    public class PriestCard:Card
    {
        public PriestCard(string givenName)
        {
            value = 2;
            role = "Priest";
            cardName = givenName;
            description = "See another players card.";
        }
        public override void play(Player accuser, Player accused){
            Debug.Log(accused.getCardFromHand(0).getValue() + accused.getCardFromHand(0).getCardRole());
        }
    }
    
    public class BaronCard:Card
    {
        public BaronCard(string givenName)
        {
            value = 3;
            role = "Baron";
            cardName = givenName;
            description = "Challenge another player. Whoever has the lowest value, between your remaining card and the opponent's card, goes out.";
        }
        public override void play(Player accuser, Player accused)
        {
            if (accuser.getCardFromHand(0).getValue() > accused.getCardFromHand(0).getValue())
            {
                Debug.Log("You won the challenge");
                accused.takeOut();
            }
            else if (accuser.getCardFromHand(0).getValue() < accused.getCardFromHand(0).getValue())
            {
                Debug.Log("You lost the challenge.");
                accuser.takeOut();
            }
            else
            {
                Debug.Log("Both players have the same card");
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
            description = "Protect yourself for 1 round.";
        }
        public override void play(Player accuser, Player accused)
        {
            accuser.changeProtection(true);
        }
    }
    
    public class PrinceCard:Card
    {
        public PrinceCard(string givenName)
        {
            value = 5;
            role = "Prince";
            cardName = givenName;
            description = "Force another player to discard their card.";
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
            description = "Swap your remaining card with another player's card.";
        }
        public override void play(Player accuser, Player accused)
        {
            accused.getCardFromHand(0).discard(accuser);
        }
    }
    
    public class CountessCard:Card
    {
        public CountessCard(string givenName)
        {
            value = 7;
            role = "Countess";
            cardName = givenName;
            description = "Does nothing when played.Will discard itself if paired with the king or prince.";
        }
        public override void pickUp(Player picker)
        {
            if (picker.getCardFromHand(0).getValue() == 5 || picker.getCardFromHand(0).getValue() == 6)
            {
                picker.getCardFromHand(1).discard(picker);
            }
        }
    }
    
    public class PrincessCard:Card
    {
        public PrincessCard(string givenName)
        {
            value = 8;
            role = "Princess";
            cardName = givenName;
            description = "Playing this will cause you to go out. Keep at all costs.";
        }
        public override void discard(Player discarder)
        {
            discarder.takeOut();
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
        public bool isIn = true;
        public bool isProtected = false;

        public Player(int num, string name)
        {
            playerNumber = num;
            playerName = name;
            heldCards = new GroupedCards();
            playedCards = new GroupedCards();
        }
        public Card getCardFromHand(int get)
        {
            return heldCards.getCard(get);
        }
        public void takeCard(Card taken)
        {
            heldCards.addCard(taken);
        }
        public string getPlayerName()
        {
            return playerName;
        }
        public void takeOut()
        {
            if (heldCards.getCount() > 1)
            {
                playDiscard(1);
                playDiscard(0);
            }
            else if (heldCards.getCount() == 1)
            {
                playDiscard(0);
            }
            isIn = false;
        }
        public void playDiscard(int playedCard)
        {
            if (heldCards.getCount() > 0)
            {
                playedCards.addCard(heldCards.takeCard(playedCard));
            }
        }
        public void changeProtection(bool protection)
        {
            isProtected = protection;
        }
        public bool isPlaying()
        {
            return isIn;
        }
        public int allCards()
        {
            return heldCards.getCount();
        }
    }
}
