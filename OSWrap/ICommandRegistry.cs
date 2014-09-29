using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OSWrap
{
    public interface ICommandRegistry
    {
        IDisposable RegisterCommand(ICommand command, InputGesture gesture);
    }
}
