using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateAfter(typeof(FindCloseEntities))]
class AoeDamageAuthoring : MonoBehaviour
{
    public float radius;

    public Faction damageToFaction;
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
            AddComponent(e, new FindCloseEntities()
            {
                targetFaction = authoring.damageToFaction,
                range = authoring.radius,
            });
            AddBuffer<CloseEntity>(e);
        }
    }
}


public struct AoeDamage : IComponentData
{
    public int damage;
}