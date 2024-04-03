# Arctop Stream Client

This project is an example of using the Arctop Stream server inside Unity3D.

# Prerequistes

- Arctop app installed on the device and the user has calibrated.

- Both the device running your Unity app and the android device running need to be connected to the same LAN.

- A client must have an API key to be able and connect to a running server.

- You should review the resrouces listed below to get a better understanding of the Arctop platform and SDK.

- The code files contain complete documentation, please be sure to read through those.

# Installation

- The release page includes several versions of the unity package. Depending on your unity version / project packages, you might have the newtonsoft Json Libraray installed as a package. For that reason, a variant with the NoJson package is available. It is recommended that you try that package first. If you encounter compile errors, stating that newtonsoft cannot be found, you can either install the other package, or preferbly, add the official unity package into your project. See the Depndencies section below.

# Instructions

1. Start Arctop app on the Android device and activate the SDK Stream.
With the stream active, Arctop app will produce realtime values and transmit those over a socket to authenticated clients.

2. Run this example app.

# Project Structure

The client implementation handles connection and authentication, and uses UnityEvents to transmit the parsed data. This allows connecting listeners inside the Unity inspector.

The project is very basic, containing a very rudimentary UI to display the incoming data. A single GameController script handles all input and UI, and is responsible for interacting with the Arctop Stream client code.

namespace *io.neuos* contains 2 classes.

*[NeuosStreamConstants](/Assets/NeuosSocketClient/Scripts/io/neuos/NeuosStreamConstants.cs)* holds all relavent constants needed, and is derived off of [NeuosSDK.java](https://github.com/arctop/Neuos-SDK/blob/main/neuosSDK/src/main/java/io/neuos/NeuosSDK.java)
 and [NeuosStreamService.kt](https://github.com/arctop/Neuos-SDK/blob/main/neuosSDK/src/main/java/io/neuos/NeuosStreamService.kt)

*[NeuosStreamClient](/Assets/Scripts/io/neuos/NeuosStreamClient.cs)* is a MonoBehaviour that does all the heavy lifting of connection, authentication, parsing and socket handling.

With these 2 classes and [Json.Net](https://www.newtonsoft.com/json) in your project, you can implement a project using the Neuos Stream. 

The project was developed using Unity 2019.4 LTS.

# Resources

[Android Native SDK](https://github.com/arctop/android-sdk) - The main Arctop Android Native SDK git hub project page.


[Arctop Stream Server Documentation](https://github.com/arctop/android-sdk/blob/main/Arctop-Stream.md) - Readme file for the Arctop Stream Server.

[Arctop Android app](https://play.google.com/store/apps/details?id=io.neuos.central) - Arctop Android play store page.

# Dependencies 

The project uses [Json.Net](https://www.newtonsoft.com/json). Originally, Unity did not have an official package available. This has changed, and as such, we have provided a package without the DLL to avoid assembly clashing. 

If this package does not compile due to assembly missing, it is recommended that you add the official unity package from the package manager, as oppossed to using the bundled dll. 

To add the package, in Unity, go to Window -> Package Manager, click the + sign, and select "Add package by name...". Enter com.unity.nuget.newtonsoft-json into the name field, and hit add. This will import the package and resolve compilation errors.

If you had the old DLL in the project, feel free to delete it and add the package as a replacement.
