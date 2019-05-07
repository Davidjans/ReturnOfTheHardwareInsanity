using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBOXParty;

namespace HardwareInsanity
{
    public class PlayerInstantiation : MonoBehaviour
    {
		[SerializeField] private List<GameObject> M_PlayerPrefabs;
		[SerializeField] List<Transform> m_SpawnLocations;
		[SerializeField] List<TimeManager> m_Timers;
		public int m_TotalPlayers;
		public Vector3 m_TimerPosition = new Vector3(0, 50, 0);
		public List<PlayerController> m_PlayerControllers = new List<PlayerController>();
		public List<PlayerController> m_AlivePlayers = new List<PlayerController>();
		private GlobalGameManager m_Instance = GlobalGameManager.Instance;
		public int m_PlayersDead;

        void Start()
        {
			for (int i = 0; i < m_Instance.PlayerCount; i++)
			{
				GameObject player = Instantiate<GameObject>(M_PlayerPrefabs[i]);
				M_PlayerPrefabs[i].transform.position = m_SpawnLocations[i].position;
				m_PlayerControllers.Add(player.GetComponent<PlayerController>());
				m_PlayerControllers[i].m_PlayerInstantiation = this;
				m_Timers[i].m_LinkedPlayer = m_PlayerControllers[i];
				m_Timers[i].gameObject.SetActive(true);
			}
			m_TotalPlayers = m_PlayerControllers.Count;
			m_AlivePlayers = m_PlayerControllers;
        }

	}
}
