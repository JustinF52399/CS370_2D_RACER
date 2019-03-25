using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Vector3> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;
    public Path(Vector3 p1, Vector3 p2)
    {
        points = new List<Vector3>
        {
            p1,
            p1 + (Vector3.left + Vector3.forward)*20f,//foward = up
            p2 ,
            p2 + (Vector3.right+Vector3.back)*20f
            
        };
    }

    public Vector3 this[int i]
    {
        get
        {
            return points[i];
        }
    }
    public bool AutoSetControlPoints
    {
        get
        {
            return autoSetControlPoints;
        }
        set
        {
            if (autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }
    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }
    public int NumSegments
    {
        get
        {
            return points.Count / 3;
        }
    }

    public void ClearPoints()
    {
        points.Clear();
    }
    public void AddSegment(Vector3 ancorPos)
    {
        isClosed = false;
        points.Add(points[points.Count - 1] + ancorPos);
        points.Add(points[points.Count - 1] - points[points.Count - 2]);
        
        points.Add(ancorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public Vector3[] GetPointsInSegment(int i)
    {
        return new Vector3[] {points[i*3],
                              points[i*3+1],
                              points[i*3+2],
                              points[LoopIndex(i*3+3)]};
    }

    public void MovePoint(int i, Vector3 pos)
    {
        Vector3 deltaMove = pos - points[i];

        if (i % 3 == 0 || !autoSetControlPoints)
        {

            points[i] = pos;

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {
                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                    {
                        points[LoopIndex(i + 1)] += deltaMove;

                    }
                    if (i - 1 >= 0 || isClosed)
                    {
                        points[LoopIndex(i - 1)] += deltaMove;

                    }

                }
                else
                {
                    bool nextPointIsAnchor = (i + 1) % 3 == 0;
                    int corrospondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                    int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;
                    if (corrospondingControlIndex >= 0 && corrospondingControlIndex < points.Count || isClosed)
                    {
                        float distance = (points[LoopIndex(anchorIndex)] - points[LoopIndex(corrospondingControlIndex)]).magnitude;
                        Vector3 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                        points[LoopIndex(corrospondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * distance;

                    }
                }
            }
        }
    }
    public void ToggleClosed()
    {
        isClosed = !isClosed;
        if (isClosed)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add(points[0] * 2 - points[1]);
            if (autoSetControlPoints)
            {
                AutoSetAnchorControPoints(0);
                AutoSetAnchorControPoints(points.Count - 3);
            }
        }
        else
        {
            points.RemoveRange(points.Count - 2, 2);
            if (autoSetControlPoints)
            {
                AutoSetStartAndEndControls();
            }
        }
    }

    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
        {
            if (i >= 0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControPoints(LoopIndex(i));
            }
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControPoints(i);
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControPoints(int anchorIndex)
    {
        Vector3 anchorPos = points[anchorIndex];
        Vector3 dir = Vector3.zero;
        float[] neighborDst = new float[2];
        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector3 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;//should check if there are any difference between vector3 and vector2 version of this method
            neighborDst[0] = offset.magnitude;//ditto
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            Vector3 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
            dir -= offset.normalized;//should check if there are any difference between vector3 and vector2 version of this method
            neighborDst[1] = -offset.magnitude;//ditto
        }
        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighborDst[i] * .5f;
            }
        }
    }

    public void SplitSegement(Vector3 anchorPoint, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new Vector3[] { Vector3.zero, anchorPoint, Vector3.zero });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControPoints(segmentIndex * 3 + 3);
        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * .5f;
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
        }
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }

    public void SetFlat()
    {
        if (!autoSetControlPoints)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] -= new Vector3(0, points[i].y, 0);
            }
        }
    }
}
