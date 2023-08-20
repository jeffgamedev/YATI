namespace YATI;

using Godot;

public static class Importer
{
    public static Node2D Import(string sourceFile)
	{
        var tilemapCreator = new TilemapCreator();
		tilemapCreator.SetMapLayersToTilemaps(true);
        var node2D = tilemapCreator.Create(sourceFile);
        if (node2D == null)
		{
            return null;
		}
		return node2D;
    }
}
