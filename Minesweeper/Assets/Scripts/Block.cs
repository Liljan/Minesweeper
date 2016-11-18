using UnityEngine;
using System.Collections;

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

    private SpriteRenderer spriteRenderer;
    public Sprite[] emptyTextures;
    public Sprite mineTexture;

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
        spriteRenderer.sprite = mineTexture;
    }

    public void SetSpriteNumber(int adjacent)
    {
        spriteRenderer.sprite = emptyTextures[adjacent];
    }
    private void OnMouseUpAsButton()
    {
        Debug.Log(indX + " , " + indY);

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

    public bool GetIsMine() { return isMine; }

}



