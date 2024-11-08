public interface ISpawnableObjectInitializeParameter { }

public interface ISpawnableObject
{
    void Initialize(ISpawnableObjectInitializeParameter param);
    void Despawn();
}