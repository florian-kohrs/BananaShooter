using BovineLabs.Core.Spatial;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct FindCloseEntitiesSystem : ISystem
{

    private EntityQuery nonFriendlyQuery;
    private EntityQuery nonEnemyQuery;

    public void OnCreate(ref SystemState state)
    {
        nonFriendlyQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, UnitHealth>().WithAbsent<Friendly>().Build();
        nonEnemyQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, UnitHealth>().WithAbsent<Enemy>().Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var enemies = this.nonFriendlyQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out var dependency);
        state.Dependency = dependency;

        var enemyPositions = this.nonFriendlyQuery.ToComponentDataListAsync<LocalTransform>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        var enemyHealths = this.nonFriendlyQuery.ToComponentDataListAsync<UnitHealth>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;


        var friendlys = this.nonEnemyQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        var friendlyPositions = this.nonEnemyQuery.ToComponentDataListAsync<LocalTransform>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        var friendlyHealths = this.nonEnemyQuery.ToComponentDataListAsync<UnitHealth>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        new FindCloseEntitiesJob
        {
            enemies = enemies.AsDeferredJobArray(),
            enemyPositions = enemyPositions.AsDeferredJobArray(),
            enemyHealth = enemyHealths.AsDeferredJobArray(),
            friendlies = friendlys.AsDeferredJobArray(),
            friendlyPositions = friendlyPositions.AsDeferredJobArray(),
            friendlyHealths = friendlyHealths.AsDeferredJobArray(),
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }

}

[BurstCompile]
public partial struct FindCloseEntitiesJob : IJobEntity
{
    [ReadOnly]
    public float deltaTime;

    [ReadOnly]
    public NativeArray<Entity> friendlies;

    [ReadOnly]
    public NativeArray<Entity> enemies;

    [ReadOnly]
    public NativeArray<UnitHealth> friendlyHealths;

    [ReadOnly]
    public NativeArray<UnitHealth> enemyHealth;

    [ReadOnly]
    public NativeArray<LocalTransform> friendlyPositions;

    [ReadOnly]
    public NativeArray<LocalTransform> enemyPositions;

    private void Execute(DynamicBuffer<CloseEntity> closeEntity, ref FindCloseEntities findTarget, in LocalToWorld localTransform)
    {
        closeEntity.Clear();

        findTarget.timer -= deltaTime;
        if (findTarget.timer > 0)
            return;

        findTarget.timer = findTarget.timerMax;

        bool targetEnemies = findTarget.targetFaction == Faction.Enemy;

        float range = findTarget.range;
        float sqrRange = range * range;

        int length;
        if (targetEnemies)
            length = enemies.Length;
        else
            length = friendlies.Length;

        for (int item = 0; item < length; item++)
        {
            Entity otherEntity;
            if (targetEnemies)
                otherEntity = this.enemies[item];
            else
                otherEntity = this.friendlies[item];

            float3 otherPosition;
            if (targetEnemies)
                otherPosition = this.enemyPositions[item].Position;
            else
                otherPosition = this.friendlyPositions[item].Position;

            float sqrDistance = math.distancesq(localTransform.Position.xz, otherPosition.xz);
            if (sqrDistance < sqrRange)
            {
                UnitHealth otherHealth;
                if (targetEnemies)
                    otherHealth = this.enemyHealth[item];
                else
                    otherHealth = this.friendlyHealths[item];

                closeEntity.Add(
                    new CloseEntity 
                    { 
                        closeEntity = otherEntity,
                        entityPosition = otherPosition,
                        health = otherHealth 
                    });
            }
        }
    }
}