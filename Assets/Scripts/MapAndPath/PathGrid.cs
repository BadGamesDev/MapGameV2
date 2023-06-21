    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PathGrid : MonoBehaviour
    {
        public bool drawGrid = true;
        //float delay = 2; might delete

        private PathNode[,] grid;
        public int width = 0;
        public int height = 0;

        LayerMask defaultMask;

        private void Start()
        {
            defaultMask = LayerMask.GetMask("Tiles");
        }
    
        public void DrawGrid()
        {
            grid = new PathNode[width, height];

            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Vector2 pos = new Vector2(x, y);
                    if (y % 2 == 0)
                    {
                        pos.x += 0.5f;
                    }
                
                    pos.y *= 0.86f;

                    grid[x, y] = new PathNode(pos);

                    int open = 0;

                    RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, defaultMask);
                    if (hit)
                    {
                        if (hit.collider.gameObject.GetComponent<TileProps>().type == 1)
                        {
                            open = 10;
                        }
                        if (hit.collider.gameObject.GetComponent<TileProps>().type == 2)
                        {
                            open = 5;
                        }
                    }

                    grid[x, y].open = open;
                    grid[x, y].oldOpen = open;

                }
            }
        }
        //might delete
        //private void Update()
        //{
        //    delay -= Time.deltaTime;

        //    if (drawGrid && delay < 0)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            for (int y = 0; y < height; y++)
        //            {
        //                if (grid[x, y].open == 10)
        //                {
        //                    Debug.DrawLine(grid[x, y].pos - new Vector2(0.5f, 0.5f), grid[x, y].pos + new Vector2(0.5f, 0.5f), Color.red, 0.5f);
        //                }
        //                else if (grid[x,y].open == 5)
        //                {
        //                    Debug.DrawLine(grid[x, y].pos - new Vector2(0.5f, 0.5f), grid[x, y].pos + new Vector2(0.5f, 0.5f), Color.yellow, 0.5f);
        //                }
        //            }
        //        }
        //    }
        //}

        public PathNode GetNode(Vector2 pos)
        {
            pos.y = Mathf.RoundToInt(pos.y / 0.86f);
            if (pos.y % 2 == 0)
            {
                pos.x -= 0.5f;
            }
            return grid[(int)pos.x,(int)pos.y];
        }

        public PathNode GetNodeInt(int x, int y)
        {
            return grid[x, y];
        }
    }
