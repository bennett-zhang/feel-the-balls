package com.bennettzhang.vibrationlibrary;

import android.content.Context;
import android.content.Intent;
import android.os.VibrationEffect;
import android.os.Vibrator;

public class Vibration {
    private Context context;
    private Vibrator vibrator;

    public void setContext(Context context) {
        this.context = context;
    }

    // Start the vibrator service
    public void startPluginService() {
        context.startService(new Intent(context, Vibration.class));
        vibrator = (Vibrator) context.getSystemService(Context.VIBRATOR_SERVICE);
    }

    /*
    Vibrate the device for a certain duration and strength.
    Constrains the amplitude between 0 and 255, inclusive.
    */
    public void createOneShot(long milliseconds, int amplitude) {
        if (amplitude > 0) {
            if (amplitude > 255)
                amplitude = 255;

            vibrator.vibrate(VibrationEffect.createOneShot(milliseconds, amplitude));
        }
    }

    /*
    Vibrate the device for a certain duration and strength.
    Casts the amplitude to an integer.
    */
    public void createOneShot(long milliseconds, float amplitude) {
        createOneShot(milliseconds, (int)amplitude);
    }
}