using System.Text.RegularExpressions;

class ModeratorApp
{
    static void Main()
    {
        try
        {
            Console.Write("Enter the path to the text file: ");
            string textFilePath = Console.ReadLine();

            if (!File.Exists(textFilePath))
            {
                Console.WriteLine("Text file does not exist.");
                return;
            }

            Console.Write("Enter the path to the moderation words file: ");
            string moderationFilePath = Console.ReadLine();

            if (!File.Exists(moderationFilePath))
            {
                Console.WriteLine("Moderation words file does not exist.");
                return;
            }

            string[] moderationWords = File.ReadAllLines(moderationFilePath);
            string textContent = File.ReadAllText(textFilePath);

            Console.WriteLine("Moderation words:");
            foreach (string line in moderationWords)
            {
                string[] words = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    Console.WriteLine($"'{word}'");
                }
            }

            Console.WriteLine("Original content:");
            Console.WriteLine(textContent);

            foreach (string line in moderationWords)
            {
                string[] words = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (string.IsNullOrWhiteSpace(word)) continue;

                    string pattern = $@"\b{Regex.Escape(word)}\b";
                    string replacement = new string('*', word.Length);
                    string oldContent = textContent;
                    textContent = Regex.Replace(textContent, pattern, replacement, RegexOptions.IgnoreCase);

                    Console.WriteLine($"Replacing '{word}' with '{replacement}'");
                    Console.WriteLine("Content after replacement:");
                    Console.WriteLine(textContent);
                    Console.WriteLine("--------------------");
                }
            }

            Console.WriteLine("Modified content:");
            Console.WriteLine(textContent);

            File.WriteAllText(textFilePath, textContent);

            Console.WriteLine("Moderation complete. All specified words have been replaced with '*'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
