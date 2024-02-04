using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Eto.Forms;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
	internal class BytesPanel : Panel
	{
        public byte[] Value { get; private set; }
        public string Filename { get; private set; }
        public string Extension { get; private set; }

        private readonly ParameterCollectionPanelOptions _options;

		public BytesPanel(ParameterCollectionPanelOptions options, byte[] value, Parameter parameter)
		{
            _options = options;
            Value = value;

            var selectButton = new Button
            {
                Text = "Change",
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
            if (options.ReadOnly)
            {
                selectButton.Text = "Can't change";
                selectButton.Enabled = false;
                selectButton.Visible = false;
            }
            else
            {
                selectButton.Click += SelectButton_Click;
            }
            var statusText = "Selected item has size: " + value.Length.GetSizeInMemory();
            if (parameter.HasAdditionalInfo())
            {
                var oAdI = parameter.GetAdditionalInfo();
                if (oAdI.HasKeyAndCanConvertTo("filename", typeof(string)))
                {
                    Filename = oAdI.GetByKey<string>("filename");
                    statusText += Environment.NewLine + "Filename: " + Filename;
                }
                if (oAdI.HasKeyAndCanConvertTo("extension", typeof(string)))
                {
                    Extension = oAdI.GetByKey<string>("extension");
                }
            }

            BackgroundColor = options.NormalBackgroundColor;
            Content = new StackLayout
            {
                Items =
                {
                    selectButton,
                    options.CreateLabel(statusText)
                }
            };

            SetBytesPreview();
        }

        private void SetBytesPreview()
        {
            if (_options.BytesPreviews != null)
            {
                var previewInterface = _options.BytesPreviews.FirstOrDefault((o) =>
                {
                    return o.CanPreviewBytes(Extension, Value);
                });
                if (previewInterface != null)
                {
                    ((StackLayout)Content).Items.Add(previewInterface.GetPreviewControl(Extension, Value));
                }
            }
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            var oldParameter = (Parameter)((Control)sender).Parent.Parent.Tag;
            var dialog = new OpenFileDialog
            {
                Title = "Select file for " + oldParameter.Key,
                CheckFileExists = true,
                MultiSelect = false
            };
            if (_options.SupportedFileExtensions != null && _options.SupportedFileExtensions.Length > 0)
            {
                dialog.Filters.Add(new FileFilter("Supported file types", _options.SupportedFileExtensions));
            }

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                Value = File.ReadAllBytes(dialog.FileName);
                Filename = Path.GetFileName(dialog.FileName);

                var stackLayout = ((StackLayout)Content);
                ((Label)stackLayout.Items[1].Control).Text = "Selected item has size: " + Value.Length.GetSizeInMemory() + Environment.NewLine + "Filename: " + Filename;
                if (stackLayout.Items.Count == 3)
                {
                    stackLayout.Items.RemoveAt(2);
                }
                if (_options.BytesPreviews != null)
                {
                    Extension = Path.GetExtension(Filename);
                    var previewInterface = _options.BytesPreviews.FirstOrDefault((o) =>
                    {
                        return o.CanPreviewBytes(Extension, Value);
                    });
                    if (previewInterface != null)
                    {
                        stackLayout.Items.Add(previewInterface.GetPreviewControl(Extension, Value));
                    }
                }
            }
        }
    }
}

