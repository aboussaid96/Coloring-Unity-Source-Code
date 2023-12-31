﻿using UnityEngine;
using UnityEngine.Events;

namespace PaintIn3D
{
	/// <summary>This component invokes the <b>Action</b> event when this component is enabled.</summary>
	[HelpURL(P3dHelper.HelpUrlPrefix + "P3dActionOnEnable")]
	[AddComponentMenu(P3dHelper.ComponentMenuPrefix + "Action OnEnable")]
	public class P3dActionOnEnable : MonoBehaviour
	{
		/// <summary>The event that will be invoked.</summary>
		public UnityEvent Action { get { if (action == null) action = new UnityEvent(); return action; } } [SerializeField] public UnityEvent action;

		protected virtual void OnEnable()
		{
			if (action != null)
			{
				action.Invoke();
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dActionOnEnable;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dActionOnEnable_Editor : P3dEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("action");
		}
	}
}
#endif