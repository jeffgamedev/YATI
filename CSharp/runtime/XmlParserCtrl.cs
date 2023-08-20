namespace YATI;

using Godot;
using Godot.Collections;

public class XmlParserCtrl
{
    private readonly XmlParser _parser;
    private string _parsedFileName;

    public XmlParserCtrl()
    {
        _parser = new XmlParser();
    }
    
    public Error Open(string sourceFile)
    {
        _parsedFileName = sourceFile;
        return _parser.Open(_parsedFileName);
    }

    public string NextElement()
    {
        var err = ParseOn();
        if (err != Error.Ok)
            return "";
        if (_parser.GetNodeType() == XmlParser.NodeType.Text)
        {
            var text = _parser.GetNodeData().Trim();
            if (text.Length > 0)
                return "<data>";
        }
        while ((_parser.GetNodeType() != XmlParser.NodeType.Element) &&
               (_parser.GetNodeType() != XmlParser.NodeType.ElementEnd))
        {
            err = ParseOn();
            if (err != Error.Ok)
                return null;
        }

        return _parser.GetNodeName();
    }
    
    public bool IsEnd()
    {
        return _parser.GetNodeType() == XmlParser.NodeType.ElementEnd;
    }
    
    public bool IsEmpty()
    {
        return _parser.IsEmpty();
    }

    public string GetData()
    {
        return _parser.GetNodeData();
    }

    public Dictionary<string, string> GetAttributes()
    {
        var attributes = new Dictionary<string, string>();
        for (var i = 0; i < _parser.GetAttributeCount(); i++)
            attributes[_parser.GetAttributeName(i)] = _parser.GetAttributeValue(i);
        return attributes;
    }

    private Error ParseOn()
    {
        var err = _parser.Read();
        if (err != Error.Ok)
		{
            GD.PrintErr($"Error parsing file '{_parsedFileName}' (around line {_parser.GetCurrentLine()}).");
		}
        return err;
    }
}
