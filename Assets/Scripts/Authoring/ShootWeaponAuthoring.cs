using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ShootWeaponAuthoring : MonoBehaviour
{

    public float attackCooldown = 0.5f;
    public float range = 10;
    public int damage = 1;

    class ShootAttackAuthoringBaker : Baker<ShootWeaponAuthoring>
    {
        public override void Bake(ShootWeaponAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ShootWeapon
            {
                attackCooldown = authoring.attackCooldown,
                damage = authoring.damage,
                range = authoring.range,
            }) ;
        }
    }

}

public struct ShootWeapon : IComponentData
{
    public float range;
    public int damage;

    public float attackCooldown;
    public float nextShootTime;

    public OnShootEvent onShootEvent;

    public struct OnShootEvent
    {
        public float3 position;
        public bool isTriggered;
    }
}
