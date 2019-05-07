// From https://www.youtube.com/watch?v=ViZto58MgjM

using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Camera m_TheCamera; // The billboard will orient towards this camera. If null, it will use the main camera.

	private void Start()
	{
		if (m_TheCamera == null)
			m_TheCamera = Camera.main;
	}

	private void Update()
	{
		transform.LookAt(transform.position + m_TheCamera.transform.rotation * Vector3.back,
			m_TheCamera.transform.rotation * Vector3.up);
	}
}
