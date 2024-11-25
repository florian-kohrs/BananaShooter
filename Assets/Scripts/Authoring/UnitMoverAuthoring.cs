using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UnitMoverAuthoring : MonoBehaviour
{

    public float speed;

    public class Baker : Baker<UnitMoverAuthoring>
    {
        public override void Bake(UnitMoverAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new UnitMover 
            { 
                speed = authoring.speed, 
            });
        }
    }

}



public struct UnitMover : IComponentData
{

    public float speed;

    public float3 targetPosition;

}
