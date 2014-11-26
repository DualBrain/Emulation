using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Emulation.Debugger.MVVM
{
    internal partial class ViewModel
    {
        private CommandBindingCollection commandBindings;

        private CommandBindingCollection CommandBindings
        {
            get
            {
                if (this.commandBindings == null)
                {
                    Interlocked.CompareExchange(ref this.commandBindings, new CommandBindingCollection(), null);
                }

                return this.commandBindings;
            }
        }

        private ICommand RegisterCommand(
            string text, string name,
            InputGesture[] inputGestures,
            ExecutedRoutedEventHandler executed,
            CanExecuteRoutedEventHandler canExecute)
        {
            var command = new RoutedUICommand(text, name, this.GetType(), new InputGestureCollection(inputGestures));
            var commandBinding = new CommandBinding(command, executed, canExecute);

            CommandManager.RegisterClassCommandBinding(this.GetType(), commandBinding);
            CommandBindings.Add(commandBinding);

            return command;
        }

        protected ICommand RegisterCommand(
            string text, string name,
            Action executed,
            Func<bool> canExecute,
            params InputGesture[] inputGestures)
        {
            return RegisterCommand(
                text, name, inputGestures,
                executed: (s, e) => executed(),
                canExecute: (s, e) => e.CanExecute = canExecute());
        }

        private static T Cast<T>(object value)
        {
            return value != null
                ? (T)value
                : default(T);
        }

        protected ICommand RegisterCommand<T>(
            string text, string name,
            Action<T> executed,
            Func<T, bool> canExecute,
            params InputGesture[] inputGestures)
        {
            return RegisterCommand(
                text, name, inputGestures,
                executed: (s, e) => executed(Cast<T>(e.Parameter)),
                canExecute: (s, e) => e.CanExecute = canExecute(Cast<T>(e.Parameter)));
        }

        public static readonly DependencyProperty RegisterCommandsProperty =
            DependencyProperty.RegisterAttached(
                name: "RegisterCommands",
                propertyType: typeof(ViewModel),
                ownerType: typeof(ViewModel),
                defaultMetadata: new PropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: (dp, e) =>
                    {
                        var element = dp as UIElement;
                        if (element != null)
                        {
                            var viewModel = e.NewValue as ViewModel;
                            if (viewModel != null)
                            {
                                element.CommandBindings.AddRange(viewModel.CommandBindings);
                            }
                        }
                    }));

        public static ViewModel GetRegisterCommands(UIElement element)
        {
            return (ViewModel)element.GetValue(RegisterCommandsProperty);
        }

        public static void SetRegisterCommands(UIElement element, ViewModel value)
        {
            element.SetValue(RegisterCommandsProperty, value);
        }
    }
}
