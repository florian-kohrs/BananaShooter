//using BovineLabs.Core.Extensions;
//using BovineLabs.Core.Spatial;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEditor.Search;

//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
//[UpdateAfter(typeof(ResetTargetSystem))]
//[UpdateBefore(typeof(ShootSystem))]
//public partial struct TestSystem : ISystem
//{
//    private PositionBuilder positionBuilder;
//    private SpatialMap<SpatialPosition> spatialMap;
//    private EntityQuery otherUnitQuery;

//    public void OnCreate(ref SystemState state)
//    {
//        otherUnitQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, Unit>().Build();
//        this.positionBuilder = new PositionBuilder(ref state, otherUnitQuery);

//        const int size = 4096;
//        const int quantizeStep = 16;

//        this.spatialMap = new SpatialMap<SpatialPosition>(quantizeStep, size);
//    }

//    public void OnDestroy(ref SystemState state)
//    {
//        this.spatialMap.Dispose();
//    }

//    [BurstCompile]
//    public void OnUpdate(ref SystemState state)
//    {
//        state.Dependency = this.positionBuilder.Gather(ref state, state.Dependency, out NativeArray<SpatialPosition> positions);
//        state.Dependency = this.spatialMap.Build(positions, state.Dependency);
//        //state.Dependency = 
//        // The entities in this will match the indices from the spatial map
//        var entities = this.otherUnitQuery.ToEntityListAsync(state.WorldUpdateAllocator, state.Dependency, out var dependency);
//        state.Dependency = dependency;

//        var units = this.otherUnitQuery.ToComponentDataListAsync<Unit>(state.WorldUpdateAllocator, state.Dependency, out dependency);
//        state.Dependency = dependency;

//        new FindClosestWithinRange
//        {
//            Entities = entities.AsDeferredJobArray(),
//            Units = units.AsDeferredJobArray(),
//            Positions = positions,
//            SpatialMap = this.spatialMap.AsReadOnly(),
//            deltaTime = SystemAPI.Time.DeltaTime,
//        }.ScheduleParallel();
//    }

//    // Find and store all other entities within 10 of each other
//    [BurstCompile]
//    private partial struct FindClosestWithinRange : IJobEntity
//    {

//        [ReadOnly]
//        public float deltaTime;

//        [ReadOnly]
//        public NativeArray<Entity> Entities;


//        [ReadOnly]
//        public NativeArray<Unit> Units;

//        [ReadOnly]
//        public NativeArray<SpatialPosition> Positions;

//        [ReadOnly]
//        public SpatialMap.ReadOnly SpatialMap;

//        private void Execute(Entity entity, ref Target target, ref FindTarget findTarget, in LocalTransform localTransform)
//        {
//            findTarget.timer -= deltaTime;
//            if (findTarget.timer > 0)
//                return;
//            findTarget.timer = findTarget.timerMax;

//            float range = findTarget.range;
//            float sqrRange = range * range;

//            // Find the min and max boxes
//            var min = this.SpatialMap.Quantized(localTransform.Position.xz - range);
//            var max = this.SpatialMap.Quantized(localTransform.Position.xz + range);
//            Faction targetFaction = findTarget.targetFaction;
//            float closestSqr = sqrRange;
//            Entity closestEntity = Entity.Null;
//            for (var j = min.y; j <= max.y; j++)
//            {
//                for (var i = min.x; i <= max.x; i++)
//                {
//                    var hash = this.SpatialMap.Hash(new int2(i, j));

//                    if (!this.SpatialMap.Map.TryGetFirstValue(hash, out int item, out var it))
//                    {
//                        continue;
//                    }

//                    do
//                    {
//                        var otherEntity = this.Entities[item];

//                        // Don't add ourselves
//                        if (otherEntity.Equals(entity))
//                            continue;

//                        // Don't attack friendlies
//                        if (this.Units[item].faction != targetFaction)
//                            continue;

//                        var otherPosition = this.Positions[item].Position;
//                        float sqrDistance = math.distancesq(localTransform.Position.xz, otherPosition.xz);
//                        if (sqrDistance < closestSqr && sqrDistance < sqrRange)
//                        {
//                            closestSqr = sqrDistance;
//                            closestEntity = otherEntity;
//                        }
//                    }
//                    while (this.SpatialMap.Map.TryGetNextValue(out item, ref it));
//                }
//            }
//            target.target = closestEntity;
//        }
//    }

//}