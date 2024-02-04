using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using System.Data.Common;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace YngveHestem.GenericParameterCollection.EtoForms
{	
	public class ControlDialog<TResult> : Dialog<TResult>
	{
        public bool Success { get; private set; }

        private List<ICustomParameterControl> _customControls;
        private ParameterCollection _additionalInfo;
        private ParameterType _type;
        private Control _control;


        public ControlDialog(TResult currentValue, ParameterCollectionPanelOptions options, ParameterType type, ParameterCollection additionalInfo, List<ICustomParameterControl> customControls)
		{
			if (options == null)
			{
				options = new ParameterCollectionPanelOptions();
			}
			_control = null;
            _customControls = customControls;
            _additionalInfo = additionalInfo;
            _type = type;

            ICustomParameterControl customControl = null;
            if (customControls != null)
            {
                customControl = customControls.FirstOrDefault(cc => cc.CanCreateControl(currentValue, type, additionalInfo, options));
            }

            if (customControl != null)
            {
                _control = customControl.CreateControl(currentValue, type, additionalInfo, options);
            }
            else if (type == ParameterType.String)
			{
                _control = options.CreateTextBox(currentValue.ToString());
            }
			else if (type == ParameterType.String_Multiline)
			{
				_control = options.CreateTextArea(currentValue.ToString());
			}
            else if (type == ParameterType.Int)
            {
                _control = options.CreateNumericStepper((double)(object)currentValue, true);
            }
            else if (type == ParameterType.Decimal)
            {
                _control = options.CreateNumericStepper((double)(object)currentValue);
            }
            else if (type == ParameterType.Bool)
            {
                _control = options.CreateCheckBox((bool)(object)currentValue);
            }
            else if (type == ParameterType.DateTime || type == ParameterType.Date)
            {
                _control = options.CreateDateTimePicker((DateTime)(object)currentValue, type == ParameterType.DateTime ? DateTimePickerMode.DateTime : DateTimePickerMode.Date);
            }
            else if (type == ParameterType.ParameterCollection)
            {
                _control = new ParameterCollectionOpenDialogPanel((ParameterCollection)(object)currentValue, options);
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
                    _control
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
            DefaultButton.Click += DefaultButton_Click;

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

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            var cType = _control.GetType();
            ICustomParameterControl customControl = null;
            if (_customControls != null)
            {
                customControl = _customControls.FirstOrDefault(cc => cc.CanGetValue(cType, _control, _type, _additionalInfo));
            }

            if (customControl != null)
            {
                Result = (TResult)customControl.GetValue(cType, _control, _type, _additionalInfo);
            }
            else if (cType == typeof(TextBox))
            {
                Result = (TResult)(object)((TextBox)_control).Text;
            }
            else if (cType == typeof(TextArea))
            {
                Result = (TResult)(object)((TextArea)_control).Text;
            }
            else if (cType == typeof(NumericStepper))
            {
                Result = (TResult)(object)((NumericStepper)_control).Value;
            }
            else if (cType == typeof(CheckBox))
            {
                Result = (TResult)(object)((CheckBox)_control).Checked;
            }
            else if (cType == typeof(DateTimePicker))
            {
                Result = (TResult)(object)((DateTimePicker)_control).Value;
            }
            else if (cType == typeof(ParameterCollectionOpenDialogPanel))
            {
                Result = (TResult)(object)((ParameterCollectionOpenDialogPanel)_control).Value;
            }
            else
            {
                throw new NotImplementedException("When getting value from control in ControlDialog, the control " + cType + " is not supported.");
            }
            Success = true;
            Close();
        }
    }
}

