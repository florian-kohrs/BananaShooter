using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

[UpdateAfter(typeof(ResetTargetSystem))]
partial struct AttackIfInRangeSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float elapsed = (float)SystemAPI.Time.ElapsedTime;
        ComponentLookup<UnitHealth> healthLookup = SystemAPI.GetComponentLookup<UnitHealth>();
        AttackIfInRangeJob job = new AttackIfInRangeJob { elapsed = elapsed, HealthLookup = healthLookup };
        job.Schedule();
    }
}

[BurstCompile]
public partial struct AttackIfInRangeJob : IJobEntity
{

    public float elapsed;

    public ComponentLookup<UnitHealth> HealthLookup;

    public void Execute(in LocalTransform local, in Target target, in TargetPosition targetPosition, ref AttackIfInRange attacker)
    {
        if (target.target == Entity.Null)
            return;

        if (attacker.nextAttackTime > elapsed)
            return;

        float sqrDist = math.distancesq(targetPosition.targetPosition, local.Position);
        if (sqrDist > math.pow(attacker.range, 2))
            return;

        Entity other = target.target;
        UnitHealth health = HealthLookup[other];

        if (health.currentHealth <= 0)
            return;

        attacker.nextAttackTime = elapsed + attacker.attackCooldown;

        health.currentHealth -= attacker.damage;
        HealthLookup[target.target] = health;

        attacker.onAttackEvent.attackTriggered = true;
    }


}