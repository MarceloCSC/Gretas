# Gretas

Interactive 3D art gallery developed in Unity and published for the browser using WebGL.

## Overview

Gretas is a virtual exhibition space that allows users to explore a 3D gallery environment directly in the browser. The project focuses on combining visual presentation with a data-driven architecture for loading and displaying artworks dynamically.

Instead of embedding all assets locally, the application retrieves artwork data and images at runtime, making the system more flexible and scalable.

## Features

- 3D interactive gallery environment
- Browser-based experience using Unity WebGL
- Dynamic loading of artwork images from remote storage
- Artwork metadata system (artist, title, materials, etc.)
- Image caching system to improve performance
- Image optimization and quality handling
- Modular and organized code structure

## Tech Stack

- Unity
- C#
- WebGL
- Azure Blob Storage

## Architecture Highlights

### Data-Driven Artwork System
Artworks are defined using structured data (`ArtworkInfo`), allowing the gallery to be populated dynamically without hardcoding content.

### Runtime Image Loading
Images are loaded from a remote source (Azure Blob Storage) at runtime, enabling:
- Reduced build size
- Flexible content updates
- Separation between application and assets

### Caching Layer
A centralized caching system (`DataCache`) stores loaded textures to:
- Avoid redundant network requests
- Improve performance and responsiveness

### Modular Structure
The project is organized into clear domains:

- `Gretas.Artworks` → artwork display, metadata, and image handling  
- `Gretas.Core` → shared systems (e.g., caching)  

This separation helps maintain clarity and scalability.

## Technical Scope

The project was developed end-to-end, including:

- Player movement and interaction systems  
- Artwork display and UI logic  
- Runtime asset loading and caching  
- Data modeling for artworks  
- WebGL build and deployment  
- Hosting via Azure Static Web Apps  

## Purpose

Gretas was created to explore how interactive 3D environments can be used as digital exhibition spaces, with a focus on accessibility through the browser and scalable content delivery.

## What I Learned

- Designing data-driven systems in Unity  
- Implementing runtime asset loading from external sources  
- Managing performance with caching strategies  
- Structuring Unity projects with clear separation of concerns  
- Deploying Unity applications for the web using WebGL  

## Author

Marcelo Camargo  
[LinkedIn](https://www.linkedin.com/in/marcelocsc/)  
[GitHub](https://github.com/MarceloCSC)
