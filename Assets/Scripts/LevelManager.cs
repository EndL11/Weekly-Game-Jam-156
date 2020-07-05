using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int score;

    public Text scoreText;
    public Text noodleCount;
    public Image noodleIcon;
    public int currentCount;
    public int expectedCount;
    public GameObject progressObject;

    private NoodleType[] db;

    public GameObject[] bowlCards;
    private int cardsDone = 0;
    private int currentCard = 0;

    public bool win = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        db = GetComponent<NoodleDB>().noodles;
        ToggleProgressObject(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !win)
        {            
            ChangeBowl();
        }
    }

    public void IncreaseCurrentCount()
    {
        currentCount++;
        ChangeNoodleProgressText();

        if(currentCount == expectedCount)
        {
            SelectNextCard();
            ChangeBowl();
            int prev = currentCard - 1;
            if ((currentCard - 1) == bowlCards.Length - 1)
            {
                prev = bowlCards.Length - 2;
            }
            else if ((currentCard - 1) == -1)
            {
                prev = bowlCards.Length - 1;
            }
            BowlCards doneBowlCard = bowlCards[prev].GetComponent<BowlCards>();
            doneBowlCard.Done();
            cardsDone++;
        }

        if (cardsDone == bowlCards.Length + 1)
        {
            WinPreparation();
        }

    }

    private void WinPreparation()
    {
        win = true;
        Time.timeScale = 0;
        Debug.Log($"You win this lvl with score: {score}");
    }

    private BowlCards GetCurrentCard()
    {
        return bowlCards[currentCard].GetComponent<BowlCards>();
    }

    private void ChangeBowl()
    {
        if(cardsDone == bowlCards.Length)
        {
            return;
        }

        BowlCards bowlCardComponent = GetCurrentCard();
        while (bowlCardComponent.done)
        {
            SelectNextCard();
            bowlCardComponent = bowlCards[currentCard].GetComponent<BowlCards>();
        }
        bowlCardComponent.SwapBowl();
        if (bowlCards.Length > 1)
        {
            SelectNextCard();
        }
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"Score: {score}";
    }

    public void SetProgress(GameObject bowlObj=null)
    {
        ToggleProgressObject(false);

        Sprite noodleSprite = null;
        if (bowlObj)
        {
            Bowl bowl = bowlObj.GetComponent<Bowl>();
            for (int i = 0; i < db.Length; i++)
            {
                if (bowl.type == db[i].type)
                {
                    noodleSprite = db[i].sprite;
                }
            }
            noodleIcon.sprite = noodleSprite;
            currentCount = bowl.currentProgress;
            expectedCount = bowl.expectedProgress;
        }
        else
        {
            Bowl bowl = GameObject.FindGameObjectWithTag("Bowl").GetComponent<Bowl>();
            for (int i = 0; i < db.Length; i++)
            {
                if (bowl.type == db[i].type)
                {
                    noodleSprite = db[i].sprite;
                }
            }
            noodleIcon.sprite = noodleSprite;
            currentCount = bowl.currentProgress;
            expectedCount = bowl.expectedProgress;
        }

        ChangeNoodleProgressText();

        ToggleProgressObject(true);
    }

    private void ToggleProgressObject(bool value)
    {
        progressObject.SetActive(value);
    }

    private void ChangeNoodleProgressText()
    {
        noodleCount.text = $"{currentCount} / {expectedCount}";
    }

    private void SelectNextCard()
    {
        if (cardsDone == bowlCards.Length)
        {
            return;
        }
        currentCard = (bowlCards.Length) > (currentCard + 1) ? currentCard + 1 : 0;
    }
}
