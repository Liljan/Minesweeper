using UnityEngine;
using System.Collections;
using System;

public struct Mouse
{
    public static int LEFT = 0, RIGHT = 1;
}

public class Block : MonoBehaviour
{
    private Gamemanager gm;

    private bool isMine;
    private bool isCovered = true;
    private bool isFlagged = false;

    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite flagSprite;
    public Sprite mineSprite;
    public Sprite[] numberSprite;

    private int indX, indY;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpriteMine()
    {
        spriteRenderer.sprite = mineSprite;
    }

    public void SetSpriteNumber(int adjacent)
    {
        spriteRenderer.sprite = numberSprite[adjacent];
        isCovered = false;
    }

    internal void Init(Gamemanager gm, int i, int j, bool isMine)
    {
        this.gm = gm;
        indX = i;
        indY = j;
        this.isMine = isMine;
    }

    public void SetSpriteFlag()
    {
        spriteRenderer.sprite = flagSprite;
    }

    public void SetSpriteDefault()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(Mouse.LEFT) && !isFlagged && isCovered)
        {
            if (isMine)
            {
                gm.UnCoverMines();

                Debug.Log("DEATH!");
            }
            else
            {
                SetSpriteNumber(gm.AdjacentMines(indX, indY));

                gm.FloodFillUncover(indX, indY, new bool[gm.w, gm.h]);

                if (gm.IsFinished())
                {
                    Debug.Log("Victory. Dumbass.");
                }
            }
        }
        else if (Input.GetMouseButtonDown(Mouse.RIGHT) && isCovered)
        {
            if (!isFlagged)
            {
                SetSpriteFlag();
                isFlagged = true;
            }
            else
            {
                SetSpriteDefault();
                isFlagged = false;
            }
        }
    }

    public bool GetIsMine() { return isMine; }
    public bool GetIsCovered() { return isCovered; }
}



