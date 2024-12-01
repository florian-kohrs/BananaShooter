using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

class EquipmentHorderAuthoring : MonoBehaviour
{

    public List<GameObject> equipedWeapons;

    class EquipmentHorderAuthoringBaker : Baker<EquipmentHorderAuthoring>
    {
        public override void Bake(EquipmentHorderAuthoring authoring)
        {
            var buffer = AddBuffer<WeaponHorder>(GetEntity(TransformUsageFlags.Dynamic));
            for (int i = 0; i < authoring.equipedWeapons.Count; i++)
            {
                buffer.Add(new WeaponHorder { weapon = GetEntity(authoring.equipedWeapons[i], TransformUsageFlags.Dynamic) });
            }
        }
    }
}

public struct WeaponHorder : IBufferElementData
{
    public Entity weapon;
}