using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using System.Data.Common;

namespace YngveHestem.GenericParameterCollection.EtoForms
{	
	public class ControlDialog<TResult> : Dialog<TResult>
	{
        public bool Success { get; private set; }
		public ControlDialog(TResult currentValue, ParameterCollectionPanelOptions options, ParameterType type)
		{
			if (options == null)
			{
				options = new ParameterCollectionPanelOptions();
			}
			Control control = null;
            if (type == ParameterType.String)
			{
                control = options.CreateTextBox(currentValue.ToString());
            }
			else if (type == ParameterType.String_Multiline)
			{
				control = options.CreateTextArea(currentValue.ToString());
			}
            else if (type == ParameterType.Int)
            {
                control = options.CreateNumericStepper((double)(object)currentValue, true);
            }
            else if (type == ParameterType.Float
				|| type == ParameterType.Double
				|| type == ParameterType.Long)
            {
                control = options.CreateNumericStepper((double)(object)currentValue);
            }
            else if (type == ParameterType.Bool)
            {
                control = options.CreateCheckBox((bool)(object)currentValue);
            }
            else if (type == ParameterType.DateTime || type == ParameterType.Date)
            {
                control = options.CreateDateTimePicker((DateTime)(object)currentValue, type == ParameterType.DateTime ? DateTimePickerMode.DateTime : DateTimePickerMode.Date);
            }
            else if (type == ParameterType.ParameterCollection)
            {
                control = options.CreateParameterCollectionOpenDialogPanel((ParameterCollection)(object)currentValue);
            }
            else
            {
                throw new NotImplementedException("When creating control in ControlDialog, the Type " + type + " is not supported.");
            }

            Title = "Value";
            Content = new StackLayout
            {
                Items =
                {
                    control
                }
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
                var cType = control.GetType();
                if (cType == typeof(TextBox))
                {
                    Result = (TResult)(object)((TextBox)control).Text;
                }
                else if (cType == typeof(TextArea))
                {
                    Result = (TResult)(object)((TextArea)control).Text;
                }
                else if (cType == typeof(NumericStepper))
                {
                    Result = (TResult)(object)((NumericStepper)control).Value;
                }
                else if (cType == typeof(CheckBox))
                {
                    Result = (TResult)(object)((CheckBox)control).Checked;
                }
                else if (cType == typeof(DateTimePicker))
                {
                    Result = (TResult)(object)((DateTimePicker)control).Value;
                }
                else if (cType == typeof(Panel))
                {
                    Result = (TResult)(object)((StackLayout)((Panel)control).Content).Tag;
                }
                else
                {
                    throw new NotImplementedException("When getting value from control in ControlDialog, the control " + cType + " is not supported.");
                }
                Success = true;
                Close();
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
                Success = false;
                Close();
            };
            NegativeButtons.Add(AbortButton);
        }
    }
}

