# HubSpot API Client

A simple and efficient C# client library for interacting with the HubSpot CRM API. This library provides easy-to-use methods for managing HubSpot contacts through the HubSpot CRM v3 API.

## Features

- **Contact Management**: Full CRUD operations for HubSpot contacts [1](#0-0) 
- **Authentication**: Secure API key-based authentication [2](#0-1) 
- **Pagination Support**: Efficiently retrieve large contact lists with automatic pagination [3](#0-2) 
- **Error Handling**: Built-in error handling and validation [4](#0-3) 
- **Async/Await**: Modern asynchronous programming support

## Installation

Add the HubSpotClient to your .NET project.

## Quick Start

### Initialize the Client

```csharp
var client = new HubSpotClient("your-api-key-here");
```

### Basic Usage Examples

#### Get Contact by Email
```csharp
string contactId = await client.GetContactID("contact@example.com");
```

#### Get Contact by ID
```csharp
Contact contact = await client.GetContact(contactId);
```

#### Create a New Contact
```csharp
await client.CreateContact("John", "Doe", "john.doe@example.com", "+1234567890");
```

#### Update Contact Information
```csharp
await client.UpdateContact(contactId, firstname: "Jane", email: "jane.doe@example.com");
```

#### Delete a Contact
```csharp
await client.DeleteContact(contactId);
```

#### Get All Contacts
```csharp
List<Contact> allContacts = await client.GetAllContacts();
```

## API Methods

| Method | Description |
|--------|-------------|
| `GetContactID(string email)` | Retrieves contact ID by email address [5](#0-4)  |
| `GetContact(string id, List<string> contactProperties = null)` | Gets contact details by ID [6](#0-5)  |
| `CreateContact(string firstname, string lastname, string email, string phone)` | Creates a new contact [7](#0-6)  |
| `UpdateContact(string id, ...)` | Updates existing contact information [8](#0-7)  |
| `DeleteContact(string id)` | Deletes a contact by ID [9](#0-8)  |
| `GetAllContacts(List<string> contactProperties = null)` | Retrieves all contacts with pagination support [3](#0-2)  |

## Configuration

### API Key Setup

You need a valid HubSpot API key to use this client. The API key is passed to the constructor: [2](#0-1) 

### Default Contact Properties

The client uses these default contact properties when none are specified: [10](#0-9) 
- firstname
- lastname  
- email
- phone

## Error Handling

The client includes built-in error handling for common scenarios:
- Invalid or empty API keys [11](#0-10) 
- HTTP request failures [4](#0-3) 
- Non-existent or archived contacts [12](#0-11) 

## Requirements

- .NET Core/.NET 5+ 
- Valid HubSpot API key
- Internet connection for API requests

## API Endpoint

This client connects to the HubSpot CRM v3 API: [13](#0-12) 

## Notes

- All methods are asynchronous and return Tasks
- The client automatically handles pagination for large contact lists
- Contact properties can be customized by passing a list of property names
- API responses are automatically deserialized to strongly-typed objects
