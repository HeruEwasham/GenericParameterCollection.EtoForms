# GenericParameterCollection.EtoForms

This provides controls for using [GenericParameterCollection](https://github.com/HeruEwasham/GenericParameterCollection) in the GUI-framework [Eto.Forms](https://github.com/picoe/Eto).

## How to use this package

The easiest way to use the package is to download it from nuget: https://www.nuget.org/packages/YngveHestem.GenericParameterCollection.EtoForms

## Main features/controls

### ParameterCollectionPanel

This is a control based on Eto.Forms Panel-class. This is the main control that handles editing a given ParameterCollection object.

#### Methods

Here is a list of some interesting methods.

##### GetParameters()

Call this when you want to get the updated parameters back. This creates a new ParameterCollection-object with all custom converters and other information copied from the original ParameterCollection-object.

### ParameterCollectionDialog

This is a dialog that implements the ParameterCollectionPanel and a button to submit and a button to cancel. See this example for how to use this (the variable parameterCollection contains the ParameterCollection):

```
var dialog = new ParameterCollectionDialog(parameterCollection);
var value = dialog.ShowModal();
if (value != null)
{
	// Submit/OK-button was clicked. The value-variable contains the updated ParameterCollection.
}
```

### Options

The controls let you provide a ParameterCollectionPanelOptions. Here you can define some customisation of how the control looks and works. Most are both self explanatory and well documented in code. Some of theese options can also for a specific parameter if the option AdditionalInfoWillOverride is set to true (default is true). Then one or more of the given parameters below be given in a parameters additionalInfo.

#### Different options

Here is a list of parameters that can either be defined in ParameterCollectionPanelOptions or as a ParameterCollection (some can only be given in one, while many can be given both ways).

If you define this in a ParameterCollection-ParameterType, the changes will affect all parameters in that ParameterCollection.

Mark that it exist multiple parameters that currently can not be defined in a ParameterCollection. This must be defined in the object directly, and can't be changed via a ParameterCollection. This list only contains the parameters that can be changed.

| Variable name in option-class | Parameter key | Type | Description | Default value in option-class |
| ----------- | ----------- | ----------- | ----------- | ----------- |
| AdditionalInfoWillOverride |  | bool | Can parameters from a ParameterCollection, like AdditionalInfo from a parameter, override the values defined in this options-object | true |
| ShowParameterNameAsHumanReadable | humanReadable | bool | Change if the parameter-key should be tried to be written more human readable | True |
| ReadOnly | readOnly | bool | If true, the control that shows the parameters value should be read only/disabled | False |
| NormalFont |  | Font | The font to be used on text | SystemFont.Default |
| LabelFont |  | Font | The font to be used on text on labels | SystemFont.Label |
| ParameterNameFont |  | Font | The font to be used on parameter-names | SystemFont.Bold |
| SubmitAddFont |  | Font | Font used on submit or add buttons text | SystemFont.Default |
| EditFont |  | Font | Font used on edit buttons text | SystemFont.Default |
| CancelDeleteFont |  | Font | Font used on cancel or delete buttons text | SystemFont.Default |
| NormalTextColor |  | Color | Color used on text | Colors.Black |
| LabelTextColor |  | Color | Color used on label text | Colors.Black |
| ParameterNameTextColor |  | Color | Color used on parameter name text | Colors.Black |
| SubmitAddTextColor |  | Color | Color used on submit or add buttons text | Colors.Black |
| EditTextColor |  | Color | Color used on edit buttons text | Colors.Black |
| CancelDeleteTextColor |  | Color | Color used on cancel or delete buttons text | Colors.Black |
| NormalBackgroundColor |  | Color | Color used on control background | Colors.Transparent |
| LabelBackgroundColor |  | Color | Color used on label background | Colors.Transparent |
| ParameterNameBackgroundColor |  | Color | Color used on parameter names background | Colors.Transparent |
| SubmitAddBackgroundColor |  | Color | Color used on submit or add buttons background | Colors.LimeGreen |
| EditBackgroundColor |  | Color | Color used on edit buttons background | Colors.LightBlue |
| CancelDeleteBackgroundColor |  | Color | Color used on cancel or delete buttons background | Colors.Red |
| IncrementIntValue and IncrementDecimalValue | increment | double | Defines how much a number (int, float, double, long) should increment/decrement with if the increment/decrement buttons are used | if int, 1, if a decimal-number (float, double or long), 0.1 |
| IncrementIntValue | incrementInt | int | Defines how much an int-value should be incremented (this will only apply to Int-parameters) | 1 |
| IncrementDecimalValue | incrementDecimal | double | Defines how much a decimal-value (float, double or long) should be incremented (this will only apply to decimal-parameters (float, double or long)) | 0.1 |
| DecimalPlaces | decimalPlaces | int | How many decimal-places that should be shown | 2 |
| MinDate | minDate | DateTime | What should be the lowest date that can be selected | DateTime.MinDate |
| MaxDate | maxDate | DateTime | What should be the highest date that can be selected | DateTime.MaxValue |
| EnumSelection | enumSelection | Enum of SelectControl | Define what control should be used for enums (valid values are "DropDown", "ComboBox", "GridView") | DropDown |
| SingleSelection | singleSelection | Enum of SelectControl | Define what control should be used for selecting a single value from a list (valid values are "DropDown", "ComboBox", "GridView") | DropDown |
| ComboBoxReadOnly | comboBoxReadOnly | bool | If true the value in the textbox in a combobox can not be changed manually (you need to find the value in the list)| True |
| DateTimeFormat | dateTimeFormat | string | Define how the DateTime should be formatted when converted to a string (for ParameterType.DateTime) | g |
| DateFormat | dateFormat | string | Define how the DateTime should be formatted when converted to a string (for ParameterType.Date) | d |
|  | defaultValue | TValue (Generic baseed on value (IEnumerable<TValue>)) | This is used on IEnumerable-types to define their Default-value (which is their initial state when adding new value) | If not defined, this will either be defined by a CustomParameterControl, be default(TValue) or String.Empty if TValue is string or DateTime.Now if TValue is DateTime |
| SupportedFileExtensions | supportedExtensions | string[] | Defines what types of file extensions is supported when selecting files for ParameterType.Bytes. All must have a leading . | Empty string[] or null means all types supported/no filter added |
|  | filename | string | This can be added to a Bytes-parameter to give information on what the filename of the file was. This is just for cosmetics and is not neccessarry (but will provide info to the user). When a Bytes-parameter is updated, this parameter in Additionalinfo will also be added/updated by the editor (so if you want to know the filename and uses this editor, this parameter will give you that info) |  |
|  | extension | string | The file extension for the filetype a Bytes-parameter has. The value should have a leading . This parameter is most likely needed if a preview of the file is wanted. This parameter in Additionalinfo will also be added/updated by the editor when the Bytes-parameter is updated |  |
| BytesPreviews |  | IBytesPreview[] | List with all supported preview-implementation for byte-arrays. If one or more parameters has ParameterType.Bytes, the editor will check this list for possible preview-functionality. If it finds a suitable fit, it will select the first it finds.

### The possibillity to easily add custom controls to parameters

If you want to show a value a specific way that are not supported by default, like for example let the user select a color, a font or let the user select a value based on a slider, it is possible to do so.

There are an interface, ICustomParameterControl, that lets you do this. There is also an abstract class, CustomParameterControlBase, that tries to simplify this a little bit, and is recommended to use.

When you use this, IEnumerables (ie. list, collection, etc.) of the generated types will automatically be supported. But you will also be able to show IEnumerables another way also. Please see the TestProject in the repository for a simple example for how it can be used.

## Notice

The code written in this repository/nuget package is MIT-licensed. But the code may be dependent on other nuget-packages that has other licenses. By using one or more of the packages in this repository you need to be aware of this and be sure to comply to these licenses as well. Look at each package on nuget to get a list of the packages that each package is dependent on.