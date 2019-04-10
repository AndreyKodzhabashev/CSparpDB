namespace BillsPaymentSystem.App.Core.Contracts
{
    using System;
    using Data;

    public interface ICommandInterpreter
    {
        string ReadCommand(string[] inputParams, IDisposable context);
    }
}