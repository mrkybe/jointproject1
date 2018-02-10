using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//<summary>
// DetectController is used to detect if there is a controller
//</summary>
public class DetectController : MonoBehaviour {

    public Move moveScript;
	//<summary>
	// Every frame, check if there is input from the mouse, or from the contoller. Turn the controller flag on or off
	// corresponding to what input is detected.
	//</summary>
	void Update () {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            moveScript.controller = false;
        if (Input.GetAxisRaw("HorizontalR") != 0 || Input.GetAxisRaw("VerticalR") != 0)
            moveScript.controller = true;
	}
}
