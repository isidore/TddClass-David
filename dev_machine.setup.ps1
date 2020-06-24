# To run this script directly, run this in an elevated admin PowerShell prompt:
#     Invoke-WebRequest -UseBasicParsing https://raw.githubusercontent.com/isidore/TddClass-David/master/dev_machine.setup.ps1 | Invoke-Expression

# This script is intended to setup a dev machine from scratch. Very useful for setting up a EC2 instance for mobbing.
#


iwr -useb cin.st | iex
cinst -y vscode win-no-annoy visualstudio2019professional googlechrome git anydesk github-desktop resharper-ultimate-all visualstudio2019-workload-manageddesktop netfx-4.8-devpack notepadplusplus beyondcompare poshgit
# When we tried to use chocolatey to install NCrunch, NCrunch didn't show up in Visual Studio
start-process https://www.ncrunch.net/download
start-process https://github.com/GreatWebGuy/MobTime/releases
& "C:\Program Files\Git\cmd\git.exe" clone https://github.com/isidore/TddClass-David.git C:\Code\TddClass
