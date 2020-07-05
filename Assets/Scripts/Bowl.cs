using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Borders
{
    public float minX_offset = 1.1f;
    public float maxX_offset = 1.1f;
    public float minY_offset = .3f;
    public float maxY_offset = 1.1f;
    [HideInInspector]
    public float minX, maxX, minY, maxY;
}

public class Bowl : MonoBehaviour
{
    public Borders borders;
    public float speed = 5f;
    private Camera _camera;
    public ParticleSystem correctNoodle;
    public ParticleSystem wrongNoodle;

    public NoodleTypes.types type;

    public int currentProgress = 0;
    public int expectedProgress = 15;

    private void Awake()
    {
        _camera = Camera.main;
        ResizeBorders();
    }

    private void Start()
    {
        LevelManager.instance.SetProgress(this.gameObject);
    }

    public void SetProgress(Bowl temp)
    {
        currentProgress = temp.currentProgress;
        expectedProgress = temp.expectedProgress;
    }

    public void SetType(Bowl temp)
    {
        type = temp.type;
    }

    private void Update()
    {
        MoveHorizontalToMouse();

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, borders.minX, borders.maxX),
                                Mathf.Clamp(transform.position.y, borders.minY, borders.maxY));
    }

    private void ResizeBorders()
    {
        borders.minX = _camera.ViewportToWorldPoint(Vector2.zero).x + borders.minX_offset;
        borders.minY = _camera.ViewportToWorldPoint(Vector2.zero).y + borders.minY_offset;
        borders.maxX = _camera.ViewportToWorldPoint(Vector2.right).x - borders.maxX_offset;
        borders.maxY = _camera.ViewportToWorldPoint(Vector2.up).y - borders.maxY_offset;
    }

    private void MoveHorizontalToMouse()
    {
        float mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        Vector2 newPosition = transform.position;
        newPosition.x = mousePositionX;
        //transform.position = Vector2.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);
        transform.position = newPosition;
    }

    private void WrongitemInBowlPS()
    {
        LevelManager.instance.AddScore(-50);
        wrongNoodle.Play();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Noodle"))
        {
            Noodle current = collision.GetComponent<Noodle>();
            if (current.noodleType == type)
            {
                currentProgress++;
                LevelManager.instance.IncreaseCurrentCount();
                correctNoodle.Play();
                LevelManager.instance.AddScore(150);
            }
            else
            {
                WrongitemInBowlPS();
            }

        }
        else if (collision.CompareTag("NotNoodle"))
        {
            WrongitemInBowlPS();
        }
    }
}
