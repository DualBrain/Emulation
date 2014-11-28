using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emulation.Debugger.Controls
{
    public class AutoDisableImage : Image
    {
        static AutoDisableImage()
        {
            IsEnabledProperty.OverrideMetadata(
                forType: typeof(AutoDisableImage),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: true,
                    propertyChangedCallback: (s, e) =>
                    {
                        var image = s as AutoDisableImage;
                        var isEnabled = Convert.ToBoolean(e.NewValue);
                        if (isEnabled)
                        {
                            image.Source = ((FormatConvertedBitmap)image.Source).Source;
                            image.OpacityMask = null;
                        }
                        else
                        {
                            var bitmapImage = new BitmapImage(new Uri(image.Source.ToString()));
                            image.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0.0);
                            image.OpacityMask = new ImageBrush(bitmapImage);
                        }
                    }));
        }
    }
}
