WPFCustomMessageBox
===================== sdafas f

*WPFCustomMessageBox* is a WPF clone of the native Windows/.NET MessageBox with extra features like custom button text.

![WPFCustomMessageBox example screenshot](http://i.stack.imgur.com/AQgEj.png)

I created this library because I wanted to use verbs for my MessageBox buttons to [help users better understand the functionality of the buttons](http://ux.stackexchange.com/a/9960/12349) - which isn't possible in the standard Windows MessageBox. With this library, you can offer your users button descriptions like `Save/Don't Save` or `Shutdown Reactor/Eject Spent Fuel Rods` rather than the standard `OK/Cancel` or `Yes/No` (although you can still use those too, if you like).

The WPFCustomMessageBox message boxes return [standard .NET MessageBoxResults](http://msdn.microsoft.com/en-us/library/system.windows.messageboxresult%28v=vs.100%29.aspx).

## Usage ##

This documentation is still in progress, so in the meantime you can explore the `CustomMessageBoxDemo` project which should have a variety of demos.

You can grab the compiled library in the `downloads` folder and add them to your project just like a normal .NET library.

WPFCustomMessageBox uses static methods just like the standard .NET MessageBox, so you can plug-and-play the new library without modifying any code. When you want to add custom text, just use the special methods outlined below.

**Standard .NET Message Box**


```csharp
MessageBox.Show("Hello World!", "This is the title of the MessageBox", MessageBoxButton.OKCancel);
```

**WPFCustomMessageBox Equivalent**


```csharp
using WPFCustomMessageBox;

CustomMessageBox.Show("Hello World!", "This is the title of the MessageBox", MessageBoxButton.OKCancel);
```

**Adding custom button text to WPFCustomMessageBox**

```csharp
using WPFCustomMessageBox;

CustomMessageBox.ShowOKCancel(
    "Are you sure you want to eject the nuclear fuel rods?",
    "Confirm Fuel Ejection",
    "Eject Fuel Rods",
    "Don't do it!");
```

**Custom Button Methods**

The WPFCustomMessageBox library provides customizable equivalents of all .NET MessageBox button types:

* `CustomMessageBox.Show()` - Standard MessageBox
* `CustomMessageBox.ShowOK()` - MessageBox with single customizable "OK" button. Returns `MessageBoxResult.OK`.
* `CustomMessageBox.ShowOKCancel()` - MessageBox with customizable "OK" and "Cancel" buttons. Returns either `MessageBoxResult.OK` or `MessageBoxResult.Cancel`.
* `CustomMessageBox.ShowYesNo()` - MessageBox with customizable "Yes" and "No" buttons. Returns either `MessageBoxResult.Yes` or `MessageBoxResult.No`.
* `CustomMessageBox.ShowYesNoCancel()` - MessageBox with customizable "Yes", "No", and "Cancel" buttons. Returns either `MessageBoxResult.Yes`, `MessageBoxResult.No`, or `MessageBoxResult.Cancel`.

## todo ##

* Create Nuget package
* i18n support (especially for languages that read right-to-left)

## License ##

**The MIT License**

Copyright (c) 2013 Evan Wondrasek

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
