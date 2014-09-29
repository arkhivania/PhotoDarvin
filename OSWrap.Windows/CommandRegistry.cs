using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OSWrap.Windows
{
    public class CommandRegistry : ICommandRegistry
    {
        class Remover : IDisposable
        {
            private readonly UIElement window;

            private readonly InputBinding binding;

            public Remover(UIElement window, InputBinding binding)
            {
                this.window = window;
                this.binding = binding;
            }

            public void Dispose()
            {
                window.InputBindings.Remove(binding);
            }
        }

        readonly UIElement window;
        public CommandRegistry(UIElement window)
        {
            this.window = window;
        }

        public IDisposable RegisterCommand(ICommand command, InputGesture gesture)
        {
            var binding = new InputBinding(command, gesture);
            window.InputBindings.Add(binding);
            return new Remover(window, binding);
        }
    }
}
