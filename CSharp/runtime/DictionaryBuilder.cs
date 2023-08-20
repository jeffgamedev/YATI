namespace YATI;

using Godot;
using System.Linq;
using Godot.Collections;

public static class DictionaryBuilder
{
    private enum FileType
    {
        Xml,
        Json,
        Unknown
    }

    public static Dictionary GetDictionary(string sourceFile)
    {
        var checkedFile = sourceFile;
        if (!FileAccess.FileExists(checkedFile))
        {
            checkedFile = sourceFile.GetBaseDir().PathJoin(sourceFile);
            if (!FileAccess.FileExists(checkedFile))
            {
                return null;
            }
        }
        
        var type = FileType.Unknown;
        var extension = sourceFile.GetFile().GetExtension();
        if (new[] { "tmx", "tsx", "xml", "tx" }.Contains(extension))
            type = FileType.Xml;
        else if (new[] { "tmj", "tsj", "json", "tj" }.Contains(extension))
            type = FileType.Json;
        else
        {
            var file = FileAccess.Open(checkedFile, FileAccess.ModeFlags.Read);
            var chunk = System.Text.Encoding.UTF8.GetString(file.GetBuffer(12));
            if (chunk.StartsWith("<?xml "))
                type = FileType.Xml;
            else if (chunk.StartsWith("{ \""))
                type = FileType.Json;
        }

        switch (type)
        {
            case FileType.Xml:
            {
                var dictBuilder = new DictionaryFromXml();
                return dictBuilder.Create(checkedFile);
            }
            case FileType.Json:
            {
                var json = new Json();
                var file = FileAccess.Open(checkedFile, FileAccess.ModeFlags.Read);
                if (json.Parse(file.GetAsText()) == Error.Ok)
                    return (Dictionary)json.Data;
                break;
            }
            case FileType.Unknown:
                GD.PrintErr($"ERROR: File '{sourceFile}' has an unknown type. -> Continuing but result may be unusable");
                break;
        }

        return null;
    }
}
