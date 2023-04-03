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

        public ParameterCollectionPanel(ParameterCollection parameters, ParameterCollectionPanelOptions options = null)
        {
            _parameters = parameters;
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

                result.Add(new Parameter(oldParameter.Key, GetValue(row.Cells[1].Control, oldParameter.Type, oldParameter), oldParameter.Type, oldParameter.GetAdditionalInfo()));
            }

            return result;
        }

        private object GetValue(Control control, ParameterType returnType, Parameter oldParameter)
        {
            var type = control.GetType();
            if (type == typeof(TextBox))
            {
                return ((TextBox)control).Text;
            }
            else if (type == typeof(TextArea))
            {
                return ((TextArea)control).Text;
            }
            else if (type == typeof(NumericStepper))
            {
                if (returnType == ParameterType.Int)
                {
                    return (int)((NumericStepper)control).Value;
                }
                else if (returnType == ParameterType.Float)
                {
                    return (float)((NumericStepper)control).Value;
                }
                else if (returnType == ParameterType.Long)
                {
                    return (long)((NumericStepper)control).Value;
                }
                return ((NumericStepper)control).Value;
            }
            else if (type == typeof(Panel))
            {
                return ((Panel)control).Content.Tag;
            }
            else if (type == typeof(CheckBox))
            {
                return ((CheckBox)control).Checked;
            }
            else if (type == typeof(DateTimePicker))
            {
                return ((DateTimePicker)control).Value.Value;
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
                return param;
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
                return param;
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
                return result;
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

                if (parameter.Type == ParameterType.String)
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
                else if (parameter.Type == ParameterType.Float
                    || parameter.Type == ParameterType.Double
                    || parameter.Type == ParameterType.Long)
                {
                    control = parameterOptions.CreateNumericStepper(parameter.GetValue<double>()).AddParameter(parameter);
                }
                else if (parameter.Type == ParameterType.Bytes)
                {
                    control = parameterOptions.CreateBytesPanel(parameter.GetValue<byte[]>()).AddParameter(parameter);
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
                    control = parameterOptions.CreateList<string>(parameter);
                }
                else if (parameter.Type == ParameterType.Int_IEnumerable)
                {
                    control = parameterOptions.CreateList<int>(parameter);
                }
                else if (parameter.Type == ParameterType.Float_IEnumerable)
                {
                    control = parameterOptions.CreateList<float>(parameter);
                }
                else if (parameter.Type == ParameterType.Double_IEnumerable)
                {
                    control = parameterOptions.CreateList<double>(parameter);
                }
                else if (parameter.Type == ParameterType.Long_IEnumerable)
                {
                    control = parameterOptions.CreateList<long>(parameter);
                }
                else if (parameter.Type == ParameterType.Bool_IEnumerable)
                {
                    control = parameterOptions.CreateList<bool>(parameter);
                }
                else if (parameter.Type == ParameterType.DateTime_IEnumerable || parameter.Type == ParameterType.Date_IEnumerable)
                {
                    control = parameterOptions.CreateList<DateTime>(parameter);
                }
                else if (parameter.Type == ParameterType.ParameterCollection_IEnumerable)
                {
                    control = parameterOptions.CreateList<ParameterCollection>(parameter);
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

