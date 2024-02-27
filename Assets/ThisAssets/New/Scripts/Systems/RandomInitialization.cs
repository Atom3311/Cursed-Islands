using Unity.Entities;
using System;
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct RandomInitialization : ISystem
{
    private void OnCreate(ref SystemState state)
    {
        CursedIsland.Random.MainRandom.InitState((uint)DateTime.Now.Ticks);
    }
}