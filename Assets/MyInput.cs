using UnityEngine;
using System;
using System.Collections.Generic;

public class MyInput : MonoBehaviour {

	private static MyInput _instance = null;
	private static MyInput instance {
		get {
			if(_instance == null){
				GameObject o = new GameObject();
				o.name = "MyInput";
				_instance = o.AddComponent<MyInput>();
				_instance.Init();
			}
			return _instance;
		}
	}

	public class ButtonData {
		public bool up, down, held;
		public float timeHeld, timeUnheld, timeSinceDown, timeSinceUp;

		public ButtonData() {
			Reset();
		}

		public ButtonData(ButtonData o){
			Copy(o);
		}

		public void Reset(){
			up = down = held = false;
			timeUnheld = timeSinceDown = timeSinceUp = 1000;
		}

		public void Copy(ButtonData o){
			up = o.up;
			down = o.down;
			held = o.held;
			timeHeld = o.timeHeld;
			timeUnheld = o.timeUnheld;
			timeSinceDown = o.timeSinceDown;
			timeSinceUp = o.timeSinceUp;
		}
	}

	// Static methods
	//

	public static ButtonData Button(Button button) {
		return instance.buttons[button];
	}

	public static void DidMove() {
		foreach(Button button in Enum.GetValues(typeof(Button))){
			ButtonData data = instance.buttons[button];
			data.Reset();
		}
	}

	// 

	Dictionary<Button, ButtonData> buttons;

	void Init () {
		buttons = new Dictionary<Button, ButtonData>();
		foreach(Button button in Enum.GetValues(typeof(Button))){
			buttons[button] = new ButtonData();
		}
	}

	void LateUpdate () {

		float dt = Time.deltaTime;

		foreach(Button button in Enum.GetValues(typeof(Button))){

			string buttonName = button.ToString();

			ButtonData prevData = buttons[button];
			ButtonData data = new ButtonData(prevData);

			data.up = Input.GetButtonUp(buttonName);
			data.down = Input.GetButtonDown(buttonName);
			data.held = Input.GetButton(buttonName);

			// Time held
			if(prevData.up){
				data.timeHeld = 0;
			} 
			if(data.held){
				data.timeHeld += dt;
			}

			// Time unheld
			if(prevData.down){
				data.timeUnheld = 0;
			}
			if(!data.held){
				data.timeUnheld += dt;
			}

			// Time since down
			if(prevData.down){
				data.timeSinceDown = 0;
			} else {
				data.timeSinceDown += dt;
			}

			// Time since up
			if(prevData.up){
				data.timeSinceUp = 0;
			} else {
				data.timeSinceUp += dt;
			}

			buttons[button] = data;
		}
	}
}
