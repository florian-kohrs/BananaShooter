using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class TargetPositionAuthoring : MonoBehaviour
{
    class TargetPositionAuthoringBaker : Baker<TargetPositionAuthoring>
    {
        public override void Bake(TargetPositionAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TargetPosition
            {
            });
        }
    }

}


public struct TargetPosition : IComponentData
{


    public float3 targetPosition;

}
