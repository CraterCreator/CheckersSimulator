using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGL;

public class PiecePicker : MonoBehaviour
{
    public float pieceHeight = 5f;
    public float rayDistance = 1000f;
    public LayerMask selectionIgnoreLayer;

    private Piece selectedPiece;
    private CheckerBoard board;

    void Start()
    {
        // Find the checker board in the scene
        board = FindObjectOfType<CheckerBoard>();
    }

    void FixedUpdate()
    {
        CheckSelection();
        MoveSelection();
    }

    void MoveSelection()
    {
        // Check if we have piece selected
        if(selectedPiece != null)
        {
            // Create a new ray from camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GizmosGL.AddLine(ray.origin, ray.origin + ray.direction * rayDistance, 0.1f, 0.1f, Color.yellow, Color.yellow);
            RaycastHit hit;
            // Raycast to only hit objects that arent pieces
            if(Physics.Raycast(ray, out hit, rayDistance, ~selectionIgnoreLayer))
            {
                // Obtain the hit point
                GizmosGL.color = Color.red;
                GizmosGL.AddSphere(hit.point, 0.5f);
                // Move the piece to position
                Vector3 piecePos = hit.point + Vector3.up * pieceHeight;
                selectedPiece.transform.position = piecePos;

            }
        }
    }

    void CheckSelection()
    {
        // Create a ray from camera mouse position to world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GizmosGL.AddLine(ray.origin, ray.origin + ray.direction * rayDistance);

        RaycastHit hit;
        // Check if the player hits the mouse button
        if(Input.GetMouseButtonDown(0))
        {
            // Cast a ray
            if(Physics.Raycast(ray, out hit, rayDistance))
            {
                // Set the selected piece to be the hit object
                selectedPiece = hit.collider.GetComponent<Piece>();
                // If the user did not hit a piece
                if(selectedPiece == null)
                {
                    //Display log mesage
                    Debug.Log("cannot pick up object: " + hit.collider.name);
                }
            }
        }
    }
}
