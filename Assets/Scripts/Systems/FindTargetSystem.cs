using BovineLabs.Core.Spatial;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Timeline;

partial struct FindTargetSystem : ISystem
{
    private EntityQuery enemyQuery;
    private EntityQuery friendlyQuery;

    public void OnCreate(ref SystemState state)
    {
        enemyQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().WithPresent<Enemy>().Build();
        friendlyQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform>().WithPresent<Friendly>().Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var enemies = this.enemyQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out var dependency);
        state.Dependency = dependency;

        var enemyPositions = this.enemyQuery.ToComponentDataListAsync<LocalTransform>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        var friendlys = this.friendlyQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        var friendlyPositions = this.friendlyQuery.ToComponentDataListAsync<LocalTransform>(state.WorldUpdateAllocator, state.Dependency, out dependency);
        state.Dependency = dependency;

        new FindClosestEnemyWithinRange
        {
            enemies = enemies.AsDeferredJobArray(),
            enemyPositions = enemyPositions.AsDeferredJobArray(),
            friendlies = friendlys.AsDeferredJobArray(),
            friendlyPositions = friendlyPositions.AsDeferredJobArray(),
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct FindClosestEnemyWithinRange : IJobEntity
{

    [ReadOnly]
    public float deltaTime;

    [ReadOnly]
    public NativeArray<Entity> friendlies;

    [ReadOnly]
    public NativeArray<Entity> enemies;

    [ReadOnly]
    public NativeArray<LocalTransform> friendlyPositions;

    [ReadOnly]
    public NativeArray<LocalTransform> enemyPositions;

    private void Execute(ref Target target, ref FindTarget findTarget, in LocalToWorld localTransform)
    {
        findTarget.timer -= deltaTime;
        if (findTarget.timer > 0)
            return;
        findTarget.timer = findTarget.timerMax;

        bool targetEnemies = findTarget.targetFaction == Faction.Enemy;

        Entity closestEntity = Entity.Null;
        float range = findTarget.range;
        float sqrRange = range * range;
        float closestSqr = sqrRange;

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
            if (sqrDistance < closestSqr && sqrDistance < sqrRange)
            {
                closestSqr = sqrDistance;
                closestEntity = otherEntity;
            }
        }
        target.target = closestEntity;
    }
}

//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
//[UpdateAfter(typeof(ResetTargetSystem))]
//[UpdateBefore(typeof(ShootSystem))]
//public partial struct FindTargetPhysicallySystem : ISystem
//{


//    [BurstCompile]
//    public void OnUpdate(ref SystemState state)
//    {
//        CollisionWorld world = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
//        FindTargetPhysicallyJob job = new FindTargetPhysicallyJob
//        {
//            deltaTime = SystemAPI.Time.DeltaTime,
//            collisionWorld = world
//        };
//        job.ScheduleParallel();
//    }

//}

//[BurstCompile]
//public partial struct FindTargetPhysicallyJob : IJobEntity
//{
//    public float deltaTime;

//    [ReadOnly]
//    public CollisionWorld collisionWorld;

//    public void Execute(in LocalTransform position, ref FindCloseEntities find, DynamicBuffer<CloseEntity> closeEnemies)
//    {
//        find.timer -= deltaTime;
//        if (find.timer > 0)
//            return;

//        find.timer = find.timerMax;
//        closeEnemies.Clear();

//        NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);
//        /// Return if no collision happens
//        if (!collisionWorld.OverlapSphere(position.Position, find.range, ref hits,
//            new CollisionFilter
//            {
//                BelongsTo = ~0u,
//                CollidesWith = (uint)find.targetFaction,
//                GroupIndex = 0
//            }))
//            return;
//        //NativeParallelHashSet<Entity> foundEntities = new NativeParallelHashSet<Entity>();

//        for (int i = 0; i < hits.Length; i++)
//        {
//            DistanceHit hit = hits[i];
//            Entity e = hit.Entity;
//            //if (foundEntities.Contains(e))
//            //    continue;

//            //foundEntities.Add(e);
//            closeEnemies.Add(new CloseEntity { closeEntity = e, entityPosition = hit.Position });
//        }
//    }

//}

