using System.Collections.Generic;
using UnityEngine;

namespace HardwareInsanity {
	public class PickupSpawner : MonoBehaviour {
		[SerializeField]
		private List<Object> m_PickupPrefabs;
		[SerializeField]
		private float m_MinTime, m_MaxTime;

		private float m_TimeToNextSpawn;

		private void Start()
		{
			m_TimeToNextSpawn = Random.Range(m_MinTime, m_MaxTime);
		}

		private void Update()
		{
			m_TimeToNextSpawn -= Time.deltaTime;
			if (m_TimeToNextSpawn < 0)
			{
				m_TimeToNextSpawn = Random.Range(m_MinTime, m_MaxTime);
				Instantiate(m_PickupPrefabs[Random.Range(0, m_PickupPrefabs.Count)]);
			}
		}
	}
}
