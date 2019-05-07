using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HardwareInsanity
{
	/// <summary>
	/// This script will move a camera so that it can see multiple targets at once, similar to the camera in Super Smash Bros.
	/// </summary>
	/// <remarks>
	/// While it was designed for a camera, it does not actually use the camera and this will work with any GameObject. The results may be a bit odd outside the context of a camera, though.
	/// 
	/// From a tutorial by Brackeys: https://www.youtube.com/watch?v=aLpixrPvlB8.
	/// The video shows how to make a camera zoom in by setting its FOV, but I stopped watching before that and made it zoom by moving it along an offset vector.
	/// I later tried doing it the same way as that guy did, but David said the old way looked better and I agreed.
	/// </remarks>
	public class MultipleTargetsCamera : MonoBehaviour
	{
		[SerializeField]
		private Vector3 m_OffsetVector;
		[SerializeField]
		private float m_MinimumDistance = 10, m_DistanceMultiplier = 2;
		[SerializeField]
		private float m_SmoothTime = 0.5f;

		private Vector3 m_Velocity;
		private List<Transform> m_Targets;

		public void SetTargets(IEnumerable<Transform> targets)
		{
			m_Targets = targets.ToList();
		}

		private void Start()
		{
			if (m_Targets == null)
			{
				// Automatically generate targets list if SetTargets was not called after instantiation
				m_Targets = FindObjectOfType<PlayerInstantiation>().m_AlivePlayers.Select(player => player.transform).ToList();
			}
		}

		private void LateUpdate()
		{
			if (m_Targets.Count == 0)
			{
				Debug.LogWarning("MultipleTargetsCamera attached to " + name + " does not have any targets.");
				return;
			}

			m_Targets.RemoveAll(transform => !transform.gameObject.activeSelf); // Remove all transforms from the list that are not active.
			Bounds targetBounds = GetTargetBounds();
			Vector3 newPosition = (m_OffsetVector.normalized * Mathf.Max(targetBounds.extents.magnitude * m_DistanceMultiplier, m_MinimumDistance)) + targetBounds.center;
			transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref m_Velocity, m_SmoothTime);
		}

		private Bounds GetTargetBounds()
		{
			if (m_Targets.Count == 1)
			{
				return new Bounds(m_Targets[0].transform.position, Vector3.zero);
			}
			else
			{
				Bounds bounds = new Bounds(m_Targets[0].transform.position, Vector3.zero);
				for (int i = 1; i < m_Targets.Count; i++)
				{
					bounds.Encapsulate(m_Targets[i].transform.position);
				}
				return bounds;
			}
		}
	}
}
