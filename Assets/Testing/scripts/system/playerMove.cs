using Unity.Transforms;
using Unity.Entities;
using UnityEngine.EventSystems;
using UnityEngine;

public partial class playerMove : SystemBase
{
    protected override void OnUpdate()
    {
        RotateCubeJob rotateCubeJob = new RotateCubeJob { };

        rotateCubeJob.Schedule();
    }
}


public partial struct RotateCubeJob : IJobEntity
{
    void Execute(ref SpeedData speedData, ref LocalTransform localTransform)
    {
        localTransform.Position.z += speedData.value;
    }

}

