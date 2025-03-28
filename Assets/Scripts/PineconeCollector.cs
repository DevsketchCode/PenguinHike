using UnityEngine;
using System.Collections.Generic; // Required for using Lists

public class PineconeCollector : MonoBehaviour
{
    public string pineconeTag = "Pinecone"; // Tag of the Pinecone objects
    public float initialPineconeOffset = 0.5f; // Vertical Offset for the first pinecone
    public float subsequentPineconeOffset = 0.37f; // Vertical Offset for the third,fourth, etc.

    private List<GameObject> collectedPinecones = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pineconeTag))
        {
            GameObject pinecone = collision.gameObject;
            CollectPinecone(pinecone);
        }
    }

    private void CollectPinecone(GameObject pinecone)
    {
        // Disable the Pinecone's collider to prevent further collisions.
        Collider2D pineconeCollider = pinecone.GetComponent<Collider2D>();
        if (pineconeCollider != null)
        {
            pineconeCollider.enabled = false;
        }

        // Disable the Rigidbody 2D.
        Rigidbody2D pineconeRigidbody = pinecone.GetComponent<Rigidbody2D>();
        if (pineconeRigidbody != null)
        {
            pineconeRigidbody.simulated = false; // Prevents physics simulation
        }

        // Add the pinecone to the collected list.
        collectedPinecones.Add(pinecone);

        // Set the pinecone's parent to this object (the player) to move with it.
        pinecone.transform.SetParent(transform);

        // Calculate the local position relative to the player, for the pinecone.
        float currentOffset = initialPineconeOffset; // Default to initial offset
        if (collectedPinecones.Count >= 2) // If it's the second or later pinecone
        {
            currentOffset = subsequentPineconeOffset;
        }

        // Calculate the local position based on the number of pinecones already collected.
        Vector3 newLocalPosition = Vector3.up * currentOffset * collectedPinecones.Count;
        if (collectedPinecones.Count > 1) //Adjusting the position for the second, and subsequent pinecones.
        {
            newLocalPosition = Vector3.up * initialPineconeOffset;
            for (int i = 1; i < collectedPinecones.Count; i++)
            {
                newLocalPosition += Vector3.up * subsequentPineconeOffset;
            }
        }

        // Set the local position, not the global position.
        pinecone.transform.localPosition = newLocalPosition;

        Debug.Log("Pinecone collected! Total: " + collectedPinecones.Count);
    }
}