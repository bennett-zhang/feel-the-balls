using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    private const long VIBRATION_DURATION = 35L; // How long each vibration lasts in milliseconds
    private const float MOVEMENT_MULTIPLIER = 2; // Determines how much the vibration strength changes depending on the speed of the balls' movement
    private const float MAX_MOVEMENT_AMPLITUDE = 100; // The maximum vibration strength that the balls' movement will make
    private const float COLLISION_MULTIPLIER = 4; // Determines how much the vibration strength changes depending on the speed of the collision
    private const float MIN_COLLISION_AMPLITUDE = 50; // The minimum vibration strength that a collision will make
    private const float COLLISION_DAMPENING = 0.5f; // Determines how much the collision amplitude should decrease at the end of an update
    private static AndroidJavaObject vibration; // An object allowing access to the Android vibration library
    private static float collisionAmplitude = 0; // The vibration strength made by collisions

    // Start is called before the first frame update
    private void Start()
    {
        vibration = new AndroidJavaObject("com.bennettzhang.vibrationlibrary.Vibration");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibration.Call("setContext", activity);
        vibration.Call("startPluginService");
    }

    // Update is called once per frame
    private void Update()
    {
        float movementAmplitude = 0; // The vibration strength made by the movement of each ball

        // Get the vibration strength from the sum of the speeds of each ball
        for (int i = 0; i < BallManager.balls.Count; i++)
            movementAmplitude += Mathf.Min(MOVEMENT_MULTIPLIER * BallManager.balls[i].GetComponent<Rigidbody>().velocity.magnitude, MAX_MOVEMENT_AMPLITUDE);

        // Average the vibration strength made by the movement of each ball
        movementAmplitude /= BallManager.balls.Count;

        // Create a vibration whose strength is determined by the average movement and collision speeds of each ball
        vibration.Call("createOneShot", VIBRATION_DURATION, movementAmplitude + collisionAmplitude);

        // Decrease the collision amplitude
        collisionAmplitude *= COLLISION_DAMPENING;
    }

    public static void VibrateOnCollision(float speed)
    {
        // Get the vibration strength from the speed of the collision
        collisionAmplitude += COLLISION_MULTIPLIER * speed + MIN_COLLISION_AMPLITUDE;
    }
}
