# Agile Team Tools
A collection of tools that are commonly used by teams following standard agile methodologies

# Contributing
This project welcomes contributions and suggestions. Pull Requests will only be accepted if they adhere to the following criteria:
1. Target the **main** branch of this repository
2. The Continuos Integration build must be buildable and all unit tests pass
3. If any features are added or modified then they must be accompanied by unit tests that verify the functionality and pass via GitHub Actions

# Requirements
* Visual Studio 2019 or later
* .NET 5

# Projects
## Applications
|Project|Type|Purpose|
|---|---|---|
|AgileTeamTools.Ui|Blazor Server|User Interface for the application|

## Tests
|Project|Type|Purpose|
|---|---|---|
|AgileTeamTools.Ui.Tests|xUnit Tests|Unit Tests for the Blazor UI project|

# Pipelines
GitHub Actions are used to perform buil and release tasks
|Defintion|Purpose|
|---|---|
|[ci-build.yml](/.github/workflows/ci-build.yml)|Continous Integration Build|
|[blazor-deploy.yml](/.github/workflows/blazor-deploy.yml)|Deploys the Blazor App to Azure|

# Development Environment Setup
To run Agile Team Tools in your development environment, use the following steps.

## Default
No special settings are required.  Pull down the code, build, and run.