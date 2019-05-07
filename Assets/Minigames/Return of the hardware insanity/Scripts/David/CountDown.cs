using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HardwareInsanity {
	public class CountDown : MonoBehaviour {
		[SerializeField] private float m_StartTimers = 3;
		[SerializeField] private List<Sprite> m_CountDownSprites;
		[SerializeField] private PlayerInstantiation m_PlayerInstantiation;
		[SerializeField] private Image m_CountDownImage;
		private List<PlayerController> m_PlayerControllers;

		// Use this for initialization
		void Start() {
			m_PlayerControllers = m_PlayerInstantiation.m_PlayerControllers;
		}
		// Update is called once per frame
		void Update() {
			m_StartTimers -= Time.deltaTime;
			if (m_StartTimers >= 2 && m_StartTimers <= 3)
			{
				m_CountDownImage.sprite = m_CountDownSprites[2];
			}
			else if (m_StartTimers >= 1 && m_StartTimers <= 2)
			{
				m_CountDownImage.sprite = m_CountDownSprites[1];
			}
			else if (m_StartTimers >= 0 && m_StartTimers <= 1)
			{
				m_CountDownImage.sprite = m_CountDownSprites[0];
			}
			else if (m_StartTimers <= 0)
			{
				m_PlayerControllers[Random.Range(0, m_PlayerControllers.Count)].m_HasVirus = true;
				Destroy(this.gameObject);
			}
		}
	}
}