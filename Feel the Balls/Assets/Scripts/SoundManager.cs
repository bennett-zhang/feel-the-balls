using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const float MAX_VOLUME_DELTA = 0.02f; // The most the volume can change between updates
    private const float PITCH_MULTIPLIER = 0.02f; // Determines much the pitch changes depending on the speed of the balls
    private const float VOLUME_MULTIPLER = 0.02f; // Determines much the volume changes depending on the speed of the balls
    private GameObject rollingSound; // The game object containing the rolling sound effect
    private float prevVolume = 0; // The volume of the previous update

    // Start is called before the first frame update
    private void Start()
    {
        rollingSound = GameObject.Find("Rolling Sound");
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 position = new Vector3(0, 0, 0);
        float speed = 0;

        // Get the sum of the speeds of each ball
        for (int i = 0; i < BallManager.balls.Count; i++)
        {
            Rigidbody ballRigidbody = BallManager.balls[i].GetComponent<Rigidbody>();
            speed += ballRigidbody.velocity.magnitude;
        }

        // Get the sum of the positions of each ball, weighted towards the faster moving balls
        for (int i = 0; i < BallManager.balls.Count; i++)
        {
            Rigidbody ballRigidbody = BallManager.balls[i].GetComponent<Rigidbody>();
            position += ballRigidbody.position * ballRigidbody.velocity.magnitude / speed;
        }

        // Average the position and speed
        position /= BallManager.balls.Count;
        speed /= BallManager.balls.Count;

        // Set the position of the sound object to the weighted average position of the balls
        rollingSound.transform.position = position;

        AudioSource rollingAudioSource = rollingSound.GetComponent<AudioSource>();

        // Increase the pitch based on the average speed of the balls
        rollingAudioSource.pitch = 1 + speed * PITCH_MULTIPLIER;

        float volume = speed * VOLUME_MULTIPLER;

        // Ensure that the volume doesn't change more than MAX_VOLUME_DELTA
        volume = Mathf.MoveTowards(prevVolume, volume, MAX_VOLUME_DELTA);

        rollingAudioSource.volume = volume;
        prevVolume = volume;
    }
}
