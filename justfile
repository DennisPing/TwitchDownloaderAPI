# Variables
submodule_path := "./TwitchDownloader"
solution_file := "TwitchDownloaderAPI.sln"

# Default recipe
default:
    update-submodule
    commit-submodule
    build-debug
    
# Update submodule if there are any
update-submodule:
    cd {{submodule_path}} && \
    git fetch origin master && \
    git checkout master && \
    git pull --ff-only origin master
    @echo "Submodule updated to the latest commit on 'master' branch."
    
# Commit updates if there are any
commit-submodule:
    git add {{submodule_path}}
    git diff-index --quiet HEAD {{submodule_path}} || \
    git commit -m "Updated TwitchDownloader submodule"
    @echo "Submodule changes committed"
    
# Build Debug version
build-debug:
    @echo "Building .NET project - Debug"
    dotnet build {{solution_file}} --configuration Debug
    
# Build Release version
build-release:
    @echo "Building .NET project - Release"
    dotnet build {{solution_file}} --configuration Release
    
# Clean the project
clean:
    @echo "Cleaning build artifacts..."
    dotnet clean {{solution_file}}
