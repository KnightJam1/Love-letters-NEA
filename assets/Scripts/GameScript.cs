using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        // Collect all names from the text files
        maleNames = maleNameFile.text.Split('\n');
        femaleNames = femaleNameFile.text.Split('\n');
        prefixes = prefixFile.text.Split('\n');
        surnames = surnameFile.text.Split('\n');
        suffixes = suffixFile.text.Split('\n');
        string royalLine = surnameGen();

        // Create the empty deck
        GroupedCards deck = new GroupedCards();
        Debug.Log("Created Deck");

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

        List<Player> players = new List<Player>();
        for (int i = 0; i < 4; i++){
            players.Add(new Player(i, "Player" + (i+1)));
            Debug.Log("Added new player " + players[i].getPlayerName());
        }

        GroupedCards hiddenCard = new GroupedCards();
        hiddenCard.addCard(deck.takeCard(Random.Range(0,deck.getCount())));
        Debug.Log("Hid " + hiddenCard.getCard(0).getCardRole() + " card");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
