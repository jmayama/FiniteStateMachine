



using UnityEngine;
using System;

public class MathUtils
{
    public static float CompareEpsilon = 0.00001f;

    public static float ExponentialEase(float easeSpeed, float start, float end, float dt)
    {
        float diff = end - start;

        diff *= Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return diff + start;
    }

    public static Vector3 ExponentialEase(float easeSpeed, Vector3 start, Vector3 end, float dt)
    {
        Vector3 diff = end - start;

        diff *= Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return diff + start;
    }

    public static float CalcRotationDegs(float x, float y)
    {
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    public static bool AlmostEquals(float v1, float v2, float epsilon)
    {
        return Mathf.Abs(v2 - v1) <= epsilon;
    }

    public static bool AlmostEquals(float v1, float v2)
    {
        return AlmostEquals(v1, v2, CompareEpsilon);
    }

    public static Vector2 RandomUnitVector2()
    {
        float angleRadians = UnityEngine.Random.Range(0.0f, 2.0f * Mathf.PI);

        Vector2 unitVector = new Vector2(
            Mathf.Cos(angleRadians),
            Mathf.Sin(angleRadians)
            );

        return unitVector;
    }

    public static Vector3 ReflectIfAgainstNormal(Vector3 vec, Vector3 normal)
    {
        //If the move direction is going back into the wall reflect the movement away from the wall
        float amountAlongNormal = Vector3.Dot(vec, normal);

        //If this value is negative it means it's going in the opposite direction of the normal.  This means we
        //need to reflect it.
        if (amountAlongNormal < 0.0f)
        {
            //Calculate the projection onto the normal
            Vector3 directionAlongNormal = normal * amountAlongNormal / normal.sqrMagnitude;

            //Subtract the projection once to remove the movement into the wall, and another time to make it move
            //away from the wall the same amount.  (this adds up to subtracting twice the projection)
            vec -= directionAlongNormal * 2.0f;
        }

        return vec;
    }
}
