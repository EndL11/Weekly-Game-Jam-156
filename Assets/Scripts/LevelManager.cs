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
    public GameObject[] bowlCards;
    public bool win = false;
    public bool pause = false;
    public GameObject mainScreen;
    public GameObject pauseScreen;
    public GameObject endLevelScreen;
    public ParticleSystem allStarsEffect;
    public ParticleSystem changeBowlEffectIcons;
    public ParticleSystem changeBowlEffectSmoke;
    public GameObject starsContainer;
    public CameraMovement cm;

    [Header("Audio Clips")]
    public AudioClip changeBowlSound;
    public AudioClip endLevelSound;
    public AudioClip doneBowl;

    private Spawner spawner;
    private AudioSource audioSource;
    private NoodleType[] db;
    private int cardsDone = 0;
    private int currentCard = 0;
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
        mainScreen.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        spawner = GetComponent<Spawner>();
    }

    private void Start()
    {
        GameObject bowl = GameObject.FindGameObjectWithTag("Bowl");
        SetProgress(bowl);
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
            PasteAndPlayClip(doneBowl);
            ChangeDelayAndChance(doneBowlCard.type);
            spawner.IncreaseSpawnpointsValue();
            cardsDone++;
            cm.speed++;
        }
    }

    private void ChangeDelayAndChance(NoodleTypes.types type)
    {
        spawner.DivideDelay(1.5f);
        ItemSpawnChance item = spawner.FindNoodleByType(type);
        float sub = item.Done(1.5f);
        spawner.AddChanceToUnDoneNoodles(sub);
    }

    private void WinPreparation()
    {
        win = true;
        Time.timeScale = 0;
        CalculateProgressResult();
        endLevelScreen.SetActive(true);
        mainScreen.SetActive(false);
        starsContainer.GetComponent<Animator>().SetInteger("stars", starsPerLvl);
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

    public void ChangeBowl()
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
        changeBowlEffectIcons.Play();
        changeBowlEffectSmoke.Play();
        PasteAndPlayClip(changeBowlSound);
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
            allStarsEffect.Play();
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
    }

    public void PasteAndPlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
