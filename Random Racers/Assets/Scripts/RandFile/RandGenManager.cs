using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandGenManager
{
    float xRand;
    float yRand;
    float zRand;
    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> hull = new List<Vector3>();
    public List<Vector3> sortedPoints = new List<Vector3>();
    List<Transform> sortedTrans = new List<Transform>();
    Path path;

    // Start is called before the first frame update
    public void Start()
    {
        points.Clear();
        hull.Clear();
        sortedPoints.Clear();
        sortedTrans.Clear();

        int seed = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        Random.InitState(seed);
        
        int randVal = Random.RandomRange(10, 20);

        for (int i = 1; i <= 4; i++)
        {
            GenPointByQuadrent(i, (int)randVal / 4);
        }

        



        //for loop to push several points apart
        int pushIterator = 3;
        //add more points to the convex hull
        List<Vector3> rSet = new List<Vector3>(hull.Capacity * 2);
        Vector3 disp = new Vector3();
        float maxDisp = 20f;    
        hull = points;
    }

    private void liftY()
    {
        for (int i = 0; i < hull.Count; i++)
        {
            float xVal = hull[i].x;
            yRand = Random.Range(0f, 10f);
            float zVal = hull[i].z;

            hull[i].Set(xVal, yRand, zVal);
        }
    }

    private void GenPointByQuadrent(int quadrent = 1, int numPoints = 1)
    {
        List<Vector3> tempo = new List<Vector3>();
        switch (quadrent)
        {
            case 1:
                for (int i = 0; i < numPoints; i++)
                {
                    //get a random x and z in quadrent 1
                    xRand = Random.Range(40f, 200f);
                    yRand = Random.Range(20f, 25f);
                    zRand = Random.Range(20f, 200f);

                    
                    //create a temporary point
                    Vector3 temp = new Vector3(xRand, yRand, zRand);

                    //adds it to list
                    tempo.Add(temp);

                }
                tempo.Sort((a, b) => a.z.CompareTo(b.z));
                //if the points are too close together, pushes them apart
                for (int i = 0; i < tempo.Count - 2; i++)
                {
                    if (i + 2 < tempo.Count)
                    {

                        if (Mathf.Abs(tempo[i].z - tempo[i + 2].z) < 150.0)
                        {
                            float newZ = Mathf.Abs(tempo[i].z - tempo[i + 2].z);

                            for (int j = i + 2; j < tempo.Count; j++)
                            {
                                tempo[j] = new Vector3(tempo[j].x, tempo[j].y, tempo[j].z + (150f - newZ));
                            }
                        }
                    }
                }
                if (tempo[tempo.Count - 1].x > tempo[tempo.Count - 2].x)
                {
                    tempo[tempo.Count - 1] = new Vector3(tempo[tempo.Count - 2].x / 2, tempo[tempo.Count - 1].y, tempo[tempo.Count - 1].z);
                }
                break;
            case 2:
                for (int i = 0; i < numPoints; i++)
                {
                    //get a random x and z in quadrent 2
                    xRand = Random.Range(-200f, -40f);
                    yRand = Random.Range(20f, 25f);
                    zRand = Random.Range(20f, 200f);

                    
                    //create a temporary point
                    Vector3 temp = new Vector3(xRand, yRand, zRand);

                    //adds it to list
                    tempo.Add(temp);
                }
                tempo.Sort((a, b) => b.z.CompareTo(a.z));
                //if the points are too close together, pushes them apart
                for (int i = tempo.Count - 1; i > 1; i--)
                {
                    if (Mathf.Abs(tempo[i].z - tempo[i - 2].z) < 150.0)
                    {
                        float newZ = Mathf.Abs(tempo[i].z - tempo[i - 2].z);

                        for (int j = i - 2; j >= 0; j--)
                        {
                            tempo[j] = new Vector3(tempo[j].x, tempo[j].y, tempo[j].z + (150f - newZ));
                        }

                    }
                }
                if(tempo[tempo.Count-1].z - points[points.Count-1].z > 80 && points[points.Count-1].z <= tempo[tempo.Count - 2].z)
                {
                    xRand = Random.Range(20f, 200f);
                    yRand = Random.Range(20f, 25f);
                    zRand = Random.Range(tempo[tempo.Count - 2].z, 200f);

                    points.Add(new Vector3(xRand, yRand, zRand));
                }
                if(tempo[tempo.Count-1].x < tempo[tempo.Count - 2].x)
                {
                    if(points[points.Count-1].z< tempo[tempo.Count - 1].z)
                    {
                        xRand = Random.Range(20f, 200f);
                        yRand = Random.Range(20f, 25f);
                        zRand = Random.Range(tempo[tempo.Count - 1].z, 200f);
                        points.Add(new Vector3(xRand, yRand, zRand));
                    }
                }

                break;
            case 3:
                for (int i = 0; i < numPoints; i++)
                {
                    //get a random x and z in quadrent 3
                    xRand = Random.Range(-200f, -40f);
                    yRand = Random.Range(20f, 25f);
                    zRand = Random.Range(-200f, -40f);                   

                    //create a temporary point
                    Vector3 temp = new Vector3(xRand, yRand, zRand);

                    //adds it to list
                    tempo.Add(temp);
                }
                tempo.Sort((a, b) => b.z.CompareTo(a.z));
                //if the points are too close together, pushes them apart
                for (int i = 0; i < tempo.Count - 2; i++)
                {
                    if (Mathf.Abs(tempo[i].z - tempo[i + 2].z) < 150.0)
                    {
                        float newZ = Mathf.Abs(tempo[i].z - tempo[i + 2].z);
                        Debug.Log(newZ);
                        for (int j = i + 2; j < tempo.Count; j++)
                        {
                            tempo[j] = new Vector3(tempo[j].x, tempo[j].y, tempo[j].z - (150f - newZ));
                        }

                    }
                }
                if (tempo[tempo.Count - 1].x < tempo[tempo.Count - 2].x)
                {
                   tempo[tempo.Count - 1] = new Vector3(tempo[tempo.Count - 2].x / 2, tempo[tempo.Count - 1].y, tempo[tempo.Count - 1].z);
                }
                break;
            case 4:
                for (int i = 0; i < numPoints; i++)
                {
                    //get a random x and z in quadrent 4
                    xRand = Random.Range(40f, 200f);
                    yRand = Random.Range(20f, 25f);
                    zRand = Random.Range(-200f, -40f);

                    

                    //create a temporary point
                    Vector3 temp = new Vector3(xRand, yRand, zRand);

                    //adds it to list
                    tempo.Add(temp);
                }
                tempo.Sort((a, b) => a.z.CompareTo(b.z));
                //if the points are too close together, pushes them apart
                for (int i = tempo.Count - 1; i > 1; i--)
                {
                    if (Mathf.Abs(tempo[i].z - tempo[i - 2].z) < 150.0)
                    {
                        float newZ = Mathf.Abs(tempo[i].z - tempo[i - 2].z);

                        for (int j = i - 2; j >= 0; j--)
                        {
                            tempo[j] = new Vector3(tempo[j].x, tempo[j].y, tempo[j].z - (150f - newZ));
                        }

                    }
                }
                if (Mathf.Abs(tempo[tempo.Count - 1].z - points[points.Count - 1].z) > 80 && points[points.Count - 1].z >= tempo[tempo.Count - 2].z)
                {
                    xRand = Random.Range(-200f, -40f);
                    yRand = Random.Range(20f, 25f);
                    zRand = Random.Range(-200f,tempo[tempo.Count - 2].z);

                    points.Add(new Vector3(xRand, yRand, zRand));
                }
                break;
        }
        foreach (Vector3 i in tempo)
        {
            points.Add(i);
        }
    }
}

