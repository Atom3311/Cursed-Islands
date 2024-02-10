using Unity.Entities;
public partial struct AnimationForMovableUnits : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach ((
            RefRO<MovableUnit> unit,
            RefRO<MovePoint> targetPoint,
            AnimatorComponent animator) in SystemAPI.Query<
                RefRO<MovableUnit>,
                RefRO<MovePoint>,
                AnimatorComponent>())
        {
            if (!animator.HasMainAnimator)
                continue;

            bool isRunning = targetPoint.ValueRO.PointInWorld.HasValue;
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationRun, isRunning);
        }
    }
}