using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {
	public static readonly int PlayerCount = 4;

	private readonly InputActionMap[] actionMaps = new InputActionMap[PlayerCount];

	private readonly InputData[] inputData = new InputData[PlayerCount];

	private static InputController instance;

	void Awake() {
		if( instance == null ) {
			instance = this;
			DontDestroyOnLoad(gameObject);
			Initialize();
		}
		else {
			Destroy(gameObject);
		}
	}

	private void Initialize() {
		for( int i = 0; i < PlayerCount; ++i ) {
			var devices = FindDevices(i);
			var data = new InputData();

			actionMaps[i] = MapInput("Player_" + i.ToString(), devices, data);
			inputData[i] = data;
		}
	}

	public static void BindAxis(
		string name, InputActionMap actionMap, Action<float> setter,
		IEnumerable<string> positivePaths, IEnumerable<string> negativePaths
	) {
		var action = actionMap.AddAction(name);
		var binding = action.AddCompositeBinding("Axis");

		foreach( var positivePath in positivePaths )
			binding.With("Positive", positivePath);
		foreach( var negativePath in negativePaths )
			binding.With("Negative", negativePath);

		action.performed += context => setter(context.ReadValue<float>());
	}

	public static void BindButton(
		string name, InputActionMap actionMap, Action<bool> setter,
		params string[] paths
	) {
		BindButton(name, actionMap, setter, (IEnumerable<string>)paths);
	}

	public static void BindButton(
		string name, InputActionMap actionMap, Action<bool> setter,
		IEnumerable<string> paths
	) {
		var action = actionMap.AddAction(name);

		foreach( var path in paths )
			action.AddBinding(path);

		action.performed += _ => setter(true);
		action.canceled += _ => setter(false);
	}

	public static void BindVector2(
		string name, InputActionMap actionMap, Action<Vector2> setter,
		IEnumerable<string> upPaths,
		IEnumerable<string> downPaths,
		IEnumerable<string> leftPaths,
		IEnumerable<string> rightPaths
	) {
		var action = actionMap.AddAction(name);
		var binding = action.AddCompositeBinding("2DVector");

		foreach( var upPath in upPaths )
			binding.With("Up", upPath);
		foreach( var downPath in downPaths )
			binding.With("Down", downPath);
		foreach( var leftPath in leftPaths )
			binding.With("Left", leftPath);
		foreach( var rightPath in rightPaths )
			binding.With("Right", rightPath);

		action.performed += context => setter(context.ReadValue<Vector2>());
		action.canceled += context => setter(Vector2.zero);
	}

	private static IList<InputDevice> FindDevices(int playerId) {
		var devices = new List<InputDevice>();

		if( playerId < Gamepad.all.Count )
			devices.Add(Gamepad.all[playerId]);
		else
			Debug.Log($"Gamepad {playerId} is not connected");

		if( playerId == 0 )
			devices.Add(Keyboard.current);
		if( playerId == 1 )
			devices.Add(Mouse.current);

		return devices;
	}

	private static InputActionMap MapInput(string name, IEnumerable<InputDevice> devices, InputData data) {
		var actionMap = new InputActionMap(name);

		BindMovement(actionMap, data);
		BindAim(actionMap, data);
		BindInteraction(actionMap, data);

		actionMap.devices = devices as InputDevice[] ?? devices.ToArray();
		actionMap.Enable();

		return actionMap;
	}

	private static void BindMovement(InputActionMap actionMap, InputData data) {
		var upPaths = new List<string>() { "<Keyboard>/upArrow", "<Gamepad>/leftStick/up" };
		var downPaths = new List<string>() { "<Keyboard>/downArrow", "<Gamepad>/leftStick/down" };
		var leftPaths  = new List<string>() { "<Keyboard>/leftArrow", "<Gamepad>/leftStick/left" };
		var rightPaths = new List<string>() { "<Keyboard>/rightArrow", "<Gamepad>/leftStick/right" };

		BindVector2("Movement", actionMap, v => data.movement = v, upPaths, downPaths, leftPaths, rightPaths);
	}

	private static void BindAim(InputActionMap actionMap, InputData data) {
		var upPaths = new List<string>() { "<Keyboard>/w", "<Gamepad>/rightStick/up" };
		var downPaths = new List<string>() { "<Keyboard>/s", "<Gamepad>/rightStick/down" };
		var leftPaths = new List<string>() { "<Keyboard>/a", "<Gamepad>/rightStick/left" };
		var rightPaths = new List<string>() { "<Keyboard>/d", "<Gamepad>/rightStick/right" };

		BindVector2("Aim", actionMap, v => data.aim = v, upPaths, downPaths, leftPaths, rightPaths);
	}

	private static void BindInteraction(InputActionMap actionMap, InputData data) {
		BindButton("Start", actionMap, v => data.start = v, "<Keyboard>/z", "<Gamepad>/leftTrigger", "<Mouse>/leftButton");
		BindButton("Cancel", actionMap, v => data.cancel = v, "<Keyboard>/x", "<Gamepad>/rightTrigger", "<Mouse>/rightButton");
		BindButton("Jump", actionMap, v => data.jump = v, "<Keyboard>/e"); ///
		BindButton("Fire", actionMap, v => data.fire = v, "<Keyboard>/space"); ///
	}

	public static InputData GetData(int playerId) {
		return instance.inputData[playerId].Clone();
	}

	public static Vector2 GetMovement(int playerId) => instance.inputData[playerId].movement;

	public static bool IsMoving(int playerId, out Vector2 movement, float epsilon = float.Epsilon) {
		movement = GetMovement(playerId);
		return movement.magnitude - epsilon > 0.0f;
	}

	public static Vector2 GetAim(int playerId) => instance.inputData[playerId].aim;

	public static bool IsAiming(int playerId, out Vector2 aim, float epsilon = float.Epsilon) {
		aim = GetAim(playerId);
		return aim.magnitude - epsilon > 0.0f;
	}

	public static bool GetJumpDown(int playerId) => instance.inputData[playerId].jump.IsDown;
	public static bool GetJump(int playerId) => instance.inputData[playerId].jump;
	public static bool GetJumpUp(int playerId) => instance.inputData[playerId].jump.IsUp;

	public static bool GetFireDown(int playerId) => instance.inputData[playerId].fire.IsDown;
	public static bool GetFire(int playerId) => instance.inputData[playerId].fire;
	public static bool GetFireUp(int playerId) => instance.inputData[playerId].fire.IsUp;

	public static bool GetStartDown(int playerId) => instance.inputData[playerId].start.IsDown;
	public static bool GetStart(int playerId) => instance.inputData[playerId].start;
	public static bool GetStartUp(int playerId) => instance.inputData[playerId].start.IsUp;

	public static bool GetCancelDown(int playerId) => instance.inputData[playerId].cancel.IsDown;
	public static bool GetCancel(int playerId) => instance.inputData[playerId].cancel;
	public static bool GetCancelUp(int playerId) => instance.inputData[playerId].cancel.IsUp;
}