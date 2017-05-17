using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PieceData
{
    public int gridx, gridz;
    public PieceData()
    {
        gridx = 0;
        gridz = 0;
    }
    public PieceData(int x, int z)
    {
        gridx = x;
        gridz = z;
    }
}
public class Piece : MonoBehaviour
{
    PieceData pieceData = new PieceData();
    public int gridx
    {
        get
        {
            return gridx;
        }
        set
        {
            pieceData.gridx = value;
            gridx = value;
        }
    }

    public int gridz
    {
        get
        {
            return gridz;
        }
        set
        {
            pieceData.gridx = value;
            gridz = value;
        }
    }
}
