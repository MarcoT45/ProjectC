using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width;
    public int height;
    public float xStart;
    public float yStart;
    public float xSpace;
    public float ySpace;
    public int cellSize;
    private GameObject[,] gridArray;
    public GameObject grid;

    //A mettre dans un script à part 
    public GameObject character;
    
    [Header("Tiles")]
    [Header("Corners")]
    public GameObject[] cornerTopLeftTiles;
    public GameObject[] cornerTopRightTiles;
    public GameObject[] cornerBottomLeftTiles;
    public GameObject[] cornerBottomRightTiles;

    [Header("Content")]
    public GameObject[] topTiles;
    public GameObject[] bottomTiles;
    public GameObject[] leftTiles;
    public GameObject[] rightTiles;
    public GameObject[] centerTiles;


    public void SetCell(int x, int y)
    {
        RoomTile prevTile;
        RoomTile underTile;
        GameObject randomTile = null;


        //Coins
        //Coin BG
        if( x == 0 && y == 0)
        {
            randomTile = this.SetTile(cornerBottomLeftTiles, 0, 0);
        }
        //Coin HG
        else if( x == 0 && y == (height - 1))
        {
            underTile = gridArray[x, y-1].GetComponent<RoomTile>();
            randomTile = this.SetTile(cornerTopLeftTiles, underTile.numberDoorsTop, 0);
        }
        //Coin BD
        else if( x == (width - 1) && y == 0 )
        {
            prevTile = gridArray[x-1, y].GetComponent<RoomTile>();
            randomTile = this.SetTile(cornerBottomRightTiles, 0, prevTile.numberDoorsRight);

        }
        //Coin HD
        else if( x == (width - 1) && y == (height - 1))
        {
            prevTile = gridArray[x-1, y].GetComponent<RoomTile>();
            underTile = gridArray[x, y-1].GetComponent<RoomTile>();
            randomTile = this.SetTile(cornerTopRightTiles, underTile.numberDoorsTop, prevTile.numberDoorsRight);
        }
        //Cotés
        //G
        else if( x == 0 )
        {
            underTile = gridArray[x, y-1].GetComponent<RoomTile>();
            randomTile = this.SetTile(leftTiles, underTile.numberDoorsTop, 0);
        }
        //D
        else if( x == (width - 1)  )
        {
            prevTile = gridArray[x-1, y].GetComponent<RoomTile>();
            underTile = gridArray[x, y-1].GetComponent<RoomTile>();
            randomTile = this.SetTile(rightTiles, underTile.numberDoorsTop, prevTile.numberDoorsRight);
        }
        //H
        else if( y == (height - 1))
        {
            prevTile = gridArray[x-1, y].GetComponent<RoomTile>();
            underTile = gridArray[x, y-1].GetComponent<RoomTile>();
            randomTile = this.SetTile(topTiles, underTile.numberDoorsTop, prevTile.numberDoorsRight);
        }
        //B
        else if( y == 0 )
        {
            prevTile = gridArray[x-1, y].GetComponent<RoomTile>();
            randomTile = this.SetTile(bottomTiles, 0, prevTile.numberDoorsRight);
        }
        //Centre
        else
        {
            prevTile = gridArray[x-1, y].GetComponent<RoomTile>();
            underTile = gridArray[x, y-1].GetComponent<RoomTile>();
            randomTile = this.SetTile(centerTiles, underTile.numberDoorsTop, prevTile.numberDoorsRight);
        }

        GameObject instantiatedTile = Instantiate(randomTile,  GetWorldPosition(x, y), Quaternion.identity);
        instantiatedTile.transform.parent = grid.transform;
        instantiatedTile.name = "Room("+x+","+y+")";
        gridArray[x, y] = randomTile;
    }

    private GameObject SetTile(GameObject[] tiles, int numberDoorsBottom, int numberDoorsLeft){
        List<GameObject> listTile = new List<GameObject>();
        int randomIndex;
        RoomTile tile;

        for (int i = 0; i < tiles.Length; i++)
        {
            tile = tiles[i].GetComponent<RoomTile>();
            
            if((tile.numberDoorsBottom == numberDoorsBottom && tile.numberDoorsLeft == numberDoorsLeft) || (tile.numberDoorsBottom == 0 && tile.numberDoorsLeft == 0))
            {
                listTile.Add(tiles[i]);
            }
        }

        randomIndex = Random.Range(0,listTile.Count );
        
        return listTile[randomIndex];

    }
    
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(xStart + (xSpace * x) , yStart + ( ySpace * y )) * cellSize;
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnPoint;
        
        gridArray = new GameObject[width,height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {   
                this.SetCell(x,y);
            }
        }

        //A mettre dans un script à part 
        spawnPoint = GameObject.Find("SpawnPoint");
        Instantiate(character, spawnPoint.transform.position , Quaternion.identity);
        
    }

}
