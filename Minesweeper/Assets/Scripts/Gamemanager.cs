using UnityEngine;
using System.Collections;

public class Gamemanager : MonoBehaviour
{
    public int w = 2, h = 2;
    public Block[,] blocks;

    public GameObject BlockPrefab;

    // yet to be implemented
    public int mines;
    public float mineChance;

    public void Start()
    {
        blocks = new Block[w, h];
        SpawnBlocks();
    }

    public void SpawnBlocks()
    {
        bool isMine;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                isMine = Random.value < mineChance;

                GameObject spawned = Instantiate(BlockPrefab, new Vector2(i, j), Quaternion.identity) as GameObject;
                Block b = spawned.GetComponent<Block>();
                blocks[i, j] = b;
                b.Init(this, i, j, isMine);
            }
        }
    }

    public void UnCoverMines()
    {
        foreach (Block b in blocks)
        {
            if (b.GetIsMine())
            {
                b.SetSpriteMine();
            }
        }
    }

    public bool MineAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
            return blocks[x, y].GetIsMine();

        return false;
    }

    public int AdjacentMines(int x, int y)
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

        return count;
    }

    public void FloodFillUncover(int x, int y, bool[,] visited)
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

    public bool IsFinished()
    {
        foreach (Block b in blocks)
            if (b.GetIsCovered() && !b.GetIsMine())
                return false;

        return true;
    }
}
