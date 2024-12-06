using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

class DeathLootAuthoring : MonoBehaviour
{

    public int referenceIndex;

    class DeathLootAuthoringBaker : Baker<DeathLootAuthoring>
    {
        public override void Bake(DeathLootAuthoring authoring)
        {
            Entity e = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(e, new LootReference { referenceIndex = authoring.referenceIndex });
        }
    }

}

public struct LootReference : IComponentData
{
    public int referenceIndex;
}