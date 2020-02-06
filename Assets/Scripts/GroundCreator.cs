using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GroundCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public int Width;
    public int Height;

    public float TriangleSize;

    [Header("Coarse Noise")]
    public float CoarsePerlinWidthScale;
    public float CoarsePerlinHeightScale;

    [Header("Fine Noise")]
    public float FinePerlinWidthScale;
    public float FinePerlinHeightScale;

    private MeshRenderer rend;
    private MeshFilter filter;

    public void Start()
    {
        rend = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        Generate();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }
    public void Generate()
    {
        rend.sharedMaterial = new Material(Shader.Find("Standard"));

        Mesh mesh = new Mesh();

        List<Vector3> VerticesList = new List<Vector3>();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float height = Mathf.PerlinNoise(x * CoarsePerlinWidthScale, y * CoarsePerlinWidthScale) * CoarsePerlinHeightScale + Mathf.PerlinNoise(x * FinePerlinWidthScale, y * FinePerlinWidthScale) * FinePerlinHeightScale;
                VerticesList.Add(new Vector3(x * TriangleSize, height, y * TriangleSize));
            }
        }
        mesh.vertices = VerticesList.ToArray(); ;

        List<int> TrisList = new List<int>();

        for (int y = 0; y < Height - 1; y++)
        {
            for (int x = 0; x < Width - 1; x++)
            {
                TrisList.Add(y * Width + x);
                TrisList.Add((y + 1) * Width + x);
                TrisList.Add(y * Width + x + 1);

                TrisList.Add((y + 1) * Width + x);
                TrisList.Add((y + 1) * Width + x + 1);
                TrisList.Add(y * Width + x + 1);
            }
        }

        mesh.triangles = TrisList.ToArray();

        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }
}
