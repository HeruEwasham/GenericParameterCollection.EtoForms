﻿using System;
using Eto.Forms;
using Eto.Drawing;
using YngveHestem.GenericParameterCollection.EtoForms;
using YngveHestem.GenericParameterCollection;
using System.Globalization;

namespace TestProject
{
	public partial class MainForm : Form
	{
		public ParameterCollection Parameters = new ParameterCollection
		{
			new Parameter("Test string simple", "This is a test"),
			new Parameter("Test string multiline", "This is a string.\n\nThis is another line.", true),
			new Parameter("test boolean", true),
			new Parameter("WhatIs this number", (int)-1),
			new Parameter("A decimal number", 0.56f),
			new Parameter("Date", DateTime.Now, true),
			new Parameter("Date and time", DateTime.Now, false),
			new Parameter("Choose a control", SelectControl.GridView),
			new Parameter("Choose a control (with an option for showing as gridview)", SelectControl.ComboBox, new ParameterCollection
			{
				{ "enumSelection", "GridView" }
			}),
			new Parameter("Choose something", "Option 1", new string[] { "Option 1", "Option 2", "Option 92", "Option 3"}),
			new Parameter("Choose something 2", new string[] { "Option 1", "Option 3" }, new string[] { "Option 1", "Option 2", "Option 92", "Option 3", "Option 4"}),
			new Parameter("Choose something 3", new string[] { }, new string[] { "Option 1", "Option 2", "Option 92", "Option 3", "Option 4"}),
			new Parameter("Some more questions", new ParameterCollection
			{
				{ "Param1", true },
				{ "Param2", "title test" },
				{ "Description", "Title", true },
				{ "Actors", new string[] { } }
			}),
			new Parameter("A question", new string[] { "new test", "test 2" }),
			new Parameter("Some dates", new DateTime[] { DateTime.Now, DateTime.MaxValue, DateTime.MinValue, DateTime.UtcNow, new DateTime(2021, 06, 15) }, true),
            new Parameter("Some dates and times", new DateTime[] { DateTime.Now, DateTime.MaxValue, DateTime.MinValue, DateTime.UtcNow, new DateTime(2021, 06, 15) }, false),
        };

		public MainForm()
		{
			Title = "My Eto Form";
			MinimumSize = new Size(200, 200);

			var button = new Button { Text = "A button" };
			button.Click += (s, e) =>
			{
				var dialog = new ParameterCollectionDialog(Parameters);
				var value = dialog.ShowModal();
				if (value != null)
				{
					Parameters = value;
				}
			};
			var label = new Label { Text = "Font on button text: " + button.Font };

			Content = new StackLayout
			{
				Padding = 10,
				Items =
				{
					"Hello World!",
					// add more controls here
					button,
					label,
					new Label {Text = "Font on label: " + label.Font}
				}
			};

			// create a few commands that can be used for the menu and toolbar
			var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
			clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

			var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
			quitCommand.Executed += (sender, e) => Application.Instance.Quit();

			var aboutCommand = new Command { MenuText = "About..." };
			aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

			// create menu
			Menu = new MenuBar
			{
				Items =
				{
					// File submenu
					new SubMenuItem { Text = "&File", Items = { clickMe } },
					// new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
					// new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
				ApplicationItems =
				{
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
				},
				QuitItem = quitCommand,
				AboutItem = aboutCommand
			};

			// create toolbar			
			ToolBar = new ToolBar { Items = { clickMe } };
		}
	}
}

