using System;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;

namespace Edibles.Pools
{
	public class EdiblePool : ObjectPool<Edible>
	{
		public bool isInitialized { get; private set; }
		private Edible prefab;
		private Transform parent;
		private Action<Edible> callback;

		public EdiblePool(Edible prefab, Transform parent)
		{
			this.prefab = prefab;
			this.parent = parent;
		}

		protected override Edible CreateInstance()
		{
			var instance = UnityEngine.Object.Instantiate(prefab, parent);
			instance.transform.SetAsLastSibling();
			return instance;
		}	
	}
}