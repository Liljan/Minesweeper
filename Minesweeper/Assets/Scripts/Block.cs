using UnityEngine;
using System.Collections;


public struct Mouse
{
    public static int LEFT = 0, RIGHT = 1;
}

public static class Grid
{
    public static int w = 10, h = 13;
    public static Block[,] blocks = new Block[w, h];

    public static void UnCoverMines()
    {
        foreach (Block b in blocks)
        {
            if (b.GetIsMine())
            {
                b.SetSpriteMine();
            }
        }
    }

    public static bool MineAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
            return blocks[x, y].GetIsMine();

        return false;
    }

    public static int AdjacentMines(int x, int y)
    {
        int count = 0;

        if (MineAt(x - 1, y - 1)) ++count;
        if (MineAt(x, y - 1)) ++count;
        if (MineAt(x + 1, y - 1)) ++count;

        if (MineAt(x - 1, y)) ++count;
        if (MineAt(x + 1, y)) ++count;

        if (MineAt(x - 1, y + 1)) ++count;
        if (MineAt(x, y + 1)) ++count;
        if (MineAt(x + 1, y + 1)) ++count;

        Debug.Log(count);
        return count;
    }

    public static void FloodFillUncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
        {

            if (visited[x, y])
                return;

            int adj = AdjacentMines(x, y);
            blocks[x, y].SetSpriteNumber(adj);

            if (adj > 0)
                return;

            // set visited
            visited[x, y] = true;

            FloodFillUncover(x - 1, y, visited);
            FloodFillUncover(x + 1, y, visited);
            FloodFillUncover(x, y - 1, visited);
            FloodFillUncover(x, y + 1, visited);
        }
    }
}

public class Block : MonoBehaviour
{
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

        isMine = Random.value < 0.15f;

        indX = (int)(transform.position.x);
        indY = (int)(transform.position.y);
        Grid.blocks[indX, indY] = this;
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
        if (Input.GetMouseButtonDown(Mouse.LEFT) && !isFlagged)
        {
            // Debug.Log(indX + " , " + indY);

            isCovered = false;

            if (isMine)
            {
                Grid.UnCoverMines();

                Debug.Log("DEATH!");
            }
            else
            {
                SetSpriteNumber(Grid.AdjacentMines(indX, indY));

                Grid.FloodFillUncover(indX, indY, new bool[Grid.w, Grid.h]);
            }
        }
        else if (Input.GetMouseButtonDown(Mouse.RIGHT))
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



