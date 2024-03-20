using Unity.Entities;

//Структура PackageOfMovableUnit объявлена как readonly partial, что означает, что она не может быть изменена после инициализации и может быть разбита на части для определения в разных файлах.
//-В структуре содержатся три поля, каждое из которых объявлено как readonly и имеет тип RefRO<тип>, где RefRO означает ссылку на компонент с возможностью только чтения.
public readonly partial struct PackageOfMovableUnit : IAspect
{
    //
    public readonly RefRO<MovableUnit> Unit;
    public readonly RefRO<SpeedComponent> SpeedComponent;
    public readonly RefRW<MovePoint> MovePoint;
}
// структура PackageOfMovableUnit используется для объединения ссылок на различные компоненты, относящиеся к подвижному юниту в контексте ECS, обеспечивая логичное и эффективное управление данными сущностей