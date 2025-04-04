# C# Botnet Project (NullBotnet)

This project implements a client-server architecture using TCP communication on port `6969`. It is designed for ethical and legal purposes only, such as testing network systems or learning about TCP communication in a controlled environment. 

### This is only a Botnet Base

## Features

1. **TCP Communication**:
   - Establishes a TCP connection between client and server.
   - Uses port `6969` for communication.

2. **Client Commands**:
   - Accepts commands from the server to be executed on the client side.
   - Returns output of executed commands back to the server.

3. **Cross-System Compatibility**:
   - Works on any system with a .NET or compatible framework installed.

4. **No Persistence**:
   - The client does not add itself to autostart mechanisms or modify system files, ensuring it is non-intrusive.

## Requirements

### Server:
- A computer running the C# server application.
- Open port `6969` for incoming connections.

### Client:
- A computer running the C# client application.
- TCP access to the server on port `6969`.

## How to Use

### Setting Up the Server
1. Compile the C# server application.
2. Run the server application.
3. Wait for incoming client connections on port `6969`.

### Setting Up the Client
1. Compile the C# client application.
2. Run the client application on the target machine.
3. The client will attempt to connect to the server on port `6969`.

### Sending Commands
1. Once the client connects to the server, you can send shell commands from the server.
2. The client will execute the commands and send back the output.
3. Use the `exit` command to terminate the connection.

## Code Highlights

### Server
- Listens for incoming connections on port `6969`.
- Accepts commands from the user and sends them to the connected client.
- Displays command output received from the client.

### Client
- Connects to the server on port `6969`.
- Executes received commands using the system shell.
- Sends command output back to the server.

## Security Considerations

- **Intended Use**: This project is intended for legal and ethical purposes only, such as network security testing or educational purposes.
- **No Autostart**: The client does not install itself or persist in the system.
- **Network Access**: Ensure the port `6969` is open only in secure environments to avoid unauthorized connections.

## Disclaimer
This project must be used responsibly and within the bounds of the law. Misuse of this tool for unauthorized access or malicious purposes is strictly prohibited and may result in legal consequences. Always obtain proper authorization before using this tool on any system.
## Contributers
- Drxem
