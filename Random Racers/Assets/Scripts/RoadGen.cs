using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class RoadGen : MonoBehaviour
{
    Path path;
    RandGenManager manager = new RandGenManager();

    [Range(.05f, 100f)]
    public float spacing = 1;
    public float roadWidth = 10;
    public bool autoUpdate;
    // Start is called before the first frame update
    // Creats predefinedpoints for the Practice track
    void Start()
    {
        List<Vector3> track = new List<Vector3>() { new Vector3(350, 0, 350),
            new Vector3(450, 0, 0),
            new Vector3(350, 0, -350),
            new Vector3(-350, 0, -350),
            new Vector3(-450, 0, 0),
            new Vector3(-350, 0, 350) }; 
        Vector3 temp = track[0];
        Vector3 temp2 = track[1];
        path = new Path(temp, temp2);


        if (path.NumSegments < 10)
        {
            for (int i = 2; i < track.Count; i++)
            {
                path.AddSegment(track[i]);

            }
            path.AutoSetAllControlPoints();
            path.ToggleClosed();
        }
        Vector3[] points = path.CalculateEvenlySpacedPoints(spacing);
        
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(points, path.IsClosed);

    }

    //creates the road mesh
    Mesh CreateRoadMesh(Vector3[] pnts, bool isClosed)
    {

        Vector3[] vertecies = new Vector3[pnts.Length * 2];
        Vector3[] uvs = new Vector3[vertecies.Length];
        int numTriangles = 2 * (pnts.Length - 1) + ((isClosed) ? 2 : 0);
        int[] triangles = new int[numTriangles * 3];
        int v = 0;
        int t = 0;
        for (int i = 0; i < pnts.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < pnts.Length - 1 || isClosed)
            {
                forward += pnts[(i + 1) % pnts.Length] - pnts[i];
            }
            if (i > 0 || isClosed)
            {
                forward += pnts[i] - pnts[(i - 1 + pnts.Length) % pnts.Length];
            }
            forward.Normalize();

            Vector3 left = new Vector3(-forward.z, 0, forward.x);

            vertecies[v] = pnts[i] + left * roadWidth * .5f;
            vertecies[v + 1] = pnts[i] - left * roadWidth * .5f;

            float completionPercent = i / (float)(pnts.Length - 1);
            uvs[v] = new Vector3(0, completionPercent);
            uvs[v + 1] = new Vector3(1, completionPercent);
            if (i < pnts.Length - 1 || isClosed)
            {
                triangles[t] = v;
                triangles[t + 1] = (v + 2) % vertecies.Length;
                triangles[t + 2] = v + 1;

                triangles[t + 3] = v + 1;
                triangles[t + 4] = (v + 2) % vertecies.Length;
                triangles[t + 5] = (v + 3) % vertecies.Length;
            }
            t += 6;
            v += 2;

        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;

    }
}
