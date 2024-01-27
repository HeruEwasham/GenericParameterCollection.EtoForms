using System;
using Eto.Drawing;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace TestProject
{
    public class SimpleColorConverter : ParameterCollectionParameterConverter<Color>
    {
        protected override bool CanConvertFromParameterCollection(ParameterCollection value)
        {
            return value.HasKeyAndCanConvertTo("r", typeof(float))
                && value.HasKeyAndCanConvertTo("g", typeof(float))
                && value.HasKeyAndCanConvertTo("b", typeof(float));
        }

        protected override bool CanConvertToParameterCollection(Color value)
        {
            return true;
        }

        protected override Color ConvertFromParameterCollection(ParameterCollection value)
        {
            var r = value.GetByKey<float>("r");
            var g = value.GetByKey<float>("g");
            var b = value.GetByKey<float>("b");
            var a = 1f;
            if (value.HasKeyAndCanConvertTo("a", typeof(double)))
            {
                a = value.GetByKey<int>("a");
            }
            return new Color(r, g, b, a);
        }

        protected override ParameterCollection ConvertToParameterCollection(Color value)
        {
            return new ParameterCollection
            {
                { "r", value.R },
                { "g", value.G },
                { "b", value.B },
                { "a", value.A },
            };
        }
    }
}

