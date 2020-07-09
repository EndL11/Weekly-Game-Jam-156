using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private int lvl_1_stars;
    private int lvl_2_stars;
    private int lvl_3_stars;

    [Header("Levels")]
    public GameObject level_1;
    public GameObject level_2;
    public GameObject level_3;

    public Color starColor;

    private void Awake()
    {
        lvl_1_stars = PlayerPrefs.HasKey("Level 1") ? PlayerPrefs.GetInt("Level 1") : -1;
        lvl_2_stars = PlayerPrefs.HasKey("Level 2") ? PlayerPrefs.GetInt("Level 1") : -1;
        lvl_3_stars = PlayerPrefs.HasKey("Level 3") ? PlayerPrefs.GetInt("Level 1") : -1;
    }

    private void Start()
    {      
        ShowStars(lvl_1_stars, level_1.transform.GetChild(0).gameObject);
        ShowStars(lvl_2_stars, level_2.transform.GetChild(0).gameObject);
        ShowStars(lvl_3_stars, level_3.transform.GetChild(0).gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void ShowStars(int stars, GameObject starsObject)
    {
        if (stars == 3)
        {
            starsObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = starColor;
            starsObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = starColor;
            starsObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = starColor;
        }
        else if (stars == 2)
        {
            starsObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = starColor;
            starsObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = starColor;
        }
        else if (stars == 1)
        {
            starsObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = starColor;
        }
        else if (stars == -1)
        {
            starsObject.SetActive(false);
            return;
        }
        starsObject.SetActive(true);
    }

    public void LoadLevel(int lvlNumber)
    {
        SceneManager.LoadScene(lvlNumber);
    }
}
