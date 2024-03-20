using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

[UpdateAfter(typeof(BuilderHealthStateControll))]
// Структура BuildControll, реализующая интерфейс ISystem
public partial struct BuildControll : ISystem
{
    // Метод OnCreate вызывается при создании системы
    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BuildingParametersComponent>();
    }

    // Метод OnUpdate вызывается при обновлении системы
    private void OnUpdate(ref SystemState state)
    {
        // Создаем буфер команд для работы с Entity
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // Получаем компонент BuildingParametersComponent из синглтона
        var buildingParametersComopnent = SystemAPI.GetSingleton<BuildingParametersComponent>();

        // Перебираем все сущности с компонентами Foundation и LocalTransform
        foreach (var (foundation, transform, entity) in SystemAPI.Query<
                 RefRW<Foundation>,
                 RefRW<LocalTransform>>()
                 .WithEntityAccess())
        {
            // Получаем Entity строителя, связанного с фундаментом
            Entity builderEntity = foundation.ValueRO.Builder;

            // Проверяем, существует ли строитель или у строителя нет компонента Build
            if (builderEntity == Entity.Null || !SystemAPI.HasComponent<Build>(builderEntity))
            {
                foundation.ValueRW.DuringTime = 0;
                continue;
            }

            // Если прошло достаточно времени для постройки
            if (foundation.ValueRO.DuringTime >= Constants.NeededTimeForBuild)
            {
                // Получаем доступ к компоненту Builder
                RefRW<Builder> builder = SystemAPI.GetComponentRW<Builder>(builderEntity);
                builder.ValueRW.TargetFoundation = Entity.Null;

                ecb.RemoveComponent<Build>(builderEntity);

                ecb.DestroyEntity(entity);

                // Создаем новую сущность на основе сущности Building
                Entity createdEntity = ecb.Instantiate(buildingParametersComopnent.Building);

                // Устанавливаем позицию созданной сущности
                float3 position = transform.ValueRO.Position;
                position.y = 0.75f;
                LocalTransform transformForCreatedEntity = LocalTransform.Identity.WithPosition(position);
                transformForCreatedEntity = transformForCreatedEntity.Rotate(quaternion.EulerXYZ(-math.PI / 2, math.PI, 0));
                ecb.SetComponent(createdEntity, transformForCreatedEntity);

                // Добавляем компоненты Unit, Building, PlayerBuilding к созданной сущности
                ecb.AddComponent<Unit>(createdEntity);
                ecb.AddComponent<Building>(createdEntity);
                ecb.AddComponent<PlayerBuilding>(createdEntity);
            }
            else
            {
                // Увеличиваем время строительства фундамент
                foundation.ValueRW.DuringTime += SystemAPI.Time.DeltaTime;
            }
        }

        // Применяем выполненные команды к EntityManager
        ecb.Playback(state.EntityManager);
    }
}



//- `OnCreate`: Этот метод вызывается при создании системы и запрашивает обновление по BuildingParametersComponent.
//- `OnUpdate`: Основной метод, вызываемый при обновлении системы. В цикле перебираются сущности с компонентами Foundation и LocalTransform.
//    - Проверяется наличие строителя и компонента Build. Если строитель отсутствует или у него нет компонента Build, то время строительства обнуляется.
//    - Если прошло достаточно времени для постройки, то создается новая сущность здания, устанавливается ее позиция и добавляются необходимые компоненты.
//    - В противном случае увеличивается время строительства фундамента.
//- После выполнения всех операций команды в буфере воспроизводятся в EntityManager.
