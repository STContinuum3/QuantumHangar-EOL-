# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

QuantumHangar is a Torch plugin for Space Engineers dedicated servers that provides a server-side hangar system for grid storage and a cross-server marketplace. The project is currently marked as End of Life (EOL) but may still receive updates.

## Build Configuration

**Primary Build Method**: Use Visual Studio 2022's MSBuild for building the solution.

### Setup Requirements
1. Run `Setup (run before opening solution).bat` first to create symlinks to Torch binaries
2. The script will prompt for your Torch.Server.exe folder location
3. This creates a `TorchBinaries` junction to your Torch installation

### Build Commands
```
msbuild QuantumHangar.sln /p:Configuration=Release /p:Platform="Any CPU"
```

## Architecture

### Project Structure
- **QuantumHangar** (Main Plugin): Core hangar functionality, commands, and market system
- **HangarStoreMod** (Client Mod): Space Engineers client-side mod for market block interaction

### Key Components

#### Core Systems
- **Hangar.cs**: Main plugin entry point, implements TorchPluginBase and IWpfPlugin
- **HangarCommandSystem.cs**: Async command execution system with task queuing
- **Commands/**: Command handlers for player, faction, alliance, and admin operations
- **HangarChecks/**: Validation and permission systems for different hangar types

#### Storage & Serialization
- **Serialization/GridSerializer.cs**: Grid serialization for storage and transfer
- **Configs/**: Configuration management and settings persistence

#### Market System
- **HangarMarket/**: Cross-server marketplace implementation
- **HangarMarketController.cs**: Market operations and transaction management
- **MarketListing.cs**: Market item data structures

#### Utilities
- **Utils/**: Supporting utilities including character management, grid operations, and Nexus API integration
- **UI/**: WPF user interface components for admin management

### Plugin Dependencies
- Torch Server Framework (.NET Framework 4.8)
- VRage/Sandbox APIs for Space Engineers integration
- Newtonsoft.Json for data serialization
- NLog for logging
- Optional: Nexus plugin for cross-server functionality

### Directory Structure
The plugin manages three main storage directories (configurable):
- Player hangars
- Faction hangars
- Alliance hangars

### Key Features
- Grid storage and retrieval with validation
- Cross-server marketplace with economy integration
- Multi-level hangar support (Player/Faction/Alliance)
- Auto-hangar and auto-sell capabilities
- PCU and block limit checking
- Enemy proximity checks for PvP balance

## Development Notes

### Framework Targets
- QuantumHangar: .NET Framework 4.8
- HangarStoreMod: .NET Framework 4.7.2

### Assembly Output
Both projects output to `bin\Release\` and include automatic ZIP packaging in `Build\` folder.

### Cross-Server Support
The plugin supports cross-server operations by sharing storage directories between server instances. Market functionality requires all servers to point to the same storage location.