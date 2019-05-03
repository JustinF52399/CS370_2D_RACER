using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class RoadGenerator : MonoBehaviour
{
    
    Path path;
    RandGenManager manager = new RandGenManager();
   
    LeftWall lWall = new LeftWall();
    RightWall rWall = new RightWall();
    [Range(.05f, 100f)]
    public float spacing = 1;
    public float roadWidth = 10;
    public bool autoUpdate;
    public GameObject car;
    public GameObject trafficCone;
    public GameObject wall;
    public GameObject barrel;
    public GameObject speedBump;
    GameObject dot;
    GameObject gameCar;
    Vector3[] points;
    Vector3[] lVerts;
    Vector3[] rTris;
    public GameObject checkpoint1;
    public GameObject checkpoint2;
    public GameObject checkpoint3;
    public GameObject checkpoint4;
    public GameObject startline;
    public Mesh roadMesh;

    // Start is called before the first frame update
    //calls the methods to create the path, spawn the car, spawn the obsitcles and spawn the checkpoints
    void Start()
    {
        
        manager.Start();
        Vector3 temp = manager.hull[0];
        Vector3 temp2 = manager.hull[1];
        path = new Path(temp, temp2);

        if (Random.value > 0.9)
        {
            RenderSettings.fogDensity = 0.10f;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fog = true;
        }



        if (path.NumSegments < 10)
        {
            for (int i = 2; i < manager.hull.Count; i++)
            {               
                path.AddSegment(manager.hull[i]);           
                
            }
            path.ToggleClosed();
            path.AutoSetAllControlPoints();
            
        }
        points = path.CalculateEvenlySpacedPoints(spacing);      

        roadMesh = CreateRoadMesh(points, path.IsClosed);
        GetComponent<MeshFilter>().mesh = roadMesh;
        GetComponent<MeshCollider>().sharedMesh = roadMesh;

        GenerateObjects();
        GenerateCheckpoints();
    }
    //spawns the check points
    void GenerateCheckpoints()
    {

        int dist = points.Length;
        int quart = dist / 4;       
        GameObject spawnPreferences;
        Vector3 c_point1offset = new Vector3(0, 2.3f, 0);
        Vector3 c_point2offset = new Vector3(0, 0, 0);
        Vector3 c_point3offset = new Vector3(0, 0, 0);
        Vector3 c_point4offset = new Vector3(0, 0, 0);
        Vector3 startlineoffset = new Vector3(0, 0, 0);

        Vector3 temp;


        spawnPreferences = Instantiate(startline, (points[0] + startlineoffset), Quaternion.LookRotation(points[1] - points[0]));
        //spawnPreferences.transform.rotation = new Quaternion(90f, spawnPreferences.transform.rotation.y, spawnPreferences.transform.rotation.z,1f);
        spawnPreferences.transform.position += new Vector3(0f, 10f, 0f);
        spawnPreferences.transform.localScale = new Vector3(3.5f, 1f, 1f);
        spawnPreferences.name = "startlinetest";
        temp = spawnPreferences.transform.eulerAngles;

        spawnPreferences = Instantiate(checkpoint1, (points[1] + c_point1offset), Quaternion.LookRotation(roadMesh.vertices[(1 * 2)] - points[1]));
        spawnPreferences.name = "cpoint1";
        spawnPreferences.tag = "c1";
        temp = spawnPreferences.transform.eulerAngles;



        spawnPreferences = Instantiate(checkpoint2, (points[quart] + c_point2offset), Quaternion.LookRotation(roadMesh.vertices[(quart * 2)] - points[quart]));
        spawnPreferences.name = "cpoint2";
        spawnPreferences.tag = "c2";
        temp = spawnPreferences.transform.eulerAngles;


        spawnPreferences = Instantiate(checkpoint3, (points[quart * 2] + c_point3offset), Quaternion.LookRotation(roadMesh.vertices[((quart * 2) * 2)] - points[quart * 2]));
        spawnPreferences.name = "cpoint3";
        spawnPreferences.tag = "c3";
        temp = spawnPreferences.transform.eulerAngles;


        spawnPreferences = Instantiate(checkpoint4, (points[quart * 3] + c_point3offset), Quaternion.LookRotation(roadMesh.vertices[((quart * 3) * 2)] - points[quart * 3]));
        spawnPreferences.name = "cpoint4";
        spawnPreferences.tag = "c4";
        temp = spawnPreferences.transform.eulerAngles;



    }
    //spawns the car, minimap dot, and randomly positions the obsticles
    void GenerateObjects()
    {
        int rVal = points.Length;
        GameObject spawnPreferences;
        GameObject gameWall;
        Vector3 temp;
        int numPointDist = 10;
        Vector3 wallOffset = new Vector3(0, 2.3f, 0);
        Vector3 barrelOffset = new Vector3(0, 2.5f, 0);
        Vector3 trafficOffset = new Vector3(0, 2.5f, 0);
        Vector3 sbOffset = new Vector3(0, 0, 0);
        RaycastHit hit = new RaycastHit();

        Vector3 spawnPos= new Vector3(points[0].x, points[0].y+3, points[0].z);
        

        Vector3 firstPoint = points[1] + spawnPos;

        gameCar = Instantiate(car, spawnPos, Quaternion.LookRotation(points[1]-spawnPos));
        gameCar.transform.rotation = new Quaternion(gameCar.transform.rotation.x, gameCar.transform.rotation.y, 0f, 1f);
        dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dot.transform.position = new Vector3(spawnPos.x, spawnPos.y +40f, spawnPos.z);
        dot.transform.localScale = new Vector3(30, 30, 30);
        dot.GetComponent<Renderer>().material.color = Color.red;
        dot.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        dot.layer = 9;





        for (int i = 1; i < rVal; i++)
        {
            float f = Random.Range(1f, 4.5f);
            int iz = (int)Mathf.Floor(f);

            switch (iz)
            {
                case 1:
                    if (i % numPointDist == 0)
                    {
                        gameWall = Instantiate(wall, (points[i] + wallOffset), Quaternion.LookRotation(roadMesh.vertices[(i * 2)] - points[i]));
                        gameWall.name = "Test_Wall";
                        temp = gameWall.transform.eulerAngles;
                    }
                    break;
                case 2:
                    if (i % numPointDist == 0)
                    {
                        spawnPreferences = Instantiate(barrel, (points[i] + barrelOffset), new Quaternion(0f, Vector3.Angle(points[i], points[i + 1]), 0f, 0f));
                        spawnPreferences.name = "Test_Barrel";
                        temp = spawnPreferences.transform.eulerAngles;
                    }
                    break;
                case 3:
                    if (i % numPointDist == 0)
                    {
                        spawnPreferences = Instantiate(trafficCone, (points[i] + trafficOffset), new Quaternion(0f, Vector3.Angle(points[i], points[i + 1]), 0f, 0f));
                        spawnPreferences.name = "Test_TrafficCone";
                        temp = spawnPreferences.transform.eulerAngles;
                    }
                    break;
                case 4:
                    if (i % numPointDist == 0)
                    {
                        spawnPreferences = Instantiate(speedBump, (points[i] + sbOffset), Quaternion.LookRotation(roadMesh.vertices[(i * 2)] - points[i]));
                        spawnPreferences.name = "Test_SpeedBump";
                        temp = spawnPreferences.transform.eulerAngles;
                    }
                    break;
            }
        }
    }
    //updates the position of the minimap dot
    void Update()
    {
        Vector3 temp = gameCar.transform.position;
        dot.transform.position = new Vector3(temp.x, temp.y + 40f, temp.z);
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
                triangles[t + 2] = (v + 1) % vertecies.Length;

                triangles[t + 3] = (v + 1) % vertecies.Length;
                triangles[t + 4] = (v + 2) % vertecies.Length;
                triangles[t + 5] = (v + 3) % vertecies.Length;
            }
            t += 6;
            v += 2;
            

        }
        lWall.generateLeftWall(pnts, isClosed, roadWidth);
        rWall.generateRightWall(pnts, isClosed, roadWidth);
        Mesh roadMesh = new Mesh();
        roadMesh.vertices = vertecies;
        roadMesh.triangles = triangles;
        roadMesh.RecalculateNormals();
        

        return roadMesh;

    }   
}
