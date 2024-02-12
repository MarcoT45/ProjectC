using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class HubController : MonoBehaviour
{

    [SerializeField]
    private Tilemap sortieTileMap;
    public LayerMask layerSortie;
    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Physics2D.OverlapCircle(character.transform.position, .2f, layerSortie))
        {
            SceneManager.LoadScene(2);
        }*/

        Vector3Int gridPosition = sortieTileMap.WorldToCell(character.transform.position);
        if (sortieTileMap.HasTile(gridPosition))
        {
            SceneManager.LoadScene(2);
        }
    }
}
