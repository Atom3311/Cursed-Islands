using Unity.Entities;
using UnityEngine;
[UpdateBefore(typeof(HandlerEventChangedResources))]
public partial class PaddleInformationAboutResources : SystemBase
{
    private TextWithResource[] _targetTexts;
    protected override void OnCreate()
    {
        RequireForUpdate<InformationAboutResources>();
        RequireForUpdate<EventChangedResources>();
    }
    protected override void OnStartRunning()
    {
        _targetTexts = Object.FindObjectsOfType<TextWithResource>();
        Entity entityWithInformation = SystemAPI.GetSingletonEntity<InformationAboutResources>();
        InformationAboutResources information = SystemAPI.GetSingleton<InformationAboutResources>();
        EventChangedResources handler = EntityManager.GetComponentObject<EventChangedResources>(entityWithInformation);
        handler.TargetEvent += () =>
        {
            InformationAboutResources information = SystemAPI.GetSingleton<InformationAboutResources>();
            foreach (TextWithResource text in _targetTexts)
            {
                Resource targetResource = text.TargetResource;
                switch (targetResource)
                {
                    case Resource.Gold: text.WriteText(information.Gold); break;
                    case Resource.Wood: text.WriteText(information.Wood); break;
                    case Resource.Food: text.WriteText(information.Food); break;
                }
            }
        };
    }
    protected override void OnUpdate()
    {
    }
}
