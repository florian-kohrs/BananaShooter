using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private void Update()
    {
        float3 inputDir = GetInputDirection();
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery query = new EntityQueryBuilder(Allocator.Temp).WithAll<TargetPosition, LocalTransform>().WithPresent<Friendly>().Build(entityManager);

        NativeArray<LocalTransform> positionArray = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
        NativeArray<TargetPosition> moverArray = query.ToComponentDataArray<TargetPosition>(Allocator.Temp);
        for (int i = 0; i < moverArray.Length; i++)
        {
            TargetPosition mover = moverArray[i];
            mover.targetPosition = positionArray[i].Position + inputDir;
            moverArray[i] = mover;
        }
        query.CopyFromComponentDataArray(moverArray);
    }


    public float3 GetInputDirection()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return float3.zero;

        return new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}
