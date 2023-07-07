# Pretty Doc Comments
A Visual Studio extension that overlays XML doc comments with a rendered image. It makes the comment much more readable and reduces visual noise created by XML-tags.

[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments)

### Compatibility
It works with single line comments in **C#**, **C++**, **F#** (`///`) and **VB** (`'''`). Multiline comments (`/**  */`) are ignored.

| original | prettyfied |
| --- | --- |
| <img src="https://github.com/OJacot-Descombes/PrettyDocComments/original.png" /> | <img src="https://github.com/OJacot-Descombes/PrettyDocComments/prettyfied.png" /> |

## Getting Started
[Download](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments) and run the extension (VSIX) for Visual Studio 2022 or later from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments) or, from within Visual Studio, search for "PrettyDocComments" in the "Extensions and Updates" UI.

### Editing XML doc comments
When the text cursor is on a doc comment line the pretty image is hidden and the original XML text is revealed and ready to be edited the usual way.

### Configuration

You can configure the appearance of the prettyfied comments in menu ***Tools / Options...***, section ***Pretty Doc Comments***

<img src="https://github.com/OJacot-Descombes/PrettyDocComments/options.png" />
