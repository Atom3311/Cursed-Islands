using System.Diagnostics;
using Unity.Entities;
using UnityEngine.SceneManagement;
[UpdateAfter(typeof(OrdersController))]
[UpdateAfter(typeof(InputHandler))]
public partial class BuyPlayerUnit : SystemBase
{
    private bool _isInit;
    protected override void OnCreate()
    {
        RequireForUpdate<OrderInformation>();
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (scene.isSubScene)
                return;
            UnityEngine.Debug.Log(1);
            _isInit = true;
        };
    }
    protected override void OnUpdate()
    {
        if (!_isInit)
            return;

        OrderInformation orderInformation = SystemAPI.GetSingleton<OrderInformation>();
        var input = SystemAPI.GetSingleton<InformationAboutInputPlayer>();
        
        if (orderInformation.DuringOrder == Order.None || orderInformation.DuringOrder != Order.Buy)
            return;

        if (PointOnScreen.PointOnUIElement(input.MousePosition))
            return;



    }
}