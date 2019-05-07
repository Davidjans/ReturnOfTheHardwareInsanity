using UnityEngine;
using XBOXParty;

public class MinigameManager : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < GlobalGameManager.Instance.PlayerCount; i++)
		{
			InputManager.Instance.BindAxis("HardwareInsanity_MoveAhead" + i, i, ControllerAxisCode.LeftStickY);
			InputManager.Instance.BindAxis("HardwareInsanity_Strafe" + i, i, ControllerAxisCode.LeftStickX);
			InputManager.Instance.BindButton("HardwareInsanity_Dash" + i, i, ControllerButtonCode.A, ButtonState.OnPress);
		}
	}
}
