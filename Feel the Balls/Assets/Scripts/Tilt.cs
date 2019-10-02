using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tilt : MonoBehaviour
{
    private const float MAX_TILT_DELTA = 8; // The greatest number of degrees the tilt can change between updates
    private const float MAX_BALL_REPEL_DIST = 8; // The greatest distance two balls can be to repel each other
    private const float REPEL_FORCE_STRENGTH = 20; // How strongly two balls should repel each other
    private new Rigidbody rigidbody; // This box's rigid body

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        Input.gyro.enabled = true; // Enable gyro controls
        Input.gyro.updateInterval = 0.01f; // How often the gyro should be updated in seconds
    }

    private void FixedUpdate()
    {
        Quaternion rotation = Input.gyro.attitude; // Get the rotation of the gyro

        // Change the rotation to align with the axes of the box
        rotation = new Quaternion(-rotation.x, -rotation.z, -rotation.y, rotation.w);

        // Ensure that the rotation angle doesn't change more than MAX_TILT_DELTA
        rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, MAX_TILT_DELTA));

        // Loop through each pair of balls
        for (int i = 0; i < BallManager.balls.Count - 1; i++)
        {
            for (int j = i + 1; j < BallManager.balls.Count; j++)
            {
                Rigidbody ballRigidbody1 = BallManager.balls[i].GetComponent<Rigidbody>();
                Rigidbody ballRigidbody2 = BallManager.balls[j].GetComponent<Rigidbody>();
                
                // Get the direction from one ball to the other
                Vector3 direction = ballRigidbody1.position - ballRigidbody2.position;

                // If the balls are close enough to repel each other
                if (direction.magnitude <= MAX_BALL_REPEL_DIST)
                {
                    direction = direction.normalized; // Normalize the direction

                    // Apply forces so that the two balls repel each other
                    ballRigidbody1.AddForce(REPEL_FORCE_STRENGTH * direction);
                    ballRigidbody2.AddForce(-REPEL_FORCE_STRENGTH * direction);
                }
            }
        }
    }
}
