using Unity.Entities;
using UnityEngine;

public class LookAtTargetAuthoring : MonoBehaviour
{
    class LookAtTargetAuthoringBaker : Baker<LookAtTargetAuthoring>
    {
        public override void Bake(LookAtTargetAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new LookAtTarget());
        }
    }

}

public struct LookAtTarget : IComponentData
{ 

}