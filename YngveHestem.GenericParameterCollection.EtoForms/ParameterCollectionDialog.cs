using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace YngveHestem.GenericParameterCollection.EtoForms
{	
	public class ParameterCollectionDialog : Dialog<ParameterCollection>
	{	
		public ParameterCollectionDialog(ParameterCollection parameters, ParameterCollectionPanelOptions options = null, IEnumerable<ICustomParameterControl> customParameterControls = null, string title = "Show/Edit parameters")
		{
			if (options == null)
			{
				options = new ParameterCollectionPanelOptions();
			}

			var parametersPanel = new ParameterCollectionPanel(parameters, options, customParameterControls);

            Title = title;

			Resizable = true;

			Content = new Scrollable {
				Content = new StackLayout
				{
					Items =
					{
						parametersPanel,
					},
					HorizontalContentAlignment = HorizontalAlignment.Stretch,
					VerticalContentAlignment = VerticalAlignment.Stretch
				},
			};

			// buttons
			DefaultButton = new Button
			{
				Text = "OK",
				BackgroundColor = options.SubmitAddBackgroundColor,
				TextColor = options.SubmitAddTextColor,
				Font = options.SubmitAddFont
			};
			DefaultButton.Click += (sender, e) =>
			{
				Close(parametersPanel.GetParameters());
			};
			PositiveButtons.Add(DefaultButton);

			AbortButton = new Button
			{
				Text = "C&ancel",
				BackgroundColor = options.CancelDeleteBackgroundColor,
				TextColor = options.CancelDeleteTextColor,
				Font = options.CancelDeleteFont
			};
			AbortButton.Click += (sender, e) =>
			{
				Close(null);
			};
			NegativeButtons.Add(AbortButton);
		}
	}
}

