using Edibles;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx.Toolkit;
using UnityEngine;
using UniRx;
namespace Edibles
{
	public class HumanPool : ObjectPool<Human>
	{
		public bool isInitialized { get; private set; }
		private Human prefab;
		private Transform parent;
		private Action<Human> callback;
		private Dictionary<Human, Action<Edible>> wrappers = new Dictionary<Human, Action<Edible>>();
		public HumanPool(Human prefab, Transform parent)
		{
			this.prefab = prefab;
			this.parent = parent;
		}

		protected override Human CreateInstance()
		{
			var instance = UnityEngine.Object.Instantiate(prefab, parent);
			instance.transform.SetAsLastSibling();
			return instance;
		}		
	}
}

