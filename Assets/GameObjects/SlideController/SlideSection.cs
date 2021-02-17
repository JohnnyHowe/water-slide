using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSection
{
    Vector3 startPoint;
    Vector3 midPoint;
    Vector3 endPoint;

    float startAngle;
    float endAngle;
    float angleChange;
    Vector2 circleCenter;
    float radius;

    public SlideSection(Vector3 startPoint, Vector3 midPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.midPoint = midPoint;
        this.endPoint = endPoint;

        Vector2 startPoint2d = new Vector2(startPoint.x, startPoint.z);
        Vector2 midPoint2d = new Vector2(midPoint.x, midPoint.z);
        Vector2 endPoint2d = new Vector2(endPoint.x, endPoint.z);

        Vector2 line1Direction = midPoint2d - startPoint2d;
        Vector2 line2Direction = endPoint2d - midPoint2d;

        Vector2 line1AntiClockwisePerp = new Vector2(-line1Direction.y, line1Direction.x);
        Vector2 line2AntiClockwisePerp = new Vector2(-line2Direction.y, line2Direction.x);
        Vector2 line1ClockwisePerp = new Vector2(line1Direction.y, -line1Direction.x);
        Vector2 line2ClockwisePerp = new Vector2(line2Direction.y, -line2Direction.x);

        Vector2 intermediate1 = (startPoint2d + midPoint2d) / 2;
        Vector2 intermediate2 = (midPoint2d + endPoint2d) / 2;

        circleCenter = GetLineIntersection(line1AntiClockwisePerp, intermediate1, line2AntiClockwisePerp, intermediate2);
        radius = (intermediate1 - circleCenter).magnitude;

        float intermediate1Angle = AngleWith(Vector2.up, line1ClockwisePerp);
        float intermediate2Angle = AngleWith(Vector2.up, line2ClockwisePerp);
        //Debug.Log(intermediate1Angle * 180 / Mathf.PI + " " + intermediate2Angle * 180 / Mathf.PI);

        startAngle = intermediate2Angle % (Mathf.PI * 2);
        endAngle = intermediate1Angle % (Mathf.PI * 2);

        // If left turn, fix cornder being on wrong side of circle
        //Debug.Log((AngleWith(line1ClockwisePerp, line2ClockwisePerp) + Mathf.PI * 2) % (Mathf.PI * 2));
        if ((AngleWith(line1ClockwisePerp, line2ClockwisePerp) + Mathf.PI * 2) % (Mathf.PI * 2) < Mathf.PI)
        {
            startAngle = (startAngle + Mathf.PI) % (Mathf.PI * 2);
            endAngle = (endAngle + Mathf.PI) % (Mathf.PI * 2);
        }

        angleChange = endAngle - startAngle;

        //If angle is too big. make it small and negative
        if (angleChange > Mathf.PI)
        {
            angleChange = -(Mathf.PI * 2 - angleChange);
        }
        else if (angleChange < -Mathf.PI)
        {
            angleChange = (Mathf.PI * 2 + angleChange);
        }

        //Debug.Log(startAngle * 180 / Mathf.PI + " " + endAngle * 180 / Mathf.PI + " " + angleChange);
    }

    float AngleWith(Vector2 vec1, Vector2 vec2)
    {
        return (Mathf.Atan2(vec1.y, vec1.x) - Mathf.Atan2(vec2.y, vec2.x)) % (Mathf.PI * 2);
    }

    Vector2 GetLineIntersection(Vector2 line1_direction, Vector2 line1Point, Vector2 line2_direction, Vector2 line2Point)
    {
        Vector2 line1Point2 = line1Point + line1_direction;
        Vector2 line2Point2 = line2Point + line2_direction;

        float x1 = line1Point.x;
        float x2 = line1Point2.x;
        float y1 = line1Point.y;
        float y2 = line1Point2.y;

        float x3 = line2Point.x;
        float x4 = line2Point2.x;
        float y3 = line2Point.y;
        float y4 = line2Point2.y;

        float den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

        float x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / den;
        float y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / den;
        return new Vector2(x, y);
    }

    /// <summary>
    /// What direction is the path pointing when at d distance / progress
    /// result is normalized
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public Vector3 GetDirectionVector(float d)
    {
        float step = 0.01f;
        float mind = Mathf.Max(d - step, 0);
        Vector3 last = GetPositionOnSlide(mind);
        Vector3 next = GetPositionOnSlide(mind + step);
        return next - last;
    }

    public Vector3 GetPositionOnSlide(float d)
    {
        float angle = startAngle + angleChange * (d % 1);
        Vector2 point = circleCenter + new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        return new Vector3(point.x, 0, point.y);
    }

    public float GetAngle(float d)
    {
        Vector3 vec = GetDirectionVector(d);
        return -Mathf.Atan2(vec.z, vec.x);
    }
}
