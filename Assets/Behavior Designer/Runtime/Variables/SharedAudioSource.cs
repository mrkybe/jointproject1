using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedAudioSource : SharedVariable<AudioSource>
	{
		public static implicit operator SharedAudioSource(AudioSource value) { return new SharedAudioSource { mValue = value }; }
	}
}
