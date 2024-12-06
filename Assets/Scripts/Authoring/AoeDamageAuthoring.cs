using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class AoeDamageAuthoring : MonoBehaviour
{
    public int damage;

    class AoeDamageAuthoringBaker : Baker<AoeDamageAuthoring>
    {
        public override void Bake(AoeDamageAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new AoeDamage()
            {
                damage = authoring.damage,
            });
            AddBuffer<CloseEntity>(e);
        }
    }
}


public struct AoeDamage : IComponentData
{
    public int damage;
}