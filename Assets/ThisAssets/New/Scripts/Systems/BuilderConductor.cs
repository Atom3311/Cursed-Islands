using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;


// Структура BuilderConductor, реализующая интерфейс ISystem и управляющая поведением строителей
[UpdateAfter(typeof(BuilderHealthStateControll))] // Указание на обновление после системы BuilderHealthStateControll
public partial struct BuilderConductor : ISystem
{
    // Метод OnCreate вызывается при создании системы
    private void OnCreate(ref SystemState state)
    {
        // Запрос обновлений для BuildingParametersComponent и InformationAboutResources
        state.RequireForUpdate<BuildingParametersComponent>();
        state.RequireForUpdate<InformationAboutResources>();
    }

    // Метод OnUpdate вызывается при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Создание буфера команд для работы с Entity
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        // Перебор всех сущностей с компонентами Builder, PackageOfMovableUnit, AttentionPoint, LocalTransform
        foreach (var (builder, packageOfMovableUnit, attentionPoint, transform, entity) in SystemAPI.Query<
            RefRW<Builder>,
            PackageOfMovableUnit,
            RefRW<AttentionPoint>,
            RefRO<LocalTransform>>()
            .WithEntityAccess())
        {
            // Получение сущности фундамента
            Entity foundationEntity = builder.ValueRO.TargetFoundation;

            // Пропуск текущей итерации, если сущность фундамента не определена
            if (foundationEntity == Entity.Null)
                continue;

            // Получение местоположения фундамента
            LocalTransform targetTransform = SystemAPI.GetComponent<LocalTransform>(foundationEntity);

            // Установка точки внимания на позицию фундамента
            attentionPoint.ValueRW.Point = targetTransform.Position;

            // Расчет расстояния между строителем и фундаментом
            float distance = math.distance(transform.ValueRO.Position, targetTransform.Position);

            // Проверка, находится ли строитель в достаточном расстоянии для постройки
            if (distance > Constants.RangeForBuild)
            {
                // Установка точки перемещения на позицию фундамента и удаление компонента Build
                packageOfMovableUnit.MovePoint.ValueRW.PointInWorld = targetTransform.Position;
                ecb.RemoveComponent<Build>(entity);
            }
            else
            {
                // Сброс точки перемещения и добавление компонента Build
                packageOfMovableUnit.MovePoint.ValueRW.PointInWorld = null;
                ecb.AddComponent<Build>(entity);
            }
        }

        // Применение накопленных команд к EntityManager
        ecb.Playback(state.EntityManager);
    }
}


//В этом скрипте структура BuilderConductor управляет поведением строителей в игре.
//Он обрабатывает их действия, устанавливая точки внимания, проверяя расстояния до фундаментов и управляя процессом постройки.
//Комментарии к каждой части кода помогают понять его функциональность и влияние на игровой процесс.
