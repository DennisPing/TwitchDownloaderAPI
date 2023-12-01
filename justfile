# Variables
submodule_path := "./TwitchDownloader"
solution_file := "TwitchDownloaderAPI.sln"

# Default recipe
default:
    just update-submodule
    just commit-submodule
    just build-debug
    
# Update submodule to the latest tag on master branch
update-submodule:
    cd {{submodule_path}} && \
    git fetch --tags && \
    latest_tag=$(git describe --tags `git rev-list --tags --max-count=1 --branches=master`) && \
    git checkout $latest_tag
    @echo "Submodule updated to the latest tag on 'master' branch"
    
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
