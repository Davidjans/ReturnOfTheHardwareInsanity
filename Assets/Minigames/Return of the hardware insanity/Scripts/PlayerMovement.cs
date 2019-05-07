// Uncomment this to control player 1 with keyboard controls 
//#define NoController

using UnityEngine;
using XBOXParty;

namespace HardwareInsanity
{
	public class PlayerMovement : MonoBehaviour
	{
		public PlayerController Controller { get; private set; }
		
		[SerializeField]
		private float m_MoveSpeed, m_TurnSpeed;
		[SerializeField]
		private float m_DashDistance = 5;
		[SerializeField]
		private bool m_DashAvailable = true;
		[SerializeField]
		private PlayerOverheadIcon m_DashIcon;

		private int m_PlayerID;
		private float m_MoveSpeedModifier = 1;
		private PCAnimationState m_AnimationState;

		private Animator m_Animator;
		private Rigidbody m_RB;

		private void Start()
		{
			Controller = GetComponent<PlayerController>();
			m_PlayerID = Controller.m_PlayerId;

			m_Animator = GetComponent<Animator>();
			m_RB = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			Vector3 movementInput;
			// If the NoController flag at the top of this file is set, then control movement with the keyboard, but only if we're player 1.
#if NoController
			if (m_PlayerID == 0)
			{
				movementInput = new Vector3(
					(Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0),
					0,
					(Input.GetKey(KeyCode.A) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
					);
			}
			else
			{
#endif
			movementInput = new Vector3(
				InputManager.Instance.GetAxis("HardwareInsanity_Strafe" + m_PlayerID),
				0,
				InputManager.Instance.GetAxis("HardwareInsanity_MoveAhead" + m_PlayerID)
				);
#if NoController
			}
#endif
			m_RB.velocity = movementInput.normalized * m_MoveSpeed * m_MoveSpeedModifier; // Set velocity to a vector with the direction of localVelocity and the magnitude of m_MoveSpeed

			if (m_RB.velocity.sqrMagnitude > 0)
			{
				m_AnimationState = PCAnimationState.Moving;
				transform.forward = Vector3.Slerp(transform.forward, movementInput.normalized, m_TurnSpeed * Time.deltaTime);
			}
			else
			{
				m_AnimationState = PCAnimationState.Idle;
			}

			m_Animator.SetInteger("MovementState", (int) m_AnimationState); // This is a float because the blend tree wouldn't let me use an int

			if (m_DashAvailable && InputManager.Instance.GetButton("HardwareInsanity_Dash" + m_PlayerID))
			{
				float distance = m_DashDistance;
				RaycastHit hitInfo;
				if (Physics.Raycast(new Ray(transform.position, transform.forward), out hitInfo, m_DashDistance))
				{
					distance = hitInfo.distance;
				}
				Debug.Log(distance);
				transform.Translate(0, 0, distance, Space.Self);
				m_DashAvailable = false;
				m_DashIcon.SetState(false);
			}
		}

		public void SetSpeedModifier(float modifier)
		{
			m_MoveSpeedModifier = modifier;
		}

		public void GiveDash()
		{
			m_DashAvailable = true;
			m_DashIcon.SetState(true);
		}
	}

	public enum PCAnimationState
	{
		// I'd love to use a bool for this but David says we might expand it later. So here's a 2-state enum.
		Idle, Moving
	}
}
