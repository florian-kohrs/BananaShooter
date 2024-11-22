using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class MuzzleAuthoring : MonoBehaviour
{
    public float timeToVanish;
    public quaternion startRotation;
    public quaternion endRotation;

    class MuzzleAuthoringBaker : Baker<MuzzleAuthoring>
    {
        public override void Bake(MuzzleAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Muzzle
            {
                timeToVanish = authoring.timeToVanish,
                startRotation = authoring.startRotation,
                endRotation = authoring.endRotation,
            });
        }
    }
}

public struct Muzzle : IComponentData
{
    public float timeToVanish;
    public float startTime;
    public quaternion startRotation;
    public quaternion endRotation;
}