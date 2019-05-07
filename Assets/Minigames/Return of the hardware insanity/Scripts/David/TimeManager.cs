using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace HardwareInsanity
{
	public class TimeManager : MonoBehaviour
	{
		public PlayerController m_LinkedPlayer;
		[SerializeField] private Slider m_Slider;
		[SerializeField] private Sprite m_NonVirusBG;
		[SerializeField] private Sprite m_VirusBG;
		[SerializeField] private Image m_Background;


		// Use this for initialization
		void Start()
		{
			m_Slider = GetComponentInChildren<Slider>();
			m_Slider.maxValue = m_LinkedPlayer.m_MaxAllowedDemonTime;
		}

		// Update is called once per frame
		void Update()
		{
			m_Slider.value = m_LinkedPlayer.m_AllowedDemonTime;
			if (m_LinkedPlayer.m_HasVirus)
			{
				m_Background.sprite = m_VirusBG;
			}
			else
			{
				m_Background.sprite = m_NonVirusBG;
			}
		}
	}
}

