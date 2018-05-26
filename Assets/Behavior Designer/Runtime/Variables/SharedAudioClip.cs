using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedAudioClip : SharedVariable<AudioClip>
	{
		public static implicit operator SharedAudioClip(AudioClip value) { return new SharedAudioClip { mValue = value }; }
	}
}
