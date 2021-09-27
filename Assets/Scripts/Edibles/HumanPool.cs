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

		//public void PreloadAsyncWithSubscribe(int count, int threshold, Action<Human> callback)
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
		//		var human = Rent();
		//		Action<Edible> wrapper = x => callback?.Invoke(human);
		//		if (!wrappers.ContainsKey(human))
		//			wrappers.Add(human, wrapper);
		//		human.OnEated += wrapper;
		//		Return(human);
		//	}
		//	isInitialized=true;
		//}

		//protected override void OnClear(Human instance)
		//{
		//	instance.OnEated -= wrappers[instance];

		//	base.OnClear(instance);
		//}
	}
}

