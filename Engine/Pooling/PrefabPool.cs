using System.Collections;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Pooling/Prefab", fileName = "Prefab Pool")]
    public class PrefabPool : Pool<GameObject>
    {
        [HorizontalGroup("Settings", 0.5f)]
        [AssetsOnly, HideLabel, DisableInPlayMode]
        public GameObject prefab;

        [HorizontalGroup("lazy")]
        [ToggleLeft, LabelWidth(60), DisableInPlayMode]
        public bool lazyLoad;
        [ShowIf("lazyLoad"), HorizontalGroup("lazy")]
        [LabelText("Batch Size"), LabelWidth(65), DisableInPlayMode]
        public int lazyLoadBatchSize;
        [ShowIf("lazyLoad"), HorizontalGroup("lazy")]
        [LabelText("Delay"), LabelWidth(50), DisableInPlayMode]
        public float lazyLoadBatchDelay;
        
        protected override void OnPrewarm()
        {
            prefab.Disable();
            if (lazyLoad)
            {
                Game.StartRoutine(DoPrewarm());
            }
            else
            {
                base.OnPrewarm();
            }
        }

        private IEnumerator DoPrewarm()
        {
            var batchCount = math.ceil(size / (float)lazyLoadBatchSize);
            for (int i = 0; i < batchCount; i++)
            {
                Enlarge(lazyLoadBatchSize);
                yield return new WaitForSeconds(lazyLoadBatchDelay);
            }
        }

        protected override GameObject BeforeRequest(GameObject member)
        {
            if (member == null) member = Create();
            member.Enable();
            return member;
        }

        protected override void AfterReturn(GameObject member)
        {
            member.Disable();
            Game.DelayedCall(() =>
            {
                member.transform.SetParent(null);
                member.transform.position = Vector3.zero;
                member.transform.rotation = Quaternion.identity;
                member.transform.localScale = Vector3.one;
            });
        }

        protected override GameObject Create()
        {
            var member = Instantiate(prefab);
            
            var poolable = member.GetComponent<Poolable>();
            if (poolable != null) poolable.pool = this;

            return member;
        }
    }
}