using UnityEngine;

namespace HardwareInsanity
{
	public class Pickup : MonoBehaviour
	{
		public PickupType Type = PickupType.Dash;

		private void OnTriggerEnter(Collider other)
		{
			switch (Type)
			{
			case PickupType.Dash:
				PlayerMovement otherPM = other.GetComponent<PlayerMovement>();
				if (otherPM != null)
				{
					otherPM.GiveDash();
					Destroy(gameObject);
				}
				break;
				// TODO more pickup types
			}
		}
	}

	public enum PickupType
	{
		Dash
	}
}
