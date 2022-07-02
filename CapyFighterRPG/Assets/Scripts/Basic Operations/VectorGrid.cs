using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class VectorGrid
{
    #region Fields
    private Vector2 _origin;
    private Vector2 _originsNeighbour;
    private Vector2 _diagonalToOrigin;
    private Vector2 _otherNeighbour;
    private int     _rows;
    private int     _cols;
    #endregion

    #region Properties
    public Vector2 Origin => _origin;
    public Vector2 OriginsNeighbour => _originsNeighbour;
    public Vector2 DiagonalToOrigin => _diagonalToOrigin;
    public Vector2 OtherNeighbour => _otherNeighbour;
    #endregion

    #region Constructors
    /// <summary>
    /// The grid will be constructed by three points (paralelogram vertices)
    /// </summary>
    /// <param name="origin">Vertex at which the counting starts</param>
    /// <param name="originsNeighbour">Vertex adjacent to the origin</param>
    /// <param name="diagonalToOrigin">Vertex not adjacent to the origin or diagonally opposite to it</param>
    /// <param name="rows">Number of grid's rows</param>
    /// <param name="cols">Number of grud's columns</param>
    public VectorGrid(Vector2 origin, Vector2 originsNeighbour, Vector2 diagonalToOrigin, int rows, int cols)
    {
        _origin = origin;
        _originsNeighbour = originsNeighbour;
        _diagonalToOrigin = diagonalToOrigin;
        _rows = rows;
        _cols = cols;

        _otherNeighbour = CentrallyReflect(originsNeighbour);
    }
    #endregion

    #region Methods
    private Vector3 CentrallyReflect(Vector3 point)
    {
        var centre = (_origin + _diagonalToOrigin) / 2;
        var other_diagonal_v = 2 * (centre - _originsNeighbour);

        return _originsNeighbour + other_diagonal_v;
    }
    /*
    *  d - diagonal vector of the paralelogram
    *  d = d_e1 + d_e2
    *  d_e1 = m*e1  =>  e1 = d_e1 / 2m
    *  d_e2 = n*e2  =>  e2 = d_e2 / 2n
    *  rightDown + (e1 + e2)
    */
    public Vector3[] CalculateCellCentres()
    {
        var e1 = (_originsNeighbour - _origin) / (2 * _rows);
        var e2 = (_otherNeighbour - _origin) / (2 * _cols);
        var o = _origin + (e1 + e2);

        var result = new Vector3[_rows * _cols];

        for (int i = 0; i < _rows; ++i)
            for (int j = 0; j < _cols; ++j)
            {
                result[i * _cols + j] = o + 2 * i * e1 + 2 * j * e2;
            }

        return result;
    }
    #endregion
}