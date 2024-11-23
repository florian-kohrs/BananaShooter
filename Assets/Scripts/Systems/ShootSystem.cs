using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

[UpdateAfter(typeof(ResetTargetSystem))]
partial struct ShootSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float elapsed = (float)SystemAPI.Time.ElapsedTime;
        ComponentLookup<UnitHealth> healthLookup = SystemAPI.GetComponentLookup<UnitHealth>();
        ShootJob job = new ShootJob { elapsed = elapsed, HealthLookup = healthLookup };
        job.Schedule();
    }
}

[BurstCompile]
public partial struct ShootJob : IJobEntity
{

    public float elapsed;

    public ComponentLookup<UnitHealth> HealthLookup;

    private void Execute(ref ShootWeapon weapon, in Target target)
    {
        if (target.target == Entity.Null)
            return;

        if (weapon.nextShootTime > elapsed)
            return;

        UnitHealth health = HealthLookup[target.target];
        if (health.currentHealth <= 0)
            return;

        weapon.nextShootTime = elapsed + weapon.attackCooldown;

        health.currentHealth -= weapon.damage;
        HealthLookup[target.target] = health;

        weapon.onShootEvent.isTriggered = true;
        //weapon.onShootEvent.position = global.Position /*+ local.ValueRW.Forward() * shoot.ValueRO.muzzleOffset*/;
    }


}