﻿using System;
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

        public static Panel CreateBytesPanel(this ParameterCollectionPanelOptions options, byte[] value, Parameter parameter)
        {
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
                selectButton.Click += (sender, e) =>
                {
                    var oldParameter = (Parameter)((Control)sender).Parent.Parent.Tag;
                    var dialog = new OpenFileDialog
                    {
                        Title = "Select file for " + oldParameter.Key,
                        CheckFileExists = true,
                        MultiSelect = false
                    };
                    if (options.SupportedFileExtensions != null && options.SupportedFileExtensions.Length > 0)
                    {
                        dialog.Filters.Add(new FileFilter("Supported file types", options.SupportedFileExtensions));
                    }

                    if (dialog.ShowDialog(selectButton) == DialogResult.Ok)
                    {
                        var bytes = File.ReadAllBytes(dialog.FileName);
                        var filename = Path.GetFileName(dialog.FileName);
                        ((Control)sender).Parent.Tag = bytes;
                        ((Control)sender).Tag = filename;
                        var stackLayout = ((StackLayout)((Control)sender).Parent);
                        ((Label)stackLayout.Items[1].Control).Text = "Selected item has size: " + bytes.Length.GetSizeInMemory() + Environment.NewLine + "Filename: " + filename;
                        if (stackLayout.Items.Count == 3)
                        {
                            stackLayout.Items.RemoveAt(2);
                        }
                        if (options.BytesPreviews != null)
                        {
                            var ext = Path.GetExtension(filename);
                            var previewInterface = options.BytesPreviews.FirstOrDefault((o) =>
                            {
                                return o.CanPreviewBytes(ext, bytes);
                            });
                            if (previewInterface != null)
                            {
                                stackLayout.Items.Add(previewInterface.GetPreviewControl(ext, bytes));
                            }
                        }
                    }
                };
            }
            var statusText = "Selected item has size: " + value.Length.GetSizeInMemory();
            string extension = null;
            if (parameter.HasAdditionalInfo())
            {
                var oAdI = parameter.GetAdditionalInfo();
                if (oAdI.HasKeyAndCanConvertTo("filename", typeof(string)))
                {
                    statusText += Environment.NewLine + "Filename: " + oAdI.GetByKey<string>("filename");
                }
                if (oAdI.HasKeyAndCanConvertTo("extension", typeof(string)))
                {
                    extension = oAdI.GetByKey<string>("extension");
                }
            }

            var stacklayout = new StackLayout
            {
                Tag = value,
                Items =
                {
                    selectButton,
                    options.CreateLabel(statusText)
                }
            };

            if (options.BytesPreviews != null)
            {
                var previewInterface = options.BytesPreviews.FirstOrDefault((o) =>
                {
                    return o.CanPreviewBytes(extension, value);
                });
                if (previewInterface != null)
                {
                    stacklayout.Items.Add(previewInterface.GetPreviewControl(extension, value));
                }
            }

            return new Panel
            {
                Content = stacklayout
            };
        }

        public static Panel CreateParameterCollectionOpenDialogPanel(this ParameterCollectionPanelOptions options, ParameterCollection value)
        {
            var showEditButton = new Button
            {
                Text = "Show/Edit",
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont
            };
            showEditButton.Click += (sender, e) =>
            {
                var dialog = new ParameterCollectionDialog((ParameterCollection)((Control)sender).Parent.Tag, options);
                var newValue = dialog.ShowModal();
                if (newValue != null)
                {
                    ((Control)sender).Parent.Tag = newValue;
                }
            };
            return new Panel
            {
                Enabled = !options.ReadOnly,
                BackgroundColor = options.NormalBackgroundColor,
                Content = new StackLayout
                {
                    Tag = value,
                    Items =
                    {
                        showEditButton
                    }
                }
            };
        }

        public static Panel CreateList<TValue>(this ParameterCollectionPanelOptions options, Parameter parameter, List<ICustomParameterControl> customControls)
        {
            if (options == null)
            {
                options = new ParameterCollectionPanelOptions();
            }
            var currentValue = parameter.GetValue<List<TValue>>();
            var pTypeSingle = parameter.Type.ToSingle();
            var listBox = new ListBox
            {
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont,
                Enabled = !options.ReadOnly,
            };
            if (pTypeSingle == ParameterType.DateTime)
            {
                listBox.Items.AddRange(currentValue.Select(v =>
                {
                    var text = ((DateTime)(object)v).ToString(options.DateTimeFormat);
                    return new ListItem
                    {
                        Key = text,
                        Text = text,
                        Tag = v
                    };
                }));
            }
            else if (pTypeSingle == ParameterType.Date)
            {
                listBox.Items.AddRange(currentValue.Select(v =>
                {
                    var text = ((DateTime)(object)v).ToString(options.DateFormat);
                    return new ListItem
                    {
                        Key = text,
                        Text = text,
                        Tag = v
                    };
                }));
            }
            else
            {
                listBox.Items.AddRange(currentValue.Select(v =>
                {
                    var text = v.ToString();
                    return new ListItem
                    {
                        Key = text,
                        Text = text,
                        Tag = v
                    };
                }));
            }

            var defaultValue = GetDefaultValue<TValue>();
            if (parameter.HasAdditionalInfo())
            {
                var pai = parameter.GetAdditionalInfo();
                if (pai.HasKeyAndCanConvertTo("defaultValue", typeof(TValue)))
                {
                    defaultValue = pai.GetByKey<TValue>("defaultValue");
                }
            }

            var addButton = new Button
            {
                Text = "Add",
                Font = options.SubmitAddFont,
                TextColor = options.SubmitAddTextColor,
                BackgroundColor = options.SubmitAddBackgroundColor
            };
            addButton.Click += (s, e) =>
            {
                var dialog = new ControlDialog<TValue>(defaultValue, options, pTypeSingle, parameter.GetAdditionalInfo(), customControls);
                var value = dialog.ShowModal();
                if (dialog.Success)
                {
                    var text = value.ToString();
                    if (pTypeSingle == ParameterType.DateTime)
                    {
                        text = ((DateTime)(object)value).ToString(options.DateTimeFormat);
                    }
                    else if (pTypeSingle == ParameterType.Date)
                    {
                        text = ((DateTime)(object)value).ToString(options.DateFormat);
                    }
                    listBox.Items.Add(new ListItem
                    {
                        Key = text,
                        Text = text,
                        Tag = value
                    });
                    listBox.Parent.Tag = listBox.Items.GetListOfTagsOfType<TValue>();
                }
            };
            var editButton = new Button
            {
                Text = "Edit",
                Font = options.EditFont,
                TextColor = options.EditTextColor,
                BackgroundColor = options.EditBackgroundColor
            };
            editButton.Click += (s, e) =>
            {
                if (listBox.SelectedIndex >= 0)
                {
                    var item = (ListItem)listBox.Items[listBox.SelectedIndex];
                    var dialog = new ControlDialog<TValue>((TValue)item.Tag, options, parameter.Type.ToSingle(), parameter.GetAdditionalInfo(), customControls);
                    var value = dialog.ShowModal();
                    if (dialog.Success)
                    {
                        item.Key = value.ToString();
                        item.Text = value.ToString();
                        item.Tag = value;
                        listBox.Parent.Tag = listBox.Items.GetListOfTagsOfType<TValue>();
                    }
                }
            };
            var deleteButton = new Button
            {
                Text = "Delete",
                Font = options.CancelDeleteFont,
                TextColor = options.CancelDeleteTextColor,
                BackgroundColor = options.CancelDeleteBackgroundColor,
            };
            deleteButton.Click += (s, e) =>
            {
                if (listBox.SelectedIndex >= 0)
                {
                    listBox.Items.RemoveAt(listBox.SelectedIndex);
                    listBox.Parent.Tag = listBox.Items.GetListOfTagsOfType<TValue>();
                }
            };
            return new Panel
            {
                BackgroundColor = options.NormalBackgroundColor,
                Tag = parameter,
                Content = new StackLayout
                {
                    Tag = currentValue,
                    Items =
                    {
                        addButton,
                        editButton,
                        deleteButton,
                        listBox
                    }
                }
            };
        }

        private static TValue GetDefaultValue<TValue>()
        {
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

