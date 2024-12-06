using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class AttackIfInRangeAuthoring : MonoBehaviour
{

    public float attackCooldown = 0.5f;
    public float range = 10;
    public int damage = 1;

    class AttackInRangeAuthoring : Baker<AttackIfInRangeAuthoring>
    {
        public override void Bake(AttackIfInRangeAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new AttackIfInRange
            {
                attackCooldown = authoring.attackCooldown,
                damage = authoring.damage,
                range = authoring.range,
            }) ;
        }
    }

}

public struct AttackIfInRange : IComponentData
{
    public float range;
    public int damage;

    public float attackCooldown;
    public float nextAttackTime;

    public OnAttackEvent onAttackEvent;

    public struct OnAttackEvent
    {
        public bool attackTriggered;
    }
}
