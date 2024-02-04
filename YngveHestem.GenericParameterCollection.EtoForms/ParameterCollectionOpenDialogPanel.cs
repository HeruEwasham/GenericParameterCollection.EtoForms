using System;
using Eto.Forms;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
	internal class ParameterCollectionOpenDialogPanel : Panel
	{
        public ParameterCollection Value { get; private set; }

        private readonly ParameterCollectionPanelOptions _options;

        public ParameterCollectionOpenDialogPanel(ParameterCollection value, ParameterCollectionPanelOptions options)
		{
            Value = value;
            _options = options;
            var showEditButton = new Button
            {
                Text = "Show/Edit",
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont,
                Enabled = !options.ReadOnly
            };
            showEditButton.Click += ShowEditButton_Click;
            BackgroundColor = options.NormalBackgroundColor;
            Content = showEditButton;
        }

        private void ShowEditButton_Click(object sender, EventArgs e)
        {
            var dialog = new ParameterCollectionDialog(Value, _options);
            var newValue = dialog.ShowModal();
            if (newValue != null)
            {
                Value = newValue;
            }
        }
    }
}

