using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GroundCreator WorldPiece;
    public GameObject PlayerObject;
    public int ChunkRenderDistance;
    public Vector2 PlayerChunkLocation;

    private List<GroundCreator> Pieces = new List<GroundCreator>(); 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get all of the world elements close to the player
        ////world chunks are 10x10 units
        ////divide player location by 10 and floor to get chunk id of player
        ///
        foreach (GroundCreator piece in Pieces)
        {
            piece.gameObject.SetActive(false);
        }

        PlayerChunkLocation = new Vector2(Mathf.Floor(PlayerObject.transform.position.x / 10f), Mathf.Floor(PlayerObject.transform.position.z / 10f));
        for (int x = (int)(PlayerChunkLocation.x - ChunkRenderDistance / 2); x < PlayerChunkLocation.x + ChunkRenderDistance / 2; x++)
        {
            for (int y = (int)(PlayerChunkLocation.y - ChunkRenderDistance / 2); y < PlayerChunkLocation.y + ChunkRenderDistance / 2; y++)
            {
                List<GroundCreator> matches = Pieces.Where(p => p.ChunkX == x && p.ChunkY == y).ToList();
                if (matches.Count > 0)
                {//The piece has already been generated, just show it
                    matches[0].gameObject.SetActive(true);
                }
                else
                {
                    GroundCreator Piece = Instantiate(WorldPiece.gameObject, new Vector3(x * 10, 0, y * 10), Quaternion.identity).GetComponent<GroundCreator>();
                    Piece.ChunkX = x;
                    Piece.ChunkY = y;
                    Pieces.Add(Piece);
                }
            }

        }
    }
}
