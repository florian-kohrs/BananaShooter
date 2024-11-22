//using Unity.Burst;
//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Transforms;

//[UpdateAfter(typeof(ShootSystem))]
//partial struct ShootLightSystem : ISystem
//{

//    [BurstCompile]
//    public void OnUpdate(ref SystemState state)
//    {
//        //EntitiesReferences references = SystemAPI.GetSingleton<EntitiesReferences>();
//        //EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

//        foreach ((
//            RefRO<ShootWeapon> shoot, 
//            RefRW<LocalTransform> transform) 
//            in SystemAPI.Query<
//                RefRO<ShootWeapon>, 
//                RefRW<LocalTransform>
//                >())
//        {
//            if (!shoot.ValueRO.onShootEvent.isTriggered)
//                continue;

//            //Entity muzzle = state.EntityManager.Instantiate(references.muzzleFlashPrefab);
//            //SystemAPI.SetComponent(muzzle, LocalTransform.FromPositionRotationScale(
//            //    new float3(1,1,1),
//            //    quaternion.EulerXYZ(new float3(90,0,-90)),
//            //    0.38128f));
//            //Entity muzzle = commandBuffer.Instantiate(references.muzzleFlashPrefab);
//            //commandBuffer.AddComponent(muzzle, LocalTransform.FromPosition(new float3(5, 10, 5)));
//        }
//    }


//}
