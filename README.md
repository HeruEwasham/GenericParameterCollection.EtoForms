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

#### Define options from a ParameterCollection

If you define this in a ParameterCollection-ParameterType, the changes will affect all parameters in that ParameterCollection.

Mark that it exist multiple parameters that currently can not be defined in a ParameterCollection. This must be defined in the object directly, and can't be changed via a ParameterCollection. This list only contains the parameters that can be changed.

| Parameter key | Type | Description | Default value in option-class |
| ----------- | ----------- | ----------- | ----------- |
| humanReadable | Bool | Change if the parameter-key should be tried to be written more human readable | True |
| readOnly | Bool | If true, the control that shows the parameters value should be read only/disabled | False |
| increment | Double | Defines how much a number (int, float, double, long) should increment/decrement with if the increment/decrement buttons are used | if int, 1, if a decimal-number (float, double or long), 0.1 |
| incrementInt | Int | Defines how much an int-value should be incremented (this will only apply to Int-parameters) | 1 |
| incrementDecimal | Double | Defines how much a decimal-value (float, double or long) should be incremented (this will only apply to decimal-parameters (float, double or long)) | 0.1 |
| decimalPlaces | Int | How many decimal-places that should be shown | 2 |
| minDate | DateTime | What should be the lowest date that can be selected | DateTime.MinDate |
| maxDate | DateTime | What should be the highest date that can be selected | DateTime.MaxValue |
| enumSelection | Enum | Define what control should be used for enums (valid values are "DropDown", "ComboBox", "GridView") | DropDown |
| singleSelection | Enum | Define what control should be used for selecting a single value from a list (valid values are "DropDown", "ComboBox", "GridView") | DropDown |
| comboBoxReadOnly | Bool | If true the value in the textbox in a combobox can not be changed manually (you need to find the value in the list)| True |
| dateTimeFormat | String | Define how the DateTime should be formatted when converted to a string (for ParameterType.DateTime) | g |
| dateFormat | String | Define how the DateTime should be formatted when converted to a string (for ParameterType.Date) | d |