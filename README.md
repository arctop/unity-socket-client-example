# Neuos™ Stream Client

This project is an example of using the Neuos SDK stream server inside Unity3D.

# Prerequistes

- Neuos Central installed on the device and the user has calibrated.

- Both the device running your Unity app and the android device running need to be connected to the same LAN.

- A client must have an API key to be able and connect to a running server.

- You should review the resrouces listed below to get a better understanding of the Neuos platform and SDK.

- The code files contain complete documentation, please be sure to read through those.

- The project has been tested on Window 10 only.

# Instructions

1. Start Neuos Central app on the Android device and activate the SDK Stream.
With the stream active, Neuos will produce realtime values and transmit those over a socket to authenticated clients.

2. Run this example app.

# Project Structure

The client implementation handles connection and authentication, and uses UnityEvents to transmit the parsed data. This allows connecting listeners inside the Unity inspector.

The project is very basic, containing a very rudimentary UI to display the incoming data. A single GameController script handles all input and UI, and is responsible for interacting with the Neuos Client code.

namespace *io.neuos* contains 2 classes.

*[NeuosStreamConstants](/Assets/Scripts/io/neuos/NeuosStreamConstants.cs)* holds all relavent constants needed, and is derived off of [NeuosSDK.java](https://github.com/arctop/Neuos-SDK/blob/main/neuosSDK/src/main/java/io/neuos/NeuosSDK.java)
 and [NeuosStreamService.kt](https://github.com/arctop/Neuos-SDK/blob/main/neuosSDK/src/main/java/io/neuos/NeuosStreamService.kt)

*[NeuosStreamClient](/Assets/Scripts/io/neuos/NeuosStreamClient.cs)* is a MonoBehaviour that does all the heavy lifting of connection, authentication, parsing and socket handling.

With these 2 classes and [Json.Net](https://www.newtonsoft.com/json) in your project, you can implement a project using the Neuos Stream. 

The project was developed using Unity 2019.4 LTS.

# Resources

[Neuos SDK](https://github.com/arctop/Neuos-SDK) - The main Neuos SDK git hub project page.

[Neuos Stream Server Documentation](https://github.com/arctop/Neuos-SDK/blob/main/Neuos-Stream.md) - Readme file for the Neuos Stream Server.

[Neuos Central](https://play.google.com/store/apps/details?id=io.neuos.central) - Neuos Central Android play store page.

# Depndencies 

The project uses [Json.Net](https://www.newtonsoft.com/json).
