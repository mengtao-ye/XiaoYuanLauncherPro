using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"System.Core.dll",
		"System.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Action<UnityEngine.Vector2>
	// System.Action<UnityEngine.Vector3>
	// System.Action<int>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector2>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.Comparer<UnityEngine.Vector2>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<UnityEngine.Vector2>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IComparer<UnityEngine.Vector2>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IList<UnityEngine.Vector2>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector2>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.List<UnityEngine.Vector2>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.Stack.Enumerator<float>
	// System.Collections.Generic.Stack<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector2>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Comparison<UnityEngine.Vector2>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,byte>
	// System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Func<int,byte>
	// System.Func<int,int>
	// System.Func<int,object>
	// System.Func<object,byte>
	// System.Func<object,int>
	// System.Func<object,object>
	// System.Func<object>
	// System.Linq.Buffer<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Buffer<int>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.Iterator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.Iterator<int>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.WhereArrayIterator<int>
	// System.Linq.Enumerable.WhereEnumerableIterator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.WhereEnumerableIterator<int>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.Enumerable.WhereListIterator<int>
	// System.Linq.Enumerable.WhereSelectArrayIterator<int,object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,int>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<int,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,int>
	// System.Linq.Enumerable.WhereSelectListIterator<int,object>
	// System.Linq.Enumerable.WhereSelectListIterator<object,int>
	// System.Linq.EnumerableSorter<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.EnumerableSorter<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.EnumerableSorter<int,int>
	// System.Linq.EnumerableSorter<int>
	// System.Linq.EnumerableSorter<object,int>
	// System.Linq.EnumerableSorter<object>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<int>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<object>
	// System.Linq.OrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>,object>
	// System.Linq.OrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Linq.OrderedEnumerable<int,int>
	// System.Linq.OrderedEnumerable<int>
	// System.Linq.OrderedEnumerable<object,int>
	// System.Linq.OrderedEnumerable<object>
	// System.Nullable<UnityEngine.Rendering.RenderStateBlock>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Predicate<UnityEngine.Vector2>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// UnityEngine.Events.UnityAction<UnityEngine.LocationInfo>
	// UnityEngine.ExposedReference<object>
	// UnityEngine.Playables.ScriptPlayable<object>
	// UnityEngine.Pool.CollectionPool.<>c<object,System.Collections.Generic.KeyValuePair<int,int>>
	// UnityEngine.Pool.CollectionPool.<>c<object,UnityEngine.Vector2>
	// UnityEngine.Pool.CollectionPool.<>c<object,int>
	// UnityEngine.Pool.CollectionPool<object,System.Collections.Generic.KeyValuePair<int,int>>
	// UnityEngine.Pool.CollectionPool<object,UnityEngine.Vector2>
	// UnityEngine.Pool.CollectionPool<object,int>
	// UnityEngine.Pool.GenericPool.<>c<object>
	// UnityEngine.Pool.GenericPool<object>
	// UnityEngine.Pool.IObjectPool<object>
	// UnityEngine.Pool.LinkedPool.LinkedPoolItem<object>
	// UnityEngine.Pool.LinkedPool<object>
	// UnityEngine.Pool.ObjectPool<object>
	// UnityEngine.Pool.PooledObject<object>
	// UnityEngine.Pool.UnsafeGenericPool.<>c<object>
	// UnityEngine.Pool.UnsafeGenericPool<object>
	// }}

	public void RefMethods()
	{
		// object[] System.Array.Empty<object>()
		// bool System.Linq.Enumerable.All<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// bool System.Linq.Enumerable.Any<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// int System.Linq.Enumerable.Count<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// int System.Linq.Enumerable.FirstOrDefault<int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,bool>)
		// int System.Linq.Enumerable.Max<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Linq.IOrderedEnumerable<System.Collections.Generic.KeyValuePair<object,int>> System.Linq.Enumerable.OrderBy<System.Collections.Generic.KeyValuePair<object,int>,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,object>)
		// System.Linq.IOrderedEnumerable<int> System.Linq.Enumerable.OrderByDescending<int,int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,int>)
		// System.Linq.IOrderedEnumerable<object> System.Linq.Enumerable.OrderByDescending<object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Select<object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<int,object>(System.Collections.Generic.IEnumerable<int>,System.Func<int,object>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.Dictionary<object,int> System.Linq.Enumerable.ToDictionary<object,object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,int>)
		// System.Collections.Generic.Dictionary<object,int> System.Linq.Enumerable.ToDictionary<object,object,int>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>,System.Func<object,int>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>> System.Linq.Enumerable.Where<System.Collections.Generic.KeyValuePair<object,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>,System.Func<System.Collections.Generic.KeyValuePair<object,int>,bool>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Where<int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,bool>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Iterator<object>.Select<int>(System.Func<object,int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<int>.Select<object>(System.Func<int,object>)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// object System.Threading.Interlocked.CompareExchange<object>(object&,object,object)
		// UnityEngine.ContactFilter2D UnityEngine.Component.GetComponent<UnityEngine.ContactFilter2D>()
		// UnityEngine.ContactPoint UnityEngine.Component.GetComponent<UnityEngine.ContactPoint>()
		// UnityEngine.ContactPoint2D UnityEngine.Component.GetComponent<UnityEngine.ContactPoint2D>()
		// UnityEngine.Jobs.TransformAccess UnityEngine.Component.GetComponent<UnityEngine.Jobs.TransformAccess>()
		// UnityEngine.Jobs.TransformAccessArray UnityEngine.Component.GetComponent<UnityEngine.Jobs.TransformAccessArray>()
		// UnityEngine.ParticleSystem.ColliderData UnityEngine.Component.GetComponent<UnityEngine.ParticleSystem.ColliderData>()
		// UnityEngine.ParticleSystem.CollisionModule UnityEngine.Component.GetComponent<UnityEngine.ParticleSystem.CollisionModule>()
		// UnityEngine.ParticleSystem.ShapeModule UnityEngine.Component.GetComponent<UnityEngine.ParticleSystem.ShapeModule>()
		// UnityEngine.ParticleSystemJobs.ParticleSystemJobData UnityEngine.Component.GetComponent<UnityEngine.ParticleSystemJobs.ParticleSystemJobData>()
		// UnityEngine.ParticleSystemJobs.ParticleSystemNativeArray3 UnityEngine.Component.GetComponent<UnityEngine.ParticleSystemJobs.ParticleSystemNativeArray3>()
		// UnityEngine.ParticleSystemJobs.ParticleSystemNativeArray4 UnityEngine.Component.GetComponent<UnityEngine.ParticleSystemJobs.ParticleSystemNativeArray4>()
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// System.Void UnityEngine.Playables.PlayableExtensions.Destroy<UnityEngine.Animations.AnimationClipPlayable>(UnityEngine.Animations.AnimationClipPlayable)
		// System.Void UnityEngine.Playables.PlayableExtensions.Destroy<UnityEngine.Audio.AudioClipPlayable>(UnityEngine.Audio.AudioClipPlayable)
		// UnityEngine.Playables.PlayableGraph UnityEngine.Playables.PlayableExtensions.GetGraph<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// System.Void UnityEngine.Playables.PlayableExtensions.Play<UnityEngine.Audio.AudioClipPlayable>(UnityEngine.Audio.AudioClipPlayable)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetDuration<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,double)
		// System.Void UnityEngine.Playables.PlayableExtensions.SetSpeed<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable,double)
		// object UnityEngine.ScriptableObject.CreateInstance<object>()
		// string string.Join<int>(string,System.Collections.Generic.IEnumerable<int>)
		// string string.JoinCore<int>(System.Char*,int,System.Collections.Generic.IEnumerable<int>)
	}
}