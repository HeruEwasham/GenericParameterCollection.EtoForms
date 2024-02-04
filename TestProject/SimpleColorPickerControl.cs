using System;
using Eto.Drawing;
using Eto.Forms;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.EtoForms;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace TestProject
{
    public class SimpleColorPickerControl : CustomParameterControlBase
    {
        private static readonly IParameterValueConverter[] _colorConverter = new[] { new SimpleColorConverter() };

        public override bool CanGetDefaultValue(Type returnType, ParameterType returnParameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions)
        {
            return CanCreateColorPicker(returnParameterType, additionalInfo);
        }

        public override bool CanGetValue(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo)
        {
            return controlType == typeof(ColorPicker) && returnType == ParameterType.ParameterCollection;
        }

        public override Control CreateControl(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions)
        {
            if (!CanCreateColorPicker(parameterType, additionalInfo) || currentValue.GetType() != typeof(ParameterCollection) || !((ParameterCollection)currentValue).CanConvertToObject<Color>(_colorConverter))
            {
                throw new ArgumentException("Tried to create a color picker from a non-color value");
            }

            var allowAlpha = true;
            if (additionalInfo.HasKeyAndCanConvertTo("allowAlpha", typeof(bool)))
            {
                allowAlpha = additionalInfo.GetByKey<bool>("allowAlpha");
            }

            return new ColorPicker
            {
                Value = ((ParameterCollection)currentValue).ToObject<Color>(_colorConverter),
                Enabled = !parameterOptions.ReadOnly,
                AllowAlpha = allowAlpha
            };
        }

        public override Control CreateControl(Parameter parameter, ParameterCollectionPanelOptions parameterOptions)
        {
            return CreateControl(parameter.GetValue<ParameterCollection>(), parameter.Type, parameter.GetAdditionalInfo(), parameterOptions);
        }

        public override TReturnType GetDefaultValue<TReturnType>(ParameterType returnType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions options)
        {
            if (!CanCreateColorPicker(returnType, additionalInfo))
            {
                throw new ArgumentException("Can not get default value with theese parameters by using " + nameof(SimpleColorPickerControl));
            }

            return (TReturnType)(object)ParameterCollection.FromObject(Colors.Black, _colorConverter);
        }

        public override object GetValue(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo)
        {
            if (!(controlType == typeof(ColorPicker) && returnType == ParameterType.ParameterCollection))
            {
                throw new ArgumentException("Can not get the value of " + controlType.GetType().ToString() + "and convert to " + Enum.GetName(returnType) + " with " + nameof(SimpleColorPickerControl));
            }

            return ParameterCollection.FromObject(((ColorPicker)control).Value, _colorConverter);
        }

        public override string ToString(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions)
        {
            var v = ((ParameterCollection)currentValue);
            if (!v.CanConvertToObject<Color>(_colorConverter))
            {
                throw new ArgumentException("Can not set string value with a parameter with theese parameters by using " + nameof(SimpleColorPickerControl));
            }

            return v.ToObject<Color>(_colorConverter).ToString();
        }

        protected override bool CanCreateControl(ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions)
        {
            return CanCreateColorPicker(parameterType, additionalInfo);
        }

        private bool CanCreateColorPicker(ParameterType parameterType, ParameterCollection additionalInfo)
        {
            return parameterType == ParameterType.ParameterCollection  && additionalInfo != null && additionalInfo.HasKeyAndCanConvertTo("type", typeof(string)) && additionalInfo.GetByKey<string>("type") == "color";
        }
    }
}

