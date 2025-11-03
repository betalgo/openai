namespace Betalgo.Ranul.OpenAI.Contracts.Interfaces;

public interface IDefaultResult<out T>
{
    public T? Result { get; }
}
public interface IDefaultResults<T>
{
    public List<T>? Results { get; }
}