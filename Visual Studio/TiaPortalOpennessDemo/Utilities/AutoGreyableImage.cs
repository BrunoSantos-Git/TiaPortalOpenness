using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TiaPortalOpennessDemo.Utilities
{
    /// <summary>
    /// Class used to have an image that is able to be gray when the control is not enabled.
    /// Based on the version by Thomas LEBRUN (http://blogs.developpeur.org/tom)
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Image" />
    public class AutoGreyableImage : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGreyableImage" /> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        static AutoGreyableImage()
        {
            // Override the metadata of the IsEnabled and Source property.
            IsEnabledProperty.OverrideMetadata(typeof(AutoGreyableImage), new FrameworkPropertyMetadata(true, OnAutoGreyScaleImageIsEnabledPropertyChanged));
            SourceProperty.OverrideMetadata(typeof(AutoGreyableImage), new FrameworkPropertyMetadata(null, OnAutoGreyScaleImageSourcePropertyChanged));
        }

        /// <summary>Gets the image with source.</summary>
        /// <param name="source">The source.</param>
        /// <returns>AutoGreyableImage</returns>
        /// TODO Edit XML Comment Template for GetImageWithSource
        protected static AutoGreyableImage GetImageWithSource(DependencyObject source)
        {
            var image = source as AutoGreyableImage;
            if (image == null)
                return null;

            if (image.Source == null)
                return null;

            return image;
        }

        /// <summary>Called when [auto grey scale image source property changed].</summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected static void OnAutoGreyScaleImageSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var image = GetImageWithSource(source);
            if (image != null)
                ApplyGreyScaleImage(image, image.IsEnabled);
        }

        /// <summary>
        /// Called when [auto grey scale image is enabled property changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var image = GetImageWithSource(source);
            if (image != null)
            {
                var isEnabled = Convert.ToBoolean(args.NewValue, CultureInfo.InvariantCulture);
                ApplyGreyScaleImage(image, isEnabled);
            }
        }

        /// <summary>Applies the grey scale image.</summary>
        /// <param name="autoGreyScaleImage">The automatic grey scale image.</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// TODO Edit XML Comment Template for ApplyGreyScaleImage
        protected static void ApplyGreyScaleImage(Image autoGreyScaleImage, Boolean isEnabled)
        {
            try
            {
                if (!isEnabled)
                {
                    BitmapSource bitmapImage;

                    if (autoGreyScaleImage.Source is FormatConvertedBitmap)
                    {
                        // Already grey !
                        return;
                    }
                    if (autoGreyScaleImage.Source is BitmapSource)
                    {
                        bitmapImage = (BitmapSource)autoGreyScaleImage.Source;
                    }
                    else // trying string 
                    {
                        bitmapImage = new BitmapImage(new Uri(autoGreyScaleImage.Source.ToString()));
                    }
                    var conv = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                    autoGreyScaleImage.Source = conv;

                    // Create Opacity Mask for greyscale image as FormatConvertedBitmap does not keep transparency info
                    autoGreyScaleImage.OpacityMask = new ImageBrush(((FormatConvertedBitmap)autoGreyScaleImage.Source).Source); //equivalent to new ImageBrush(bitmapImage)
                }
                else
                {
                    if (autoGreyScaleImage.Source is FormatConvertedBitmap)
                    {
                        autoGreyScaleImage.Source = ((FormatConvertedBitmap)autoGreyScaleImage.Source).Source;
                    }
                    else if (autoGreyScaleImage.Source is BitmapSource)
                    {
                        // Should be full color already.
                        return;
                    }

                    // Reset the Opcity Mask
                    autoGreyScaleImage.OpacityMask = null;
                }
            }
            catch (ArgumentException)
            {
                // nothin'
            }

        }

    }
}