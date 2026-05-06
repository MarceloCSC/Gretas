# Gretas

Interactive 3D art gallery developed in Unity and published for the browser using WebGL.

# Overview

Gretas is a virtual 3D exhibition space built in Unity that allows users to explore artworks directly in the browser through an interactive environment.

The project was developed as an experiment in combining digital exhibition spaces, real-time navigation, and runtime content loading. Instead of embedding all artwork assets directly into the application build, images and metadata are loaded remotely during execution, reducing the initial application size and allowing the gallery content to be updated independently.

The gallery also explores non-Euclidean spatial concepts, including impossible layouts and continuous navigation between interconnected spaces.

# Features

- Interactive 3D gallery environment
- Browser-based experience using Unity WebGL
- Runtime loading of artwork images and metadata
- Remote asset delivery using Azure Blob Storage
- Texture caching system to improve performance
- Image quality handling and optimized loading
- Artwork metadata display system
- Navigation and interaction systems
- Non-Euclidean spatial exploration and impossible layouts
- Modular code organization

# Tech Stack

- Unity
- C#
- WebGL
- Azure Blob Storage

# Technical Highlights

## Runtime Artwork Loading

Artwork images and metadata are loaded remotely at runtime instead of being bundled directly into the application build.

This approach allows:

- Reduced initial build size
- Faster initial loading
- Independent content updates
- Separation between application logic and artwork assets

Artwork data is structured using dedicated data models, allowing the gallery content to be configured dynamically.

## Image Caching

The project includes a centralized texture caching system to avoid unnecessary network requests and improve responsiveness while navigating the gallery.

Loaded images are stored in memory and reused when needed, reducing repeated downloads and improving the browsing experience.

## Image Quality Handling

Different image quality levels are used depending on the viewing context, helping balance visual quality and performance in the browser environment.

Higher quality textures can be loaded when the user closely examines specific artworks.

## Navigation and Interaction Systems

The gallery includes custom systems for:

- Player navigation
- Artwork interaction
- UI display
- Metadata visualization
- Artwork presentation

These systems were developed specifically for the virtual exhibition experience.

## Non-Euclidean Spatial Design

The project explores spatial concepts inspired by non-Euclidean environments, including:

- Impossible layouts
- Continuous corridors
- Interconnected spaces
- Portal-like transitions

These elements were used to create a more immersive and unconventional navigation experience.

# Project Structure

The project is organized into separate domains and systems, including:

- `Gretas.Artworks` → artwork display, metadata handling, image loading, and presentation systems
- `Gretas.Core` → shared systems and utilities, including caching
- Additional systems related to navigation, interaction, and environment logic

This structure helps keep the project organized and easier to maintain.

# Deployment

The project was published as a WebGL application for browser access.

Azure services were used for hosting and remote asset delivery, while artwork images were stored separately from the main application build using Azure Blob Storage.

# Technical Scope

The project was developed end-to-end, including:

- Unity scene and environment development
- Gameplay and navigation systems
- Artwork visualization systems
- Runtime image loading
- Remote asset integration
- Texture caching
- UI and interaction logic
- Data modeling for artworks
- WebGL build and browser deployment

# Purpose

Gretas was created to explore how interactive 3D environments can function as digital exhibition spaces accessible through the browser.

The project combines technical experimentation with artistic and spatial exploration, focusing on accessibility, runtime content delivery, and immersive navigation.

# What I Learned

- Structuring Unity projects with modular systems
- Implementing runtime asset loading from external sources
- Managing performance in WebGL applications
- Working with caching strategies for browser-based experiences
- Handling remote image delivery and metadata systems
- Designing interactive 3D environments for the web
- Deploying Unity applications using WebGL and Azure services

## Author

Marcelo Camargo  
[LinkedIn](https://www.linkedin.com/in/marcelocsc/)  
[GitHub](https://github.com/MarceloCSC)
