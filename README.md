# Pretty Doc Comments
A Visual Studio extension that overlays XML doc comments with a rendered image. It makes the comment much more readable and reduces visual noise created by XML-tags.

[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments)

[Changelog](./changelog.md)

### Compatibility
It works with single line comments in **C#**, **C++**, **F#** (`///`) and **VB** (`'''`). Multiline comments (`/**  */`) are ignored.

| original | prettyfied |
| --- | --- |
| ![original](./original.png?raw=true) | ![prettyfied](./prettyfied.png?raw=true) |

## Getting Started
[Download](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments) and run the extension (VSIX) for Visual Studio 2022 or later from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments) or, from within Visual Studio, search for "PrettyDocComments" in the "Extensions and Updates" UI.

### Editing XML doc comments
When the text cursor (or the text selection start or end) is on a doc comment line, the pretty image is hidden and the original XML text is revealed and ready to be edited the usual way.

### Configuration

You can configure the appearance of the prettyfied comments in menu ***Tools / Options...***, section ***Pretty Doc Comments***

![options](./options.png?raw=true)

### Collapse to Summary
You can collapse the comments so that text following the &lt;summary&gt; tag will be hidden. This is a global setting affecting all the comments.

Expanded (default):<br/>
![expanded](./expanded.png?raw=true)

Collapsed:<br/>
![collapsed](./collapsed.png?raw=true)

A greater than sign (**&gt;**) appears to the lower right of the comment if text was hidden.

You can change this setting in three ways:
- In the <u>Options dialog</u> described above in the section **Sizing**, option **Collapse Comments to Summary** 
- Through the <u>menu item</u> **Edit > Toggle Collapse Comments to Summary**
  ![menu_collapse](./menu_collapse.png?raw=true)
- Through a <u>shortcut key</u>. No default key binding is assigned to this toggle command as we want to avoid any clashes with existing shortcuts.
  You can assign a shortcut key through the menu **Tools / Options...** by selecting the option **Environment > Keyboard**.
  
  Here you can search for commands containing "summ". The name of our command is **Edit.ToggleCollapseCommentstoSummary**.
  Assign it your desired shortcut key. On my GB-Ascii keyboard **Ctr+Shift+\\** works well.

### Shink Empty Lines extension
If you also have installed and enabled the **Shrink Empty Lines** extensions from the Microsoft Productivity Power Tools, it might interfere with this extension as both are changing the line heights. This can be avoided by disabling 
*Tools -> Options -> Productivity Power Tools -> General -> <u>Compress lines that do not have any alphanumeric characters</u>*. You can leave the other option *<u>Compress blank lines</u>* enabled.<br/>
![collapsed](./ShrinkEmptyLinesOptions.png?raw=true)