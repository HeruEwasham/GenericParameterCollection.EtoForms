using System;
using Eto.Forms;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
	public interface ICustomParameterControl
	{
        /// <summary>
        /// Can this class get the value from the given control for a specified parameter.
        /// </summary>
        /// <param name="controlType">The type of control we have.</param>
        /// <param name="control">The control we want to get the value from.</param>
        /// <param name="returnType">The parameterType the parameter has. Do it expect an Int, Double, Date, ParameterCollection, etc.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
        /// <returns></returns>
        bool CanGetValue(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo);

        /// <summary>
        /// Can this class create a default value based on the given inputs.
        /// </summary>
        /// <param name="returnType">The specific type to return.</param>
        /// <param name="returnParameterType">The parameterType the parameter has. Do it expect an Int, Double, Date, ParameterCollection, etc.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
        /// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
        /// <returns></returns>
        bool CanGetDefaultValue(Type returnType, ParameterType returnParameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);

		/// <summary>
		/// Can this class create a control for this parameter.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		/// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
		/// <returns></returns>
		bool CanCreateControl(Parameter parameter, ParameterCollectionPanelOptions parameterOptions);

        /// <summary>
		/// Can this class create a control for this parameter.
		/// </summary>
		/// <param name="currentValue">Current value.</param>
        /// <param name="parameterType">The parameter-type.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
		/// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
		/// <returns></returns>
		bool CanCreateControl(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);

        /// <summary>
        /// Get the value from the given control for a specified parameter.
        /// </summary>
        /// <param name="controlType">The type of control we have.</param>
        /// <param name="control">The control we want to get the value from.</param>
        /// <param name="returnType">The parameterType the parameter has. Do it expect an Int, Double, Date, ParameterCollection, etc.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
        /// <returns>The given value in an expected type, so it will convert correctly when inserted in a new parameter with the given criteria.</returns>
        object GetValue(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo);

        /// <summary>
        /// Get what should be the default value. If for instance a new object is created.
        /// </summary>
        /// <param name="returnType">The type to return.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
        /// <param name="options">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
        /// <returns>The value encoded to the correct returnType.</returns>
        TReturnType GetDefaultValue<TReturnType>(ParameterType returnType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions options);

        /// <summary>
        /// Get updated additional info for a specified parameter.
        /// </summary>
        /// <param name="controlType">The type of control we have.</param>
        /// <param name="control">The control we want to get the value from.</param>
        /// <param name="returnType">The parameterType the parameter has. Do it expect an Int, Double, Date, ParameterCollection, etc.</param>
        /// <param name="additionalInfo">The additionalInfo from the old parameter.</param>
        /// <returns>The updated additionalParameters. In most cases you might only need to return the old parameters additionalInfo, but this method might update some parameters if used.</returns>
        ParameterCollection GetAdditionalInfo(Type controlType, Control control, ParameterType returnType, ParameterCollection additionalInfo);

        /// <summary>
        /// Create a control for this parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter. Any settings here should be used on any part of the controls to follow users instructions if possible.</param>
        /// <returns>A specialized control which is used to get the correct value instead of the default one.</returns>
        Control CreateControl(Parameter parameter, ParameterCollectionPanelOptions parameterOptions);

        /// <summary>
		/// Create a control for this parameter.
		/// </summary>
		/// <param name="currentValue">Current value.</param>
        /// <param name="parameterType">The parameter-type.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
		/// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
		/// <returns></returns>
		Control CreateControl(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);

        /// <summary>
        /// Get the current value as string.
        /// </summary>
        /// <param name="currentValue">Current value.</param>
        /// <param name="parameterType">The parameter-type.</param>
        /// <param name="additionalInfo">The additional info on the parameter if any or null if not.</param>
        /// <param name="parameterOptions">A set of options, based on both the options set by user globally, but also evt. any changes to theese options given for this parameter.</param>
        /// <returns></returns>
        string ToString(object currentValue, ParameterType parameterType, ParameterCollection additionalInfo, ParameterCollectionPanelOptions parameterOptions);
    }
}

