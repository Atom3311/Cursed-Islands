using UnityEngine;
using Unity.Entities;
public partial class HandlerEventChangedResources : SystemBase
{
    private InformationAboutResources previousInformationAboutResources;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutResources>();
    }
    protected override void OnStartRunning()
    {
        InformationAboutResources duringResources = SystemAPI.GetSingleton<InformationAboutResources>();
        Execute(duringResources);
    }
    protected override void OnUpdate()
    {
        InformationAboutResources duringResources = SystemAPI.GetSingleton<InformationAboutResources>();

        if (!CheckResources(duringResources, previousInformationAboutResources))
            return;

        Execute(duringResources);

        bool CheckResources(InformationAboutResources first, InformationAboutResources second)
        {
            bool isChanged = first.Gold != second.Gold || first.Wood != second.Wood || first.Food != second.Food;
            return isChanged;
        }
        
    }
    private void Execute(InformationAboutResources duringResources)
    {
        Entity entityWithResources = SystemAPI.GetSingletonEntity<InformationAboutResources>();
        previousInformationAboutResources = duringResources;
        EventChangedResources eventChanged = EntityManager.GetComponentObject<EventChangedResources>(entityWithResources);
        eventChanged.TargetEvent?.Invoke();
    }
}