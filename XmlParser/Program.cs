// See https://aka.ms/new-console-template for more information
using XmlParser;

var xml = @"<Person>
    <Name>John</Name>
    <Age>30</Age>
    <Address Street=""Main St"" City=""New York"" />
</Person>
";
Console.WriteLine(XmlToCSharpClassConverter.ConvertXmlToCSharpClass(xml,"Person"));
