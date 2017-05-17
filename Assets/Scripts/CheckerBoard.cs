﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("CheckerBoardData")]
public class CheckerBoardData
{
    [XmlArray("Pieces")]
    [XmlArrayItem("Pieces")]
    public PieceData[] pieces;
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(CheckerBoardData));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static CheckerBoardData Load(string path)
    {
        var serializer = new XmlSerializer(typeof(CheckerBoardData));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as CheckerBoardData;
        }
    }

}
public class CheckerBoard : MonoBehaviour
{

    public string fileName;
    public GameObject blackPiece;
    public GameObject whitePiece;

    public int boardX = 8, boardZ = 8;
    public float pieceRadius = 0.5f;

    public Piece[,] pieces;
    private int halfBoardX, halfBoardZ;
    private float pieceDiameter;
    private Vector3 bottomLeft;
    private CheckerBoardData data;

    // Use this for initialization
    void Start()
    {
        // calculate a few values
        halfBoardX = boardX / 2;
        halfBoardZ = boardZ / 2;
        pieceDiameter = pieceRadius * 2;
        bottomLeft = transform.position - Vector3.right * halfBoardX - Vector3.forward * halfBoardZ;

        string path = Application.persistentDataPath + "/" + fileName;
        data = CheckerBoardData.Load(path);
        CreateGrid();
        data = new CheckerBoardData();
        data.Save(path);
    }

    void CreateGrid()
    {
        // Initialize 2D array
        pieces = new Piece[boardX, boardZ];

        #region Generate White Pieces
        // Loop through board columns and skip 2 each line
        for (int x = 0; x < boardX; x += 2)
        {
            // Loop through first 3 rows
            for (int z = 0; z < 3; z++)
            {
                // Check even row
                bool evenRow = z % 2 == 0;
                int gridX = evenRow ? x : x + 1;
                int gridZ = z;
                // Generate piece
                GeneratePiece(whitePiece, gridX, gridZ);
            }
        }

        #endregion

        #region Generate Black Pieces
        // Loop through board columns and skip 2 each line
        for (int x = 0; x < boardX; x += 2)
        {
            // Loop through first 3 rows
            for (int z = boardZ - 3; z < boardZ; z++)
            {
                // Check even row
                bool evenRow = z % 2 == 0;
                int gridX = evenRow ? x : x + 1;
                int gridZ = z;
                // Generate piece
                GeneratePiece(blackPiece, gridX, gridZ);
            }
        }


        #endregion
    }

    void GeneratePiece(GameObject piecePrefab, int x, int z)
    {
        //Create instance of piece
        GameObject clone = Instantiate(piecePrefab);
        // Set the parent to be this transform
        clone.transform.SetParent(transform);
        //Get the piece component from clone
        Piece piece = clone.GetComponent<Piece>();
        // Place the piece
        PlacePiece(piece, x, z);
    }

    void PlacePiece(Piece piece, int x, int z)
    {
        // Calculate offset based on coordinate
        float xOffset = x * pieceDiameter + pieceRadius;
        float zOffset = z * pieceDiameter + pieceRadius;
        // Set piece's new grid coordinate
        piece.gridx = x;
        piece.gridz = z;
        // Move piece physically to board coordinate
        piece.transform.position = bottomLeft + Vector3.right * xOffset + Vector3.forward * zOffset;

        // Set pieces array slot
        pieces[x, z] = piece;
    }

    public void PlacePiece(Piece piece, Vector3 position)
    {
        // Translate position to coordinate in array
        float percentX = (position.x + halfBoardX) / boardX;
        float percentZ = (position.z + halfBoardZ) / boardZ;

        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((boardX - 1) * percentX);
        int z = Mathf.RoundToInt((boardZ - 1) * percentZ);

        // Get old piece from that coordinate
        Piece oldPiece = pieces[x, z];

        // If there is an old piece in that slot
        if (oldPiece != null)
        {
            // Swap pieces
            SwapPieces(piece, oldPiece);
        }
        else
        {
            // Place piece
            int oldX = piece.gridx;
            int oldZ = piece.gridz;
            pieces[oldX, oldZ] = null;
            PlacePiece(piece, x, z);
        }
        // Swap pieces
        // else
        // Place piece
    }

    void SwapPieces(Piece pieceA, Piece pieceB)
    {
        // Check if piece a or piece b is null
        if (pieceA == null || pieceB == null)
            return;
        // exit the function

        // Piece a grid spot
        int pAX = pieceA.gridx;
        int pAZ = pieceB.gridz;

        // PieceB grid pos
        int pBX = pieceB.gridx;
        int pBZ = pieceB.gridz;

        // Swap pieces
        PlacePiece(pieceA, pBX, pBZ);
        PlacePiece(pieceB, pAX, pAZ);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
