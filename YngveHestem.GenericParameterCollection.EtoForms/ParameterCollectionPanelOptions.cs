using System;
using Eto.Drawing;
using YngveHestem.GenericParameterCollection.EtoForms.BytesPreview.Core;

namespace YngveHestem.GenericParameterCollection.EtoForms
{
    public class ParameterCollectionPanelOptions
    {
        /// <summary>
        /// Will additional parameters on a parameter override theese options if set (example, if a parameter has "readOnly" parameter set, should this override the readOnly-parameter set here).
        /// </summary>
        public bool AdditionalInfoWillOverride = true;

        /// <summary>
        /// Convert the parameter name to a human readable format. If set to false, the name will be shown as written, while if set to true, it will be altered to be read easily by a human, like setting first character to an upper character.
        /// </summary>
        public bool ShowParameterNameAsHumanReadable = true;

        /// <summary>
        /// Should all controls be readOnly as default.
        /// </summary>
        public bool ReadOnly = false;

        /// <summary>
        /// Font used on text.
        /// </summary>
        public Font NormalFont = new Font(SystemFont.Default);

        /// <summary>
        /// Font used on labels.
        /// </summary>
        public Font LabelFont = new Font(SystemFont.Label);

        /// <summary>
        /// Font used on the parameter names.
        /// </summary>
        public Font ParameterNameFont = new Font(SystemFont.Bold);

        /// <summary>
        /// Font used on submit or add buttons text.
        /// </summary>
        public Font SubmitAddFont = new Font(SystemFont.Default);

        /// <summary>
        /// Font used on edit buttons text.
        /// </summary>
        public Font EditFont = new Font(SystemFont.Default);

        /// <summary>
        /// Font used on cancel or delete buttons text.
        /// </summary>
        public Font CancelDeleteFont = new Font(SystemFont.Default);

        /// <summary>
        /// Color used on text.
        /// </summary>
        public Color NormalTextColor = Colors.Black;

        /// <summary>
        /// Color used on label text.
        /// </summary>
        public Color LabelTextColor = Colors.Black;

        /// <summary>
        /// Color used on parameter names text.
        /// </summary>
        public Color ParameterNameTextColor = Colors.Black;

        /// <summary>
        /// Color used on submit or add buttons text.
        /// </summary>
        public Color SubmitAddTextColor = Colors.Black;

        /// <summary>
        /// Color used on edit buttons text.
        /// </summary>
        public Color EditTextColor = Colors.Black;

        /// <summary>
        /// Color used on cancel or delete buttons text.
        /// </summary>
        public Color CancelDeleteTextColor = Colors.Black;

        /// <summary>
        /// Color used on control background.
        /// </summary>
        public Color NormalBackgroundColor = Colors.Transparent;

        /// <summary>
        /// Color used on label background.
        /// </summary>
        public Color LabelBackgroundColor = Colors.Transparent;

        /// <summary>
        /// Color used on parameter names background.
        /// </summary>
        public Color ParameterNameBackgroundColor = Colors.Transparent;

        /// <summary>
        /// Color used on submit or add buttons background.
        /// </summary>
        public Color SubmitAddBackgroundColor = Colors.LimeGreen;

        /// <summary>
        /// Color used on edit buttons background.
        /// </summary>
        public Color EditBackgroundColor = Colors.LightBlue;

        /// <summary>
        /// Color used on cancel or delete buttons background.
        /// </summary>
        public Color CancelDeleteBackgroundColor = Colors.Red;

        /// <summary>
        /// How much should the gui increment the value on an int value.
        /// </summary>
        public int IncrementIntValue = 1;

        /// <summary>
        /// How much should the gui increment the value on a value that can contain decimal places.
        /// </summary>
        public double IncrementDecimalValue = 0.1;

        /// <summary>
        /// How many decimal places should be shown in appropiate controls.
        /// </summary>
        public int DecimalPlaces = 2;

        /// <summary>
        /// The lowest date to be selected in appropiate controls.
        /// </summary>
        public DateTime MinDate = DateTime.MinValue;

        // <summary>
        /// The maximum date to be selected in appropiate controls.
        /// </summary>
        public DateTime MaxDate = DateTime.MaxValue;

        /// <summary>
        /// The control/method to use when selecting an enum-value.
        /// </summary>
        public SelectControl EnumSelection = SelectControl.DropDown;

        /// <summary>
        /// The control/method to use when selecting a single selection from a list of choices.
        /// </summary>
        public SelectControl SingleSelection = SelectControl.DropDown;

        /// <summary>
        /// Should a ComboBox be in read-only mode? Also so you can not write in your own words.
        /// </summary>
        public bool ComboBoxReadOnly = true;

        /// <summary>
        /// The format to use on a DateTime when converting to string when the parameter is for both date and time.
        /// </summary>
        public string DateTimeFormat = "g";

        /// <summary>
        /// The format to use on a DateTime when converting to string when the parameter is for only date.
        /// </summary>
        public string DateFormat = "d";

        /// <summary>
        /// Defines what types of file extensions is supported when selecting files for ParameterType.Bytes. All must have a leading .
        /// Empty string[] means all types supported/no filter added.
        /// </summary>
        public string[] SupportedFileExtensions = null;

        public IBytesPreview[] BytesPreviews = null;

        public static ParameterCollectionPanelOptions CreateCopy(ParameterCollectionPanelOptions options)
        {
            return new ParameterCollectionPanelOptions
            {
                AdditionalInfoWillOverride = options.AdditionalInfoWillOverride,
                ShowParameterNameAsHumanReadable = options.ShowParameterNameAsHumanReadable,
                ReadOnly = options.ReadOnly,
                NormalFont = options.NormalFont,
                LabelFont = options.LabelFont,
                ParameterNameFont = options.ParameterNameFont,
                SubmitAddFont = options.SubmitAddFont,
                EditFont = options.EditFont,
                CancelDeleteFont = options.CancelDeleteFont,
                NormalTextColor = options.NormalTextColor,
                LabelTextColor = options.LabelTextColor,
                ParameterNameTextColor = options.ParameterNameTextColor,
                SubmitAddTextColor = options.SubmitAddTextColor,
                EditTextColor = options.EditTextColor,
                CancelDeleteTextColor = options.CancelDeleteTextColor,
                NormalBackgroundColor = options.NormalBackgroundColor,
                LabelBackgroundColor = options.LabelBackgroundColor,
                ParameterNameBackgroundColor = options.ParameterNameBackgroundColor,
                SubmitAddBackgroundColor = options.SubmitAddBackgroundColor,
                EditBackgroundColor = options.EditBackgroundColor,
                CancelDeleteBackgroundColor = options.CancelDeleteBackgroundColor,
                IncrementIntValue = options.IncrementIntValue,
                IncrementDecimalValue = options.IncrementDecimalValue,
                DecimalPlaces = options.DecimalPlaces,
                MinDate = options.MinDate,
                MaxDate = options.MaxDate,
                EnumSelection = options.EnumSelection,
                SingleSelection = options.SingleSelection,
                ComboBoxReadOnly = options.ComboBoxReadOnly,
                DateTimeFormat = options.DateTimeFormat,
                DateFormat = options.DateFormat,
                SupportedFileExtensions = options.SupportedFileExtensions,
                BytesPreviews = options.BytesPreviews
            };
        }

        /// <summary>
        /// Creates a copy from a list of parameters.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <param name="defaultOptions">A list of options to use if correct parameter is not found. if this is null, the default parameter is used.</param>
        /// <returns></returns>
        public static ParameterCollectionPanelOptions CreateFromParameterCollection(ParameterCollection parameters, ParameterCollectionPanelOptions defaultOptions = null)
        {
            var result = new ParameterCollectionPanelOptions();
            if (defaultOptions != null)
            {
                result = CreateCopy(defaultOptions);
            }

            if (parameters != null)
            {
                if (parameters.HasKeyAndCanConvertTo("humanReadable", typeof(bool)))
                {
                    result.ShowParameterNameAsHumanReadable = parameters.GetByKey<bool>("humanReadable");
                }

                if (parameters.HasKeyAndCanConvertTo("readOnly", typeof(bool)))
                {
                    result.ReadOnly = parameters.GetByKey<bool>("readOnly");
                }

                if (parameters.HasKeyAndCanConvertTo("increment", typeof(double)))
                {
                    var inc = parameters.GetByKey<double>("increment");
                    if (inc < 1)
                    {
                        result.IncrementIntValue = 1;
                    }
                    else
                    {
                        result.IncrementIntValue = (int)inc;
                    }
                    result.IncrementDecimalValue = inc;
                }

                if (parameters.HasKeyAndCanConvertTo("incrementInt", typeof(int)))
                {
                    result.IncrementIntValue = parameters.GetByKey<int>("incrementInt");
                }

                if (parameters.HasKeyAndCanConvertTo("incrementDecimal", typeof(double)))
                {
                    result.IncrementDecimalValue = parameters.GetByKey<double>("incrementDecimal");
                }

                if (parameters.HasKeyAndCanConvertTo("decimalPlaces", typeof(int)))
                {
                    result.DecimalPlaces = parameters.GetByKey<int>("decimalPlaces");
                }

                if (parameters.HasKeyAndCanConvertTo("minDate", typeof(DateTime)))
                {
                    result.MinDate = parameters.GetByKey<DateTime>("minDate");
                }

                if (parameters.HasKeyAndCanConvertTo("maxDate", typeof(DateTime)))
                {
                    result.MaxDate = parameters.GetByKey<DateTime>("maxDate");
                }

                if (parameters.HasKeyAndCanConvertTo("enumSelection", typeof(SelectControl)))
                {
                    result.EnumSelection = parameters.GetByKey<SelectControl>("enumSelection");
                }

                if (parameters.HasKeyAndCanConvertTo("singleSelection", typeof(SelectControl)))
                {
                    result.SingleSelection = parameters.GetByKey<SelectControl>("singleSelection");
                }

                if (parameters.HasKeyAndCanConvertTo("comboBoxReadOnly", typeof(bool)))
                {
                    result.ComboBoxReadOnly = parameters.GetByKey<bool>("comboBoxReadOnly");
                }

                if (parameters.HasKeyAndCanConvertTo("dateTimeFormat", typeof(string)))
                {
                    result.DateTimeFormat = parameters.GetByKey<string>("dateTimeFormat");
                }

                if (parameters.HasKeyAndCanConvertTo("dateFormat", typeof(string)))
                {
                    result.DateFormat = parameters.GetByKey<string>("dateFormat");
                }

                if  (parameters.HasKeyAndCanConvertTo("supportedExtensions", typeof(string[])))
                {
                    result.SupportedFileExtensions = parameters.GetByKey<string[]>("supportedExtensions");
                }
            }
            return result;
        }
    }
}