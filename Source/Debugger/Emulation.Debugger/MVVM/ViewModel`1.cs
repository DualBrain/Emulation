using System;
using System.Windows;
using System.Windows.Controls;

namespace Emulation.Debugger.MVVM
{
    internal abstract class ViewModel<TView> : ViewModel
        where TView : ContentControl
    {
        private readonly Uri viewUri;
        private TView view;

        protected ViewModel(string viewName)
        {
            var assemblyName = this.GetType().Assembly.GetName().Name;
            var uriString = "/\{assemblyName};component/Views/\{viewName}.xaml";

            this.viewUri = new Uri(uriString, UriKind.Relative);
        }

        protected TView View => this.view;
        protected bool ViewCreated => this.view != null;

        protected virtual void OnViewCreated(TView view)
        {
            // descendents can override.
        }

        public TView CreateView()
        {
            if (ViewCreated)
            {
                throw new InvalidOperationException("View has already been created.");
            }

            var localView = Application.LoadComponent(this.viewUri) as TView;
            localView.DataContext = this;
            this.view = localView;

            OnViewCreated(localView);

            return localView;
        }
    }
}
