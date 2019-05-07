using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XBOXParty;

namespace HardwareInsanity
{
	public class Winchecker : MonoBehaviour
	{
		[SerializeField] private PlayerInstantiation m_PlayerInstantiator;
		[SerializeField] List<Text> m_Text;
		[SerializeField] List<Image> m_RankingImages;
		[SerializeField] List<Sprite> m_RankingSprites;
		[SerializeField] List<Sprite> m_WinSprites;
		[SerializeField] private GameObject m_RankingCanvas;
		[SerializeField] private Image m_WinBot;
		private List<PlayerController> m_AllPlayers = new List<PlayerController>();
		private List<int> m_Rankings;
		private bool m_AllPlayersGone = false;
		private float m_EndGameTimer = 10;
		void Start()
		{
			m_AllPlayers = m_PlayerInstantiator.m_PlayerControllers;
			m_Rankings = new List<int>();
		}

		// Update is called once per frame
		void Update()
		{
			if (m_PlayerInstantiator.m_TotalPlayers == m_Rankings.Count && m_AllPlayersGone == false)
			{
				m_Rankings.Reverse();
				m_AllPlayersGone = true;
			}
			if (m_AllPlayersGone == true)
			{
				m_RankingCanvas.SetActive(true);
				for (int i = 0; i < m_PlayerInstantiator.m_TotalPlayers; i++)
				{
					m_Text[i].gameObject.SetActive(true);
					m_RankingImages[i].sprite = m_RankingSprites[m_Rankings[i]];
				}
				m_EndGameTimer = m_EndGameTimer - Time.deltaTime;
				m_WinBot.sprite = m_WinSprites[m_Rankings[0]];

				if (m_EndGameTimer <= 0)
				{
					GlobalGameManager.Instance.SubmitGameResults(m_Rankings);
				}
			}
		}

		public void AddToRankings(int PlayerId)
		{
			m_Rankings.Add(PlayerId);
		}
	}
}
