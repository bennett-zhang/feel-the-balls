using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public const int MIN_BALLS = 1; // The least number of balls possible
    public const int MAX_BALLS = 9; // The most number of balls possible
    public static List<GameObject> balls = new List<GameObject>(); // A list of all the balls
    private static GameObject ballPrefab; // The prefab of a ball

    // Start is called before the first frame update
    private void Start()
    {
        ballPrefab = Resources.Load<GameObject>("Ball");
        Init();
    }

    public static void Init()
    {
        // Destroy each ball
        for (int i = 0; i < balls.Count; i++)
            Destroy(balls[i]);

        // Clear the list of balls
        balls.Clear();
        
        // Generate a random number of balls
        int numBalls = Random.Range(MIN_BALLS, MAX_BALLS + 1);

        for (int i = 0; i < numBalls; i++)
        {
            GameObject ball = Instantiate(ballPrefab); // Create a copy of the ball prefab
            ball.name = "Ball " + (i + 1); // Name and number the ball
            balls.Add(ball); // Add the ball to the list
        }
    }
}
