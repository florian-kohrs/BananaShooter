using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject target;

    private void Update()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery query = new EntityQueryBuilder(Allocator.Temp).WithAll<TargetPosition>().WithPresent<Enemy>().Build(entityManager);

        NativeArray<TargetPosition> moverArray = query.ToComponentDataArray<TargetPosition>(Allocator.Temp);
        for (int i = 0; i < moverArray.Length; i++)
        {
            TargetPosition target = moverArray[i];
            target.targetPosition = this.target.transform.position;
            moverArray[i] = target;
        }
        query.CopyFromComponentDataArray(moverArray);
    }

}
