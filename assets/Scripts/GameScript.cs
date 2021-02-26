using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Manager;

public class GameScript : MonoBehaviour
{
    // Opening the text files and putting them in a TextAsset object
    public TextAsset maleNameFile;
    public TextAsset femaleNameFile;
    public TextAsset prefixFile;
    public TextAsset surnameFile;
    public TextAsset suffixFile;

    private string[] maleNames;
    private string[] femaleNames;
    private string[] prefixes;
    private string[] surnames;
    private string[] suffixes;

    GameObject hiddenCardTextBox;
    GameObject deckTextBox;

    private int currentPlayerNum = 0;
    private int turnStage = 0;
    private int chosencard = 0;
    private int cardClicked = 0;
    private bool drawnLastCard = false;

    public Button cardButtonA;
    public Button cardButtonB;
    //0-draw a card, 1-wait for player to choose a card, 2-play the card, 3-initiate next turn

    GroupedCards deck = new GroupedCards();
    List<Player> players = new List<Player>();

    // MaleNameGen returns a male forename
    private string maleNameGen()
    {
        return(maleNames[Random.Range(0,maleNames.Length)]);
    }

    // FemaleNameGen returns a female forename
    private string femaleNameGen()
    {
        return(femaleNames[Random.Range(0,femaleNames.Length)]);
    }

    // SurnameGen returns a random surname
    private string surnameGen()
    {
        int variety = Random.Range(0,9);
        switch (variety)
        {
            case 0:
                return(prefixes[Random.Range(0,prefixes.Length)] + surnames[Random.Range(0,surnames.Length)] + suffixes[Random.Range(0,suffixes.Length)]);
            case 1:
                return(prefixes[Random.Range(0,prefixes.Length)] + surnames[Random.Range(0,surnames.Length)]);
            case 2:
                return(surnames[Random.Range(0,surnames.Length)] + suffixes[Random.Range(0,suffixes.Length)]);
            default:
                return(surnames[Random.Range(0,surnames.Length)]);
        }
    }

    // Turn function that draws a card for the user and makes them play one
    /*void GameTurn(Player turnPlayer)
    {
        // Draw a card
        // Display both cards
        // Do any possible draw effects
        // Make player choose a card
        // Do any possible play effects
    }*/

    private void CBAClick()
    {
        Debug.Log("A clicky");
        cardClicked = 1;
    }
    private void CBBClick()
    {
        Debug.Log("B clicky");
        cardClicked = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assign gameobjects to existing instances
        // Collect all names from the text files
        maleNames = maleNameFile.text.Split('\n');
        femaleNames = femaleNameFile.text.Split('\n');
        prefixes = prefixFile.text.Split('\n');
        surnames = surnameFile.text.Split('\n');
        suffixes = suffixFile.text.Split('\n');
        string royalLine = surnameGen();

        // Create the deck
        Debug.Log("Creating Deck");

        // Put 5 guard cards into the deck
        for (int i = 0; i < 5; i++)
        {
            if (Random.Range(0,2) == 1)
            {
                deck.addCard(new GuardCard(maleNameGen() + " " + surnameGen()));
            }
            else
            {
                deck.addCard(new GuardCard(femaleNameGen() + " " + surnameGen()));
            }
            Debug.Log("Added Guard " + (i+1) + ": " + deck.getCard(i).getCardName() + " to deck.");
        }

        // Put 2 priest cards into the deck
        for (int i = 0; i < 2; i++)
        {
            deck.addCard(new PriestCard(maleNameGen() + " " + surnameGen()));
            Debug.Log("Added Priest " + (i+1)+ ": " + deck.getCard(5+i).getCardName() + " to deck.");
        }

        // Put 2 baron cards into the deck
        for (int i = 0; i < 2; i++)
        {
            deck.addCard(new BaronCard(maleNameGen() + " " + surnameGen()));
            Debug.Log("Added Baron " + (i+1)+ ": " + deck.getCard(7 + i).getCardName() + " to deck.");
        }

        // Put 2 handmaid cards into the deck
        for (int i = 0; i < 2; i++)
        {
            deck.addCard(new HandmaidCard(femaleNameGen() + " " + surnameGen()));
            Debug.Log("Added Handmaid " + (i+1)+ ": " + deck.getCard(9+i).getCardName() + " to deck.");
        }
        
        // Put 2 prince cards into the deck
        for (int i = 0; i < 2; i++)
        {
            deck.addCard(new PrinceCard(maleNameGen() + " " + royalLine));
            Debug.Log("Added Prince " + (i+1)+ ": " + deck.getCard(11 + i).getCardName() + " to deck.");
        }
        
        // Put king into deck
        deck.addCard(new KingCard(maleNameGen() + " " + royalLine));
        Debug.Log("Added King: " + deck.getCard(13).getCardName() + " to deck.");

        // Put Countess into deck
        deck.addCard(new CountessCard(femaleNameGen() + " " + surnameGen()));
        Debug.Log("Added Countess: " + deck.getCard(14).getCardName() + " to deck.");

        // Put Princess into deck
        deck.addCard(new PrincessCard(femaleNameGen() + " " + royalLine));
        Debug.Log("Added Princess: " + deck.getCard(15).getCardName() + " to deck.");

        // Create 4 players
        for (int i = 0; i < 4; i++){
            players.Add(new Player(i, "Player" + (i+1)));
            Debug.Log("Added new player " + players[i].getPlayerName());
        }

        // Hide one card from the deck. This card will not be used in the game
        GroupedCards hiddenCard = new GroupedCards();
        hiddenCard.addCard(deck.takeCard(Random.Range(0,deck.getCount())));
        Debug.Log("Hid " + hiddenCard.getCard(0).getCardRole() + " card");

        // Take one card out of the deck for each player
        foreach (Player play in players)
        {
            play.takeCard(deck.takeCard(Random.Range(0, deck.getCount())));
            Debug.Log("Player " + play.getPlayerName() + " took " + play.getCardFromHand(0).getCardRole() + " card ");
        }
        // Adds a listener that calls the functions within the parentheses when the buttons are pressed.
        cardButtonA.onClick.AddListener(CBAClick);
        cardButtonB.onClick.AddListener(CBBClick);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Start a round system
        // Check whether there are any cards left in the deck
        if (!drawnLastCard && players.Count > 0)
        {
            // Do a turn with (player) input
            switch (turnStage)
            {
                case 0:
                    if (players[currentPlayerNum].isPlaying())
                    {

                        // Remove handmaid protection (if present from previous turn)
                        players[currentPlayerNum].changeProtection(false);
                        // Draw a new card
                        players[currentPlayerNum].takeCard(deck.takeCard(0));
                        Debug.Log("turnstage 0 reached (drawing card), with player " + currentPlayerNum);
                        turnStage = 1;
                    }
                    else
                    {
                        turnStage = 5;
                    }
                    break;
                case 1:
                    // Display cards
                    cardButtonA.GetComponentInChildren<Text>().text = ("Play " + players[currentPlayerNum].getCardFromHand(0).getCardRole() + " " + players[currentPlayerNum].getCardFromHand(0).getCardName() + ".\nThe card's value is " + players[currentPlayerNum].getCardFromHand(0).getValue() + ".\n" + players[currentPlayerNum].getCardFromHand(0).getCardDescription());
                    cardButtonB.GetComponentInChildren<Text>().text = ("Play " + players[currentPlayerNum].getCardFromHand(1).getCardRole() + " " + players[currentPlayerNum].getCardFromHand(1).getCardName() + ".\nThe card's value is " + players[currentPlayerNum].getCardFromHand(1).getValue() + ".\n" + players[currentPlayerNum].getCardFromHand(1).getCardDescription());
                    // Wait for decision
                    if (cardClicked > 0)
                    {
                        Debug.Log("Card was chosen");
                        chosencard = cardClicked - 1;
                        turnStage = 2;
                    }
                    break;
                case 2:
                    // Branch in path
                    if (players[currentPlayerNum].getCardFromHand(chosencard).getValue() < 4 || (players[currentPlayerNum].getCardFromHand(chosencard).getValue() > 4 && players[currentPlayerNum].getCardFromHand(chosencard).getValue() < 7))/*players[currentPlayerNum].getCardFromHand(chosencard).isCardDirected(chosencard)*/
                    {
                        Debug.Log("turnstage 2 reached, moving on to 3 (directed card)");
                        turnStage = 3;
                    }
                    else
                    {
                        Debug.Log("turnstage 2 reached, moving on to 4 (undirected card)");
                        turnStage = 4;
                    }
                    break;
                case 3:
                    // Direct the card at an opponent
                    int targetplayer = (currentPlayerNum + 1) % 4;
                    // Play card
                    players[currentPlayerNum].getCardFromHand(chosencard).play(players[currentPlayerNum],players[targetplayer]);
                    players[currentPlayerNum].getCardFromHand(chosencard).discard(players[currentPlayerNum]);
                    if (players[currentPlayerNum].allCards() > 0)
                    {
                        players[currentPlayerNum].playDiscard(chosencard);
                    }
                    Debug.Log("turnstage 3 reached (play directed card)");
                    turnStage = 5;
                    break;
                case 4:
                    // Play card with no target
                    players[currentPlayerNum].getCardFromHand(chosencard).play(players[currentPlayerNum], null);
                    players[currentPlayerNum].getCardFromHand(chosencard).discard(players[currentPlayerNum]);
                    players[currentPlayerNum].playDiscard(chosencard);
                    //Debug.Log("turnstage 4 reached (play undirected card)");
                    turnStage = 5;
                    break;
                case 5:
                    // Move to next players turn
                    currentPlayerNum = (currentPlayerNum + 1) % 4;
                    Debug.Log("turnstage 5 reached, returning to 0 with a new player");
                    turnStage = 0;
                    break;
                default:
                    // Nothing should happen here
                    break;
            }
        }
        else
        {
            Debug.Log("got here 3");
            // No :
            // Compare values of players cards
            int winner = 0;
            int greatest = 0;
            for(int i = 0; i < 4; i++)
            {
                if (players[i].getCardFromHand(0).getValue()> greatest)
                {
                    greatest = players[i].getCardFromHand(0).getValue();
                    winner = i;
                }
            }
            // Decide a winner
            Debug.Log("The winner is player " + (winner + 1) + ": " + players[winner].getPlayerName());

        }
        cardClicked = 0;

    }
}
