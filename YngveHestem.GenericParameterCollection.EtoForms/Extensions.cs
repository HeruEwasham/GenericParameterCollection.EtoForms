using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Eto.Forms;
using System.IO;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
	internal static class Extensions
	{
        public static string HumanReadable(this string text)
        {
            return text.FirstCharToUpper();
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
        }

        public static Control AddParameter(this Control control, Parameter parameter)
        {
            control.Tag = parameter;
            return control;
        }

        public static Label CreateLabel(this ParameterCollectionPanelOptions options, string text, bool isParameterName = false)
        {
            return new Label
            {
                Text = isParameterName && options.ShowParameterNameAsHumanReadable ? text.HumanReadable() : text,
                Font = isParameterName ? options.ParameterNameFont : options.LabelFont,
                TextColor = isParameterName ? options.ParameterNameTextColor : options.LabelTextColor,
                BackgroundColor = isParameterName ? options.ParameterNameBackgroundColor : options.LabelBackgroundColor
            };
        }

        public static TextBox CreateTextBox(this ParameterCollectionPanelOptions options, string value)
        {
            return new TextBox
            {
                Text = value,
                ReadOnly = options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
        }

        public static TextArea CreateTextArea(this ParameterCollectionPanelOptions options, string value)
        {
            return new TextArea
            {
                Text = value,
                ReadOnly = options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
        }

        public static NumericStepper CreateNumericStepper(this ParameterCollectionPanelOptions options, double value, bool onlyInt = false)
        {
            return new NumericStepper
            {
                Value = value,
                ReadOnly = options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont,
                DecimalPlaces = onlyInt ? 0 : options.DecimalPlaces,
                Increment = onlyInt ? options.IncrementIntValue : options.IncrementDecimalValue
            };
        }

        public static CheckBox CreateCheckBox(this ParameterCollectionPanelOptions options, bool value)
        {
            return new CheckBox
            {
                Checked = value,
                Enabled = !options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
        }

        public static DropDown CreateDropDown(this ParameterCollectionPanelOptions options, string value, IEnumerable<string> choices)
        {
            var control = new DropDown
            {
                Enabled = !options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
            control.Items.AddRange(choices.Select((v) =>
            {
                return new ListItem
                {
                    Key = v,
                    Text = v,
                    Tag = v
                };
            }));
            control.SelectedKey = value;
            return control;
        }

        public static ComboBox CreateComboBox(this ParameterCollectionPanelOptions options, string value, IEnumerable<string> choices)
        {
            var control = new ComboBox
            {
                Enabled = !options.ReadOnly,
                ReadOnly = options.ComboBoxReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
            control.Items.AddRange(choices.Select((v) =>
            {
                return new ListItem
                {
                    Key = v,
                    Text = v,
                    Tag = v
                };
            }));
            control.SelectedKey = value;
            return control;
        }

        public static GridView<string> CreateGridViewSingleSelect(this ParameterCollectionPanelOptions options, string value, List<string> choices)
        {
            var control = new GridView<string>
            {
                Enabled = !options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                DataStore = choices,
                AllowEmptySelection = false,
                AllowMultipleSelection = false,
                AllowDrop = false,
                AllowColumnReordering = false,
                ShowHeader = false,
                SelectedRow = choices.FindIndex((s) =>
                {
                    return s == value;
                })
            };
            control.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<string, string>(s => s)
                },
                AutoSize = true,
                Editable = false,
                HeaderText = string.Empty,
                Resizable = true,
                Sortable = false
            });
            return control;
        }

        public static GridView<string> CreateGridViewMultiSelect(this ParameterCollectionPanelOptions options, List<string> values, List<string> choices)
        {
            var control = new GridView<string>
            {
                Enabled = !options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                DataStore = choices,
                AllowEmptySelection = true,
                AllowMultipleSelection = true,
                AllowDrop = false,
                AllowColumnReordering = false,
                ShowHeader = false
            };

            var valRows = new List<int>();

            foreach(var value in values)
            {
                valRows.Add(choices.FindIndex((s) =>
                {
                    return s == value;
                }));
            }

            control.SelectedRows = valRows;

            control.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = Binding.Property<string, string>(s => s)
                },
                AutoSize = true,
                Editable = false,
                HeaderText = string.Empty,
                Resizable = true,
                Sortable = false
            });
            return control;
        }

        public static DateTimePicker CreateDateTimePicker(this ParameterCollectionPanelOptions options, DateTime value, DateTimePickerMode mode)
        {
            return new DateTimePicker
            {
                Value = value,
                Enabled = !options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont,
                MinDate = options.MinDate,
                MaxDate = options.MaxDate,
                Mode = mode
            };
        }

        internal static TValue GetDefaultValue<TValue>(ICustomParameterControl customControl, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions options)
        {
            if (customControl != null)
            {
                return customControl.GetDefaultValue<TValue>(parameterType, additionalInfo, options);
            }

            var type = typeof(TValue);
            if (type == typeof(string))
            {
                return (TValue)(object)string.Empty;
            }
            else if (type == typeof(DateTime))
            {
                return (TValue)(object)DateTime.Now;
            }
            else
            {
                return default(TValue);
            }
        }

        public static List<T> GetListOfTagsOfType<T>(this ListItemCollection listItems)
        {
            var list = new List<T>();
            foreach(var item in listItems)
            {
                list.Add((T)((ListItem)item).Tag);
            }
            return list;
        }

        public static string GetSizeInMemory(this int bytesize)
        {
            return GetSizeInMemory((long)bytesize);
        }

        public static string GetSizeInMemory(this long bytesize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Convert.ToDouble(bytesize);
            int order = 0;
            while (len >= 1024D && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", len, sizes[order]);
        }

        public static ParameterType ToSingle(this ParameterType type)
        {
            if (type == ParameterType.String || type == ParameterType.String_IEnumerable)
            {
                return ParameterType.String;
            }
            else if (type == ParameterType.String_Multiline || type == ParameterType.String_Multiline_IEnumerable)
            {
                return ParameterType.String_Multiline;
            }
            else if (type == ParameterType.Int || type == ParameterType.Int_IEnumerable)
            {
                return ParameterType.Int;
            }
            else if (type == ParameterType.Decimal || type == ParameterType.Decimal_IEnumerable)
            {
                return ParameterType.Decimal;
            }
            else if (type == ParameterType.Bool || type == ParameterType.Bool_IEnumerable)
            {
                return ParameterType.Bool;
            }
            else if (type == ParameterType.DateTime || type == ParameterType.DateTime_IEnumerable)
            {
                return ParameterType.DateTime;
            }
            else if (type == ParameterType.Date || type == ParameterType.Date_IEnumerable)
            {
                return ParameterType.Date;
            }
            else if (type == ParameterType.ParameterCollection || type == ParameterType.ParameterCollection_IEnumerable)
            {
                return ParameterType.ParameterCollection;
            }
            else
            {
                return type;
            }
        }
    }
}

