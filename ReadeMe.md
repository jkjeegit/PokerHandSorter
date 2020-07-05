
1) Download / Clone the repo [Repo Url] from github in to a [local folder]

2) Using command prompt, navigate to the [local folder]

3) Run the following command, this is application is developed using .net framework 4.7.2. Make sure the framework sdk or runtime is installed before deploying

msbuild "[local folder]\PokerHandSorter\PokerHandSorter.csproj" /p:Configuration=Release

4) In the commnad prompt navigate to the folder [local folder]\PokerHandSorter\bin\Release and find the executable PokerHandSorter.exe

5) Run the command to execute the Poker Hand Sorter

"[local folder]\PokerHandSorter\bin\Release\PokerHandSorter.exe" "[Folder path of input file]\poker-hands.txt"