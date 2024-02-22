using Unity.Entities;
public partial struct AnimationForMovableUnits : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        foreach ((
            RefRO<MovableUnit> unit,
            RefRO<MovePoint> targetPoint,
            RefRO<HealthState> healthState,
            AnimatorComponent animator) in SystemAPI.Query<
                RefRO<MovableUnit>,
                RefRO<MovePoint>,
                RefRO<HealthState>,
                AnimatorComponent>())
        {
            if (!animator.HasMainAnimator)
                continue;

            bool isRunning = targetPoint.ValueRO.PointInWorld.HasValue && !healthState.ValueRO.IsDead;
            animator.ThisAnimator.SetBool(Constants.NameOfFieldForAnimationRun, isRunning);
        }
    }
}