using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private const float PAUSE_BEFORE_BOX = 3; // How many seconds to pause before revealing the contents of the box
    private const float PAUSE_BEFORE_CANVAS = 5; // How many seconds to pause before showing the canvas
    private Canvas canvas; // The area for the UI elements to be drawn
    private Slider slider; // The slider to receive the user's guess for the number of balls
    private TextMeshProUGUI sliderLabel; // The numerical label for the slider
    private Button submitButton; // The button for the user to submit their guess
    private TextMeshProUGUI results; // Displays whether or not the user's guess was correct

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.minValue = BallManager.MIN_BALLS;
        slider.maxValue = BallManager.MAX_BALLS;

        sliderLabel = GameObject.Find("Slider/Handle Slide Area/Handle/Label").GetComponent<TextMeshProUGUI>();
        sliderLabel.text = "" + slider.value;

        submitButton = GameObject.Find("Submit Button").GetComponent<Button>();
        results = GameObject.Find("Results").GetComponent<TextMeshProUGUI>();
    }

    public void OnSliderChange(float value)
    {
        if (sliderLabel != null)
            sliderLabel.text = "" + value; // Update the slider label to display the value of the slider
    }

    public void OnSubmit()
    {
        // Disable the UI elements
        slider.enabled = false;
        submitButton.enabled = false;

        int numBalls = BallManager.balls.Count;

        // Display whether or not the user's guess was correct
        if (slider.value == numBalls)
            results.text = "<color=green>Great job!</color>";
        else
            results.text = "<color=red>Wow, you're really bad at this!</color>";

        results.text += "\nThere" + (numBalls == 1 ? "'s " : " are ");

        if (slider.value != numBalls)
            results.text += "actually ";

        // Display the actual number of balls in the box
        results.text += numBalls + " ball" + (numBalls == 1 ? "!" : "s!");

        StartCoroutine(ShowBox());
    }

    private IEnumerator ShowBox()
    {
        // Pause before revealing the contents of the box
        yield return new WaitForSeconds(PAUSE_BEFORE_BOX);

        canvas.enabled = false; // Disable the canvas
        StartCoroutine(ShowCanvas());
    }

    private IEnumerator ShowCanvas()
    {
        // Pause before showing the canvas
        yield return new WaitForSeconds(PAUSE_BEFORE_CANVAS);

        // Enable and reset the UI elements
        canvas.enabled = true;
        slider.enabled = true;
        slider.value = slider.minValue;
        submitButton.enabled = true;
        results.text = "";
        
        BallManager.Init(); // Generate a random number of balls
    }
}
