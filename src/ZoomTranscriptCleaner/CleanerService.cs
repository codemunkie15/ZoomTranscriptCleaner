using System.Text.RegularExpressions;

namespace ZoomTranscriptCleaner
{
    public class CleanerService
    {
        private string transcription;
        private List<string> newLines;
        private string? currentSpeaker;

        public CleanerService(string transcription)
        {
            this.transcription = transcription;
            this.newLines = new List<string>();
        }

        public string Clean()
        {
            // Remove timestamps using regex
            transcription = Regex.Replace(transcription, @"[0-9]*\r\n[0-9]*:.*\r\n", "");

            var lines = transcription.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                ProcessLine(line);
            }

            return string.Join(Environment.NewLine, newLines);
        }

        private void ProcessLine(string line)
        {
            var lineSpeaker = GetLineSpeaker(line);

            if (string.IsNullOrEmpty(lineSpeaker))
            {
                newLines.Add(line);
                newLines.Add("");
                return;
            }

            var lineWithoutSpeaker = GetLineWithoutSpeaker(line);

            if (currentSpeaker != lineSpeaker)
            {
                newLines.Add(lineSpeaker);
                newLines.Add(lineWithoutSpeaker!);
                newLines.Add("");
            }
            else
            {
                newLines[^2] = newLines[^2] + " " + lineWithoutSpeaker;
            }

            currentSpeaker = lineSpeaker;
        }

        private static string? GetLineSpeaker(string line)
        {
            var colonIndex = line.IndexOf(':');

            if (colonIndex < 0)
            {
                return null;
            }

            return line.Substring(0, colonIndex);
        }

        private static string? GetLineWithoutSpeaker(string line)
        {
            var colonIndex = line.IndexOf(':') + 2;

            if (colonIndex < 0)
            {
                return null;
            }

            return line[colonIndex..];
        }
    }
}
