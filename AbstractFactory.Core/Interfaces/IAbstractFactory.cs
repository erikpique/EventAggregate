namespace AbstractFactory.Core.Interfaces
{
    public interface IAbstractFactory
    {
        TOutput CreateInstance<TOutput>(params object[] parameters);
    }
}
