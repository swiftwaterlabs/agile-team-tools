# Agile Team Tools

# Requirements
* Visual Studio 2022 or later
* .NET 6
* Azure Functions Runtime v4 or later

# Projects
## Applications
| Project            | Type               | Purpose                            |
| ------------------ | ------------------ | ---------------------------------- |
| AgileTeamTools.Api | Azure Function     | API Layer for the application      |
| AgileTeamTools.Ui  | Blazor WebAssembly | User Interface for the application |

## Tests
| Project                        | Type        | Purpose                              |
| ------------------------------ | ----------- | ------------------------------------ |
| AgileTeamTools.Api.Tests       | xUnit Tests | Unit Tests for the Api project       |
| AgileTeamTools.Blazor.Ui.Tests | xUnit Tests | Unit Tests for the Blazor UI project |

# Pipelines
GitHub Actions are used to perform buil and release tasks
| Defintion                                       | Purpose                     |
| ----------------------------------------------- | --------------------------- |
| [ci-build.yml](/.github/workflows/ci-build.yml) | Continous Integration Build |

# Development Environment Setup
To run Agile Team Tools in your development environment, use the following steps.

## Azure Function Settings File
Be sure you have a local.settings.json file in the function root directory.  Sample files are below:

### API
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureSignalRConnectionString": "Endpoint=https://agileteam-dev.service.signalr.net;AccessKey=super-secret;Version=1.0;",
  }
}
```