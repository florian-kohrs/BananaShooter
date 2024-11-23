using Unity.Entities;
using UnityEngine;

class UnitAuthoring : MonoBehaviour
{

    class UnitBaker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Unit
            {
                faction = (Faction)(1<<authoring.gameObject.layer),
            });
        }
    }

}

public partial struct Unit : IComponentData
{
    public Faction faction;
}