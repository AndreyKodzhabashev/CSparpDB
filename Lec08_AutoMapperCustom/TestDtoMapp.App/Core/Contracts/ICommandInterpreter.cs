namespace TestDtoMapp.App.Core.Contracts
{
    public interface ICommandInterpreter
    {
        string Read(string[] args);
    }
}