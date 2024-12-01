using Unity.Entities;
using UnityEngine;

class PickupCloseItemsAuthoring : MonoBehaviour
{
    public float range;
    
    class PickupCloseItemsAuthoringBaker : Baker<PickupCloseItemsAuthoring>
    {
        public override void Bake(PickupCloseItemsAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new PickupCloseItems { range = authoring.range });
        }
    }

}


public struct PickupCloseItems : IComponentData
{
    public float nextTime;
    public float cooldown;
    public float range;
}