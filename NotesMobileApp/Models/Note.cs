﻿namespace NotesMobileApp.Models;

internal class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public Note()
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Text = "";
        Date = DateTime.Now;
    }

    public void Save() => File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void Delete() => File.Delete(Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static Note Load(string filename)
    {
        filename = Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<Note> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        return Directory
            // Select the file names from the directory
            .EnumerateFiles(appDataPath, "*.notes.txt")
            // Each file name is used to load a note
            .Select(filename => Load(Path.GetFileName(filename)))
            // With the final collection of notes, order them by date
            .OrderByDescending(note => note.Date);
    }
}