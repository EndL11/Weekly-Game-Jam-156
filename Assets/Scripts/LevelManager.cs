using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int score;

    public Text scoreText;
    public Image noodleIcon;
    public Text noodleCount;
    public int currentCount;
    public int expectedCount;
    public GameObject progressObject;

    private NoodleType[] db;

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

    private void Start()
    {

    }


    public void IncreaseCurrentCount()
    {
        currentCount++;
        ChangeNoodleProgressText();
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"Score: {score}";
    }

    public void ChangeBowl(GameObject bowl)
    {
        ToggleProgressObject(false);

        Bowl newBowl = bowl.GetComponent<Bowl>();
        Sprite noodleSprite = null;
        for(int i = 0; i < db.Length; i++)
        {
            if(newBowl.type == db[i].type)
            {
                noodleSprite = db[i].sprite;
            }
        }
        noodleIcon.sprite = noodleSprite;
        currentCount = newBowl.currentProgress;
        expectedCount = newBowl.expectedProgress;
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

    public void RefreshProgress()
    {
        ToggleProgressObject(false);

        Bowl bowl = GameObject.FindGameObjectWithTag("Bowl").GetComponent<Bowl>();
        Sprite noodleSprite = null;
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
        ChangeNoodleProgressText();

        ToggleProgressObject(true);
    }
}
