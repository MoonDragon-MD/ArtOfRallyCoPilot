# Art Of Rally CoPilot
This is a test to integrate audio into the [Art Of Rally Pace Notes](https://github.com/Theaninova/ArtOfRallyPaceNotes) project

NB: at the moment it compiles but I have not tested in the game

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

dotnet restore

nuget restore ArtOfRallyCoPilot.csproj

msbuild ArtOfRallyCoPilot.csproj

