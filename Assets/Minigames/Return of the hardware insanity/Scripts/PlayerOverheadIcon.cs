using UnityEngine;

public class PlayerOverheadIcon : MonoBehaviour {
	[SerializeField]
	// If either of these is null, they will simply be ignored with no warning given.
	private Sprite m_EnabledIcon, m_DisabledIcon;
	[SerializeField]
	private bool m_InitialState = false;
	
	private SpriteRenderer m_Renderer;

	private bool m_EnabledState;

	private void Start() {
		m_Renderer = GetComponent<SpriteRenderer>();
		m_EnabledState = m_InitialState;
		if (m_InitialState)
		{
			m_Renderer.sprite = m_EnabledIcon;
		}
		else
		{
			m_Renderer.sprite = m_DisabledIcon;
		}
	}

	// Orient the camera after all movement is completed this frame to avoid jittering
	// From https://wiki.unity3d.com/index.php/CameraFacingBillboard
	void LateUpdate()
	{
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
			Camera.main.transform.rotation * Vector3.up);
	}

	public void SetState(bool state)
	{
		m_EnabledState = state;
		if (state)
		{
			m_Renderer.sprite = m_EnabledIcon;
		}
		else
		{
			m_Renderer.sprite = m_DisabledIcon;
		}
	}
}
