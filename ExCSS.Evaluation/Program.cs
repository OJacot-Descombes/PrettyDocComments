using System;
using ExCSS;

string[] styleExamples = {
    "font-weight:bold;",
    "text-align:right",
    "background-color:red",
    "background-color:#00FF00",
    "font-family:verdana",
    "font-family:'Courier New'",
    "font-size:30px",
    "width:50%;text-align:left;margin-left:0",
    "height:2px;border-width:0;color:gray;background-color:gray",
    "width:50%",
    "vertical-align:middle;margin:50px 0px",
    "list-style-type:lower-alpha",
    "list-style-type:decimal",
    "fill:rgb(0,0,255);stroke-width:10;stroke:rgb(0,0,0)"
};

var parser = new StylesheetParser();
foreach (string style in styleExamples) {
    var stylesheet = parser.Parse(".root{" + style);
    Console.WriteLine();
    Console.WriteLine(style);
    foreach (var node in stylesheet.StyleRules) {
        Console.WriteLine(node.Style.CssText);
    }
}
Console.ReadKey();