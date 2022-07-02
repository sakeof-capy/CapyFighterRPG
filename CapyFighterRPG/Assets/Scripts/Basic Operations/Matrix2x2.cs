using System;
using UnityEngine;

public struct Matrix2x2
{
    public float A11 { get; set; }
    public float A12 { get; set; }
    public float A21 { get; set; }
    public float A22 { get; set; }

    public Matrix2x2(float a11, float a12, float a21, float a22)
    {
        A11 = a11;
        A12 = a12;
        A21 = a21;
        A22 = a22;
    }

    public Matrix2x2(float c) : this(c, 0, 0, c) { }

    public static Matrix2x2 Identity() => new Matrix2x2(1, 0, 0, 1);

    public float Det() => A11 * A22 - A12 * A21;

    public Matrix2x2 Inverse()
    {
        if (!IsInvertible())
            throw new InvalidOperationException("Cannot invert an uninvertible matrix.");
        return Adjugate() / Det();
    }

    public Matrix2x2 Transpose() => new Matrix2x2(A11, A21, A12, A22);

    public Matrix2x2 Adjugate() => new Matrix2x2(A22, -A12, -A21, A11);

    public bool IsInvertible() => Det() != 0;

    public static Matrix2x2 operator +(Matrix2x2 A, Matrix2x2 B)
        => new Matrix2x2
            (
            A.A11 + B.A11,
            A.A12 + B.A12,
            A.A21 + B.A21,
            A.A22 + B.A22
            );

    public static Matrix2x2 operator *(Matrix2x2 A, Matrix2x2 B)
        => new Matrix2x2
           (
           A.A11 * B.A11 + A.A12 * B.A21,
           A.A11 * B.A12 + A.A12 * B.A22,
           A.A21 * B.A11 + A.A22 * B.A21,
           A.A21 * B.A12 + A.A22 * B.A22
           );

    public static Matrix2x2 operator *(Matrix2x2 A, float c)
        => A * new Matrix2x2(c);

    public static Matrix2x2 operator *(float c, Matrix2x2 A)
        => A * new Matrix2x2(c);

    public static Matrix2x2 operator /(Matrix2x2 A, float c)
        => A * new Matrix2x2(1 / c);

    public static Vector2 operator *(Matrix2x2 A, Vector2 v)
        => new Vector2
        (
            A.A11 * v.x + A.A12 * v.y,
            A.A21 * v.x + A.A22 * v.y
        );
}