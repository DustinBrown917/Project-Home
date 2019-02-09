using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputListener : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        LogJoystickElementValues(ReInput.controllers.GetController(ControllerType.Joystick, 0));

    }

    void LogJoystickElementValues(Controller joystick)
    {
        // Log Joystick button values
        for (int i = 0; i < joystick.buttonCount; i++)
        {
            if (joystick.Buttons[i].value)
            {
                Debug.Log( joystick.mapTypeString + " " +  joystick.name + "Button " + i + " = " + joystick.Buttons[i].value); // get the current value of the button
            }
            
        }

    }
}
