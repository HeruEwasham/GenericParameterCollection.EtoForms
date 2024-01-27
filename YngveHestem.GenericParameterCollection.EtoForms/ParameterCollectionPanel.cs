using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
    public class ParameterCollectionPanel : Panel
    {
        private ParameterCollection _parameters;
        private List<ICustomParameterControl> _customControls;

        public ParameterCollectionPanel(ParameterCollection parameters, ParameterCollectionPanelOptions options = null, IEnumerable<ICustomParameterControl> customParameterControls = null)
        {
            _parameters = parameters;
            if (customParameterControls != null)
            {
                _customControls = customParameterControls.ToList();
            }
            Content = new TableLayout(GetRows(parameters, options))
            {
                Spacing = new Size(10, 10)
            };
        }

        public ParameterCollection GetParameters()
        {
            var result = new ParameterCollection();
            result.AddCustomConverter(_parameters.GetCustomConverters());

            foreach (var row in ((TableLayout)Content).Rows)
            {
                var oldParameter = (Parameter)row.Cells[1].Control.Tag;
                var value = GetValue(row.Cells[1].Control, oldParameter.Type, oldParameter);
                result.Add(new Parameter(oldParameter.Key, value.Item1, oldParameter.Type, value.Item2));
            }

            return result;
        }

        /// <summary>
        /// Gets the new values. The object-part is the value itself, while the ParameterCollection is the additionalInfo.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="returnType"></param>
        /// <param name="oldParameter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Tuple<object, ParameterCollection> GetValue(Control control, ParameterType returnType, Parameter oldParameter)
        {
            var type = control.GetType();
            ICustomParameterControl customControl = null;
            if (_customControls != null)
            {
                customControl = _customControls.FirstOrDefault(cc => cc.CanGetValue(type, control, returnType, oldParameter));
            }

            if (customControl != null)
            {
                return new Tuple<object, ParameterCollection>(customControl.GetValue(type, control, returnType, oldParameter), customControl.GetAdditionalInfo(type, control, returnType, oldParameter));
            }
            else if (type == typeof(TextBox))
            {
                return new Tuple<object, ParameterCollection>(((TextBox)control).Text, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(TextArea))
            {
                return new Tuple<object, ParameterCollection>(((TextArea)control).Text, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(NumericStepper))
            {
                if (returnType == ParameterType.Int)
                {
                    return new Tuple<object, ParameterCollection>((int)((NumericStepper)control).Value, oldParameter.GetAdditionalInfo());
                }
                else if (returnType == ParameterType.Decimal)
                {
                    return new Tuple<object, ParameterCollection>((decimal)((NumericStepper)control).Value, oldParameter.GetAdditionalInfo());
                }
                return new Tuple<object, ParameterCollection>(((NumericStepper)control).Value, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(Panel))
            {
                var adI = oldParameter.GetAdditionalInfo();
                if (returnType == ParameterType.Bytes)
                {
                    if (((StackLayout)((Panel)control).Content).Items[0].Control.Tag != null)
                    {
                        if (adI == null)
                        {
                            adI = new ParameterCollection();
                        }

                        var filename = (string)((StackLayout)((Panel)control).Content).Items[0].Control.Tag;
                        var ext = Path.GetExtension(filename);
                        if (adI.HasKey("filename"))
                        {
                            adI.GetParameterByKey("filename").SetValue(filename);
                        }
                        else
                        {
                            adI.Add("filename", filename);
                        }
                        if (adI.HasKey("extension"))
                        {
                            adI.GetParameterByKey("extension").SetValue(ext);
                        }
                        else
                        {
                            adI.Add("extension", ext);
                        }
                    }
                }
                return new Tuple<object, ParameterCollection>(((Panel)control).Content.Tag, adI);
            }
            else if (type == typeof(CheckBox))
            {
                return new Tuple<object, ParameterCollection>(((CheckBox)control).Checked, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(DateTimePicker))
            {
                return new Tuple<object, ParameterCollection>(((DateTimePicker)control).Value.Value, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(DropDown))
            {
                var p = oldParameter.GetValue<ParameterCollection>();
                var param = new ParameterCollection {
                    { "value", ((DropDown)control).SelectedKey },
                    { "choices", p.GetByKey<string[]>("choices") }
                };
                if (p.HasKeyAndCanConvertTo("type", typeof(string)))
                {
                    param.Add("type", p.GetByKey<string>("type"));
                }
                return new Tuple<object, ParameterCollection>(param, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(ComboBox))
            {
                var p = oldParameter.GetValue<ParameterCollection>();
                var param = new ParameterCollection {
                    { "value", ((ComboBox)control).SelectedKey },
                    { "choices", p.GetByKey<string[]>("choices") }
                };
                if (p.HasKeyAndCanConvertTo("type", typeof(string)))
                {
                    param.Add("type", p.GetByKey<string>("type"));
                }
                return new Tuple<object, ParameterCollection>(param, oldParameter.GetAdditionalInfo());
            }
            else if (type == typeof(GridView<string>))
            {
                var p = oldParameter.GetValue<ParameterCollection>();
                var result = new ParameterCollection {
                    { "choices", p.GetByKey<string[]>("choices") }
                };
                if (p.HasKeyAndCanConvertTo("type", typeof(string)))
                {
                    result.Add("type", p.GetByKey<string>("type"));
                }
                if (returnType == ParameterType.SelectMany)
                {
                    result.Add("value", ((GridView<string>)control).SelectedItems);
                }
                else
                {
                    result.Add("value", ((GridView<string>)control).SelectedItem);
                }
                return new Tuple<object, ParameterCollection>(result, oldParameter.GetAdditionalInfo());
            }
            else
            {
                throw new NotImplementedException("Could not get value from control.");
            }
        }

        private List<TableRow> GetRows(ParameterCollection parameters, ParameterCollectionPanelOptions options)
        {
            if (options == null)
            {
                options = new ParameterCollectionPanelOptions();
            }

            var rows = new List<TableRow>();
            foreach (var parameter in parameters)
            {
                Control control = null;
                var parameterOptions = options;
                if (parameterOptions.AdditionalInfoWillOverride && parameter.HasAdditionalInfo())
                {
                    parameterOptions = ParameterCollectionPanelOptions.CreateFromParameterCollection(parameter.GetAdditionalInfo(), parameterOptions);
                }

                ICustomParameterControl customControl = null;
                if (_customControls != null)
                {
                    customControl = _customControls.FirstOrDefault(cc => cc.CanCreateControl(parameter, parameterOptions));
                }

                if (customControl != null)
                {
                    control = customControl.CreateControl(parameter, parameterOptions).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.String)
                {
                    control = parameterOptions.CreateTextBox(parameter.GetValue<string>()).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.String_Multiline)
                {
                    control = parameterOptions.CreateTextArea(parameter.GetValue<string>()).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.Int)
                {
                    control = parameterOptions.CreateNumericStepper(parameter.GetValue<double>(), true).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.Decimal)
                {
                    control = parameterOptions.CreateNumericStepper(parameter.GetValue<double>()).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.Bytes)
                {
                    control = parameterOptions.CreateBytesPanel(parameter.GetValue<byte[]>(), parameter).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.Bool)
                {
                    control = parameterOptions.CreateCheckBox(parameter.GetValue<bool>()).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.DateTime || parameter.Type == ParameterType.Date)
                {
                    control = parameterOptions.CreateDateTimePicker(parameter.GetValue<DateTime>(), parameter.Type == ParameterType.DateTime ? DateTimePickerMode.DateTime : DateTimePickerMode.Date).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.ParameterCollection)
                {
                    control = parameterOptions.CreateParameterCollectionOpenDialogPanel(parameter.GetValue<ParameterCollection>()).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.String_IEnumerable || parameter.Type == ParameterType.String_Multiline_IEnumerable)
                {
                    control = parameterOptions.CreateList<string>(parameter, _customControls);
                }
                else if (parameter.Type == ParameterType.Int_IEnumerable)
                {
                    control = parameterOptions.CreateList<int>(parameter, _customControls);
                }
                else if (parameter.Type == ParameterType.Decimal_IEnumerable)
                {
                    control = parameterOptions.CreateList<decimal>(parameter, _customControls);
                }
                else if (parameter.Type == ParameterType.Bool_IEnumerable)
                {
                    control = parameterOptions.CreateList<bool>(parameter, _customControls);
                }
                else if (parameter.Type == ParameterType.DateTime_IEnumerable || parameter.Type == ParameterType.Date_IEnumerable)
                {
                    control = parameterOptions.CreateList<DateTime>(parameter, _customControls);
                }
                else if (parameter.Type == ParameterType.ParameterCollection_IEnumerable)
                {
                    control = parameterOptions.CreateList<ParameterCollection>(parameter, _customControls);
                }
                else if (parameter.Type == ParameterType.Enum || parameter.Type == ParameterType.SelectOne)
                {
                    var controlType = parameterOptions.SingleSelection;
                    if (parameter.Type == ParameterType.Enum)
                    {
                        controlType = parameterOptions.EnumSelection;
                    }

                    if (controlType == SelectControl.DropDown)
                    {
                        control = parameterOptions.CreateDropDown(parameter.GetValue<string>(), parameter.GetChoices()).AddParameter(parameter);
                    }
                    else if (controlType == SelectControl.ComboBox)
                    {
                        control = parameterOptions.CreateComboBox(parameter.GetValue<string>(), parameter.GetChoices()).AddParameter(parameter);
                    }
                    else if (controlType == SelectControl.GridView)
                    {
                        control = parameterOptions.CreateGridViewSingleSelect(parameter.GetValue<string>(), parameter.GetChoices().ToList()).AddParameter(parameter);
                    }
                }
                else if (parameter.Type == ParameterType.SelectMany)
                {
                    control = parameterOptions.CreateGridViewMultiSelect(parameter.GetValue<List<string>>(), parameter.GetChoices().ToList()).AddParameter(parameter);
                }

                rows.Add(new TableRow(parameterOptions.CreateLabel(parameter.Key, true), control));
            }

            return rows;
        }
    }
}

