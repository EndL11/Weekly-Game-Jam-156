using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int score;

    public int correctNoodleValue = 150;
    public int wrongNoodleValue = -70;

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

    public bool pause = false;

    public GameObject mainScreen;
    public GameObject pauseScreen;
    public GameObject endLevelScreen;

    private float mistake = 31f;
    private int starsPerLvl = 0;

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
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !win)
        {            
            ChangeBowl();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
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
            if ((currentCard - 1) == -1)
            {
                prev = bowlCards.Length - 1;
            }
            BowlCards doneBowlCard = bowlCards[prev].GetComponent<BowlCards>();
            doneBowlCard.Done();
            cardsDone++;
        }
    }

    private void WinPreparation()
    {
        win = true;
        Time.timeScale = 0;
        Debug.Log($"You win this lvl with score: {score}");
        CalculateProgressResult();
        endLevelScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    private BowlCards GetCurrentCard()
    {
        return bowlCards[currentCard].GetComponent<BowlCards>();
    }

    private void CheckWin()
    {
        if (cardsDone == bowlCards.Length + 1)
        {
            WinPreparation();
        }
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
        CheckWin();
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

    public void Pause()
    {
        if (win)
        {
            return;
        }
        pause = !pause;
        pauseScreen.SetActive(pause);
        mainScreen.SetActive(!pause);
        Time.timeScale = pause ? 0 : 1;
    }

    public void NextLevel()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        if ((SceneManager.sceneCountInBuildSettings - 1) != activeScene)
        {
            SceneManager.LoadScene(activeScene + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void CalculateProgressResult()
    {
        int totalProgressCount = 0;
        foreach(GameObject bowlcard in bowlCards)
        {
            BowlCards bc = bowlcard.GetComponent<BowlCards>();
            totalProgressCount += bc.expectedCount;
        }
        Bowl currentBowl = GameObject.FindGameObjectWithTag("Bowl").GetComponent<Bowl>();
        totalProgressCount += currentBowl.expectedProgress;
        float maxScore = totalProgressCount * correctNoodleValue;

        float firstGradeMistake = maxScore * (mistake / 100);
        float threeStars = maxScore - firstGradeMistake;
        float twoStars = maxScore - firstGradeMistake * 2;
        float oneStars = maxScore - firstGradeMistake * 3;

        if (score <= maxScore && score >= threeStars)
        {
            starsPerLvl = 3;
        }
        else if (score >= twoStars && score <= threeStars)
        {
            starsPerLvl = 2;
        }
        else if (score >= oneStars && score <= twoStars)
        {
            starsPerLvl = 1;
        }
        else if (score <= oneStars)
        {
            starsPerLvl = 0;
        }

        Debug.Log($"You have {starsPerLvl} stars!");
    }
}
