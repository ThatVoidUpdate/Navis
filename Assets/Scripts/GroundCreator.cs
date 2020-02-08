using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GroundCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public int Width = 100;
    public int Height = 100;

    public float TriangleSize = 1;

    [Header("Coarse Noise")]
    public float CoarsePerlinWidthScale;
    public float CoarsePerlinHeightScale;

    [Header("Fine Noise")]
    public float FinePerlinWidthScale;
    public float FinePerlinHeightScale;

    private MeshRenderer rend;
    private MeshFilter filter;

    [HideInInspector]
    public int ChunkX = 0;
    [HideInInspector]
    public int ChunkY = 0;

    private bool HasGenerated = false;

    public void Start()
    {
        rend = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        Generate();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !HasGenerated)
        {
            GenerateSurrounding();
            HasGenerated = true;
        }
    }
    public void Generate()
    {
        //rend.sharedMaterial = new Material(Shader.Find("Standard"));

        Mesh mesh = new Mesh();

        List<Vector3> VerticesList = new List<Vector3>();
        print("Chunk: " + ChunkX + " " + ChunkY + ": " + (ChunkX * Width) + ", " + (ChunkY * Height));
        for (int y = 0; y < Height + 1; y++)
        {
            for (int x = 0; x < Width + 1; x++)
            {
                float height = Mathf.PerlinNoise((x + (ChunkX * Width)) * CoarsePerlinWidthScale, (y + (ChunkY * Width)) * CoarsePerlinWidthScale) * CoarsePerlinHeightScale +
                               Mathf.PerlinNoise((x + (ChunkX * Width)) * FinePerlinWidthScale, (y + (ChunkY * Width)) * FinePerlinWidthScale) * FinePerlinHeightScale;
                VerticesList.Add(new Vector3(x * TriangleSize, height, y * TriangleSize));
            }
        }
        mesh.vertices = VerticesList.ToArray();

        List<int> TrisList = new List<int>();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                TrisList.Add(y * (Width + 1) + x);
                TrisList.Add((y + 1) * (Width + 1) + x);
                TrisList.Add(y * (Width + 1) + x + 1);

                TrisList.Add((y + 1) * (Width + 1) + x);
                TrisList.Add((y + 1) * (Width + 1) + x + 1);
                TrisList.Add(y * (Width + 1) + x + 1);
            }
        }

        mesh.triangles = TrisList.ToArray();

        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }

    public void GenerateSurrounding()
    {
        Instantiate(this.gameObject, transform.position + new Vector3(Width * TriangleSize, 0, 0), Quaternion.identity).GetComponent<GroundCreator>().ChunkX = ChunkX + 1;
        Instantiate(this.gameObject, transform.position + new Vector3(0, 0, Height * TriangleSize), Quaternion.identity).GetComponent<GroundCreator>().ChunkY = ChunkY + 1;
    }


}
