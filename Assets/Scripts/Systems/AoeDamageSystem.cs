using BovineLabs.Core.Spatial;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct AoeDamageSystem : ISystem
{

    private PositionBuilder positionBuilder;
    private SpatialMap<SpatialPosition> spatialMap;
    private EntityQuery otherUnitQuery;

    public void OnCreate(ref SystemState state)
    {
        otherUnitQuery = SystemAPI.QueryBuilder().WithAll<UnitHealth, LocalTransform, Unit>().Build();
        this.positionBuilder = new PositionBuilder(ref state, otherUnitQuery);

        const int size = 4096;
        const int quantizeStep = 16;

        this.spatialMap = new SpatialMap<SpatialPosition>(quantizeStep, size);
    }

    public void OnDestroy(ref SystemState state)
    {
        this.spatialMap.Dispose();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

    }

}

[BurstCompile]
public partial struct AoeDamageJob : IJobEntity
{

    [ReadOnly]
    public NativeArray<Unit> Units;

    public NativeArray<UnitHealth> healths;

    [ReadOnly]
    public NativeArray<LocalTransform> Positions;

    private void Execute(in AoeDamage damage)
    {
        Faction targetFaction = damage.damageToFaction;
        float sqrRange = damage.radius * damage.radius;
        for (int item = 0; item < Positions.Length; item++)
        {
            if (this.Units[item].faction != targetFaction)
                continue;

            var otherPosition = Positions[item].Position;
            float sqrDistance = math.distancesq(damage.position.xz, otherPosition.xz);
            if (sqrDistance <= sqrRange)
            {

            }

        }
    }

}
