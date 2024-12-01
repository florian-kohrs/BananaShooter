using Unity.Entities;
using UnityEngine;

class AttackClosestAuthoring : MonoBehaviour
{
    class AttackClosestAuthoringBaker : Baker<AttackClosestAuthoring>
    {
        public override void Bake(AttackClosestAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new AttackClosest());
        }
    }

}

public struct AttackClosest : IComponentData
{
}