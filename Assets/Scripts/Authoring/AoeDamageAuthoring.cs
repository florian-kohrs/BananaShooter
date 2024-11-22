using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class AoeDamageAuthoring : MonoBehaviour
{
    public float radius;

    public Faction damageToFaction;
    public int damage;

    class AoeDamageAuthoringBaker : Baker<AoeDamageAuthoring>
    {
        public override void Bake(AoeDamageAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.None), new AoeDamage()
            {
                radius = authoring.radius,
                damage = authoring.damage,
                damageToFaction = authoring.damageToFaction,
            });
        }
    }
}


public struct AoeDamage : IComponentData
{
    public float3 position;
    public float radius;

    public Faction damageToFaction;
    public int damage;
}