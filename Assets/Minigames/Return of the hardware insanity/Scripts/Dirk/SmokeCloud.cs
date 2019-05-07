using System.Collections.Generic;
using UnityEngine;

namespace HardwareInsanity
{
	[RequireComponent(typeof(ParticleSystem))]
	public class SmokeCloud : MonoBehaviour
	{
		/// Set this to -1 to not start automatically
		public float m_StopAfterSeconds = 3f;

		private float m_EmissionTimer;
		private List<PlayerMovement> m_CollidingPlayers; // Contains players currently in the trigger, so we can apply and undo debuffs if we start/stop after they entered the trigger.

		private ParticleSystem m_PS;

		
		private void Awake()
		{
			m_PS = GetComponent<ParticleSystem>();
			m_CollidingPlayers = new List<PlayerMovement>(4);
		}

		private void Start()
		{
			StartEmitting(m_StopAfterSeconds);
		}

		private void FixedUpdate()
		{
			/*/ remove the * to test smoke
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				StartEmitting(2);
			}//*/

			// This needs to be in FixedUpdate because it needs to be in sync with OnTriggerEnter/Exit/Stay functions

			if (m_EmissionTimer != -1) // -1 means it should emit forever.
			{
				m_EmissionTimer -= Time.deltaTime;
				if (m_EmissionTimer < 0)
				{
					m_EmissionTimer = 0;
					// Disable emission
					m_PS.Stop();

					// Undo slow modifier for all players currently in the trigger. They won't get reverted by OnTriggerExit.
					foreach (PlayerMovement otherPM in m_CollidingPlayers)
					{
						otherPM.StartCoroutine(Util.RunAfterSeconds(2f, () => { otherPM.SetSpeedModifier(1f); }));
					}
					Destroy(gameObject);
				}
			}
		}
		
		/// <param name="time">-1 to emit indefinitely</param>
		public void StartEmitting(float time)
		{
			// Enable emission
			m_PS.Play();
			m_EmissionTimer = time;

			// Apply debuff to all players in range.
			foreach (PlayerMovement otherPM in m_CollidingPlayers)
			{
				if (!otherPM.Controller.m_HasVirus)
				{
					otherPM.SetSpeedModifier(0.5f);
				}
			}
		}

		public void StopEmitting(bool destroy)
		{
			m_PS.Stop();
			foreach (PlayerMovement otherPM in m_CollidingPlayers)
			{
				otherPM.StartCoroutine(Util.RunAfterSeconds(2f, () => { otherPM.SetSpeedModifier(1f); }));
			}
			if (destroy)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			PlayerMovement otherPM = other.GetComponent<PlayerMovement>();
			if (otherPM != null)
			{
				// Add player to list so we can apply the debuff when we start emitting, and remove it we stop emitting.
				if (!m_CollidingPlayers.Contains(otherPM))
				{
					m_CollidingPlayers.Add(otherPM);
				}

				// If we're currently emitting, apply the debuff immediately. Still keep the player in the list so we can remove the debuff if we stop before they leave.
				if (m_EmissionTimer > 0 || m_EmissionTimer == -1)
				{
					if (!otherPM.Controller.m_HasVirus)
					{
						otherPM.SetSpeedModifier(0.5f);
					}
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			PlayerMovement otherPM = other.GetComponent<PlayerMovement>();
			if (otherPM != null)
			{
				// Remove the player from the list.
				if (m_CollidingPlayers.Contains(otherPM))
				{
					m_CollidingPlayers.Remove(otherPM);
				}

				// If we are currently emitting, stop the debuff after two seconds.
				if (m_EmissionTimer > 0 || m_EmissionTimer == -1)
				{
					otherPM.StartCoroutine(Util.RunAfterSeconds(2f, () => { otherPM.SetSpeedModifier(1f); }));
				}
			}
		}
	}
}
