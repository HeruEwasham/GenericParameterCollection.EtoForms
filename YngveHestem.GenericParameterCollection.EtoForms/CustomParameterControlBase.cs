using System;
using Eto.Forms;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
    public abstract class CustomParameterControlBase : ICustomParameterControl
    {
        public virtual bool CanCreateControl(Parameter parameter, ParameterCollectionPanelOptions parameterOptions)
        {
            return CanCreateControl(parameter.Type, parameter.GetAdditionalInfo(), parameterOptions);
        }

        public virtual bool CanCreateControl(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions)
        {
            return CanCreateControl(parameterType, additionalInfo, parameterOptions);
        }

        public abstract bool CanGetDefaultValue(Type returnType, ParameterType returnParameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);

        public abstract bool CanGetValue(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo);

        public abstract Control CreateControl(Parameter parameter, ParameterCollectionPanelOptions parameterOptions);

        public abstract Control CreateControl(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);

        public virtual ParameterCollection GetAdditionalInfo(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo)
        {
            return additionalInfo;
        }

        public abstract TReturnType GetDefaultValue<TReturnType>(ParameterType returnType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions options);

        public abstract object GetValue(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo);

        public abstract string ToString(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);

        /// <summary>
        /// Can this class create a control for this parameter. This omits the value-parameter as this is most likely not neccessarry to use. If you should need it, you must override the other CanCreateControl-methods, that do contain it.
        /// </summary>
        /// <param name="parameterType">The parameter-type.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
        /// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
        /// <returns></returns>
        protected abstract bool CanCreateControl(ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);
    }
}

