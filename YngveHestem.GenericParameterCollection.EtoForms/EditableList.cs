using System;
using Eto.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
	internal class EditableList<TValue> : Panel
	{
        public List<TValue> Value { get; private set; }

        private ParameterType _pTypeSingle;
        private ParameterCollection _pAdditionalInfo;
        private ParameterCollectionPanelOptions _options;
        private List<ICustomParameterControl> _customControls;
        private ICustomParameterControl _customControl;
        private ListBox _listBox;
        private TValue _defaultValue;


        public EditableList(Parameter parameter, ParameterCollectionPanelOptions options, List<ICustomParameterControl> customControls)
		{
            if (options == null)
            {
                options = new ParameterCollectionPanelOptions();
            }
            Value = parameter.GetValue<List<TValue>>();
            _pTypeSingle = parameter.Type.ToSingle();
            _pAdditionalInfo = parameter.GetAdditionalInfo();
            _options = options;
            _customControls = customControls;

            _listBox = new ListBox
            {
                BackgroundColor = options.NormalBackgroundColor,
                TextColor = options.NormalTextColor,
                Font = options.NormalFont,
                Enabled = !options.ReadOnly,
            };

            _customControl = null;
            if (customControls != null)
            {
                _customControl = customControls.FirstOrDefault(cc => cc.CanGetDefaultValue(typeof(TValue), _pTypeSingle, _pAdditionalInfo, options));
            }

            if (_customControl != null)
            {
                _listBox.Items.AddRange(Value.Select(v =>
                {
                    var text = _customControl.ToString(v, _pTypeSingle, _pAdditionalInfo, options);
                    return new ListItem
                    {
                        Key = text,
                        Text = text,
                        Tag = v
                    };
                }));
            }
            else if (_pTypeSingle == ParameterType.DateTime)
            {
                _listBox.Items.AddRange(Value.Select(v =>
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
            else if (_pTypeSingle == ParameterType.Date)
            {
                _listBox.Items.AddRange(Value.Select(v =>
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
                _listBox.Items.AddRange(Value.Select(v =>
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

            _defaultValue = Extensions.GetDefaultValue<TValue>(_customControl, _pTypeSingle, _pAdditionalInfo, options);

            if (_pAdditionalInfo != null)
            {
                if (_pAdditionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(TValue)))
                {
                    _defaultValue = _pAdditionalInfo.GetByKey<TValue>("defaultValue");
                }
            }

            var stackLayout = new StackLayout();

            if (!_options.ReadOnly)
            {
                var addButton = new Button
                {
                    Text = "Add",
                    Font = options.SubmitAddFont,
                    TextColor = options.SubmitAddTextColor,
                    BackgroundColor = options.SubmitAddBackgroundColor
                };
                addButton.Click += AddButton_Click;
                stackLayout.Items.Add(addButton);

                var editButton = new Button
                {
                    Text = "Edit",
                    Font = options.EditFont,
                    TextColor = options.EditTextColor,
                    BackgroundColor = options.EditBackgroundColor
                };
                editButton.Click += EditButton_Click;
                stackLayout.Items.Add(editButton);

                var deleteButton = new Button
                {
                    Text = "Delete",
                    Font = options.CancelDeleteFont,
                    TextColor = options.CancelDeleteTextColor,
                    BackgroundColor = options.CancelDeleteBackgroundColor,
                };
                deleteButton.Click += DeleteButton_Click;
                stackLayout.Items.Add(deleteButton);
            }

            stackLayout.Items.Add(_listBox);
            BackgroundColor = options.NormalBackgroundColor;
            Content = stackLayout;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_listBox.SelectedIndex >= 0)
            {
                _listBox.Items.RemoveAt(_listBox.SelectedIndex);
                Value = _listBox.Items.GetListOfTagsOfType<TValue>();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_listBox.SelectedIndex >= 0)
            {
                var item = (ListItem)_listBox.Items[_listBox.SelectedIndex];
                var dialog = new ControlDialog<TValue>((TValue)item.Tag, _options, _pTypeSingle, _pAdditionalInfo, _customControls);
                var value = dialog.ShowModal();
                if (dialog.Success)
                {
                    var text = GetText(value);
                    item.Key = text;
                    item.Text = text;
                    item.Tag = value;
                    Value = _listBox.Items.GetListOfTagsOfType<TValue>();
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var dialog = new ControlDialog<TValue>(_defaultValue, _options, _pTypeSingle, _pAdditionalInfo, _customControls);
            var value = dialog.ShowModal();
            if (dialog.Success)
            {
                var text = GetText(value);
                _listBox.Items.Add(new ListItem
                {
                    Key = text,
                    Text = text,
                    Tag = value
                });
                Value = _listBox.Items.GetListOfTagsOfType<TValue>();
            }
        }

        private string GetText(TValue value)
        {
            var text = value.ToString();
            if (_customControl != null)
            {
                text = _customControl.ToString(value, _pTypeSingle, _pAdditionalInfo, _options);
            }
            else if (_pTypeSingle == ParameterType.DateTime)
            {
                text = ((DateTime)(object)value).ToString(_options.DateTimeFormat);
            }
            else if (_pTypeSingle == ParameterType.Date)
            {
                text = ((DateTime)(object)value).ToString(_options.DateFormat);
            }
            return text;
        }
    }
}

