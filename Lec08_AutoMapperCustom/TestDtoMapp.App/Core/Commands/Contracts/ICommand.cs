namespace TestDtoMapp.App.Core.Commands.Contracts
{
    public interface ICommand
    {
        string Execute(string[] args);
    }
}