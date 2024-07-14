# MerakiUserManager

MerakiUserManager is a command-line tool that automates the management of users in Cisco Meraki organizations. It simplifies the process of adding and removing technicians across multiple organizations, reducing manual effort and improving efficiency.

## Features

- Add a user to all or selected organizations associated with the provided API key.
- Remove a user from all or selected organizations associated with the provided API key.
- View all users from all or selected organizations associated with the provided API key. 
- User-friendly, menu-driven command-line interface.

## Requirements

- .NET 5.0 or later
- Cisco Meraki API key

## Installation

1. **Clone the repository:**

```sh
git clone https://github.com/nicholxs/meraki-user-manager.git
cd MerakiUserManager
```

2. Ensure you have .NET 5.0 or later installed:
   Download and install the .NET SDK from the official .NET website.

4. Build the project:
```sh
dotnet build
```

5. Run the app:
```sh
dotnet run
```

## Limitations

- Assigns full access at organization level to said user - this will include all sub networks. In the future I may include setting access levels.

## Usage
![image](https://github.com/user-attachments/assets/1f66cee9-fa0a-4f6a-9384-f4de1d5c3498)
![image](https://github.com/user-attachments/assets/9c436069-ac33-464a-a29c-57971a2eeee4)
