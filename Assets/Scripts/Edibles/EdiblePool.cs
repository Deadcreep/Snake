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

		//public void PreloadAsyncWithSubscribe(int count, int threshold, Action<Edible> callback)
		//{
		//	var unit = PreloadAsync(count, threshold);
		//	if (callback != null)
		//	{
		//		this.callback = callback;
		//		unit.Subscribe((x) => Subscribe(count));
		//	}
		//}

		//private void Subscribe(int count)
		//{
		//	for (int i = 0; i < count; i++)
		//	{
		//		var edible = Rent();
		//		edible.OnEated += callback;
		//		Return(edible);
		//	}
		//	isInitialized = true;
		//}


		//protected override void OnClear(Edible instance)
		//{
		//	if (callback != null)
		//		instance.OnEated -= callback;
		//	base.OnClear(instance);
		//}
	}
}