using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject target;

    private void Update()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery query = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover>().WithPresent<Enemy>().Build(entityManager);

        NativeArray<UnitMover> moverArray = query.ToComponentDataArray<UnitMover>(Allocator.Temp);
        for (int i = 0; i < moverArray.Length; i++)
        {
            UnitMover mover = moverArray[i];
            mover.targetPosition = target.transform.position;
            moverArray[i] = mover;
        }
        query.CopyFromComponentDataArray(moverArray);
    }

}
