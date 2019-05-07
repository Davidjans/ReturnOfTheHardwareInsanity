using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XBOXParty;

namespace HardwareInsanity
{
	public class PlayerController : MonoBehaviour
	{
		public PlayerMovement Movement { get; private set; }

		public int m_PlayerId;

		public bool m_HasVirus = false;
		public bool m_RecentlySwitched = false;
		public float m_MaxAllowedDemonTime;
		public float m_AllowedDemonTime;
		public List<Material> m_VirusMaterials;
		public List<Material> m_OriginalMaterials;
		public List<MeshRenderer> m_BodyParts;
		public GameObject m_VirusChange;

		[SerializeField]
		private GameObject m_SmokeCloudPrefab;

		private float m_SwitchTimer = 1;
		private float m_SmokeTimer = 0;
		private float m_SmokeTimerMax = 0.125f;

		private Animator m_Animator;

		private List<PlayerController> m_OPC = new List<PlayerController>();
		private float m_SmallestDistance;
		private int m_SPC;

		public PlayerInstantiation m_PlayerInstantiation;

		private void Start()
		{
			m_OPC = GameObject.Find("PlayerInstantiation").GetComponent<PlayerInstantiation>().m_PlayerControllers;
			m_AllowedDemonTime = m_MaxAllowedDemonTime;
			m_Animator = GetComponent<Animator>();
			Movement = GetComponent<PlayerMovement>();
			if (m_HasVirus)
			{
				for (int i = 0; i < m_BodyParts.Count; i++)
				{
					m_BodyParts[i].material = m_VirusMaterials[i];
				}
			}
			else if (!m_HasVirus)
			{
				for (int i = 0; i < m_BodyParts.Count; i++)
				{
					m_BodyParts[i].material = m_OriginalMaterials[i];
				}
			}
		}

		void Update()
		{
			if (m_PlayerInstantiation.m_AlivePlayers.Count == 1)
			{
				GameObject.Find("PlayerInstantiation").GetComponent<Winchecker>().AddToRankings(m_PlayerId);
				gameObject.SetActive(false);
			}
			if (m_RecentlySwitched)
			{
				CurrentlySwitching();
			}
			if (m_HasVirus && !m_RecentlySwitched)
			{
				m_AllowedDemonTime -= Time.deltaTime;
			}
			if (m_HasVirus)
			{
				m_SmokeTimer -= Time.deltaTime;
				if (m_SmokeTimer < 0)
				{
					m_SmokeTimer = m_SmokeTimerMax;
					// Spawn smoke cloud
					Instantiate(m_SmokeCloudPrefab, transform.position, transform.rotation);
				}
			}

			if (m_AllowedDemonTime <= 0)
			{
				m_SmallestDistance = float.MaxValue;
				for (int i = 0; i < m_OPC.Count; i++)
				{
					if (m_OPC[i] != null)
					{
						float distance = Vector3.Distance(m_OPC[i].transform.position, transform.position);
						if (distance < m_SmallestDistance && m_OPC[i].m_PlayerId != m_PlayerId)
						{
							m_SmallestDistance = distance;
							m_SPC = i;
						}
					}
				}
				m_OPC[m_SPC].m_HasVirus = true;
				m_OPC[m_SPC].m_RecentlySwitched = true;
				GameObject infection = Instantiate(m_VirusChange);
				infection.transform.position = new Vector3(m_OPC[m_SPC].transform.position.x, m_OPC[m_SPC].transform.position.y + 2, m_OPC[m_SPC].transform.position.z);
				for (int i = 0; i < m_OPC[m_SPC].m_BodyParts.Count; i++)
				{
					m_OPC[m_SPC].m_BodyParts[i].material = m_OPC[m_SPC].m_VirusMaterials[i];
				}
				GameObject.Find("PlayerInstantiation").GetComponent<Winchecker>().AddToRankings(m_PlayerId);
				m_PlayerInstantiation.m_AlivePlayers.Remove(this);
				gameObject.SetActive(false);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			PlayerController touchedPlayer = collision.gameObject.GetComponent<PlayerController>();
			if (touchedPlayer != null)
			{
				if (touchedPlayer.m_HasVirus && !touchedPlayer.m_RecentlySwitched)
				{
					m_HasVirus = true;
					Movement.SetSpeedModifier(1);
					touchedPlayer.Movement.SetSpeedModifier(1);
					touchedPlayer.m_HasVirus = false;
					m_RecentlySwitched = true;
					GameObject infection = Instantiate(m_VirusChange);
					infection.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
					GetComponentInChildren<Renderer>().material = m_VirusMaterials[0];
					
					for (int i = 0; i < touchedPlayer.m_BodyParts.Count; i++)
					{
						touchedPlayer.m_BodyParts[i].material = touchedPlayer.m_OriginalMaterials[i];
					}
					for (int i = 0; i < m_BodyParts.Count; i++)
					{
						m_BodyParts[i].material = m_VirusMaterials[i];
					}
				}
			}
		}

		private void CurrentlySwitching()
		{
			m_SwitchTimer -= Time.deltaTime;

			if (m_SwitchTimer <= 0)
			{
				m_RecentlySwitched = false;
				m_SwitchTimer = 1;
			}
		}
	}
}
