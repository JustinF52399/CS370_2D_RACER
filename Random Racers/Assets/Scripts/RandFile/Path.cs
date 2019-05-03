using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    public List<Vector3> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;

    //initilizes the path with the first 2 points 
    public Path(Vector3 p1, Vector3 p2)
    {
        points = new List<Vector3>
        {
            p1,
            p1 + (Vector3.left + Vector3.forward)*20f,//foward = up
            p2 + (Vector3.right+Vector3.back)*20f,
            p2
           
            
        };
    }
    //returns the specified point
    public Vector3 this[int i]
    {
        get
        {
            return points[i];
        }
    }
    //determines if the road will be closed or not, will most likely always be closed
    public bool IsClosed
    {
        get
        {
            return isClosed;
        }
        set
        {
            if(isClosed != value)
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
        }
    }
    //automatically sets the controlpoints of the points on the road
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
    //return the numper of points
    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }
    //returns the number of segments of the path
    public int NumSegments
    {
        get
        {
            return points.Count / 3;
        }
    }
    //clears the path
    public void ClearPoints()
    {
        points.Clear();
    }
    //adds a segment to the path
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
    ////
    //public void SplitSegment(Vector3 anchorPos, int segIndex)
    //{
    //    points.InsertRange(segIndex * 3 + 2, new Vector3[] { Vector3.zero, anchorPos, Vector3.zero });
    //    if (autoSetControlPoints)
    //    {
    //        AutoSetAllAffectedControlPoints(segIndex * 3 + 3);
    //    }
    //    else
    //    {
    //        AutoSetAnchorControPoints(segIndex * 3 + 3);
    //    }
    //}

    //public void DeleteSegment(int anchorIndex)
    //{
    //    if (NumSegments > 2 || !isClosed && NumSegments > 1)
    //    {

    //        if (anchorIndex == 0)
    //        {
    //            if (isClosed)
    //            {
    //                points[points.Count - 1] = points[2];
    //            }
    //            points.RemoveRange(0, 3);
    //        }
    //        else if (anchorIndex == points.Count - 1 && !isClosed)
    //        {
    //            points.RemoveRange(anchorIndex - 2, 3);
    //        }
    //        else
    //        {
    //            points.RemoveRange(anchorIndex - 1, 3);
    //        }
    //    }
    //}
    //will calculate evenly spaced points along the path to facilitate creating the road mesh
    public Vector3[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector3> evenlySpacedPoints = new List<Vector3>();
        evenlySpacedPoints.Add(points[0]);
        Vector3 previousPoint = points[0];
        float dstSinceLastEvenPoint = 0;

        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            Vector3[] pnts = GetPointsInSegment(segmentIndex);
            float controlNetLength = Vector3.Distance(pnts[0], pnts[1]) + Vector3.Distance(pnts[1], pnts[2]) + Vector3.Distance(pnts[2], pnts[3]);
            float estimatedCurveLength = Vector3.Distance(pnts[0], pnts[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1f / divisions;
                Vector3 pointOnCurve = Bezier.EvaluateCubic(pnts[0], pnts[1], pnts[2], pnts[3], t);
                dstSinceLastEvenPoint += Vector3.Distance(previousPoint, pointOnCurve);

                while (dstSinceLastEvenPoint >= spacing)
                {
                    float overshootDst = dstSinceLastEvenPoint - spacing;
                    Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    dstSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }

                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }

    public Vector3[] GetPointsInSegment(int i)
    {
        return new Vector3[] {points[i*3],
                              points[i*3+1],
                              points[i*3+2],
                              points[LoopIndex(i*3+3)]};
    }
        
    //toggles weather or not the road is closed
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
    //automatically sets the control point of the affected point on the path
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
    //automatically sets the control points of all the points on the path
    public void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControPoints(i);
        }
        AutoSetStartAndEndControls();
    }
    //auto stes the position of the points on the path
    public void AutoSetAnchorControPoints(int anchorIndex)
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
        dir.Normalize();
        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighborDst[i] * .5f;
            }
        }
    }

   
    //special case setting for if the path is open
    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * .5f;
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
        }
    }
    //returns a looped count of the points index 
    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }

    
}
