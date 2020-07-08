using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlCards : MonoBehaviour
{
    private Sprite bowlSprite;
    private Color bowlColor;

    public Bowl bowlCardStats;
    private Bowl currentBowl;

    private Bowl temp;
    private Bowl temp2;

    private GameObject t;
    private GameObject t2;

    private Sprite sprite;
    private Color color;

    public bool done = false;

    public int expectedCount = 0;

    public NoodleTypes.types type;

    void Awake()
    {
        bowlSprite = GetGameObjectSprite(bowlCardStats.gameObject);
        bowlColor = GetGameObjectColor(bowlCardStats.gameObject);

        sprite = bowlSprite;
        color = bowlColor;

        t = new GameObject();
        t2 = new GameObject();

        temp = t.AddComponent<Bowl>();
        temp2 = t2.AddComponent<Bowl>();

        temp2.SetProgress(bowlCardStats);
        temp2.SetType(bowlCardStats);

        temp.SetProgress(bowlCardStats);
        temp.SetType(bowlCardStats);
        

        ChangeAppereance();
    }

    public void SwapBowl()
    {
        currentBowl = GameObject.FindGameObjectWithTag("Bowl").GetComponent<Bowl>();

        temp2.SetProgress(currentBowl);
        temp2.SetType(currentBowl);

        currentBowl.SetProgress(temp);
        currentBowl.SetType(temp);

        temp.SetProgress(temp2);
        temp.SetType(temp2);

        sprite = GetGameObjectSprite(currentBowl.gameObject);
        color = GetGameObjectColor(currentBowl.gameObject);

        currentBowl.gameObject.GetComponent<SpriteRenderer>().sprite = bowlSprite;
        currentBowl.gameObject.GetComponent<SpriteRenderer>().color = bowlColor;

        bowlSprite = sprite;
        bowlColor = color;

        ChangeAppereance();

        LevelManager.instance.SetProgress();
    }

    private void ChangeAppereance()
    {
        GetComponent<Image>().sprite = sprite;
        GetComponent<Image>().color = color;
    }

    private Color GetGameObjectColor(GameObject bowl)
    {
        return bowl.GetComponent<SpriteRenderer>().color;
    }

    private Sprite GetGameObjectSprite(GameObject bowl)
    {
        return bowl.GetComponent<SpriteRenderer>().sprite;
    }

    public void Done()
    {
        done = true;
        GetComponent<Image>().color = Color.red;
        expectedCount = temp2.expectedProgress;
        type = temp2.type;
    }
}
