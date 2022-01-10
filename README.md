# Agile Team Tools
A collection of tools that are commonly used by teams following standard agile methodologies

# Features
## Estimation
While working with virtual teams, one of the ceremonies that can be difficult to properly perform is work estimation.  To facilitate this, the Agile Team Tools enables real-time estimation sessions by allowing people to select their estimate and then immediately share with others on the team so everyone can see the estimates and have a conversation on what the team consensus is.

## Lean Coffee
Another difficulty working with virtual teams and performing [lean coffee](https://agilecoffee.com/leancoffee/) is the ability for participants to effectively give feedback if they want to continue on a topic (thumbs up) or not (thumbs down).  Agile Team Tools enables anonympous(optional), real-time sharing of a person's opionion on whether or not to continue in a real-time manner so others on the team can see their response and decide whether to move on or not.

## Solution Architecture
This solution is broken up into two main components, an API and a static website.  The API enables serverless SignalR operations so users can be updated in realtime, and the static website is the user interface component that enables people to interact with each other in a team environment.  As this is a native Azure application, all components use Azure PaaS services such as Azure Functions, SignalR Service, Storage Blobs, and Managed Identities.

### Component Diagram
For more detail, a [Threat model](https://docs.microsoft.com/en-us/azure/security/develop/threat-modeling-tool) for the solution can be found at [docs/threat-model.tm7](docs/threat-model.tm7) that shows all the components and their interactions.  For a more detailed view of the individual components, underlying code, and their interactions see [src/README.md](src/README.md)

### Infrastructure
The infrastructure that is needed to run this service can be found at https://github.com/swiftwaterlabs/agile-team-tools-infra where it is defined using [Terraform](https://www.terraform.io/)

# Contributing
This project welcomes contributions and suggestions. Pull Requests will only be accepted if they adhere to the following criteria:
1. Target the **main** branch of this repository
2. The Continuos Integration build must be buildable and all unit tests pass
3. If any features are added or modified then they must be accompanied by unit tests that verify the functionality and pass via GitHub Actions