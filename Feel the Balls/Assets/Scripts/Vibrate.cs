using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    // Two rigid bodies which collided must be more than this distance apart before they can collide again
    private const float COLLISION_DEAD_DISTANCE = 1.3f;

    private new Rigidbody rigidbody; // This ball's rigid body
    private List<Rigidbody> collisionRigidbodyList = new List<Rigidbody>(); // A list of all the rigid bodies this ball has collided with

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the rigid bodies that are COLLISION_DEAD_DISTANCE away from this ball
        Collider[] collisions = Physics.OverlapSphere(rigidbody.position, COLLISION_DEAD_DISTANCE);

        // Loop through all the rigid bodies that this ball has collided with
        for (int i = collisionRigidbodyList.Count - 1; i >= 0; i--)
        {
            bool collisionStillInRange = false;

            // Find out if the rigid body is still within range
            for (int j = 0; j < collisions.Length; j++)
            {
                if (collisionRigidbodyList[i] == collisions[j].attachedRigidbody)
                {
                    collisionStillInRange = true;
                    break;
                }
            }

            // If the rigid body is now out of range, remove it from the list
            if (!collisionStillInRange)
                collisionRigidbodyList.RemoveAt(i);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Whether or not the ball has already collided with this rigid body
        bool collisionExists = false;

        // Loop through the list to see if the rigid body has already been collided with
        for (int i = 0; i < collisionRigidbodyList.Count; i++)
        {
            if (collisionRigidbodyList[i] == collision.rigidbody)
            {
                collisionExists = true;
                break;
            }
        }

        if (!collisionExists)
        {
            // Add the rigid body to the list
            collisionRigidbodyList.Add(collision.rigidbody);

            // Create a vibration for this collision
            VibrationManager.VibrateOnCollision(collision.relativeVelocity.magnitude);
        }
    }
}
