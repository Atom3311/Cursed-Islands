using Unity.Entities;
public partial class HandlerEventChangedResources : SystemBase
{
    private InformationAboutResources previousInformationAboutResources;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutResources>();
    }
    protected override void OnUpdate()
    {
        Entity entityWithResources = SystemAPI.GetSingletonEntity<InformationAboutResources>();
        InformationAboutResources duringResources = SystemAPI.GetSingleton<InformationAboutResources>();
        EventChangedResources eventChanged = EntityManager.GetComponentObject<EventChangedResources>(entityWithResources);


    }
}