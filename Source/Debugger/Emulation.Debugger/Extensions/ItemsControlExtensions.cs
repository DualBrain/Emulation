using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Emulation.Debugger.Extensions
{
    internal static class ItemsControlExtensions
    {
        private static MethodInfo getItemsHostMethodInfo;

        private static Panel GetItemsHost(ItemsControl itemsControl)
        {
            if (getItemsHostMethodInfo == null)
            {
                getItemsHostMethodInfo = typeof(ItemsControl).GetMethod("get_ItemsHost", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            return (Panel)(getItemsHostMethodInfo.Invoke(itemsControl, new object[0]));
        }

        public static void BringIntoView(this ItemsControl itemsControl, object item)
        {
            var element = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
            if (element != null)
            {
                element.BringIntoView();
            }
            else if (!itemsControl.IsGrouping)
            {
                var index = itemsControl.Items.IndexOf(item);
                if (index >= 0)
                {
                    var itemsHost = GetItemsHost(itemsControl) as VirtualizingPanel;
                    if (itemsHost != null)
                    {
                        itemsHost.BringIndexIntoViewPublic(index);
                    }
                }
            }
        }
    }
}
