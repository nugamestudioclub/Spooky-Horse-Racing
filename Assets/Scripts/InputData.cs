using UnityEngine;

public struct ButtonState {
	public readonly int DownFrame;
	public bool IsDown => DownFrame == Time.frameCount;

	public readonly bool IsPressed;

	public readonly int UpFrame;
	public bool IsUp => UpFrame == Time.frameCount;

	public ButtonState(bool value) {
		IsPressed = value;
		DownFrame = value ? Time.frameCount + 1 : -1;
		UpFrame = value ? -1 : Time.frameCount + 1;
	}

	public static implicit operator ButtonState(bool value) => new ButtonState(value);
	public static implicit operator bool(ButtonState buttonState) => buttonState.IsPressed;
}

public class InputData {
	public Vector2 movement;
	public Vector2 aim;
	public ButtonState jump = new ButtonState();
	public ButtonState fire = new ButtonState();
	public ButtonState start = new ButtonState();
	public ButtonState cancel = new ButtonState();

	public InputData Clone() {
		return new InputData {
			movement = movement,
			aim = aim,
			jump = jump,
			fire = fire,
			start = start,
			cancel = cancel,
		};
	}
}