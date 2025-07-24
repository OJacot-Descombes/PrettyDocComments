# Pretty Doc Comments
A Visual Studio extension that overlays XML doc comments with a rendered image. It makes the comment much more readable and reduces visual noise created by XML-tags.

[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments)

[Changelog](./changelog.md)

### Compatibility
It works with single line comments in **C#**, **C++**, **F#** (`///`) and **VB** (`'''`). Multiline comments (`/**  */`) are ignored.

| original | prettified |
| --- | --- |
| ![original](./original.png?raw=true) | ![prettyfied](./prettyfied.png?raw=true) |

## Getting Started
[Download](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments) and run the extension (VSIX) for Visual Studio 2022 or later from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments) or, from within Visual Studio, search for "PrettyDocComments" in the "Extensions and Updates" UI.

### Editing XML doc comments
When the text cursor (or the text selection start or end) is on a doc comment line, the pretty image is hidden, and the original XML text is revealed and ready to be edited the usual way.

**The XML must be valid XML!** If it is not, the original comment text and an error message are displayed.
For example, a **'&lt;'** character must be written as **\&lt;** and the ampersand character (\&) must be written as **\&amp;**

![options](./xmlerror.png?raw=true)

Note that Visual Studio cannot display tooltips from comments containing invalid XML.

### Configuration

You can configure the appearance of the prettified comments in menu ***Tools / Options...***, section ***Pretty Doc Comments***

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
  Assign it your desired shortcut key. On my GB-Ascii keyboard **Ctrl+Shift+\\** works well.

### Comment Width Options
Three settings control the width of the prettified comments:
- **Minimum comment width in columns**: This minimum width has precedence over the other two settings. The right edge of the comment can surpass the right margin setting when the comment has a large indentation.
- **Right margin column of comment**: If this value is larger than the minimum comment width, the right edges of the comments will be aligned at this column. Otherwise, the comments will have a fixed width, and their right edges will vary according to the comment indentations.
- **Adjust width to view port**: If this option is enabled, the right edge of the comment will be adjusted to the right edge of the editor window within the limits given by the two other settings. This is useful when you have a wide screen and want to use the full width of the editor.

### Compensate Line Height Rounding (experimental)
This option is enabled by default. It compensates for the rounding of the line height by Visual Studio.

Explanation: this extension expands or shrinks the line heights of the doc comments underneath the prettified comments
to make them fit the prettified comment height. However, Visual Studio rounds the line heights using an unknown algorithm, which can lead to a slight truncation of the comments. This option compensates for this rounding by using a heuristic.
In a first render pass, the deviation from the estimated height is measured and in a second pass the line heights are adjusted to compensate for this deviation. This can lead to a slight movement of the comment heights.
If this bothers you, you can disable this option.

### Shink Empty Lines extension
If you have also installed and enabled the **Shrink Empty Lines** extensions from the Microsoft Productivity Power Tools, it might interfere with this extension as both are changing the line heights. This can be avoided by disabling 
*Tools -> Options -> Productivity Power Tools -> General -> <u>Compress lines that do not have any alphanumeric characters</u>*. You can leave the other option *<u>Compress blank lines</u>* enabled.<br/>
![collapsed](./ShrinkEmptyLinesOptions.png?raw=true)