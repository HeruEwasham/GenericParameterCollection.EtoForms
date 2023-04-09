using System;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using YngveHestem.GenericParameterCollection.EtoForms.BytesPreview.Core;

namespace YngveHestem.GenericParameterCollection.EtoForms.BytesPreview.ImagePreview
{
    public class ImagePreview : IBytesPreview
    {
        private static readonly string[] _supportedExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".gif",
            ".tif",
            ".tiff"
        };

        /// <summary>
        /// How big should the preview size be. If you want that size to be sized after the image, set the size to -1. 
        /// </summary>
        public Size PreviewSize = new Size(400, 225);

        public bool CanPreviewBytes(string extension, byte[] bytes)
        {
            if (extension == null)
            {
                return false;
            }
            return _supportedExtensions.Any(s => s == extension.ToLower());
        }

        public Control GetPreviewControl(string extension, byte[] bytes)
        {
            return new ImageView
            {
                Image = new Bitmap(bytes),
                Size = PreviewSize
            };
        }
    }
}

