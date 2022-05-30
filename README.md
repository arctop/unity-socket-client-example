# Neuos� Stream Client

This project is an example of using the Neuos� SDK stream server inside Unity3D.

# Prerequistes

- A user must have Neuos Central installed on their device and complete the calibration phase.
  - Once completed, an option is avaliable to activate the SDK Stream. 
  - With the stream active, Neuos will produce realtime values, and transmit those over a socket to connected and authenticated clients.

- Both the device running your Unity app and the android device running need to be connected to the same LAN.

- A client must have an API key to be able and connect to a running server.

- You should review the resrouces listed below to get a better understanding of the Neuos� platform and SDK.

# Project Structure

The client implementation handles connection and authentication, and uses UnityEvents to transmit the parsed data. This allows connecting listeners inside the Unity inspector.

The project is very basic, containing a very rudimentary UI to display the incoming data. A single GameController script handles all input and UI, and is responsible for interacting with the Neuos Client code.

namespace *io.neuos* contains 2 classes.

*NeuosStreamConstants* holds all relavent constants needed, and is derived off of (insert java files here).

*NeuosStreamClient* is a MonoBehaviour that does all the heavy lifting of connection, authentication, parsing and socket handling.

With these 2 classes and [Json.Net](https://www.newtonsoft.com/json) in your project, you can implement a project using the Neuos Stream. 

# Resources

[Neuos� SDK](https://github.com/arctop/Neuos-SDK) - The main Neuos� SDK git hub project page.

[Neuos� Stream Server Documentation](https://github.com/arctop/Neuos-SDK/blob/main/Neuos-Stream.md) - Readme file for the Neuos� Stream Server.

[Neuos� Central](https://play.google.com/store/apps/details?id=io.neuos.central) - Neuos� Central Android play store page.

# Depndencies 

The project uses [Json.Net](https://www.newtonsoft.com/json)