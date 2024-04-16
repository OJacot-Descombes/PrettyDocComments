namespace ExamplesCSharp;

/// <summary>
/// Tags from <a href="https://www.w3schools.com/tags/tag_a.asp">https://www.w3schools.com/tags/tag_a.asp</a> 
/// </summary>
/// <remarks>
/// <para>Our aim is not to support every bells and whistles of HTML. However, since HTML-tags are allowed in doc comments, we
/// want to handle the ones we do not support in a graceful way at least.</para>
/// <para>We've also listed tags that probably don't make sense in XML doc comments to have a full basis for decision-making and possible future work.</para>
/// </remarks>
class HtmlTags
{
    static readonly int _x = 5;
    public static int X => _x;
    public int Y { get; } = _x;

    /// <!--This is a comment. Comments are not displayed in the browser-->
    /// <p>We display them because they may contain information valuable to the developer. After all, we are in the development environment.</p>
    public static void Comment() { }

    /// <!DOCTYPE html>
    /// <html>
    /// <head>
    /// <title>Title of the document</title>
    /// </head>
    ///
    /// <body>
    /// The content of the document......
    /// </body>
    ///
    /// </html>
    public static void DocType() { }

    /// <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
    /// <p>HTML 4.01</p>
    public static void DocTypeHtml_4_01() { }

    ///  <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
    ///  <p>XHTML 1.1</p>
    public static void DocTypeHtml_1_1() { }

    /// <a href="https://www.w3schools.com">Visit W3Schools.com!</a> 
    public static void A() { }

    /// <a href="https://www.w3schools.com">
    /// <img border = "0" alt="W3Schools" src="logo_w3s.gif" width="100" height="100" />
    /// </a> 
    public static void A_ImageAsLink() { }

    /// <a href="https://www.w3schools.com" target="_blank">Visit W3Schools.com!</a> 
    public static void A_target_blank() { }

    /// <a href="mailto:someone@example.com">Send email</a> 
    public static void A_mailto() { }

    /// <a href="tel:+4733378901">+47 333 78 901</a> 
    public static void A_phone() { }

    /// <a href="#section2">Go to Section 2</a> 
    public static void A_section() { }

    /// <a href="javascript:alert('Hello World!');">Execute JavaScript</a>
    /// <p>We don't execute any script of course. Let's just see how it renders.</p>
    public static void A_JavaScript() { }

    /// The <abbr title="World Health Organization">WHO</abbr> was founded in 1948. 
    public static void Abbr() { }

    // Example from https://www.educative.io/answers/how-to-use-the-acronym-element-in-html
    /// <acronym title="English Capital Letters">ABC</acronym>
    public static void Acronym() { }

    /// <p><dfn><abbr title="Cascading Style Sheets">CSS</abbr>
    /// </dfn> is a language that describes the style of an HTML document.</p> 
    public static void Addr_dfn() { }

    // I closed the <br> tags to make it work.
    ///  <address>
    /// Written by <a href="mailto:webmaster@example.com">Jon Doe</a>.<br/>
    /// Visit us at:<br/>
    /// Example.com<br/>
    /// Box 564, Disneyland<br/>
    /// USA
    /// </address>
    public static void Address() { }

    // Example from https://www.javatpoint.com/html-applet-tag
    /// <!DOCTYPE html>
    /// <html>
    /// <head>
    ///   <title>Applet Tag</title>
    ///  </head>
    ///  <body>
    ///    <p>Example of Applet Tag</p>
    ///     <applet code="Shapes.class" align="right" height="200" width="300">
    ///      <b>Sorry! you need Java to see this</b>
    ///      </applet>
    ///   </body>
    /// </html>
    public static void Applet() { }

    /// <img src="workplace.jpg" alt="Workplace" usemap="#workmap" width="400" height="379"/>
    ///
    /// <map name="workmap">
    ///   <area shape="rect" coords="34,44,270,350" alt="Computer" href="computer.htm"/>
    ///   <area shape="rect" coords="290,172,333,250" alt="Phone" href="phone.htm"/>
    ///   <area shape="circle" coords="337,300,44" alt="Cup of coffee" href="coffee.htm"/>
    /// </map>
    public static void Area() { }

    /// <img src="planets.gif" width="145" height="126" alt="Planets" usemap="#planetmap"/>
    ///
    /// <map name="planetmap">
    ///   <area shape="rect" coords="0,0,82,126" href="sun.htm" alt="Sun"/>
    ///   <area shape="circle" coords="90,58,3" href="mercur.htm" alt="Mercury"/>
    ///   <area shape="circle" coords="124,58,8" href="venus.htm" alt="Venus"/>
    /// </map>
    public static void Area_2() { }

    /// <article>
    /// <h2>Google Chrome</h2>
    /// <p>Google Chrome is a web browser developed by Google, released in 2008. Chrome is the world's most popular web browser today!</p>
    /// </article>
    ///
    /// <article>
    /// <h2>Mozilla Firefox</h2>
    /// <p>Mozilla Firefox is an open-source web browser developed by Mozilla. Firefox has been the second most popular web browser since January, 2018.</p>
    /// </article>
    ///
    /// <article>
    /// <h2>Microsoft Edge</h2>
    /// <p>Microsoft Edge is a web browser developed by Microsoft, released in 2015. Microsoft Edge replaced Internet Explorer.</p>
    /// </article>
    public static void Article() { }

    /// <html>
    /// <head>
    /// <style>
    /// .all-browsers {
    ///   margin: 0;
    ///   padding: 5px;
    ///   background-color: lightgray;
    /// }
    ///
    /// .all-browsers > h1, .browser {
    ///   margin: 10px;
    ///   padding: 5px;
    /// }
    ///
    /// .browser {
    ///   background: white;
    /// }
    ///
    /// .browser > h2, p {
    ///   margin: 4px;
    ///   font-size: 90%;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <article class="all-browsers">
    ///   <h1>Most Popular Browsers</h1>
    ///   <article class="browser">
    ///     <h2>Google Chrome</h2>
    ///     <p>Google Chrome is a web browser developed by Google, released in 2008. Chrome is the world's most popular web browser today!</p>
    ///   </article>
    ///   <article class="browser">
    ///     <h2>Mozilla Firefox</h2>
    ///     <p>Mozilla Firefox is an open-source web browser developed by Mozilla. Firefox has been the second most popular web browser since January, 2018.</p>
    ///   </article>
    ///   <article class="browser">
    ///     <h2>Microsoft Edge</h2>
    ///     <p>Microsoft Edge is a web browser developed by Microsoft, released in 2015. Microsoft Edge replaced Internet Explorer.</p>
    ///   </article>
    /// </article>
    ///
    /// </body>
    /// </html>
    public static void Article_2() { }

    /// <p>My family and I visited The Epcot center this summer. The weather was nice, and Epcot was amazing! I had a great summer together with my family!</p>
    ///
    /// <aside>
    /// <h4>Epcot Center</h4>
    /// <p>Epcot is a theme park at Walt Disney World Resort featuring exciting attractions, international pavilions, award-winning fireworks and seasonal special events.</p>
    /// </aside>
    public static void Aside() { }

    /// <html>
    /// <head>
    /// <style>
    /// aside {
    ///   width: 30%;
    ///   padding-left: 15px;
    ///   margin-left: 15px;
    ///   float: right;
    ///   font-style: italic;
    ///   background-color: lightgray;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>The aside element</h1>
    ///
    /// <p>My family and I visited The Epcot center this summer. The weather was nice, and Epcot was amazing! I had a great summer together with my family!</p>
    ///
    /// <aside>
    /// <p>The Epcot center is a theme park at Walt Disney World Resort featuring exciting attractions, international pavilions, award-winning fireworks and seasonal special events.</p>
    /// </aside>
    ///
    /// <p>My family and I visited The Epcot center this summer. The weather was nice, and Epcot was amazing! I had a great summer together with my family!</p>
    /// <p>My family and I visited The Epcot center this summer. The weather was nice, and Epcot was amazing! I had a great summer together with my family!</p>
    ///
    /// </body>
    /// </html>
    public static void Aside_2() { }

    /// <audio controls="play">
    ///   <source src="horse.ogg" type="audio/ogg"/>
    ///   <source src="horse.mp3" type="audio/mpeg"/>
    ///   Your browser does not support the audio tag.
    /// </audio>
    public static void Audio() { }

    /// <p>This is normal text - <b>and this is bold text</b>.</p> 
    public static void B() { }

    /// <p>This is normal text - <span style="font-weight:bold;">and this is bold text</span>.</p> 
    public static void B_CSS() { }

    /// <head>
    ///   <base href="https://www.w3schools.com/" target="_blank"/>
    /// </head>
    ///
    /// <body>
    /// <img src="images/stickman.gif" width="24" height="39" alt="Stickman"/>
    /// <a href="tags/tag_base.asp">HTML base Tag</a>
    /// </body>
    public static void Base() { }

    // Example from https://www.javatpoint.com/html-basefont-tag
    /// <!DOCTYPE html>
    /// <html>
    /// <head>
    /// <title>Basefont tag</title>
    ///  <basefont color="blue" size="5" face="arial"/>
    /// </head>
    /// <body>
    /// <h2>Example of Basefont tag</h2>
    /// <p>The basefornt tag is not supported in HTML5 use CSS to style the document</p>
    /// </body>
    /// </html>
    public static void Basefont() { }

    /// <html>
    /// <head>
    /// <style>
    /// body {
    ///   color: red;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>This is a heading</h1>
    /// <p>This is a paragraph.</p>
    ///
    /// </body>
    /// </html>
    public static void Basefont_replacement() { }

    /// <ul>
    ///   <li>User <bdi>hrefs</bdi>: 60 points</li>
    ///   <li>User <bdi>jdoe</bdi>: 80 points</li>
    ///   <li>User <bdi>إيان</bdi>: 90 points</li>
    /// </ul>
    public static void Bdi() { }

    /// <bdo dir="rtl">
    /// This text will go right-to-left.
    /// </bdo>
    public static void Bdo() { }

    /// <summary>
    /// The &lt;big&gt; tag was used in HTML 4 to <big>define <big>bigger</big> text</big>.
    /// </summary>
    public static void Big() { }

    /// <html>
    /// <head>
    /// <style>
    /// p.ex1 {
    ///   font-size: 30px;
    /// }
    /// p.ex2 {
    ///   font-size: 50px;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p>This is a normal paragraph.</p>
    /// <p class="ex1">This is a bigger paragraph.</p>
    /// <p class="ex2">This is a much bigger paragraph.</p>
    ///
    /// </body>
    /// </html>
    public static void Big_replacement() { }

    /// <summary>
    /// A section that is quoted from another source:
    /// <blockquote cite="http://www.worldwildlife.org/who/index.html">
    /// For 50 years, WWF has been protecting the future of nature. The world's leading conservation organization, WWF works in 100 countries and is supported by 1.2 million members in the United States and close to 5 million globally.
    /// </blockquote>
    /// End quote.
    /// </summary>
    public static void Blockquote() { }

    /// <html>
    /// <head>
    /// <style>
    /// blockquote {
    ///   margin-left: 0;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p>Here is a quote from WWF's website:</p>
    ///
    /// <blockquote cite="http://www.worldwildlife.org/who/index.html">
    /// For 50 years, WWF has been protecting the future of nature. The world's leading conservation organization, WWF works in 100 countries and is supported by 1.2 million members in the United States and close to 5 million globally.
    /// </blockquote>
    ///
    /// </body>
    /// </html>
    public static void Blockquote_2() { }

    /// <html>
    /// <head>
    ///   <title>Title of the document</title>
    /// </head>
    ///
    /// <body>
    ///   <h1>This is a heading</h1>
    ///   <p>This is a paragraph.</p>
    /// </body>
    ///
    /// </html>
    public static void Body() { }

    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// body {
    ///   background-image: url(w3s.png);
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>Hello world!</h1>
    /// <p><a href="https://www.w3schools.com">Visit W3Schools.com!</a></p>
    ///
    /// </body>
    /// </html>
    /// </example>
    ///
    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// body {
    ///   background-color: #E6E6FA;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>Hello world!</h1>
    /// <p><a href="https://www.w3schools.com">Visit W3Schools.com!</a></p>
    ///
    /// </body>
    /// </html>
    /// </example>
    ///
    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// body {
    ///   color: green;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>Hello world!</h1>
    /// <p>This is some text.</p>
    /// <p><a href="https://www.w3schools.com">Visit W3Schools.com!</a></p>
    ///
    /// </body>
    /// </html>
    /// </example>
    ///
    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// a:link {
    ///   color:#0000FF;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p><a href="https://www.w3schools.com">W3Schools.com</a></p>
    /// <p><a href="https://www.w3schools.com/html/">HTML Tutorial</a></p>
    ///
    /// </body>
    /// </html>
    /// </example>
    ///
    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// a:active {
    ///   color:#00FF00;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p><a href="https://www.w3schools.com">W3Schools.com</a></p>
    /// <p><a href="https://www.w3schools.com/html/">HTML Tutorial</a></p>
    ///
    /// </body>
    /// </html>
    /// </example>
    ///
    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// a:visited {
    ///   color:#FF0000;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p><a href="https://www.w3schools.com">W3Schools.com</a></p>
    /// <p><a href="https://www.w3schools.com/html/">HTML Tutorial</a></p>
    ///
    /// </body>
    /// </html>
    /// </example>
    public static void Body_Examples() { }

    // <br>-tags were unclosed in original example.
    /// <p>To force<br/> line breaks<br/> in a text,<br/> use the br<br/> element.</p>
    /// 
    /// <example>
    /// <p>Be not afraid of greatness.<br/>
    /// Some are born great,<br/>
    /// some achieve greatness,<br/>
    /// and others have greatness thrust upon them.</p>
    ///
    /// <p><em>-William Shakespeare</em></p>
    /// </example>
    public static void Br() { }

    /// <button type="button">Click Me!</button> 
    public static void Button() { }

    /// <canvas id="myCanvas">
    /// Your browser does not support the canvas tag.
    /// </canvas>
    ///
    /// <script>
    /// let canvas = document.getElementById("myCanvas");
    /// let ctx = canvas.getContext("2d");
    /// ctx.fillStyle = "#FF0000";
    /// ctx.fillRect(0, 0, 80, 80);
    /// </script>
    ///
    /// <example>
    /// <canvas id="myCanvas">
    /// Your browser does not support the canvas tag.
    /// </canvas>
    ///
    /// <script>
    /// let c = document.getElementById("myCanvas");
    /// let ctx = c.getContext("2d");
    /// ctx.fillStyle = "red";
    /// ctx.fillRect(20, 20, 75, 50);
    /// //Turn transparency on
    /// ctx.globalAlpha = 0.2;
    /// ctx.fillStyle = "blue";
    /// ctx.fillRect(50, 50, 75, 50);
    /// ctx.fillStyle = "green";
    /// ctx.fillRect(80, 80, 75, 50);
    /// </script>
    /// </example>
    public static void Canvas() { }

    /// <table>
    ///   <caption>Monthly savings</caption>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// <example>
    ///     <table>
    ///       <caption style="text-align:right">My savings</caption>
    ///       <tr>
    ///         <th>Month</th>
    ///         <th>Savings</th>
    ///       </tr>
    ///       <tr>
    ///         <td>January</td>
    ///         <td>$100</td>
    ///       </tr>
    ///     </table>
    /// </example>
    /// <example>
    ///     <table>
    ///       <caption style="caption-side:bottom">My savings</caption>
    ///       <tr>
    ///         <th>Month</th>
    ///         <th>Savings</th>
    ///       </tr>
    ///       <tr>
    ///         <td>January</td>
    ///         <td>$100</td>
    ///       </tr>
    ///     </table>
    /// </example>
    public static void Caption() { }

    /// <center>
    ///   This text will be centered.
    ///   <p>So will this paragraph.</p>
    /// </center>
    public static void Center() { }

    /// <html>
    /// <head>
    /// <style>
    /// h1 {text-align: center;}
    /// p {text-align: center;}
    /// div {text-align: center;}
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>This is a heading</h1>
    /// <p>This is a paragraph.</p>
    /// <div>This is a div.</div>
    ///
    /// </body>
    /// </html>
    public static void Center_replacement() { }

    /// <p><cite>The Scream</cite> by Edward Munch. Painted in 1893.</p> 
    public static void Cite() { }

    // Note: we use <c> for inline code and <code> for code blocks. HTML does not have a <c>-tag. Therefore, these
    // examples are not rendered as expected for HTML.
    /// <p>The HTML <code>button</code> tag defines a clickable button.</p>
    /// <p>The CSS <code>background-color</code> property defines the background color of an element.</p>
    /// <example>
    /// <para>Use CSS to style the &lt;code&gt; element:</para>
    ///     <html>
    ///     <head>
    ///     <style>
    ///     code {
    ///       font-family: Consolas,"courier new";
    ///       color: crimson;
    ///       background-color: #f1f1f1;
    ///       padding: 2px;
    ///       font-size: 105%;
    ///     }
    ///     </style>
    ///     </head>
    ///     <body>
    ///
    ///     <p>The HTML <code>button</code> tag defines a clickable button.</p>
    ///     <p>The CSS <code>background-color</code> property defines the background color of an element.</p>
    ///
    ///     </body>
    ///     </html>
    /// </example>
    public static void Code() { }

    /// <table>
    ///   <colgroup>
    ///     <col span="2" style="background-color:red"/>
    ///     <col style="background-color:yellow"/>
    ///   </colgroup>
    ///   <tr>
    ///     <th>ISBN</th>
    ///     <th>Title</th>
    ///     <th>Price</th>
    ///   </tr>
    ///   <tr>
    ///     <td>3476896</td>
    ///     <td>My first HTML</td>
    ///     <td>$53</td>
    ///   </tr>
    /// </table>
    public static void Col() { }

    public static void Colgroup() { }

    /// <ul>
    ///   <li><data value="21053">Cherry Tomato</data></li>
    ///   <li><data value="21054">Beef Tomato</data></li>
    ///   <li><data value="21055">Snack Tomato</data></li>
    /// </ul>
    public static void Data() { }

    /// <label for="browser">Choose your browser from the list:</label>
    /// <input list="browsers" name="browser" id="browser"/>
    ///
    /// <datalist id="browsers">
    ///   <option value="Edge"/>
    ///   <option value="Firefox"/>
    ///   <option value="Chrome"/>
    ///   <option value="Opera"/>
    ///   <option value="Safari"/>
    /// </datalist>
    public static void Datalist() { }

    /// <dl>
    ///   <dt>Coffee</dt>
    ///   <dd>Black hot drink</dd>
    ///   <dt>Milk</dt>
    ///   <dd>White cold drink</dd>
    /// </dl>
    public static void Dd() { }

    /// <summary>
    /// <p>My favorite color is <del>blue</del> <ins>red</ins>!</p> 
    /// </summary>
    public static void Del() { }

    /// <details>
    ///   <summary>Epcot Center</summary>
    ///   <p>Epcot is a theme park at Walt Disney World Resort featuring exciting attractions, international pavilions, award-winning fireworks and seasonal special events.</p>
    /// </details>
    public static void Details() { }

    /// <example>
    ///   1. Just as the content of the &lt;dfn&gt; element:<br/>
    ///   <p><dfn>HTML</dfn> is the standard markup language for creating web pages.</p>
    /// </example>
    /// <example>
    ///   2. Or, with the title attribute added:<br/>
    ///   <p><dfn title="HyperText Markup Language">HTML</dfn> is the standard markup language for creating web pages.</p> 
    /// </example>
    /// <example>
    ///   3. Or, with an &lt;abbr&gt; tag inside the &lt;dfn&gt; element:<br/>
    ///   <p><dfn><abbr title="HyperText Markup Language">HTML</abbr></dfn> is the standard markup language for creating web pages.</p> 
    /// </example>
    /// <example>
    ///   4. Or, with the id attribute added. Then, whenever a term is used, it can refer back to the definition with an &lt;a&gt; tag:<br/>
    ///   <p><dfn id="html-def">HTML</dfn> is the standard markup language for creating web pages.</p>
    ///
    ///   <p>This is some text...</p>
    ///   <p>This is some text...</p>
    ///   <p>Learn <a href="#html-def">HTML</a> now.</p>
    /// </example>
    public static void Dfn() { }

    /// <p>This is some text.</p>
    /// <dialog open="">This is an open dialog window</dialog>
    /// <p>This is some text.</p>
    public static void Dialog() { }

    // From: https://www.javatpoint.com/html-dir-tag
    /// <p>List of javaTpoint popular Tutorials</p>
    /// <dir>
    /// <li>Java-tutorial</li>
    /// <li>DBMS-tutorial</li>
    /// <li>DataStructure-tutorial</li>
    /// <li>HTML-tutorial</li>
    /// </dir>
    public static void Dir() { }

    /// <div class="myDiv">
    ///   <h2>This is a heading in a div element</h2>
    ///   <p>This is some text in a div element.</p>
    /// </div>
    /// <example>Example of an <div>inline</div> div-tag.</example>
    public static void Div() { }

    /// <dl>
    ///   <dt>Coffee</dt>
    ///   <dd>Black hot drink</dd>
    ///   <dt>Milk</dt>
    ///   <dd>White cold drink</dd>
    /// </dl>
    public static void Dl() { }

    public static void Dt() { }

    /// <p>You <em>have</em> to hurry up!</p>
    /// <p>We <em>cannot</em> live like this.</p>
    public static void Em() { }

    /// <embed type="image/jpg" src="pic_trulli.jpg" width="300" height="200"/><br/> 
    /// <embed type="text/html" src="snippet.html" width="500" height="200"/><br/>  
    /// <embed type="video/webm" src="video.mp4" width="400" height="300"/> 
    public static void Embed() { }

    /// <form action="/action_page.php">
    ///   <fieldset>
    ///     <legend>Personalia:</legend>
    ///     <label for="fname">First name:</label>
    ///     <input type="text" id="fname" name="fname"/><br/><br/>
    ///     <label for="lname">Last name:</label>
    ///     <input type="text" id="lname" name="lname"/><br/><br/>
    ///     <label for="email">Email:</label>
    ///     <input type="email" id="email" name="email"/><br/><br/>
    ///     <label for="birthday">Birthday:</label>
    ///     <input type="date" id="birthday" name="birthday"/><br/><br/>
    ///     <input type="submit" value="Submit"/>
    ///   </fieldset>
    /// </form>
    public static void Fieldset() { }

    /// <figure>
    ///   <img src="pic_trulli.jpg" alt="Trulli" style="width:100%"/>
    ///   <figcaption>Fig.1 - Trulli, Puglia, Italy.</figcaption>
    /// </figure>
    public static void Figcaption() { }

    public static void Figure() { }

    /// <h2>Example of font tag</h2>
    /// <p>This is normal text without any font styling</p>
    /// <p>
    ///   <font color="blue">Text with normal size and default face</font>
    /// </p>
    /// <p>
    ///   <font size="5" color="green">Text with Increased size and default face</font>
    /// </p>
    /// <p>
    ///   <font color="red" face="cursive">Text with Changed face</font>
    /// </p>
    public static void Font() { }

    /// <example>
    ///   Set the <b>color</b> of text (with CSS):
    ///   <p style="color:red">This is a paragraph.</p>
    ///   <p style="color:blue">This is another paragraph.</p>
    /// </example>
    ///
    /// <example>
    ///   Set the <b>font</b> of text (with CSS):
    ///   <p style="font-family:verdana">This is a paragraph.</p>
    ///   <p style="font-family:'Courier New'">This is another paragraph.</p>
    /// </example>
    ///
    /// <example>
    ///   Set the <b>size</b> of text (with CSS):
    ///   <p style="font-size:30px">This is a paragraph.</p>
    ///   <p style="font-size:11px">This is another paragraph.</p>
    /// </example>
    public static void Font_replacement() { }

    /// A footer section in a document:
    /// <footer>
    ///   <p>Author: Hege Refsnes</p>
    ///   <p><a href="mailto:hege@example.com">hege@example.com</a></p>
    /// </footer>
    public static void Footer() { }

    /// <form action="/action_page.php" method="get">
    ///   <label for="fname">First name:</label>
    ///   <input type="text" id="fname" name="fname"/><br/><br/>
    ///   <label for="lname">Last name:</label>
    ///   <input type="text" id="lname" name="lname"/><br/><br/>
    ///   <input type="submit" value="Submit"/>
    /// </form>
    public static void Form() { }

    /// From: https://www.javatpoint.com/html-frame-tag<br/>
    /// <frameset cols="25%,50%,25%">
    ///   <frame src="frame1.html"/>
    ///   <frame src="frame2.html"/>
    ///   <frame src="frame3.html"/>
    /// </frameset>
    public static void Frame() { }

    public static void Frameset() { }

    /// <h1>This is heading 1</h1>
    /// <h2>This is heading 2</h2>
    /// <h3>This is heading 3</h3>
    /// <h4>This is heading 4</h4>
    /// <h5>This is heading 5</h5>
    /// <h6>This is heading 6</h6>
    /// Normal Text
    public static void H1_h6() { }

    /// <!DOCTYPE html>
    /// <html lang="en">
    /// <head>
    ///   <title>Title of the document</title>
    /// </head>
    /// <body>
    /// <p>A simple HTML document, with a &lt;title&gt; tag inside the head section:</p>
    ///
    /// <h1>This is a heading</h1>
    /// <p>This is a paragraph.</p>
    ///
    /// </body>
    /// </html>
    public static void Head() { }

    /// <article>
    ///   <header>
    ///     <h1>A heading here</h1>
    ///     <p>Posted by John Doe</p>
    ///     <p>Some additional information here</p>
    ///   </header>
    ///   <p>Lorem Ipsum dolor set amet....</p>
    /// </article>
    public static void Header() { }

    /// <h4>Use the <hr/> tag to define thematic changes in the content:</h4>
    /// <h1>The Main Languages of the Web</h1>
    /// <p>HTML is the standard markup language for creating Web pages. HTML describes the structure of a Web page, and consists of a series of elements. HTML elements tell the browser how to display the content.</p>
    /// <hr/>
    /// <p>CSS is a language that describes how HTML elements are to be displayed on screen, paper, or in other media. CSS saves a lot of work, because it can control the layout of multiple web pages all at once.</p>
    /// <hr/>
    /// <p>JavaScript is the programming language of HTML and the Web. JavaScript can change HTML content and attribute values. JavaScript can change CSS. JavaScript can hide and show HTML elements, and more.</p>
    ///
    /// <example>
    /// Align a &lt;hr&gt; element (with CSS):
    /// <hr style="width:50%;text-align:left;margin-left:0"/>
    /// End of the example.
    /// </example>
    ///
    /// <example>
    /// A noshaded &lt;hr&gt; (with CSS):
    /// <hr style="height:2px;border-width:0;color:gray;background-color:gray"/>
    /// End of the example.
    /// </example>
    ///
    /// <example>
    /// Set the height of a &lt;hr&gt; element (with CSS):
    /// <hr style="height:30px"/>
    /// End of the example.
    /// </example>
    ///
    /// <example>
    /// Set the width of a &lt;hr&gt; element (with CSS):
    /// <hr style="width:50%"/>
    /// End of the example.
    /// </example>
    /// 
    /// <example>
    /// <list type="table">
    ///   <listheader>
    ///      <term>Column 1</term>
    ///      <term>Column 2</term>
    ///      <term/>
    ///   </listheader>
    ///   <item>
    ///       <term>Row 1,<hr/> Column 1</term>
    ///       <term>Row 1<br/>Column 2</term>
    ///   </item>
    ///   <item>
    ///       <term>Row 2<br/>Column 1</term>
    ///       <term>Row 2, Column 2</term>
    ///   </item>
    /// </list>
    /// </example>
    public static void Hr() { }

    /// <!DOCTYPE html>
    /// <html lang="en">
    /// <head>
    ///   <title>Title of the document</title>
    /// </head>
    /// <body>
    ///
    /// <h1>This is a heading</h1>
    /// <p>This is a paragraph.</p>
    ///
    /// </body>
    /// </html>
    public static void Html() { }

    /// <p><i>Lorem ipsum</i> is the most popular filler text in history.</p>
    ///
    /// <p>The <i>RMS Titanic</i>, a luxury steamship, sank on April 15, 1912 after striking an iceberg.</p>
    public static void I() { }

    /// <iframe src="https://www.w3schools.com" title="W3Schools Free Online Web Tutorials"></iframe> 
    public static void Iframe() { }

    /// <img src="img_girl.jpg" alt="Girl in a jacket" width="500" height="600"/> 
    /// <example>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="vertical-align:bottom"/>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="vertical-align:middle"/>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="vertical-align:top"/>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="float:right"/>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="float:left"/>
    /// </example>
    /// <example>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="border:5px solid black"/> 
    /// </example>
    /// <example>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="vertical-align:middle;margin:0px 50px"/> 
    /// </example>
    /// <example>
    ///   <img src="smiley.gif" alt="Smiley face" width="42" height="42" style="vertical-align:middle;margin:50px 0px"/> 
    /// </example>
    /// <example>
    ///   <img src="/images/stickman.gif" alt="Stickman" width="24" height="39"/>
    ///   <img src="https://www.w3schools.com/images/lamp.jpg" alt="Lamp" width="32" height="32"/>
    /// </example>
    /// <example>
    ///   <a href="https://www.w3schools.com">
    ///   <img src="w3html.gif" alt="W3Schools.com" width="100" height="132"/>
    ///   </a>
    /// </example>
    /// <example>
    /// <img src="workplace.jpg" alt="Workplace" usemap="#workmap" width="400" height="379"/>
    ///
    /// <map name="workmap">
    ///   <area shape="rect" coords="34,44,270,350" alt="Computer" href="computer.htm"/>
    ///   <area shape="rect" coords="290,172,333,250" alt="Phone" href="phone.htm"/>
    ///   <area shape="circle" coords="337,300,44" alt="Cup of coffee" href="coffee.htm"/>
    /// </map>
    /// </example>
    public static void Img() { }

    /// <table>
    /// <tr><td>Button:</td><td><input type="button" value="Hello world"/></td></tr>
    /// <tr><td>Checkbox:</td><td><input type="checkbox"/></td></tr>
    /// <tr><td>Color:</td><td><input type="color"/></td></tr>
    /// <tr><td>Date:</td><td><input type="date"/></td></tr>
    /// <tr><td>Datetime-local:</td><td><input type="datetime-local"/></td></tr>
    /// <tr><td>E-Mail:</td><td><input type="email"/></td></tr>
    /// <tr><td>File:</td><td><input type="file"/></td></tr>
    /// <tr><td>Hidden:</td><td><input type="hidden"/></td></tr>
    /// <tr><td>Image:</td><td><input type="image" src="smiley.gif"/></td></tr>
    /// <tr><td>Month:</td><td><input type="month"/></td></tr>
    /// <tr><td>Number:</td><td><input type="number"/></td></tr>
    /// <tr><td>Password:</td><td><input type="password"/></td></tr>
    /// <tr><td><input type="radio"/>Radio</td></tr>
    /// <tr><td>Range:</td><td><input type="range"/></td></tr>
    /// <tr><td>Resert:</td><td><input type="reset"/></td></tr>
    /// <tr><td>Search:</td><td><input type="search"/></td></tr>
    /// <tr><td>Submit:</td><td><input type="submit"/></td></tr>
    /// <tr><td>Tel:</td><td><input type="tel"/></td></tr>
    /// <tr><td>Text:</td><td><input type="text"/> (default value)</td></tr>
    /// <tr><td>Default:</td><td><input /></td></tr>
    /// <tr><td>Time:</td><td><input type="time"/></td></tr>
    /// <tr><td>Url:</td><td><input type="url"/></td></tr>
    /// <tr><td>Week:</td><td><input type="week"/></td>
    /// </tr>
    /// </table>
    public static void Input() { }

    /// A text with a deleted part, and a new, inserted part:
    /// <p>My favorite color is <del>blue</del> <ins>red</ins>!</p> 
    public static void Ins() { }

    /// <p>Press <kbd>Ctrl</kbd> + <kbd>C</kbd> to copy text (Windows).</p>
    /// <p>Press <kbd>Cmd</kbd> + <kbd>C</kbd> to copy text (Mac OS).</p>
    public static void Kbd() { }

    /// <html>
    /// <head>
    /// <style>
    /// kbd {
    ///   border-radius: 2px;
    ///   padding: 2px;
    ///   border: 1px solid black;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p>Press <kbd>Ctrl</kbd> + <kbd>C</kbd> to copy text (Windows).</p>
    /// <p>Press <kbd>Cmd</kbd> + <kbd>C</kbd> to copy text (Mac OS).</p>
    ///
    /// </body>
    /// </html>
    public static void Kbd_Css() { }

    /// <form action="/action_page.php">
    ///   <input type="radio" id="html" name="fav_language" value="HTML"/>
    ///   <label for="html">HTML</label><br/>
    ///   <input type="radio" id="css" name="fav_language" value="CSS"/>
    ///   <label for="css">CSS</label><br/>
    ///   <input type="radio" id="javascript" name="fav_language" value="JavaScript"/>
    ///   <label for="javascript">JavaScript</label><br/><br/>
    ///   <input type="submit" value="Submit"/>
    /// </form>
    public static void Label() { }

    /// <form action="/action_page.php">
    ///   <fieldset>
    ///     <legend>Personalia:</legend>
    ///     <label for="fname">First name:</label>
    ///     <input type="text" id="fname" name="fname"/><br/><br/>
    ///     <label for="lname">Last name:</label>
    ///     <input type="text" id="lname" name="lname"/><br/><br/>
    ///     <label for="email">Email:</label>
    ///     <input type="email" id="email" name="email"/><br/><br/>
    ///     <label for="birthday">Birthday:</label>
    ///     <input type="date" id="birthday" name="birthday"/><br/><br/>
    ///     <input type="submit" value="Submit"/>
    ///   </fieldset>
    /// </form>
    public static void Legend() { }

    /// One ordered (&lt;ol&gt;) and one unordered (&lt;ul&gt;) HTML list:<br/>
    /// <ol>
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ol>
    ///
    /// <ul>
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    /// <example>
    /// Use of the value attribute in an ordered list:<br/>
    /// <ol>
    ///   <li value="100">Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    ///   <li>Water</li>
    ///   <li>Juice</li>
    ///   <li>Beer</li>
    /// </ol>
    /// </example>
    /// <example>
    /// Set different list style types (with CSS):<br/>
    /// <ol>
    ///   <li>Coffee</li>
    ///   <li style="list-style-type:lower-alpha">Tea</li>
    ///   <li>Milk</li>
    /// </ol>
    ///
    /// <ul>
    ///   <li>Coffee</li>
    ///   <li style="list-style-type:square">Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    /// <example>
    /// Create a list inside a list (a nested list):<br/>
    /// <ul>
    ///   <li>Coffee</li>
    ///   <li>Tea
    ///     <ul>
    ///       <li>Black tea</li>
    ///       <li>Green tea</li>
    ///     </ul>
    ///   </li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    /// <example>
    /// Create a more complex nested list:<br/>
    /// <ul>
    ///   <li>Coffee</li>
    ///   <li>Tea
    ///     <ul>
    ///       <li>Black tea</li>
    ///       <li>Green tea
    ///         <ul>
    ///           <li>China</li>
    ///           <li>Africa</li>
    ///         </ul>
    ///       </li>
    ///     </ul>
    ///   </li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    public static void Li() { }

    /// <head>
    ///   <link rel="stylesheet" href="styles.css"/>
    /// </head>
    public static void Link() { }

    /// <main>
    ///   <h1>Most Popular Browsers</h1>
    ///   <p>Chrome, Firefox, and Edge are the most used browsers today.</p>
    ///
    ///   <article>
    ///     <h2>Google Chrome</h2>
    ///     <p>Google Chrome is a web browser developed by Google, released in 2008. Chrome is the world's most popular web browser today!</p>
    ///   </article>
    ///
    ///   <article>
    ///     <h2>Mozilla Firefox</h2>
    ///     <p>Mozilla Firefox is an open-source web browser developed by Mozilla. Firefox has been the second most popular web browser since January, 2018.</p>
    ///   </article>
    ///
    ///   <article>
    ///     <h2>Microsoft Edge</h2>
    ///     <p>Microsoft Edge is a web browser developed by Microsoft, released in 2015. Microsoft Edge replaced Internet Explorer.</p>
    ///   </article>
    /// </main>
    public static void MainTag() { }

    /// <html>
    /// <head>
    /// <style>
    /// main {
    ///   margin: 0;
    ///   padding: 5px;
    ///   background-color: lightgray;
    /// }
    ///
    /// main > h1, p, .browser {
    ///   margin: 10px;
    ///   padding: 5px;
    /// }
    ///
    /// .browser {
    ///   background: white;
    /// }
    ///
    /// .browser > h2, p {
    ///   margin: 4px;
    ///   font-size: 90%;
    /// }
    /// </style>
    /// </head>
    /// <body>
    /// Use CSS to style the &lt;main&gt; element:<br/>
    /// <main>
    ///   <h1>Most Popular Browsers</h1>
    ///   <p>Chrome, Firefox, and Edge are the most used browsers today.</p>
    ///   <article class="browser">
    ///     <h2>Google Chrome</h2>
    ///     <p>Google Chrome is a web browser developed by Google, released in 2008. Chrome is the world's most popular web browser today!</p>
    ///   </article>
    ///   <article class="browser">
    ///     <h2>Mozilla Firefox</h2>
    ///     <p>Mozilla Firefox is an open-source web browser developed by Mozilla. Firefox has been the second most popular web browser since January, 2018.</p>
    ///   </article>
    ///   <article class="browser">
    ///     <h2>Microsoft Edge</h2>
    ///     <p>Microsoft Edge is a web browser developed by Microsoft, released in 2015. Microsoft Edge replaced Internet Explorer.</p>
    ///   </article>
    /// </main>
    ///
    /// </body>
    /// </html>
    public static void Main_Css() { }

    /// <img src="workplace.jpg" alt="Workplace" usemap="#workmap" width="400" height="379"/>
    ///
    /// <map name="workmap">
    ///   <area shape="rect" coords="34,44,270,350" alt="Computer" href="computer.htm"/>
    ///   <area shape="rect" coords="290,172,333,250" alt="Phone" href="phone.htm"/>
    ///   <area shape="circle" coords="337,300,44" alt="Cup of coffee" href="coffee.htm"/>
    /// </map>
    /// <example>
    /// <img src="planets.gif" width="145" height="126" alt="Planets"
    /// usemap="#planetmap"/>
    ///
    /// <map name="planetmap">
    ///   <area shape="rect" coords="0,0,82,126" href="sun.htm" alt="Sun"/>
    ///   <area shape="circle" coords="90,58,3" href="mercur.htm" alt="Mercury"/>
    ///   <area shape="circle" coords="124,58,8" href="venus.htm" alt="Venus"/>
    /// </map>
    /// </example>
    public static void Map() { }

    /// <p>Do not forget to buy <mark>milk</mark> today.</p> 
    public static void Mark() { }

    /// <head>
    ///   <meta charset="UTF-8"/>
    ///   <meta name="description" content="Free Web tutorials"/>
    ///   <meta name="keywords" content="HTML, CSS, JavaScript"/>
    ///   <meta name="author" content="John Doe"/>
    ///   <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    ///   <!-- Define keywords for search engines: -->
    ///   <meta name="keywords" content="HTML, CSS, JavaScript"/>
    ///   <!-- Define a description of your web page: -->
    ///   <meta name="description" content="Free Web tutorials for HTML and CSS"/>
    ///   <!-- Define the author of a page: -->
    ///   <meta name="author" content="John Doe"/>
    ///   <!-- Refresh document every 30 seconds: -->
    ///   <meta http-equiv="refresh" content="30"/>
    ///   <!-- Setting the viewport to make your website look good on all devices: -->
    ///   <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    ///   <!-- Setting the Viewport -->
    ///   <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    /// </head>
    public static void Meta() { }

    /// <label for="disk_c">Disk usage C:</label>
    /// <meter id="disk_c" value="2" min="0" max="10">2 out of 10</meter><br/>
    ///
    /// <label for="disk_d">Disk usage D:</label>
    /// <meter id="disk_d" value="0.6">60%</meter>
    public static void Meter() { }

    /// <nav>
    ///   <a href="/html/">HTML</a> |
    ///   <a href="/css/">CSS</a> |
    ///   <a href="/js/">JavaScript</a> |
    ///   <a href="/python/">Python</a>
    /// </nav>
    public static void Nav() { }

    /// <!DOCTYPE html>
    /// <html lang="en-US">
    ///   <head>
    ///     <!-- From: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/noframes -->
    ///     <!-- Document metadata goes here -->
    ///   </head>
    ///   <frameset rows="45%, 55%">
    ///     <frame src="https://developer.mozilla.org/en/HTML/Element/frameset" />
    ///     <frame src="https://developer.mozilla.org/en/HTML/Element/frame" />
    ///     <noframes>
    ///       <p>
    ///         It seems your browser does not support frames or is configured to not
    ///         allow them.
    ///       </p>
    ///     </noframes>
    ///   </frameset>
    /// </html>
    public static void Noframes() { }

    /// <script>
    /// document.write("Hello World!")
    /// </script>
    /// <noscript>Your browser does not support JavaScript!</noscript>
    public static void Noscript() { }

    /// <object data="pic_trulli.jpg" width="300" height="200"></object>
    /// <object data="snippet.html" width="500" height="200"></object>
    /// <object data="video.mp4" width="400" height="300"></object> 
    public static void Object() { }

    /// <ol>
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol start="50">
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ol>
    /// <example>
    /// Display all the different list types available with CSS:
    /// <ol style="list-style-type:armenian">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:cjk-ideographic">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:decimal">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:decimal-leading-zero">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:georgian">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:hebrew">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:hiragana">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:hiragana-iroha">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:katakana">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:katakana-iroha">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type: lower-alpha">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:lower-greek">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:lower-latin">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:lower-roman">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:upper-alpha">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:upper-latin">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:upper-roman">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:none">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// <br/>
    /// <ol style="list-style-type:inherit">
    /// <li>Coffee</li>
    /// <li>Tea</li>
    /// <li>Milk</li>
    /// </ol>
    /// </example>
    public static void Ol() { }

    /// <label for="cars">Choose a car:</label>
    /// <select  name="cars" id="cars">
    ///   <optgroup label="Swedish Cars">
    ///     <option value="volvo">Volvo</option>
    ///     <option value="saab">Saab</option>
    ///   </optgroup>
    ///   <optgroup label="German Cars">
    ///     <option value="mercedes">Mercedes</option>
    ///     <option value="audi">Audi</option>
    ///   </optgroup>
    /// </select>
    public static void Optgroup() { }

    /// <label for="cars">Choose a car:</label>
    ///
    /// <select id="cars">
    ///   <option value="volvo">Volvo</option>
    ///   <option value="saab">Saab</option>
    ///   <option value="opel">Opel</option>
    ///   <option value="audi">Audi</option>
    /// </select>
    ///
    /// <example>
    /// Use of &lt;option&gt; in a &lt;datalist&gt; element:<br/>
    /// <label for="browser">Choose your browser from the list:</label>
    /// <input list="browsers" name="browser" id="browser"/>
    ///
    /// <datalist id="browsers">
    ///   <option value="Edge"/>
    ///   <option value="Firefox"/>
    ///   <option value="Chrome"/>
    ///   <option value="Opera"/>
    ///   <option value="Safari"/>
    /// </datalist>
    /// </example>
    ///
    /// <example>
    /// Use of &lt;option&gt; in &lt;optgroup&gt; elements:<br/>
    /// <label for="cars">Choose a car:</label>
    /// <select id="cars">
    ///   <optgroup label="Swedish Cars">
    ///     <option value="volvo">Volvo</option>
    ///     <option value="saab">Saab</option>
    ///   </optgroup>
    ///   <optgroup label="German Cars">
    ///     <option value="mercedes">Mercedes</option>
    ///     <option value="audi">Audi</option>
    ///   </optgroup>
    /// </select>
    /// </example>
    public static void Option() { }

    /// <!DOCTYPE html>
    /// <html>
    /// <body>
    ///
    /// <h1>The output element</h1>
    ///
    /// <form oninput="x.value=parseInt(a.value)+parseInt(b.value)">
    /// <input type="range" id="a" value="50"/>
    /// +<input type="number" id="b" value="25"/>
    /// =<output name="x" for="a b"></output>
    /// </form>
    ///
    /// <p><strong>Note:</strong> The output element is not supported in Edge 12 (or earlier).</p>
    ///
    /// </body>
    /// </html>
    public static void Output() { }

    /// <p>This is some text in a paragraph.</p>
    /// <p style="text-align:right">This is some text in a paragraph.</p>
    /// <p>
    /// This paragraph
    /// contains a lot of lines
    /// in the source code,
    /// but the browser
    /// ignores it.
    /// </p>
    /// <p>
    ///    My Bonnie lies over the ocean.
    ///    My Bonnie lies over the sea.
    ///    My Bonnie lies over the ocean.
    ///    Oh, bring back my Bonnie to me.
    /// </p>
    ///
    /// <p>Note that the browser simply ignores the line breaks in the source code!</p>
    public static void P() { }

    /// <h1>The HTML param element</h1>
    /// <object data="horse.wav">
    /// <param name="autoplay" value="true"/>
    /// </object>
    /// <br/>
    /// <hr/>
    /// <h2>The XML doc comment top-level version of &lt;param&gt;</h2>
    /// <param name="name">name description</param>
    /// <param name="key">key description</param>
    /// <example>
    /// <h2>The XML doc comment nested version of &lt;param&gt;</h2>
    /// <param name="name">name description</param>
    /// <param name="key">key description</param>
    /// </example>
    public static void Param() { }

    /// <picture>
    ///   <source media="(min-width:650px)" srcset="img_pink_flowers.jpg"/>
    ///   <source media="(min-width:465px)" srcset="img_white_flower.jpg"/>
    ///   <img src="img_orange_flowers.jpg" alt="Flowers" style="width:auto;"/>
    /// </picture>
    public static void Picture() { }

    /// <h1>The pre element</h1>
    /// Before
    /// <pre>
    /// Text in a pre element
    /// is displayed in a fixed-width
    /// font, and it preserves
    /// both      spaces and
    /// line breaks
    /// </pre>
    /// After
    public static void Pre() { }

    /// <label for="file">Downloading progress:</label>
    /// <progress id="file" value="32" max="100"> 32% </progress>
    public static void Progress() { }

    /// <h1>The q element</h1>
    ///
    /// <p>WWF's goal is to:
    /// <q>Build a future where people live in harmony with nature.</q>
    /// We hope they succeed.</p>
    public static void Q() { }

    /// <ruby>
    /// Ruby <rp>(</rp><rt>annotation</rt><rp>)</rp>
    /// </ruby>
    public static void Rp() { }

    public static void Ruby() { }

    public static void Rt() { }

    /// <h1>The s element</h1>
    ///
    /// <p><s>Only 50 tickets left!</s></p>
    /// <p>SOLD OUT!</p>
    public static void S() { }

    /// <h1>The samp element</h1>
    ///
    /// <p>Message from my computer:</p>
    ///
    /// <p>Before <samp>File not found.<br/>Press F1 to continue</samp> after</p>
    public static void Samp() { }

    /// <!DOCTYPE html>
    /// <html>
    /// <body>
    ///
    /// <h1>The script element</h1>
    ///
    /// <p id="demo"></p>
    ///
    /// <script>
    /// document.getElementById("demo").innerHTML = "Hello JavaScript!";
    /// </script>
    ///
    /// </body>
    /// </html>
    public static void Script() { }

    /// <!DOCTYPE html>
    /// <html>
    /// <head>
    /// <style>
    /// section {
    ///   display: block;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <p>A section element is displayed like this:</p>
    ///
    /// <section>
    ///   <h1>WWF</h1>
    ///   <p>The World Wide Fund for Nature (WWF) is an international organization working on issues regarding the conservation, research and restoration of the environment, formerly named the World Wildlife Fund. WWF was founded in 1961.</p>
    /// </section>abc<section>123</section><section>xyz</section>
    ///
    /// <p>Change the default CSS settings to see the effect.</p>
    ///
    /// </body>
    /// </html>
    public static void Section() { }

    /// <h3>Create a drop-down list with four options:</h3>
    /// <label for="cars">Choose a car:</label>
    ///
    /// <select name="cars" id="cars">
    ///   <option value="volvo">Volvo</option>
    ///   <option value="saab">Saab</option>
    ///   <option value="mercedes">Mercedes</option>
    ///   <option value="audi">Audi</option>
    /// </select>
    public static void Select() { }

    /// <h1>The small element</h1>
    ///
    /// <p>This is some normal text.</p>
    /// <p><small>This is some smaller text.</small></p>
    /// This is a <small>small</small> word.
    public static void Small() { }

    /// <audio controls="">
    ///   <source src="horse.ogg" type="audio/ogg"/>
    ///   <source src="horse.mp3" type="audio/mpeg"/>
    ///   Your browser does not support the audio element.
    /// </audio>
    public static void Source() { }

    /// <h3>A &lt;span&gt; element which is used to color a part of a text:</h3>
    /// <p>My mother has <span style="color:blue">blue</span> eyes.</p> 
    /// The &lt;span&gt; tag is much like the &lt;div&gt; element, but &lt;div&gt; is a block-level element and &lt;span&gt; is an inline element.
    public static void Span() { }

    // From: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/strike
    /// &lt;strike&gt;: <strike>Today's Special: Salmon</strike> SOLD OUT<br />
    /// &lt;s&gt;: <s>Today's Special: Salmon</s> SOLD OUT
    public static void Strike() { }

    /// <strong>This text is important!</strong> This one not.
    public static void Strong() { }

    /// <html>
    /// <head>
    /// <style>
    ///   h1 {color:red;}
    ///   p {color:blue;}
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <h1>A heading</h1>
    /// <p>A paragraph.</p>
    ///
    /// </body>
    /// </html>
    public static void Style() { }

    ///<p>This text contains <sub>subscript</sub> text where some unicode subscipt variants are missing.</p>
    ///<p>F<sub>(x+5-β)</sub> (here all subscript variants exist).</p>
    public static void Sub() { }

    /// <details>
    ///   <summary>Epcot Center</summary>
    ///   <p>Epcot is a theme park at Walt Disney World Resort featuring exciting attractions, international pavilions, award-winning fireworks and seasonal special events.</p>
    /// </details>
    public static void Summary() { }

    /// <p>This text contains <sup>superscript</sup> text.</p> 
    /// <p>This text contains <sup>UPPER</sup> case supercript text.</p> 
    /// <p>This text contains missing <sup>supercase Variants</sup>.</p> 
    /// <p>F<sup>(x+5-β)</sup></p>
    public static void Sup() { }

    /// <h5>Draw a circle:</h5>
    /// <svg width="100" height="100">
    ///   <circle cx="50" cy="50" r="40" stroke="green" stroke-width="4" fill="yellow" />
    /// </svg>
    /// <example>
    /// <h5>Draw a rectangle:</h5>
    /// <svg width="400" height="100">
    ///   <rect width="400" height="100" style="fill:rgb(0,0,255);stroke-width:10;stroke:rgb(0,0,0)" />
    /// </svg>
    /// </example>
    /// <example>
    /// <h5>Draw a square with rounded corners:</h5>
    /// <svg width="400" height="180">
    ///   <rect x="50" y="20" rx="20" ry="20" width="150" height="150" style="fill:red;stroke:black;stroke-width:5;opacity:0.5" />
    /// </svg>
    /// </example>
    /// <example>
    /// <h5>Draw a star:</h5>
    /// <svg width="300" height="200">
    ///   <polygon points="100,10 40,198 190,78 10,78 160,198"
    ///   style="fill:lime;stroke:purple;stroke-width:5;fill-rule:evenodd;" />
    /// </svg>
    /// </example>
    /// <example>
    /// <h5>Draw an SVG logo:</h5>
    /// <svg height="130" width="500">
    /// <defs>
    /// <linearGradient id="grad1" x1="0%" y1="0%" x2="100%" y2="0%">
    ///   <stop offset="0%" style="stop-color:rgb(255,255,0);stop-opacity:1" />
    ///   <stop offset="100%" style="stop-color:rgb(255,0,0);stop-opacity:1" />
    /// </linearGradient>
    /// </defs>
    ///
    /// <ellipse cx="100" cy="70" rx="85" ry="55" fill="url(#grad1)" />
    ///
    /// <text fill="#ffffff" font-size="45" font-family="Verdana" x="50" y="86">SVG</text>
    /// </svg>
    /// </example>
    public static void Svg() { }

    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// <example>
    /// <h5>How to add collapsed borders to a table (with CSS):</h5>
    /// <html>
    /// <head>
    /// <style>
    /// table, th, td {
    ///   border: 1px solid black;
    ///   border-collapse: collapse;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    ///
    /// </body>
    /// </html>
    /// </example>
    /// <example>
    /// <h5>How to right-align a table (with CSS):</h5>
    /// <table style="float:right">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to center-align a table (with CSS):</h5>
    /// <html>
    /// <head>
    /// <style>
    /// table, th, td {
    ///   border: 1px solid black;
    /// }
    /// table.center {
    ///   margin-left: auto;
    ///   margin-right: auto;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <table class="center">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </body>
    /// </html>
    /// </example>
    /// <example>
    /// <h5>How to add background-color to a table (with CSS):</h5>
    /// <table style="background-color:#00FF00">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to add padding to a table (with CSS):</h5>
    /// <html>
    /// <head>
    /// <style>
    /// table, th, td {
    ///   border: 1px solid black;
    /// }
    ///
    /// th, td {
    ///   padding: 10px;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    ///
    /// </body>
    /// </html>
    /// </example>
    /// <example>
    /// <h5>How to set table width (with CSS):</h5>
    /// <table style="width:400px">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to create table headers:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th>Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to create a table with a caption:</h5>
    /// <table>
    ///   <caption>Monthly savings</caption>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to define table cells that span more than one row or one column:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th colspan="2">Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///     <td>212-00-546</td>
    ///   </tr>
    /// </table>
    /// </example>
    public static void Table() { }

    /// <table>
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    ///   <tfoot>
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// <example>
    /// <h5>Style &lt;thead&gt;, &lt;tbody&gt;, and &lt;tfoot&gt; with CSS:</h5>
    /// <html>
    /// <head>
    /// <style>
    /// thead {color: green;}
    /// tbody {color: blue;}
    /// tfoot {color: red;}
    ///
    /// table, th, td {
    ///   border: 1px solid black;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <table>
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    ///   <tfoot>
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// </body>
    /// </html>
    /// </example>
    /// <example>
    /// <h5>How to align content inside &lt;tbody&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody style="text-align:right">
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to vertical align content inside &lt;tbody&gt; (with CSS):</h5>
    /// <table style="width:50%;">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tbody style="vertical-align:bottom">
    ///     <tr style="height:100px">
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr style="height:100px">
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    /// </table>
    /// </example>
    public static void Tbody() { }

    /// <h5>A simple HTML table, with two rows and four table cells:</h5>
    /// <table>
    ///   <tr>
    ///     <td>Cell A</td>
    ///     <td>Cell B</td>
    ///   </tr>
    ///   <tr>
    ///     <td>Cell C</td>
    ///     <td>Cell D</td>
    ///   </tr>
    /// </table>
    /// <example>
    /// <h5>How to align content inside &lt;td&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td style="text-align:right">$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td style="text-align:right">$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to add background-color to table cell (with CSS):</h5>
    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td style="background-color:#FF0000">January</td>
    ///     <td style="background-color:#00FF00">$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to set the height of a table cell (with CSS):</h5>
    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td style="height:100px">January</td>
    ///     <td style="height:100px">$100</td>
    ///   </tr>
    /// </table> 
    /// </example>
    /// <example>
    /// <h5>How to specify no word-wrapping in table cell (with CSS):</h5>
    /// <table>
    ///   <tr>
    ///     <th>Poem</th>
    ///   </tr>
    ///   <tr>
    ///     <td style="white-space:nowrap">Never increase, beyond what is necessary, the number of entities required to explain anything</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to vertical align content inside &lt;td&gt; (with CSS):</h5>
    /// <table style="width:50%;">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr style="height:100px">
    ///     <td style="vertical-align:bottom">January</td>
    ///     <td style="vertical-align:bottom">$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to set the width of a table cell (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td style="width:70%">January</td>
    ///     <td style="width:30%">$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to create table headers:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th>Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to create a table with a caption:</h5>
    /// <table>
    ///   <caption>Monthly savings</caption>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to define table cells that span more than one row or one column:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th colspan="2">Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///     <td>212-00-546</td>
    ///   </tr>
    /// </table>
    /// </example>
    public static void Td() { }

    /// <h5>Use &lt;template&gt; to hold some content that will be hidden when the page loads. Use JavaScript to display it:</h5>
    /// <button onclick="showContent()">Show hidden content</button>
    ///
    /// <template>
    ///   <h2>Flower</h2>
    ///   <img src="img_white_flower.jpg" width="214" height="204"/>
    /// </template>
    ///
    /// <script>
    /// function showContent() {
    ///   let temp = document.getElementsByTagName("template")[0];
    ///   let clon = temp.content.cloneNode(true);
    ///   document.body.appendChild(clon);
    /// }
    /// </script>
    /// <example>
    /// <h5>Fill the web page with one new div element for each item in an array. The HTML code of each div element is inside the template element:</h5>
    /// <template>
    ///   <div class="myClass">I like: </div>
    /// </template>
    ///
    /// <script>
    /// let myArr = ["Audi", "BMW", "Ford", "Honda", "Jaguar", "Nissan"];
    /// function showContent() {
    ///   let temp, item, a, i;
    ///   temp = document.getElementsByTagName("template")[0];
    ///   item = temp.content.querySelector("div");
    ///   for (i = 0; i &lt; myArr.length; i++) {
    ///     a = document.importNode(item, true);
    ///     a.textContent += myArr[i];
    ///     document.body.appendChild(a);
    ///   }
    /// }
    /// </script>
    /// </example>
    /// <example>
    /// <h5>Check browser support for &lt;template&gt;:</h5>
    /// <script>
    /// if (document.createElement("template").content) {
    ///   document.write("Your browser supports template!");
    /// } else {
    ///   document.write("Your browser does not supports template!");
    /// }
    /// </script>
    /// </example>
    public static void Template() { }

    /// <label for="w3review">Review of W3Schools:</label>
    ///
    /// <textarea id="w3review" name="w3review" rows="4" cols="50">
    /// At w3schools.com you will learn how to make a <b>website</b>. They offer free tutorials in all web development technologies.
    /// </textarea>
    /// <example>
    /// <h5>Disable default resize option:</h5>
    /// <html>
    /// <head>
    /// <style>
    /// textarea {
    ///   resize: none;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <label for="w3review">Review of W3Schools:</label>
    ///
    /// <textarea id="w3review" name="w3review" rows="4" cols="50">
    /// At w3schools.com you will learn how to make a website. They offer free tutorials in all web development technologies.
    /// </textarea>
    ///
    /// </body>
    /// </html>
    /// </example>
    public static void Textarea() { }

    /// <h5>An HTML table with a &lt;thead&gt;, &lt;tbody&gt;, and a &lt;tfoot&gt; element:</h5>
    /// <table>
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    ///   <tfoot>
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// <example>
    /// <h5>Style &lt;thead&gt;, &lt;tbody&gt;, and &lt;tfoot&gt; with CSS:</h5>
    /// <html>
    /// <head>
    /// <style>
    /// thead {color: green;}
    /// tbody {color: blue;}
    /// tfoot {color: red;}
    ///
    /// table, th, td {
    ///   border: 1px solid black;
    /// }
    /// </style>
    /// </head>
    /// <body>
    ///
    /// <table>
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    ///   <tfoot>
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// </body>
    /// </html>
    /// </example>
    /// <example>
    /// <h5>How to align content inside &lt;tfoot&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    ///   <tfoot style="text-align:center">
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// </example>
    /// <example>
    /// <h5>How to vertical align content inside &lt;tfoot&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    ///   <tfoot style="vertical-align:bottom">
    ///     <tr style="height:100px">
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// </example>
    public static void Tfoot() { }

    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// <example>
    /// <h5>How to align content inside &lt;th&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th style="text-align:left">Month</th>
    ///     <th style="text-align:left">Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to add background-color to table header cell (with CSS):</h5>
    /// <table>
    ///   <tr>
    ///     <th style="background-color:#FF0000">Month</th>
    ///     <th style="background-color:#00FF00">Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///  </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to set the height of a table header cell (with CSS):</h5>
    /// <table>
    ///   <tr>
    ///     <th style="height:100px">Month</th>
    ///     <th style="height:100px">Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to specify no word-wrapping in table header cell (with CSS):</h5>
    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th style="white-space:nowrap">My Savings for a new car</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to vertical align content inside &lt;th&gt; (with CSS):</h5>
    /// <table style="width:50%;">
    ///   <tr style="height:100px">
    ///     <th style="vertical-align:bottom">Month</th>
    ///     <th style="vertical-align:bottom">Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to set the width of a table header cell (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th style="width:70%">Month</th>
    ///     <th style="width:30%">Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to create table headers:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th>Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to create a table with a caption:</h5>
    /// <table>
    ///   <caption>Monthly savings</caption>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to define table cells that span more than one row or one column:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th colspan="2">Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///     <td>212-00-546</td>
    ///   </tr>
    /// </table>
    /// </example>
    public static void Th() { }

    /// <h5>An HTML table with a &lt;thead&gt;, &lt;tbody&gt;, and a &lt;tfoot&gt; element:</h5>
    /// <table>
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    ///   <tfoot>
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    ///
    /// <example>
    /// <html>
    /// <head>
    /// <style>
    /// thead {color: green;}
    /// tbody {color: blue;}
    /// tfoot {color: red;}
    ///
    /// table, th, td {
    ///   border: 1px solid black;
    /// }
    /// </style>
    /// </head>
    /// <body>
    /// <h5>Style &lt;thead&gt;, &lt;tbody&gt;, and &lt;tfoot&gt; with CSS:</h5>
    /// <table>
    ///   <thead>
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    ///   <tfoot>
    ///     <tr>
    ///       <td>Sum</td>
    ///       <td>$180</td>
    ///     </tr>
    ///   </tfoot>
    /// </table>
    /// </body>
    /// </html>
    /// </example>
    ///
    /// <example>
    /// <h5>How to align content inside &lt;thead&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <thead style="text-align:left">
    ///     <tr>
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///   <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to vertical align content inside &lt;thead&gt; (with CSS):</h5>
    ///  <table style="width:50%;">
    ///   <thead style="vertical-align:bottom">
    ///     <tr style="height:100px">
    ///       <th>Month</th>
    ///       <th>Savings</th>
    ///     </tr>
    ///   </thead>
    ///    <tbody>
    ///     <tr>
    ///       <td>January</td>
    ///       <td>$100</td>
    ///     </tr>
    ///     <tr>
    ///       <td>February</td>
    ///       <td>$80</td>
    ///     </tr>
    ///   </tbody>
    /// </table>
    /// </example>
    public static void Thead() { }

    /// <h5>How to define a time and a date:</h5>
    /// <p>Open from <time>10:00</time> to <time>21:00</time> every weekday.</p>
    ///
    /// <p>I have a date on <time datetime="2008-02-14 20:00">Valentines day</time>.</p>
    public static void Time() { }

    /// <!DOCTYPE html>
    /// <html>
    /// <head>
    ///   <title>HTML Elements Reference</title>
    /// </head>
    /// <body>
    /// <h5>Define a title for your HTML document:</h5>
    ///
    /// <h1>This is a heading</h1>
    /// <p>This is a paragraph.</p>
    ///
    /// </body>
    /// </html>
    public static void Title() { }

    /// <h5>A simple HTML table with three rows; one header row and two data rows:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    ///   <tr>
    ///     <td>February</td>
    ///     <td>$80</td>
    ///   </tr>
    /// </table>
    ///
    /// <example>
    /// <h5>How to align content inside &lt;tr&gt; (with CSS):</h5>
    /// <table style="width:100%">
    ///   <tr>
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr style="text-align:right">
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to add background-color to a table row (with CSS):</h5>
    /// <table>
    ///   <tr style="background-color:#FF0000">
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr>
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to vertical align content inside &lt;tr&gt; (with CSS):</h5>
    /// <table style="height:200px">
    ///   <tr  style="vertical-align:top">
    ///     <th>Month</th>
    ///     <th>Savings</th>
    ///   </tr>
    ///   <tr style="vertical-align:bottom">
    ///     <td>January</td>
    ///     <td>$100</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to create table headers:</h5>
    /// <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th>Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///   </tr>
    /// </table>
    /// </example>
    ///
    /// <example>
    /// <h5>How to define table cells that span more than one row or one column:</h5>
    ///  <table>
    ///   <tr>
    ///     <th>Name</th>
    ///     <th>Email</th>
    ///     <th colspan="2">Phone</th>
    ///   </tr>
    ///   <tr>
    ///     <td>John Doe</td>
    ///     <td>john.doe@example.com</td>
    ///     <td>123-45-678</td>
    ///     <td>212-00-546</td>
    ///   </tr>
    /// </table>
    /// </example>
    public static void Tr() { }

    /// <h5>A video with subtitle tracks for two languages:</h5>
    /// <video width="320" height="240" controls="">
    ///   <source src="forrest_gump.mp4" type="video/mp4"/>
    ///   <source src="forrest_gump.ogg" type="video/ogg"/>
    ///   <track src="fgsubtitles_en.vtt" kind="subtitles" srclang="en" label="English"/>
    ///   <track src="fgsubtitles_no.vtt" kind="subtitles" srclang="no" label="Norwegian"/>
    /// </video>
    public static void Track() { }

    /// <h5>The Teletype Text element</h5>
    /// <p>
    ///   Enter the following at the telnet command prompt: <code>set localecho</code><br />
    ///
    ///   The telnet client should display: <tt>Local Echo is on</tt>
    /// </p>
    public static void Tt() { }

    /// <h5>Mark up a misspelled word with the &lt;u&gt; tag:</h5>
    /// <p>This is some <u>mispeled</u> text.</p> 
    public static void U() { }

    /// <h5>An unordered HTML list:</h5>
    /// <ul>
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    ///
    /// <example>
    /// <h5>Set the different list style types (with CSS):</h5>
    ///  <ul style="list-style-type:circle">
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    ///
    /// <ul style="list-style-type:disc">
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    ///
    /// <ul style="list-style-type:square">
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    ///
    /// <example>
    /// <h5>Expand and reduce line-height in lists (with CSS):</h5>
    ///  <ul style="line-height:180%">
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    ///
    /// <ul style="line-height:80%">
    ///   <li>Coffee</li>
    ///   <li>Tea</li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    ///
    /// <example>
    /// <h5>Create a list inside a list (a nested list):</h5>
    ///  <ul>
    ///   <li>Coffee</li>
    ///   <li>Tea
    ///     <ul>
    ///       <li>Black tea</li>
    ///       <li>Green tea</li>
    ///     </ul>
    ///   </li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    ///
    /// <example>
    /// <h5>Create a more complex nested list:</h5>
    ///  <ul>
    ///   <li>Coffee</li>
    ///   <li>Tea
    ///     <ul>
    ///       <li>Black tea</li>
    ///       <li>Green tea
    ///         <ul>
    ///           <li>China</li>
    ///           <li>Africa</li>
    ///         </ul>
    ///       </li>
    ///     </ul>
    ///   </li>
    ///   <li>Milk</li>
    /// </ul>
    /// </example>
    public static void Ul() { }

    /// <p>The area of a triangle is: 1/2 x <var>b</var> x <var>h</var>, where <var>b</var> is the base, and <var>h</var> is the vertical height.</p>
    public static void Var() { }

    /// <video width="320" height="240" controls="">
    ///   <source src="movie.mp4" type="video/mp4"/>
    ///   <source src="movie.ogg" type="video/ogg"/>
    ///   Your browser does not support the video tag.
    /// </video>
    public static void Video() { }

    /// <p>To learn AJAX, you must be familiar with the XML<wbr/>Http<wbr/>Request Object.</p> 
    ///<p>This is a veryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryveryvery<wbr/>longwordthatwillbreakatspecific<wbr/>placeswhenthebrowserwindowisresized.</p>
    public static void Wbr() { }
}
