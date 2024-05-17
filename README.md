VprForge is a library which facilitates modding Vocaloid5 project files (.VPR files).

__How does it work__

It unzips the project, extracts the Audio data, and parses the JSON-formatted "sequence" file (tracks, parts, notes, and parameters in the project), allowing users to directly modify any part of the sequence programatically.

1. Call FileIO.TryRead() to open and parse the project file into runtime objects.
2. Modify the runtime objects (read the inline documentation for guidance).
3. Write a new project file by calling FileIO.TryWrite()

__Disclaimers__

Vocaloid5 is owned completely by Yamaha. I've written this library in good faith in order to automate certain tasks in my own Vocaloid projects which were not possible in the editor.
I am posting the code here for others to learn more about how Vocaloid projects work, and maybe learn a thing or two about C# programming.
If you want to use this code to somehow circumvent any copyrights or duties to the program's owner, shame on you. Please support the things you love.
