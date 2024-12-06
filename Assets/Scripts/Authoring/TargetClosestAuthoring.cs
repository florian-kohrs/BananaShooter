using Unity.Entities;
using UnityEngine;

class TargetClosestAuthoring : MonoBehaviour
{
    class AttackClosestAuthoringBaker : Baker<TargetClosestAuthoring>
    {
        public override void Bake(TargetClosestAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new TargetClosest());
        }
    }

}

public struct TargetClosest : IComponentData
{
}