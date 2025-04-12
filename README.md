# Art Of Rally CoPilot
This mod integrates the audio as a real co -pilot, I started from the existing mod: [Art Of Rally Pace Notes](https://github.com/Theaninova/ArtOfRallyPaceNotes) 

Use only with WAV files from a second otherwise otherwise it does not load them and must be of the format: Not compressed PCM, with 8 or 16 bit, mono or stereo

Soon I will make a version for Nexusmod

### Requirements on GNU/Linux (Debian and derivatives)
sudo apt install mono-complete msbuild nuget

wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

sudo dpkg -i packages-microsoft-prod.deb

rm packages-microsoft-prod.deb

sudo apt update

sudo apt install -y apt-transport-https

sudo apt install -y dotnet-sdk-3.1

### Compilation on GNU/Linux (Debian and derivatives)

cd ArtOfRallyCoPilot

rm -rf obj/

rm -rf bin/

dotnet restore

nuget restore ArtOfRallyCoPilot.csproj

msbuild ArtOfRallyCoPilot.csproj

