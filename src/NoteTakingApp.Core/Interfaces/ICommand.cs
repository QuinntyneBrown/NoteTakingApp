using System;
using System.Collections.Generic;
using System.Text;

namespace NoteTakingApp.Core.Interfaces
{
    public interface ICommand<TResponse>
    {
        string Key { get; }
        IEnumerable<string> SideEffects { get; }
    }
}
