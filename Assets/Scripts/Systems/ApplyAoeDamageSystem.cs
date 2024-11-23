using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

[UpdateAfter(typeof(FindCloseEntitiesSystem))]
partial struct ApplyAoeDamageSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<UnitHealth> healthLookup = SystemAPI.GetComponentLookup<UnitHealth>();

        AoeDamageJob job = new AoeDamageJob
        { 
            elapsed = (float)SystemAPI.Time.ElapsedTime, 
            HealthLookup = healthLookup 
        };
        job.Schedule();
    }

}
public partial struct AoeDamageJob : IJobEntity
{
    public float elapsed;

    public ComponentLookup<UnitHealth> HealthLookup;

    private void Execute(DynamicBuffer<CloseEntity> entitiesInRange, in AoeDamage damage)
    {
        for (int i = 0; i < entitiesInRange.Length; i++)
        {
            CloseEntity e = entitiesInRange[i];
            Entity other = e.closeEntity;

            UnitHealth h = HealthLookup[other];
            if (h.currentHealth <= 0)
                continue;

            h.currentHealth -= damage.damage;
            HealthLookup[other] = h;
        }
    }
}