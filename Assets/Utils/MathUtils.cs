using System;
using UnityEngine;

public static class MathUtils
{
    public static int ArgMin<T>(params T[] values) where T : IComparable<T>
    {
        (T, int) min = (values[0], 0);
        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].CompareTo(min.Item1) < 0)
            {
                min = (values[i], i);
            }
        }

        return min.Item2;
    }

    public static (T, int) MinWithArg<T>(params T[] values) where T : IComparable<T>
    {
        (T, int) min = (values[0], 0);
        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].CompareTo(min.Item1) < 0)
            {
                min = (values[i], i);
            }
        }

        return min;
    }

    public static Vector2 CartesianToPolar(Vector2 xy)
    {
        float x = xy.x;
        float y = xy.y;
        return new Vector2(xy.magnitude, Mathf.Atan2(y, x));
    }

    public static Vector2 PolarToCartesian(Vector2 rtheta)
    {
        float r = rtheta.x;
        float theta = rtheta.y;
        return new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
    }

    public static float ToPositiveAngle(float thetaSigned)
    {
        if (thetaSigned < 0)
        {
            return 2 * Mathf.PI + (thetaSigned % (2 * Mathf.PI));
        }
        else
        {
            return thetaSigned;
        }
    }

    public static Vector4 QuaternionToVector(Quaternion q)
    {
        return new Vector4(q.x, q.y, q.z, q.w);
    }

    public static Quaternion VectorToQuaternion(Vector4 v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }

    public static float NormalizedSin(float x)
    {
        return 0.5f * Mathf.Sin(x) + 0.5f;
    }

    public static Vector3 Vector3Sin(float x, Vector3 amplitude, Vector3 frequency, Vector3 phase)
    {
        Vector3 res = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            res[i] = amplitude[i] * Mathf.Sin(frequency[i] * x + phase[i]);
        }
        return res;
    }

    public static Vector3 VectorMultiply(params Vector3[] vectors)
    {
        Vector3 res = Vector3.one;

        foreach (Vector3 v in vectors)
        {
            res.x *= v.x;
            res.y *= v.y;
            res.z *= v.z;
        }

        return res;
    }
    
    public static Vector4 VectorMultiply(params Vector4[] vectors)
    {
        Vector4 res = Vector4.one;

        foreach (Vector4 v in vectors)
        {
            res.x *= v.x;
            res.y *= v.y;
            res.z *= v.z;
            res.w *= v.w;
        }

        return res;
    }

    public static float Gaussian(float x, float mean, float variance)
    {
        // Calculate the coefficient part (1 / (sqrt(2 * PI * variance)))
        float coefficient = 1.0f / Mathf.Sqrt(2.0f * Mathf.PI * variance);

        // Calculate the exponent part (-((x - mean)^2) / (2 * variance))
        float exponent = -Mathf.Pow(x - mean, 2) / (2.0f * variance);

        // Combine the coefficient and exponent
        float result = coefficient * Mathf.Exp(exponent);

        return result;
    }

    public static float RepeatingNormalizedGaussian(float x, float multiplier = 1)
    {
        return Gaussian(x % 1, 0.5f, 0.001f * multiplier) / Gaussian(0.5f, 0.5f, 0.001f * multiplier);
    }

    public static float Mod(float a, float n)
    {
        return ((a % n) + n) % n;
    }

    public static int Mod(int a, int n)
    {
        return ((a % n) + n) % n;
    }

    public static int RangeMod(int value, int min, int max)
    {
        int range = max - min;
        if (range <= 0)
            throw new ArgumentException("max must be greater than min");

        int offsetValue = (value - min) % range;
        if (offsetValue < 0)
            offsetValue += range;

        return min + offsetValue;
    }

}
