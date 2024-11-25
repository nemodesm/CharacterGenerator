using System;
using CharacterGenerator;

#if !DEBUG
using System.IO;
#endif

Console.Write("Image Count (default 256, 0 for infinite): ");
var read = Console.ReadLine();
if (!uint.TryParse(read, out CharacterRenderer.ScreenshotCount))
{
    CharacterRenderer.ScreenshotCount = 256;
}

#if !DEBUG
/*Console.WriteLine("Enter path to save files to (UPPERCASE): ");
var path = Console.ReadLine();
CharacterRenderer.filePath = path;
if (!Directory.Exists(path))
{
    Console.Error.WriteLine("Path does not exist");
    return 1;
}*/

if (!Directory.Exists("out/upper"))
{
    Directory.CreateDirectory("out/upper");
}

CharacterRenderer.filePath = "out/upper";
#endif

CharacterRenderer.useLower = false;

var renderer = new CharacterRenderer();

if (CharacterRenderer.ScreenshotCount == 0)
{
    Console.WriteLine("Press Ctrl+C (Terminal) or Escape (Render Preview) to stop");
}
renderer.Run();
renderer.Dispose();

/*CharacterRenderer.useLower = true;

#if !DEBUG
/*Console.WriteLine("Enter path to save files to (lowercase): ");
path = Console.ReadLine();
CharacterRenderer.filePath = path;
if (!Directory.Exists(path))
{
    Console.Error.WriteLine("Path does not exist");
    return 1;
}*/

/*if (!Directory.Exists("out/lower"))
{
    Directory.CreateDirectory("out/lower");
}

CharacterRenderer.filePath = "out/lower";
#endif

renderer = new CharacterRenderer();
renderer.Run();
renderer.Dispose();*/

return 0;