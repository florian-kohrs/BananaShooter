using Unity.Entities;
using UnityEngine;

class JumpToTargetPosition : MonoBehaviour
{

    class JumpToTargetPositionBaker : Baker<JumpToTargetPosition>
    {
        public override void Bake(JumpToTargetPosition authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new JumpToTargetPos());
        }
    }

}

public struct JumpToTargetPos : IComponentData
{

}