using ZoomTranscriptCleaner;

Console.Write("Enter a directory: ");
string directory = Console.ReadLine()!;

var cleanedDirectory = Path.Combine(directory, "cleaned");
Directory.CreateDirectory(cleanedDirectory);

var files = Directory.GetFiles(directory);

foreach(var fileName in files)
{
    Console.WriteLine($"Processing {fileName}");

    var content = File.ReadAllText(fileName);
    var cleanerService = new CleanerService(content);

    var cleaned = cleanerService.Clean();

    var newFileName = Path.Combine(cleanedDirectory, Path.GetFileName(fileName));

    File.WriteAllText(newFileName, cleaned);

    Console.WriteLine($"Cleaned transcript saved to {newFileName}");
}

Console.ReadLine();