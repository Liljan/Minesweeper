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
        }
    }

    public bool GetIsMine() { return isMine; }

}



